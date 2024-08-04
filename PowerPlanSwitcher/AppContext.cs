namespace PowerPlanSwitcher
{
    using System.ComponentModel;
    using PowerPlanSwitcher.PowerManagement;
    using PowerPlanSwitcher.ProcessManagement;
    using PowerPlanSwitcher.Properties;
    using PowerPlanSwitcher.RuleManagement;

    internal class AppContext : ApplicationContext
    {
        private readonly PowerManager powerManager = new();
        private readonly BatteryMonitor batteryMonitor = new();
        private readonly ProcessMonitor processMonitor = new();
        private readonly RuleManager? ruleManager;
        private readonly TrayIcon trayIcon = new();

        public AppContext()
        {
            ToastDlg.Initialize();

            powerManager.ActivePowerSchemeChanged +=
                (s, e) => trayIcon.UpdateIcon(e.ActiveSchemeGuid);

            ruleManager = new(powerManager)
            {
                BatteryMonitor = batteryMonitor,
                ProcessMonitor = processMonitor
            };
            ruleManager.RuleApplicationChanged +=
                RuleManager_RuleApplicationChanged;
            ruleManager.StartEngine(PowerRule.GetPowerRules());

            Settings.Default.PropertyChanged +=
                Default_PropertyChanged;
        }

        private void Default_PropertyChanged(
            object? sender,
            PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "PowerRules")
            {
                return;
            }

            ruleManager?.StartEngine(PowerRule.GetPowerRules());
        }

        private void RuleManager_RuleApplicationChanged(
            object? sender,
            RuleApplicationChangedEventArgs e)
        {
            if (e.PowerSchemeGuid == powerManager.GetActivePowerSchemeGuid())
            {
                return;
            }

            powerManager.SetActivePowerScheme(e.PowerSchemeGuid);

            if (e.Reason != null && PopUpWindowLocationHelper.ShouldShowToast(e.Reason))
            {
                ToastDlg.ShowToastNotification(e.PowerSchemeGuid, e.Reason);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ruleManager?.StopEngine();
                processMonitor.Dispose();
                powerManager.Dispose();
                trayIcon.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
