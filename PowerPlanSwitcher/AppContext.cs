namespace PowerPlanSwitcher;

using System.Configuration;
using System.Threading;
using PowerManagement;
using PowerPlanSwitcher.Properties;
using ProcessManagement;
using RuleManagement;
using RuleManagement.Events;
using Serilog;
using SystemManagement;

internal class AppContext : ApplicationContext
{
    private IPowerManager PowerManager { get; init; }
    private TrayIcon TrayIcon { get; init; }
    private SynchronizationContext UiSynchronizationContext { get; }
    private Guid BaselineSchemeGuid { get; set; }
    private Guid LastHandledSystemSchemeGuid { get; set; }

    public AppContext(
        TrayIcon trayIcon,
        IPowerManager powerManager,
        RuleManager ruleManager,
        IProcessMonitor processMonitor,
        IIdleMonitor idleMonitor,
        IWindowMessageMonitor windowMessageMonitor)
    {
        TrayIcon = trayIcon;
        PowerManager = powerManager;
        UiSynchronizationContext =
            SynchronizationContext.Current
            ?? new WindowsFormsSynchronizationContext();

        BaselineSchemeGuid = powerManager.GetActivePowerSchemeGuid();
        LastHandledSystemSchemeGuid = BaselineSchemeGuid;

        ToastDlg.Initialize();

        powerManager.ActivePowerSchemeChanged += (s, e) =>
            UiSynchronizationContext.Post(
                _ => HandleSystemPowerSchemeChanged(ruleManager, e.ActiveSchemeGuid),
                null);

        ruleManager.RuleApplicationChanged +=
            RuleManager_RuleApplicationChanged;

        ruleManager.RuleApplicationChanged += (s, e) =>
            trayIcon.UpdateTooltip(e.Rule);

        Settings.Default.SettingChanging +=
            Default_SettingChanging;

        processMonitor.StartMonitoring();
        idleMonitor.StartMonitoring();
        windowMessageMonitor.StartMonitoring();
        ruleManager.StartMonitoring();
    }

    private void HandleSystemPowerSchemeChanged(
        RuleManager ruleManager,
        Guid activeSchemeGuid)
    {
        if (activeSchemeGuid == LastHandledSystemSchemeGuid)
        {
            return;
        }

        try
        {
            TrayIcon.UpdateIcon(activeSchemeGuid);
        }
        catch (ObjectDisposedException)
        {
            return;
        }

        LastHandledSystemSchemeGuid = activeSchemeGuid;

        Log.ForContext("EventType", "PowerScheme.SystemChanged")
            .Information(
            "System activated power scheme: {PowerSchemeGuid}",
            activeSchemeGuid);

        if (ruleManager.AppliedRule is not null)
        {
            return;
        }

        if (activeSchemeGuid == Guid.Empty)
        {
            return;
        }

        BaselineSchemeGuid = activeSchemeGuid;
    }

    private void Default_SettingChanging(
        object sender,
        SettingChangingEventArgs e)
    {
        if (Equals(Settings.Default[e.SettingName], e.NewValue))
        {
            return;
        }

        Log.ForContext("EventType", "Settings.Changed")
            .Information(
            "Setting changing: {SettingName} Old Value: {OldValue} New Value: {NewValue}",
            e.SettingName, Settings.Default[e.SettingName], e.NewValue);
    }

    private void RuleManager_RuleApplicationChanged(
        object? sender,
        RuleApplicationChangedEventArgs e)
    {
        if (e.Rule is null)
        {
            var baselineSchemeName =
                PowerManagement.PowerManager.Api.GetPowerSchemeName(
                    BaselineSchemeGuid)
                ?? "<No Name>";

            Log.ForContext("EventType", "PowerScheme.ActivationRequested")
                .Information(
                "Activating power scheme: {PowerSchemeName} " +
                "'{PowerSchemeGuid}' Reason: {Reason}",
                baselineSchemeName,
                BaselineSchemeGuid,
                "Restore baseline");
            _ = PowerManager.SetActivePowerSchemeAsync(BaselineSchemeGuid);
            return;
        }

        var schemeGuid = e.Rule.Dto.SchemeGuid;
        if (schemeGuid == PowerManager.GetActivePowerSchemeGuid())
        {
            return;
        }

        var schemeName =
            PowerManagement.PowerManager.Api.GetPowerSchemeName(
                schemeGuid)
            ?? "<No Name>";

        var reason = "Rule applied";

        Log.ForContext("EventType", "PowerScheme.ActivationRequested")
            .Information(
            "Activating power scheme: {PowerSchemeName} " +
            "'{PowerSchemeGuid}' Reason: {Reason} Rule: {RuleDescription}",
            schemeName,
            schemeGuid,
            reason,
            e.Rule.Dto.GetDescription());
        _ = PowerManager.SetActivePowerSchemeAsync(schemeGuid);

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

