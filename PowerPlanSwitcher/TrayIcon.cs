namespace PowerPlanSwitcher;

using PowerManagement;
using Properties;
using RuleManagement.Rules;

internal class TrayIcon : IDisposable
{
    private static readonly Icon DefaultIcon =
        IconFromImage(Resources.power_surge);

    private bool disposedValue;
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
            notifyIcon.Icon = DefaultIcon;
            return;
        }
        notifyIcon.Icon = IconFromImage(setting.Icon);
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

    private static Icon IconFromImage(Image img)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        // Header
        bw.Write((short)0);   // 0 : reserved
        bw.Write((short)1);   // 2 : 1=ico, 2=cur
        bw.Write((short)1);   // 4 : number of images
                              // Image directory
        var w = img.Width;
        if (w >= 256)
        {
            w = 0;
        }

        bw.Write((byte)w);    // 0 : width of image
        var h = img.Height;
        if (h >= 256)
        {
            h = 0;
        }

        bw.Write((byte)h);    // 1 : height of image
        bw.Write((byte)0);    // 2 : number of colors in palette
        bw.Write((byte)0);    // 3 : reserved
        bw.Write((short)0);   // 4 : number of color planes
        bw.Write((short)0);   // 6 : bits per pixel
        var sizeHere = ms.Position;
        bw.Write(0);     // 8 : image size
        var start = (int)ms.Position + 4;
        bw.Write(start);      // 12: offset of image data
                              // Image data
        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        var imageSize = (int)ms.Position - start;
        _ = ms.Seek(sizeHere, SeekOrigin.Begin);
        bw.Write(imageSize);
        _ = ms.Seek(0, SeekOrigin.Begin);

        // And load it
        return new Icon(ms);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposedValue)
        {
            return;
        }

        if (disposing)
        {
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
