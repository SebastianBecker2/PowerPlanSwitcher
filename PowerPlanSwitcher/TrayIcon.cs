namespace PowerPlanSwitcher;

using PowerManagement;
using Properties;
using RuleManagement.Rules;
using Serilog;

internal class TrayIcon : IDisposable
{
    private static readonly Icon DefaultIcon =
        IconUtilities.CreateNotifyIcon(Resources.power_surge);

    private bool disposedValue;
    private Icon? currentCustomIcon;
    private readonly NotifyIcon notifyIcon = new()
    {
        Icon = DefaultIcon,
        Text = "PowerPlanSwitcher",
        Visible = true,
    };

    public TrayIcon(ContextMenu contextMenu)
    {
        notifyIcon.ContextMenuStrip = contextMenu;
        Settings.Default.PropertyChanged += (_, _) => UpdateIcon();

        notifyIcon.MouseClick += NotifyIcon_MouseClick;

        UpdateIcon();
        UpdateTooltip(null);
    }

    private void NotifyIcon_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        using var dlg = new PowerSchemeSelectorDlg();
        _ = dlg.ShowDialog();
    }

    public void UpdateIcon()
    {
        var activeSchemeGuid = PowerManager.Static.GetActivePowerSchemeGuid();
        if (activeSchemeGuid == Guid.Empty)
        {
            return;
        }
        UpdateIcon(activeSchemeGuid);
    }

    public void UpdateIcon(Guid powerSchemeGuid)
    {
        var setting = PowerSchemeSettings.GetSetting(powerSchemeGuid);
        if (setting?.Icon is null)
        {
            ApplyDefaultIcon();
            return;
        }

        try
        {
            var customIcon = IconUtilities.CreateNotifyIcon(setting.Icon);
            var previousCustomIcon = currentCustomIcon;
            currentCustomIcon = customIcon;
            notifyIcon.Icon = customIcon;
            previousCustomIcon?.Dispose();
        }
        catch (Exception ex)
        {
            Log.Warning(
                ex,
                "Failed to apply custom tray icon for power scheme {PowerSchemeGuid}. Falling back to default icon.",
                powerSchemeGuid);
            ApplyDefaultIcon();
        }
    }

    public void UpdateTooltip(IRule? rule)
    {
        var schemeName =
            PowerManager.Static.GetPowerSchemeName(
                rule?.Dto?.SchemeGuid
                ?? PowerManager.Static.GetActivePowerSchemeGuid())
            ?? "<No Name>";

        var tooltipText = $"PowerPlanSwitcher" +
            $"\nPowerPlan: {schemeName}" +
            $"\nRule: {rule?.Dto?.GetDescription() ?? "No rule active"}";
        notifyIcon.Text = TrimTooltip(tooltipText);

        static string TrimTooltip(string text)
        {
            const int max = 127;
            if (text.Length <= max)
            {
                return text;
            }

            return text[..(max - 3)] + "...";
        }
    }

    private void ApplyDefaultIcon()
    {
        currentCustomIcon?.Dispose();
        currentCustomIcon = null;
        notifyIcon.Icon = DefaultIcon;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposedValue)
        {
            return;
        }

        if (disposing)
        {
            currentCustomIcon?.Dispose();
            currentCustomIcon = null;
            notifyIcon.Dispose();
        }
        disposedValue = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
