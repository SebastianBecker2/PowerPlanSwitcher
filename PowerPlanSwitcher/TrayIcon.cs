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
    private readonly Dictionary<Guid, Icon> customIconCache = [];
    private Guid currentIconSchemeGuid = Guid.Empty;
    private readonly NotifyIcon notifyIcon = new()
    {
        Icon = DefaultIcon,
        Text = "PowerPlanSwitcher",
        Visible = true,
    };

    public TrayIcon(ContextMenu contextMenu)
    {
        notifyIcon.ContextMenuStrip = contextMenu;
        Settings.Default.PropertyChanged += (_, _) =>
        {
            InvalidateCustomIconCache();
            UpdateIcon();
        };

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
        if (currentIconSchemeGuid == powerSchemeGuid)
        {
            return;
        }

        var setting = PowerSchemeSettings.GetSetting(powerSchemeGuid);

        if (setting?.Icon is null)
        {
            ApplyDefaultIcon(powerSchemeGuid);
            return;
        }

        try
        {
            var cacheHit = customIconCache.TryGetValue(powerSchemeGuid, out var customIcon);
            if (!cacheHit)
            {
                customIcon = IconUtilities.CreateNotifyIcon(setting.Icon);
                customIconCache[powerSchemeGuid] = customIcon;
            }

            notifyIcon.Icon = customIcon;
            currentIconSchemeGuid = powerSchemeGuid;
        }
        catch (Exception ex)
        {
            Log.Warning(
                ex,
                "Failed to apply custom tray icon for power scheme {PowerSchemeGuid}. Falling back to default icon.",
                powerSchemeGuid);
            ApplyDefaultIcon(powerSchemeGuid);
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

    private void ApplyDefaultIcon(Guid powerSchemeGuid)
    {
        notifyIcon.Icon = DefaultIcon;
        currentIconSchemeGuid = powerSchemeGuid;
    }

    private void InvalidateCustomIconCache()
    {
        foreach (var icon in customIconCache.Values)
        {
            icon.Dispose();
        }

        customIconCache.Clear();
        currentIconSchemeGuid = Guid.Empty;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposedValue)
        {
            return;
        }

        if (disposing)
        {
            InvalidateCustomIconCache();
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
