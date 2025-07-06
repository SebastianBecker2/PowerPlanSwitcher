namespace PowerPlanSwitcher
{
    using Hotkeys;
    using Properties;
    using Serilog;
    using Newtonsoft.Json;
    using PowerPlanSwitcher.PowerManagement;
    using Serilog.Formatting.Json;
    using Serilog.Core;
    using System.Diagnostics;
    using SevenZip;

    internal static class Program
    {
        public static readonly HotkeyManager HotkeyManager = new();

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
                _ = Process.Start(new ProcessStartInfo
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

        public static void RegisterHotkeys()
        {
            var cycleHotkey = JsonConvert.DeserializeObject<Hotkey>(
                Settings.Default.CyclePowerSchemeHotkey);
            if (cycleHotkey is not null)
            {
                _ = HotkeyManager.AddHotkey(
                    cycleHotkey.Key,
                    cycleHotkey.Modifier);
            }

            foreach (var hotkey in PowerManager.Static.GetPowerSchemes()
                .Select(ps => PowerSchemeSettings.GetSetting(ps.guid)?.Hotkey)
                .Where(h => h is not null))
            {
                _ = HotkeyManager.AddHotkey(hotkey!.Key, hotkey!.Modifier);
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
                "Activating power scheme: {SchemeGuid} Reason: Cycle Hotkey",
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
            var (guid, _) = PowerManager.Static.GetPowerSchemes()
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
                "Activating power scheme: {SchemeGuid} Reason: Direct Hotkey",
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
#pragma warning disable CA1305 // Specify IFormatProvider
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
#pragma warning restore CA1305 // Specify IFormatProvider

            Log.Information("Application started.");

            SevenZipBase.SetLibraryPath(ZipLibraryPath);

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                Log.Fatal(e.ExceptionObject as Exception, "Unhandled Exception");

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            if (Settings.Default.ActivateInitialPowerScheme &&
                Settings.Default.InitialPowerSchemeGuid != Guid.Empty)
            {
                Log.Information(
                    "Activating power scheme: {SchemeGuid} Reason: Initial Power Scheme",
                    Settings.Default.InitialPowerSchemeGuid);
                PowerManager.Static.SetActivePowerScheme(
                    Settings.Default.InitialPowerSchemeGuid);
            }

            RegisterHotkeys();

            HotkeyManager.HotkeyPressed += HotkeyManager_HotkeyPressed;

            Microsoft.Win32.SystemEvents.EventsThreadShutdown += (s, e) =>
                Application.Exit();

            Application.Run(new AppContext());

            Log.Information("Application exited gracefully.");
        }
    }
}
