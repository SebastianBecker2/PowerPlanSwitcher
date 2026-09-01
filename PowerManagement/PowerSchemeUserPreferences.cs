namespace PowerManagement;

using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;

/// <summary>
/// Reads Visible flags, display Order, and CycleOnlyVisible from the tray app's user.config.
/// </summary>
public sealed class PowerSchemeUserPreferences
{
    public const string DefaultCompanyName = "Sebastian Becker";
    public const string AppSettingsName = "PowerPlanSwitcher";

    private readonly Dictionary<Guid, bool> visibilityByGuid;
    private readonly Dictionary<Guid, int> orderByGuid;
    public bool CycleOnlyVisible { get; }
    public string? SourceConfigPath { get; }

    private PowerSchemeUserPreferences(
        Dictionary<Guid, bool> visibilityByGuid,
        Dictionary<Guid, int> orderByGuid,
        bool cycleOnlyVisible,
        string? sourceConfigPath)
    {
        this.visibilityByGuid = visibilityByGuid;
        this.orderByGuid = orderByGuid;
        CycleOnlyVisible = cycleOnlyVisible;
        SourceConfigPath = sourceConfigPath;
    }

    public static PowerSchemeUserPreferences Load() =>
        Load(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            ResolveCompanyFolderName(),
            ResolveTrayAppFileVersion());

    public static PowerSchemeUserPreferences Load(
        string localAppDataPath,
        string companyFolderName,
        string? fileVersion)
    {
        var configPath = FindBestUserConfig(
            localAppDataPath,
            companyFolderName,
            fileVersion);
        if (configPath is null)
        {
            return new PowerSchemeUserPreferences(
                [],
                [],
                cycleOnlyVisible: false,
                sourceConfigPath: null);
        }

        return LoadFromUserConfig(configPath);
    }

