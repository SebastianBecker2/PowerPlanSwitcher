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

            SetPositionToTaskbar();

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

        private void SetPositionToTaskbar()
        {
            switch (Taskbar.Position)
            {
                case TaskbarPosition.Left:
                    Location = Taskbar.CurrentBounds.Location +
                        Taskbar.CurrentBounds.Size;
                    Location = new Point(Location.X, Location.Y - Size.Height);
                    break;

                case TaskbarPosition.Top:
                    Location = Taskbar.CurrentBounds.Location +
                        Taskbar.CurrentBounds.Size;
                    Location = new Point(Location.X - Size.Width, Location.Y);
                    break;

                case TaskbarPosition.Right:
                    Location = Taskbar.CurrentBounds.Location - Size;
                    Location = new Point(
                        Location.X,
                        Location.Y + Taskbar.CurrentBounds.Height);
                    break;

                case TaskbarPosition.Bottom:
                    Location = Taskbar.CurrentBounds.Location - Size;
                    Location = new Point(
                        Location.X + Taskbar.CurrentBounds.Width,
                        Location.Y);
                    break;

                case TaskbarPosition.Unknown:
                default:
                    StartPosition = FormStartPosition.WindowsDefaultLocation;
                    break;
            }
        }

        private void Any_Click(object sender, EventArgs e) =>
            DialogResult = DialogResult.OK;

        private void DisplayTimer_Tick(object sender, EventArgs e) =>
            DialogResult = DialogResult.OK;
    }
}
