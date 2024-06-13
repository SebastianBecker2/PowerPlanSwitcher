namespace PowerPlanSwitcher
{
    using Microsoft.Win32;
    using Hotkeys;
    using Properties;
    using Serilog;
    using Newtonsoft.Json;

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

        private static ToastDlg? toastDlg;
        private static readonly object ToastDlgLock = new();

        public static void ShowToastNotification(
            Guid activeSchemeGuid,
            string activationReason)
        {
            var t = new Task(() =>
            {
                if (toastDlg is not null)
                {
                    try
                    {
                        toastDlg?.Invoke(new Action(() =>
                            toastDlg.DialogResult = DialogResult.OK));
                    }
                    catch (ObjectDisposedException)
                    {

                    }
                }

                lock (ToastDlgLock)
                {
                    toastDlg = new ToastDlg
                    {
                        PowerSchemeName =
                            PowerManager.GetPowerSchemeName(activeSchemeGuid)
                            ?? "",
                        PowerSchemeIcon =
                            PowerSchemeSettings.GetSetting(activeSchemeGuid)
                            ?.Icon,
                        Reason = activationReason,
                    };

                    _ = toastDlg.ShowDialog();
                    toastDlg.Dispose();
                    toastDlg = null;
                }
            });
            t.Start();
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
            var cycleHotkey = JsonConvert.DeserializeObject<Hotkey>(
                Settings.Default.CyclePowerSchemeHotkey);
            if (cycleHotkey?.Key == e.PressedKey
                && cycleHotkey?.Modifier == e.ModifierKeys)
            {
                var schemes = PowerManager.GetPowerSchemeGuids()
                    .Where(ps => !Settings.Default.CycleOnlyVisible
                        || (PowerSchemeSettings.GetSetting(ps)?.Visible ?? false))
                    .ToList();
                var index = schemes.IndexOf(
                    PowerManager.GetActivePowerSchemeGuid());
                index = (index + 1) % schemes.Count;
                PowerManager.SetActivePowerScheme(schemes[index]);
                ShowToastNotification(schemes[index], "Cycle hotkey pressed");
                return;
            }

            var (guid, _) = PowerManager.GetPowerSchemes()
            .FirstOrDefault(ps =>
            {
                var setting = PowerSchemeSettings.GetSetting(ps.guid);
                return setting?.Hotkey?.Key == e.PressedKey
                    && setting?.Hotkey?.Modifier == e.ModifierKeys;
            });

            if (guid == Guid.Empty)
            {
                return;
            }

            PowerManager.SetActivePowerScheme(guid);
            ShowToastNotification(guid, "Power Plan hotkey pressed");
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
