namespace PowerPlanSwitcher;

using System;
using System.Windows.Forms;
using PowerManagement;
using static Vanara.PInvoke.User32;

public partial class ToastDlg : Form
{
    private static readonly int DisplayDuration = 2000;
    private static readonly Size ToastMinimumClientSize = new(292, 92);

    private static SynchronizationContext? syncContext;
    private static ToastDlg? toastDlg;

    private readonly DpiImageScaler dpiImageScaler;

    private static Color ButtonBackgroundColor =>
        ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
        ? Color.WhiteSmoke
        : Color.FromArgb(0x15, 0x15, 0x15);
    private static Color ForegroundColor =>
        ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
        ? Color.Black
        : Color.White;
    private static Color TlpPowerSchemesBackColor =>
        ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
        ? Color.Silver
        : Color.DimGray;

    public ToastDlg()
    {
        InitializeComponent();
        dpiImageScaler = new DpiImageScaler(this);
    }

    protected override void OnLoad(EventArgs e)
    {
        DisplayTimer.Interval = DisplayDuration;

        Padding = new Padding(1);
        BackColor = TlpPowerSchemesBackColor;
        tableLayoutPanel1.BackColor = ButtonBackgroundColor;

        PibAppIcon.BackColor = ButtonBackgroundColor;
        LblTitle.ForeColor = ForegroundColor;
        LblTitle.BackColor = ButtonBackgroundColor;

        PibPowerSchemeIcon.BackColor = ButtonBackgroundColor;
        LblPowerSchemeName.ForeColor = ForegroundColor;
        LblPowerSchemeName.BackColor = ButtonBackgroundColor;

        LblReason.ForeColor = ForegroundColor;
        LblReason.BackColor = ButtonBackgroundColor;

        UpdateToastLayout();

        DisplayTimer.Stop();
        DisplayTimer.Start();

        base.OnLoad(e);
    }

    protected override void OnDpiChangedAfterParent(EventArgs e)
    {
        base.OnDpiChangedAfterParent(e);
        UpdateToastLayout();
    }

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            // turn on WS_EX_TOOLWINDOW style bit
            // Used to hide the banner from alt+tab
            // source: https://www.csharp411.com/hide-form-from-alttab/
            cp.ExStyle |= (int)WindowStylesEx.WS_EX_TOOLWINDOW;
            return cp;
        }
    }

    private void Any_Click(object sender, EventArgs e) => Dispose();

    private void DisplayTimer_Tick(object sender, EventArgs e) => Dispose();

    public static void Initialize()
    {
        syncContext = SynchronizationContext.Current;
        if (syncContext is not WindowsFormsSynchronizationContext)
        {
            throw new InvalidOperationException(
                "Initialize must be called from an UI thread");
        }
    }

    public static void ShowToastNotification(
        Guid activeSchemeGuid,
        string activationReason)
    {
        if (syncContext is null)
        {
            throw new InvalidOperationException(
                "ToastNotification was not initialized before use");
        }

        syncContext.Send(_ =>
        {
            if (toastDlg == null)
            {
                toastDlg = new ToastDlg();
                toastDlg.Disposed += (_, _) => toastDlg = null;
            }

            toastDlg.PibPowerSchemeIcon.Image =
                PowerSchemeSettings.GetSetting(activeSchemeGuid)?.Icon;
            toastDlg.dpiImageScaler.OverrideSource(
                toastDlg.PibPowerSchemeIcon,
                toastDlg.PibPowerSchemeIcon.Image);
            toastDlg.LblPowerSchemeName.Text =
                PowerManager.Api.GetPowerSchemeName(activeSchemeGuid);
            toastDlg.LblReason.Text = activationReason;

            toastDlg.UpdateToastLayout();

            toastDlg.DisplayTimer.Stop();
            toastDlg.DisplayTimer.Start();

            toastDlg.Show();
        }, null);
    }

    private void UpdateToastLayout()
    {
        SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();

        var minimumClientSize = new Size(
            LogicalToDeviceUnits(ToastMinimumClientSize.Width),
            LogicalToDeviceUnits(ToastMinimumClientSize.Height));

        MinimumSize = minimumClientSize;
        MaximumSize = Size.Empty;

        PerformLayout();

        ClientSize = new Size(
            Math.Max(ClientSize.Width, minimumClientSize.Width),
            Math.Max(ClientSize.Height, minimumClientSize.Height));

        Location = PopUpWindowLocationHelper.GetPositionOnTaskbar(
            Size,
            LblReason.Text);

        tableLayoutPanel2.ResumeLayout(true);
        tableLayoutPanel1.ResumeLayout(true);
        ResumeLayout(true);
    }
}

