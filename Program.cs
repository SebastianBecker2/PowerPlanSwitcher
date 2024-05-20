namespace PowerPlanSwitcher
{
    using Microsoft.Win32;
    using Properties;
    using Serilog;

    internal static class Program
    {
        private static readonly string AssemblyTitle =
            AboutBox.AssemblyTitle ?? "";
        private static readonly string LogFileName =
            $"{AssemblyTitle}.log";
        private static readonly string LocalAppDataPath =
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);
        private static readonly string LogPath =
            Path.Combine(LocalAppDataPath, AssemblyTitle, LogFileName);

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

            using var trayIcon = new TrayIcon();
            SystemEvents.EventsThreadShutdown += (s, e) => Application.Exit();
            Application.Run();
        }
    }
}
