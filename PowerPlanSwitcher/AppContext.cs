namespace PowerPlanSwitcher
{
    using System.ComponentModel;
    using System.Configuration;
    using PowerPlanSwitcher.PowerManagement;
    using PowerPlanSwitcher.ProcessManagement;
    using PowerPlanSwitcher.Properties;
    using PowerPlanSwitcher.RuleManagement;
    using PowerPlanSwitcher.RuleManagement.Rules;
    using Serilog;

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

            powerManager.ActivePowerSchemeChanged += (s, e) =>
                Log.Information(
                    "System activated power scheme: {PowerSchemeName} {PowerSchemeGuid}",
                    powerManager.GetPowerSchemeName(e.ActiveSchemeGuid) ?? "<No Name>",
                    e.ActiveSchemeGuid);

            ruleManager = new(powerManager)
            {
                BatteryMonitor = batteryMonitor,
                ProcessMonitor = processMonitor
            };
            ruleManager.RuleApplicationChanged +=
                RuleManager_RuleApplicationChanged;
            ruleManager.StartEngine(Rules.GetRules());

            // ProcessMonitor has to be started after RuleManager was started.
            // Otherweise events might be missed
            processMonitor.StartMonitoring();

            Settings.Default.PropertyChanged +=
                Default_PropertyChanged;
            Settings.Default.SettingChanging +=
                Default_SettingChanging;
        }

        private void Default_SettingChanging(
            object sender,
            SettingChangingEventArgs e)
        {
            if (Equals(Settings.Default[e.SettingName], e.NewValue))
            {
                return;
            }

            Log.Information(
                "Setting changing: {SettingName} Old Value: {OldValue} New Value: {NewValue}",
                e.SettingName, Settings.Default[e.SettingName], e.NewValue);
        }

        private void Default_PropertyChanged(
            object? sender,
            PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Rules")
            {
                return;
            }

            ruleManager?.StartEngine(Rules.GetRules());
        }

        private void RuleManager_RuleApplicationChanged(
            object? sender,
            RuleApplicationChangedEventArgs e)
        {
            if (e.PowerSchemeGuid == powerManager.GetActivePowerSchemeGuid())
            {
                return;
            }

            Log.Information(
                "Activating power scheme: {PowerSchemeName} " +
                "{PowerSchemeGuid} Reason: {Reason}",
                PowerManager.Static.GetPowerSchemeName(
                    e.PowerSchemeGuid) ?? "<No Name>",
                e.PowerSchemeGuid,
                e.Reason ?? "<Missing Reason>");
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
