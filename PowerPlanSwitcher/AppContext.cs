namespace PowerPlanSwitcher
{
    internal class AppContext : ApplicationContext
    {
        private readonly PowerManager powerManager = new();
        private readonly RuleManager ruleManager = new();
        private readonly TrayIcon trayIcon = new();

        public AppContext()
        {
            ToastDlg.Initialize();

            powerManager.ActivePowerSchemeChanged +=
                (s, e) => trayIcon.UpdateIcon(e.ActiveSchemeGuid);

            ruleManager.StartEngine();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                powerManager.Dispose();
                ruleManager.Dispose();
                trayIcon.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
