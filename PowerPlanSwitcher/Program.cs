namespace PowerPlanSwitcher;

using System.Diagnostics;
using System.Drawing;
using Autofac;
using Hotkeys;
using Newtonsoft.Json;
using PowerManagement;
using ProcessManagement;
using Properties;
using RuleManagement;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Json;
using SevenZip;
using SystemManagement;

internal static class Program
{
    private static readonly string AssemblyTitle =
        AboutBox.AssemblyTitle ?? "";
    private static readonly string LogFileName =
#if DEBUG
        $"{AssemblyTitle}.debug-.log";
#else
        $"{AssemblyTitle}-.log";
#endif
    private static readonly string LogFileNamePattern =
        $"{AssemblyTitle}*.log";
    private static readonly string LocalAppDataPath =
        Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData);
    private static readonly string LogPath =
        Path.Combine(LocalAppDataPath, AssemblyTitle);
    private static readonly string LogFilePath =
        Path.Combine(LogPath, LogFileName);
    private static readonly string ZipLibraryPath =
        Path.Combine("Resources", "7z.dll");

    private static LoggingLevelSwitch LogLevelSwitch { get; } =
        new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Fatal);

    private static Hotkey? CycleHotkey { get; set; }
    private static Dictionary<(Keys key, ModifierKeys modifier), (Guid guid, string? name)>
        DirectPowerSchemeHotkeys { get; } = [];

    public static void UpdateLogLevelSwitch(bool useExtendedLogging) =>
        LogLevelSwitch.MinimumLevel = useExtendedLogging
            ? Serilog.Events.LogEventLevel.Verbose
            : Serilog.Events.LogEventLevel.Fatal;

    public static void OpenLogPath()
    {
        if (!Directory.Exists(LogPath))
        {
            _ = Directory.CreateDirectory(LogPath);
        }
        try
        {
            _ = System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = LogPath,
                UseShellExecute = true,
                Verb = "open"
            });
        }
        catch (Exception ex)
        {
            _ = MessageBox.Show(
                $"Failed to open log path: {LogPath}",
                "Open log path",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            Log.Error(ex, "Failed to open log path: {LogPath}", LogPath);
        }
    }

    public static void ExportLog()
    {
        if (!Directory.Exists(LogPath))
        {
            Log.Error("Log path does not exist: {LogPath}", LogPath);
            return;
        }
        try
        {
            using var saveAsDlg = new SaveFileDialog()
            {
                Title = "Export Log",
                Filter = "7zip Files (*.7z)|*.7z",
                InitialDirectory = LogPath,
                FileName = $"{AssemblyTitle}.log.7z"
            };
            if (saveAsDlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using var passwordDlg = new CreatePasswordDlg();
            if (passwordDlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            SevenZipCompressor compressor = new()
            {
                CompressionLevel = CompressionLevel.Ultra,
                ArchiveFormat = OutArchiveFormat.SevenZip,
                CompressionMethod = CompressionMethod.BZip2,
                EncryptHeaders = true
            };

            var logFiles = Directory.GetFiles(LogPath, LogFileNamePattern);

            if (string.IsNullOrWhiteSpace(passwordDlg.Password))
            {
                compressor.CompressFiles(
                    saveAsDlg.FileName,
                    logFiles);
            }
            else
            {
                compressor.CompressFilesEncrypted(
                   saveAsDlg.FileName,
                   passwordDlg.Password,
                   logFiles);
            }

            Log.Information(
                "Log exported to: {ExportFilePath}",
                saveAsDlg.FileName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to export log.");
        }
    }

    public static void RegisterHotkeys(HotkeyManager hotkeyManager)
    {
        DirectPowerSchemeHotkeys.Clear();

        CycleHotkey = JsonConvert.DeserializeObject<Hotkey>(
            Settings.Default.CyclePowerSchemeHotkey);
        if (CycleHotkey is not null)
        {
            _ = hotkeyManager.AddHotkey(
                CycleHotkey.Key,
                CycleHotkey.Modifier);
        }

        foreach (var hotkey in PowerManager.Static.GetPowerSchemes()
            .Select(ps =>
            {
                var hotkey = PowerSchemeSettings.GetSetting(ps.guid)?.Hotkey;
                return (ps.guid, ps.name, hotkey);
            })
            .Where(x => x.hotkey is not null))
        {
            _ = hotkeyManager.AddHotkey(hotkey.hotkey!.Key, hotkey.hotkey.Modifier);

            var key = (hotkey.hotkey.Key, hotkey.hotkey.Modifier);
            if (!DirectPowerSchemeHotkeys.ContainsKey(key))
            {
                DirectPowerSchemeHotkeys[key] = (hotkey.guid, hotkey.name);
            }
        }
    }

    private static void LogDpiEnvironment()
    {
        try
        {
            using var graphics = Graphics.FromHwnd(IntPtr.Zero);
            var scaleX = graphics.DpiX / 96f;
            var scaleY = graphics.DpiY / 96f;
            Log.Information(
                "DPI environment initialized. HighDpiMode: {HighDpiMode}, " +
                "DpiX: {DpiX}, DpiY: {DpiY}, ScaleX: {ScaleX}, ScaleY: {ScaleY}",
                Application.HighDpiMode,
                graphics.DpiX,
                graphics.DpiY,
                scaleX,
                scaleY);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to capture DPI environment information.");
        }
    }

    private static bool AreHotkeyEqual(
        HotkeyPressedEventArgs left,
        Hotkey right) =>
        left.PressedKey == right.Key
        && left.ModifierKeys == right.Modifier;

    private static void HotkeyManager_HotkeyPressed(
        object? sender,
        HotkeyPressedEventArgs e)
    {
        CycleHotkey ??= JsonConvert.DeserializeObject<Hotkey>(
            Settings.Default.CyclePowerSchemeHotkey);

        if (CycleHotkey is not null && AreHotkeyEqual(e, CycleHotkey))
        {
            HandleCycleHotkeyPressed();
            return;
        }

        HandlePowerSchemeHotkeyPressed(e);
    }

    private static void HandleCycleHotkeyPressed()
    {
        var schemes = PowerManager.Static.GetPowerSchemeGuids()
            .Where(ps => !Settings.Default.CycleOnlyVisible
                || (PowerSchemeSettings.GetSetting(ps)?.Visible ?? false))
            .ToList();

        if (schemes.Count == 0)
        {
            Log.Warning("Cycle hotkey ignored because no eligible power schemes were found.");
            return;
        }

        var active = PowerManager.Static.GetActivePowerSchemeGuid();
        var index = GetNextCycleSchemeIndex(schemes, active);
        if (index < 0)
        {
            Log.Warning("Cycle hotkey ignored because no next scheme index could be selected.");
            return;
        }

        Log.Information(
            "Activating power scheme: {PowerSchemeName} " +
            "{PowerSchemeGuid} Reason: Cycle Hotkey",
            PowerManager.Static.GetPowerSchemeName(schemes[index]) ?? "<No Name>",
            schemes[index]);
        PowerManager.Static.SetActivePowerScheme(schemes[index]);
        if (PopUpWindowLocationHelper.ShouldShowToast("hotkey"))
        {
            ToastDlg.ShowToastNotification(
                schemes[index],
                "Cycle hotkey pressed");
        }
    }

    private static int GetNextCycleSchemeIndex(
        List<Guid> schemes,
        Guid activeSchemeGuid)
    {
        if (schemes.Count == 0)
        {
            return -1;
        }

        var index = activeSchemeGuid == Guid.Empty
            ? 0
            : schemes.IndexOf(activeSchemeGuid);

        return (index + 1) % schemes.Count;
    }

    private static void HandlePowerSchemeHotkeyPressed(
        HotkeyPressedEventArgs e)
    {
        if (!DirectPowerSchemeHotkeys.TryGetValue(
            (e.PressedKey, e.ModifierKeys),
            out var target))
        {
            return;
        }

        Log.Information(
            "Activating power scheme: {PowerSchemeName} " +
            "{PowerSchemeGuid} Reason: Direct Hotkey",
            target.name,
            target.guid);
        PowerManager.Static.SetActivePowerScheme(target.guid);
        ToastDlg.ShowToastNotification(target.guid, "Power Plan hotkey pressed");
    }

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        if (Settings.Default.UpgradeRequired)
        {
            Settings.Default.Upgrade();
            Settings.Default.UpgradeRequired = false;
            Settings.Default.Save();
        }

        UpdateLogLevelSwitch(Settings.Default.ExtendedLogging);
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(LogLevelSwitch)
            .WriteTo.File(
                path: LogFilePath,
                formatter: new JsonFormatter(),
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 500 * 1024 * 1024,
                retainedFileTimeLimit: TimeSpan.FromDays(7),
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
            .CreateLogger();

        Log.Information("Application started.");

        SevenZipBase.SetLibraryPath(ZipLibraryPath);

        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            Log.Fatal(e.ExceptionObject as Exception, "Unhandled Exception");

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        LogDpiEnvironment();

        var builder = new ContainerBuilder();
        _ = builder.RegisterInstance(Log.Logger)
            .As<ILogger>()
            .SingleInstance();

        _ = builder.RegisterType<BatteryMonitor>()
            .As<IBatteryMonitor>()
            .SingleInstance();
        _ = builder.RegisterType<ProcessMonitor>()
            .As<IProcessMonitor>()
            .WithParameter(
                "updateInterval",
                TimeSpan.FromMilliseconds(Settings.Default.ProcessMonitorIntervalInMilliseconds))
            .SingleInstance();
        _ = builder.RegisterType<IdleMonitor>()
            .As<IIdleMonitor>()
            .WithParameter(
                "pollingInterval",
                TimeSpan.FromMilliseconds(Settings.Default.IdleMonitorIntervalInMilliseconds))
            .SingleInstance();
        _ = builder.RegisterType<WindowMessageMonitor>()
            .As<IWindowMessageMonitor>()
            .WithParameter(
                "messages",
                new[]
                {
                    WindowMessage.QueryEndSession,
                    WindowMessage.EndSession,
                })
            .SingleInstance();
        _ = builder.RegisterType<SystemManager>()
            .As<ISystemManager>()
            .SingleInstance();
        _ = builder.RegisterType<PowerManager>()
            .As<IPowerManager>()
            .SingleInstance();
        _ = builder.RegisterType<RuleFactory>()
            .AsSelf()
            .SingleInstance();
        _ = builder.RegisterType<HotkeyManager>()
            .AsSelf()
            .SingleInstance();

        _ = builder.Register(c =>
        {
            var battery = c.Resolve<IBatteryMonitor>();
            var factory = c.Resolve<RuleFactory>();

            var ruleJson = Settings.Default.Rules;
            var migrationPolicy = new MigrationPolicy(
                MigratedPowerRulesToRules: Settings.Default.MigratedPowerRulesToRules,
                AcPowerSchemeGuid: Settings.Default.AcPowerSchemeGuid,
                BatteryPowerSchemeGuid: Settings.Default.BatterPowerSchemeGuid,
                MigratedStartupRule: Settings.Default.MigratedStartupRule,
                ActivateInitialPowerScheme: Settings.Default.ActivateInitialPowerScheme,
                InitialPowerSchemeGuid: Settings.Default.InitialPowerSchemeGuid);

            var ruleManager = new RuleManager(factory, ruleJson, migrationPolicy, battery);

            Log.Information(
                "Persisting migration flags. Previous values: MigratedPowerRulesToRules={MigratedPowerRulesToRules}, MigratedStartupRule={MigratedStartupRule}",
                Settings.Default.MigratedPowerRulesToRules,
                Settings.Default.MigratedStartupRule);

            Settings.Default.MigratedPowerRulesToRules = true;
            Settings.Default.MigratedStartupRule = true;

            try
            {
                Settings.Default.Save();
                Log.Information(
                    "Migration flags persisted successfully. Current values: MigratedPowerRulesToRules={MigratedPowerRulesToRules}, MigratedStartupRule={MigratedStartupRule}",
                    Settings.Default.MigratedPowerRulesToRules,
                    Settings.Default.MigratedStartupRule);
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "Failed to persist migration flags. Current values: MigratedPowerRulesToRules={MigratedPowerRulesToRules}, MigratedStartupRule={MigratedStartupRule}",
                    Settings.Default.MigratedPowerRulesToRules,
                    Settings.Default.MigratedStartupRule);
                throw;
            }

            return ruleManager;
        })
        .AsSelf()
        .SingleInstance();

        _ = builder.RegisterType<AppContext>()
            .AsSelf();
        _ = builder.RegisterType<SettingsDlg>();
        _ = builder.RegisterType<HotkeySelectionDlg>();
        _ = builder.RegisterType<ContextMenu>();
        _ = builder.RegisterType<TrayIcon>();

        var container = builder.Build();

        var ruleManager = container.Resolve<RuleManager>();
        ruleManager.RulesUpdated += (_, e) =>
        {
            Settings.Default.Rules = e.Serialized;
            Settings.Default.Save();
        };

        // Resolve AppContext and run
        using (var scope = container.BeginLifetimeScope())
        {
            var appContext = scope.Resolve<AppContext>();
            var hotkeyManager = scope.Resolve<HotkeyManager>();

            RegisterHotkeys(hotkeyManager); // still uses static Program.HotkeyManager

            hotkeyManager.HotkeyPressed += HotkeyManager_HotkeyPressed;

            Microsoft.Win32.SystemEvents.EventsThreadShutdown += (s, e) =>
                Application.Exit();

            Application.Run(appContext);
        }

        Log.Information("Application exited gracefully.");
    }
}
