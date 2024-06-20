namespace PowerPlanSwitcher
{
    internal class AppContext : ApplicationContext
    {
        private readonly PowerManager powerManager = new();
        private readonly ProcessMonitor processMonitor = new();
        private readonly TrayIcon trayIcon = new();

        public AppContext()
        {
            ToastDlg.Initialize();

            powerManager.ActivePowerSchemeChanged +=
                (s, e) => trayIcon.UpdateIcon(e.ActiveSchemeGuid);

            processMonitor.StartMonitoring();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                powerManager.Dispose();
                processMonitor.Dispose();
                trayIcon.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
