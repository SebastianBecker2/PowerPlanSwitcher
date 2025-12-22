namespace PowerPlanSwitcher;

using System.Configuration;
using PowerManagement;
using PowerPlanSwitcher.Properties;
using RuleManagement;
using Serilog;

internal class AppContext : ApplicationContext
{
    private IPowerManager PowerManager { get; init; }
    private TrayIcon TrayIcon { get; init; }
    private Guid BaselineSchemeGuid { get; set; }

    public AppContext(
        TrayIcon trayIcon,
        IPowerManager powerManager,
        RuleManager ruleManager)
    {
        TrayIcon = trayIcon;
        PowerManager = powerManager;

        BaselineSchemeGuid = powerManager.GetActivePowerSchemeGuid();

        ToastDlg.Initialize();

        powerManager.ActivePowerSchemeChanged +=
            (s, e) => trayIcon.UpdateIcon(e.ActiveSchemeGuid);

        powerManager.ActivePowerSchemeChanged += (s, e) =>
            Log.Information(
                "System activated power scheme: {PowerSchemeName} {PowerSchemeGuid}",
                powerManager.GetPowerSchemeName(e.ActiveSchemeGuid) ?? "<No Name>",
                e.ActiveSchemeGuid);

        powerManager.ActivePowerSchemeChanged += (s, e) =>
        {
            if (ruleManager.AppliedRule is not null)
            {
                return;
            }

            if (e.ActiveSchemeGuid == Guid.Empty)
            {
                return;
            }

            BaselineSchemeGuid = e.ActiveSchemeGuid;
        };

        ruleManager.RuleApplicationChanged +=
            RuleManager_RuleApplicationChanged;

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

    private void RuleManager_RuleApplicationChanged(
        object? sender,
        RuleApplicationChangedEventArgs e)
    {
        if (e.Rule is null)
        {
            Log.Information(
                "Activating power scheme: {PowerSchemeName} " +
                "{PowerSchemeGuid} Reason: {Reason}",
                "<No Name>",
                BaselineSchemeGuid,
                "Restore baseline");
            PowerManager.SetActivePowerScheme(BaselineSchemeGuid);
            return;
        }

        var schemeGuid = e.Rule.Dto.SchemeGuid;
        if (schemeGuid == PowerManager.GetActivePowerSchemeGuid())
        {
            return;
        }

        var schemeName =
            PowerManagement.PowerManager.Static.GetPowerSchemeName(
                schemeGuid)
            ?? "<No Name>";

        var reason = "Rule applied";

        Log.Information(
            "Activating power scheme: {PowerSchemeName} " +
            "{PowerSchemeGuid} Reason: {Reason}",
            schemeName,
            schemeGuid,
            reason);
        PowerManager.SetActivePowerScheme(schemeGuid);

        if (PopUpWindowLocationHelper.ShouldShowToast(reason))
        {
            ToastDlg.ShowToastNotification(schemeGuid, reason);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TrayIcon.Dispose();
        }
        base.Dispose(disposing);
    }
}