    public static PowerSchemeUserPreferences LoadFromUserConfig(string userConfigPath)
    {
        var visibility = new Dictionary<Guid, bool>();
        var orderByGuid = new Dictionary<Guid, int>();
        var cycleOnlyVisible = false;

        try
        {
            var document = XDocument.Load(userConfigPath);
            foreach (var setting in document.Descendants("setting"))
            {
                var name = (string?)setting.Attribute("name");
                var value = setting.Element("value")?.Value;
                if (string.IsNullOrWhiteSpace(name) || value is null)
                {
                    continue;
                }

                if (string.Equals(name, "CycleOnlyVisible", StringComparison.Ordinal))
                {
                    _ = bool.TryParse(value, out cycleOnlyVisible);
                    continue;
                }

                if (!string.Equals(name, "PowerSchemeSettings", StringComparison.Ordinal)
                    || string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                try
                {
                    using var documentJson = JsonDocument.Parse(value);
                    foreach (var property in documentJson.RootElement.EnumerateObject())
                    {
                        if (!Guid.TryParse(property.Name, out var guid)
                            || property.Value.ValueKind != JsonValueKind.Object)
                        {
                            continue;
                        }

                        if (property.Value.TryGetProperty("Visible", out var visibleProperty)
                            && (visibleProperty.ValueKind is JsonValueKind.True or JsonValueKind.False))
                        {
                            visibility[guid] = visibleProperty.GetBoolean();
                        }
                        else
                        {
                            visibility[guid] = true;
                        }

                        if (property.Value.TryGetProperty("Order", out var orderProperty)
                            && orderProperty.ValueKind == JsonValueKind.Number
                            && orderProperty.TryGetInt32(out var order))
                        {
                            orderByGuid[guid] = order;
                        }
                    }
                }
                catch (JsonException)
                {
                    // Keep CycleOnlyVisible even if icon-heavy JSON cannot be parsed.
                }
            }
        }
        catch (Exception)
        {
            return new PowerSchemeUserPreferences(
                [],
                [],
                cycleOnlyVisible: false,
                sourceConfigPath: userConfigPath);
        }

        return new PowerSchemeUserPreferences(
            visibility,
            orderByGuid,
            cycleOnlyVisible,
            userConfigPath);
    }

    /// <summary>
    /// Matches tray UX: missing setting means the plan is visible.
    /// </summary>
    public bool IsVisible(Guid schemeGuid) =>
        !visibilityByGuid.TryGetValue(schemeGuid, out var visible) || visible;

    public int? GetOrder(Guid schemeGuid) =>
        orderByGuid.TryGetValue(schemeGuid, out var order) ? order : null;

    public static string ToSettingsCompanyFolderName(string companyName)
    {
        if (string.IsNullOrWhiteSpace(companyName))
        {
            companyName = DefaultCompanyName;
        }

        var invalid = Path.GetInvalidFileNameChars();
        var chars = companyName
            .Select(ch => ch is ' ' || Array.IndexOf(invalid, ch) >= 0 ? '_' : ch)
            .ToArray();
        return new string(chars);
    }

    public static string ResolveCompanyFolderName()
    {
        // Settings always live under the tray app company folder. Prefer the
        // sibling PowerPlanSwitcher.exe metadata, otherwise the known default.
        var company = TryGetTrayAppCompanyName() ?? DefaultCompanyName;
        return ToSettingsCompanyFolderName(company);
    }

    public static string? ResolveTrayAppFileVersion()
    {
        var trayExe = FindTrayAppExecutablePath();
        if (trayExe is not null)
        {
            var version = FileVersionInfo.GetVersionInfo(trayExe).FileVersion;
            if (!string.IsNullOrWhiteSpace(version))
            {
                return version;
            }
        }

        var entry = Assembly.GetEntryAssembly();
        var informational = entry
            ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;
        if (!string.IsNullOrWhiteSpace(informational))
        {
            // "1.2.10-rc1+commit" -> prefer FileVersion-style when possible
            var fileVersion = entry?.GetName().Version?.ToString();
            if (!string.IsNullOrWhiteSpace(fileVersion))
            {
                return fileVersion;
            }
        }

        return entry?.GetName().Version?.ToString();
    }

    private static string? FindBestUserConfig(
        string localAppDataPath,
        string companyFolderName,
        string? fileVersion)
    {
        if (!Directory.Exists(localAppDataPath))
        {
            return null;
        }

        var companyPath = Path.Combine(localAppDataPath, companyFolderName);
        if (!Directory.Exists(companyPath))
        {
            return null;
        }

        var candidates = new List<ConfigCandidate>();

        foreach (var appDirectory in FindPowerPlanSwitcherDirectories(companyPath))
        {
            foreach (var configPath in EnumerateConfigsInAppDirectory(appDirectory))
            {
                if (!TryCreateCandidate(configPath, out var candidate))
                {
                    continue;
                }

                candidates.Add(candidate);
            }
        }

        if (candidates.Count == 0)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(fileVersion))
        {
            var versionMatches = candidates
                .Where(candidate => string.Equals(
                    candidate.VersionFolderName,
                    fileVersion,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (versionMatches.Count > 0)
            {
                return versionMatches
                    .OrderByDescending(candidate => candidate.LastWriteTimeUtc)
                    .Select(candidate => candidate.Path)
                    .First();
            }
        }

        return candidates
            .OrderByDescending(candidate => candidate.LastWriteTimeUtc)
            .Select(candidate => candidate.Path)
            .First();
    }

    private static bool TryCreateCandidate(string configPath, out ConfigCandidate candidate)
    {
        candidate = default!;
        try
        {
            var info = new FileInfo(configPath);
            if (!info.Exists)
            {
                return false;
            }

            var versionFolderName = info.Directory?.Name;
            candidate = new ConfigCandidate(
                info.FullName,
                versionFolderName,
                info.LastWriteTimeUtc);
            return true;
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException or IOException)
        {
            return false;
        }
    }

    private static IEnumerable<string> EnumerateConfigsInAppDirectory(string appDirectory)
    {
        var directConfig = Path.Combine(appDirectory, "user.config");
        if (File.Exists(directConfig))
        {
            yield return directConfig;
        }

        foreach (var versionDirectory in EnumerateDirectoriesSafe(appDirectory))
        {
            var configPath = Path.Combine(versionDirectory, "user.config");
            if (File.Exists(configPath))
            {
                yield return configPath;
            }
        }
    }

    private static IEnumerable<string> FindPowerPlanSwitcherDirectories(string companyPath)
    {
        // %LocalAppData%\Company\PowerPlanSwitcher*
        foreach (var appDirectory in EnumerateDirectoriesSafe(
                     companyPath,
                     $"*{AppSettingsName}*"))
        {
            yield return appDirectory;
        }

        // %LocalAppData%\Company\New folder\PowerPlanSwitcher*
        foreach (var midDirectory in EnumerateDirectoriesSafe(companyPath))
        {
            if (Path.GetFileName(midDirectory)
                .Contains(AppSettingsName, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            foreach (var appDirectory in EnumerateDirectoriesSafe(
                         midDirectory,
                         $"*{AppSettingsName}*"))
            {
                yield return appDirectory;
            }
        }
    }

    private static IEnumerable<string> EnumerateDirectoriesSafe(
        string path,
        string searchPattern = "*")
    {
        try
        {
            return Directory.EnumerateDirectories(path, searchPattern);
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException or DirectoryNotFoundException or IOException)
        {
            return [];
        }
    }

    private static string? FindTrayAppExecutablePath()
    {
        var baseDirectory = AppContext.BaseDirectory;
        var trayExe = Path.Combine(baseDirectory, $"{AppSettingsName}.exe");
        return File.Exists(trayExe) ? trayExe : null;
    }

    private static string? TryGetTrayAppCompanyName()
    {
        var trayExe = FindTrayAppExecutablePath();
        if (trayExe is null)
        {
            return null;
        }

        var company = FileVersionInfo.GetVersionInfo(trayExe).CompanyName;
        return string.IsNullOrWhiteSpace(company) ? null : company;
    }

    private sealed record ConfigCandidate(
        string Path,
        string? VersionFolderName,
        DateTime LastWriteTimeUtc);
}
