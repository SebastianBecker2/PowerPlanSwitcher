namespace PowerPlanSwitcher;

using System.Diagnostics;
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
        var cycleHotkey = JsonConvert.DeserializeObject<Hotkey>(
            Settings.Default.CyclePowerSchemeHotkey);
        if (cycleHotkey is not null)
        {
            _ = hotkeyManager.AddHotkey(
                cycleHotkey.Key,
                cycleHotkey.Modifier);
        }

        foreach (var hotkey in PowerManager.Static.GetPowerSchemes()
            .Select(ps => PowerSchemeSettings.GetSetting(ps.guid)?.Hotkey)
            .Where(h => h is not null))
        {
            _ = hotkeyManager.AddHotkey(hotkey!.Key, hotkey!.Modifier);
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
        var cycleHotkey = JsonConvert.DeserializeObject<Hotkey>(
            Settings.Default.CyclePowerSchemeHotkey);

        if (cycleHotkey is not null && AreHotkeyEqual(e, cycleHotkey))
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

        var active = PowerManager.Static.GetActivePowerSchemeGuid();

        var index = active == Guid.Empty ? 0 : schemes.IndexOf(active);
        index = (index + 1) % schemes.Count;

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

    private static void HandlePowerSchemeHotkeyPressed(
        HotkeyPressedEventArgs e)
    {
        var (guid, name) = PowerManager.Static.GetPowerSchemes()
            .FirstOrDefault(ps =>
            {
                var setting = PowerSchemeSettings.GetSetting(ps.guid);
                if (setting is null || setting.Hotkey is null)
                {
                    return false;
                }
                return AreHotkeyEqual(e, setting.Hotkey);
            });

        if (guid == Guid.Empty)
        {
            return;
        }

        Log.Information(
            "Activating power scheme: {PowerSchemeName} " +
            "{PowerSchemeGuid} Reason: Direct Hotkey",
            name,
            guid);
        PowerManager.Static.SetActivePowerScheme(guid);
        ToastDlg.ShowToastNotification(guid, "Power Plan hotkey pressed");
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
                BatterPowerSchemeGuid: Settings.Default.BatterPowerSchemeGuid,
                MigratedStartupRule: Settings.Default.MigratedStartupRule,
                ActivateInitialPowerScheme: Settings.Default.ActivateInitialPowerScheme,
                InitialPowerSchemeGuid: Settings.Default.InitialPowerSchemeGuid);

            var ruleManager = new RuleManager(factory, ruleJson, migrationPolicy, battery);

            Settings.Default.MigratedPowerRulesToRules = true;
            Settings.Default.MigratedStartupRule = true;
            Settings.Default.Save();

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
