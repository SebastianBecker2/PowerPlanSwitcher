namespace PowerPlanSwitcher
{
    using Microsoft.Win32;
    using Hotkeys;
    using Properties;
    using Serilog;

    internal static class Program
    {
        public static readonly HotkeyManager HotkeyManager = new();

        private static readonly string AssemblyTitle =
            AboutBox.AssemblyTitle ?? "";
        private static readonly string LogFileName =
            $"{AssemblyTitle}.log";
        private static readonly string LocalAppDataPath =
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);
        private static readonly string LogPath =
            Path.Combine(LocalAppDataPath, AssemblyTitle, LogFileName);

        public static void RegisterHotkeys()
        {
            foreach (var hotkey in PowerManager.GetPowerSchemes()
                .Select(ps => PowerSchemeSettings.GetSetting(ps.guid)?.Hotkey)
                .Where(h => h is not null))
            {
                _ = HotkeyManager.AddHotkey(hotkey!.Key, hotkey!.Modifier);
            }
        }

        private static void HotkeyManager_HotkeyPressed(
            object? sender,
            HotkeyPressedEventArgs e)
        {
            var (guid, _) = PowerManager.GetPowerSchemes()
                .FirstOrDefault(ps =>
                {
                    var setting = PowerSchemeSettings.GetSetting(ps.guid);
                    if (setting?.Hotkey is null)
                    {
                        return false;
                    }
                    var res = setting.Hotkey.Key == e.PressedKey
                        && setting.Hotkey.Modifier == e.ModifierKeys;
                    return res;
                });

            if (guid == Guid.Empty)
            {
                return;
            }

            PowerManager.SetActivePowerScheme(guid);
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            HotKey.HotKeyGuid();
            // 初始化HotKey类并开始监听热键
            HotKey.StartListening();

            // 保持程序运行以监听热键
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
#pragma warning disable CA1305 // Specify IFormatProvider
                using var log = new LoggerConfiguration()
                    .WriteTo.File(LogPath)
                    .CreateLogger();
#pragma warning restore CA1305 // Specify IFormatProvider
                log.Fatal(e.ExceptionObject as Exception, "Unhandled Exception");
            };

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            if (Settings.Default.ActivateInitialPowerScheme &&
                Settings.Default.InitialPowerSchemeGuid != Guid.Empty)
            {
                PowerManager.SetActivePowerScheme(
                    Settings.Default.InitialPowerSchemeGuid);
            }

            RegisterHotkeys();

            HotkeyManager.HotkeyPressed += HotkeyManager_HotkeyPressed;

            using var trayIcon = new TrayIcon();
            SystemEvents.EventsThreadShutdown += (s, e) => Application.Exit();
            Application.Run();
        }
    }
}
