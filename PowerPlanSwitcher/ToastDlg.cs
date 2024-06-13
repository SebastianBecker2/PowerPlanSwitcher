namespace PowerPlanSwitcher
{
    using System;
    using System.Windows.Forms;

    public partial class ToastDlg : Form
    {
        private static Color ButtonBackgroundColor =>
            ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
            ? SystemColors.Control
            : Color.FromArgb(0x15, 0x15, 0x14);
        private static Color ForegroundColor =>
            ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
            ? SystemColors.ControlText
            : SystemColors.HighlightText;

        public string PowerSchemeName { get; set; } = "";
        public Image? PowerSchemeIcon { get; set; }
        public string Reason { get; set; } = "";

        public ToastDlg() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {
            LblTitle.ForeColor = ForegroundColor;
            LblTitle.BackColor = ButtonBackgroundColor;
            PibAppIcon.BackColor = ButtonBackgroundColor;

            BackColor = ButtonBackgroundColor;

            PibPowerSchemeIcon.Image = PowerSchemeIcon;
            PibPowerSchemeIcon.BackColor = ButtonBackgroundColor;

            LblPowerSchemeName.Text = PowerSchemeName;
            LblPowerSchemeName.ForeColor = ForegroundColor;
            LblPowerSchemeName.BackColor = ButtonBackgroundColor;

            LblReason.Text = Reason;
            LblReason.ForeColor = ForegroundColor;
            LblReason.BackColor = ButtonBackgroundColor;

            Location = GetPositionOnTaskbar(Size);

            DisplayTimer.Stop();
            DisplayTimer.Start();

            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            // Brute force the dialog to frontmostestest topmostest
            WindowState = FormWindowState.Minimized;
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            Activate();
            _ = Focus();

            base.OnShown(e);
        }

        private static Point GetPositionOnTaskbar(Size windowSize)
        {
            var bounds = Taskbar.CurrentBounds;
            switch (Taskbar.Position)
            {
                case TaskbarPosition.Left:
                    bounds.Location += bounds.Size;
                    return new Point(bounds.X, bounds.Y - windowSize.Height);

                case TaskbarPosition.Top:
                    bounds.Location += bounds.Size;
                    return new Point(bounds.X - windowSize.Width, bounds.Y);

                case TaskbarPosition.Right:
                    bounds.Location -= windowSize;
                    return new Point(bounds.X, bounds.Y + bounds.Height);

                case TaskbarPosition.Bottom:
                    bounds.Location -= windowSize;
                    return new Point(bounds.X + bounds.Width, bounds.Y);

                case TaskbarPosition.Unknown:
                default:
                    return new Point(0,0);
            }
        }

        private void Any_Click(object sender, EventArgs e) =>
            DialogResult = DialogResult.OK;

        private void DisplayTimer_Tick(object sender, EventArgs e) =>
            DialogResult = DialogResult.OK;
    }
}
