namespace PowerPlanSwitcher
{
    using PowerPlanSwitcher.Properties;

    public partial class PowerSchemeSelectorDlg : Form
    {
        private static Color ButtonBackgroundColor =>
            ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
            ? SystemColors.Control
            : Color.FromArgb(0x15, 0x15, 0x14);
        private static Color SelectedButtonBackgroundColor =>
            ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
            ? SystemColors.ControlLight
            : Color.FromArgb(0x25, 0x25, 0x25);
        private static Color ForegroundColor =>
            ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
            ? SystemColors.ControlText
            : SystemColors.HighlightText;
        private static Color FAMOBColor =>
            ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
            ? Color.FromArgb(0xD8, 0xD8, 0xD8)
            : Color.FromArgb(0x35, 0x35, 0x35);

        private const int ButtonHeight = 50;
        private const int ButtonWidth = 360;

        private bool shownTriggered;

        public PowerSchemeSelectorDlg() => InitializeComponent();

        private Button CreateButton(
            Guid guid,
            string? name,
            Image? icon,
            bool active)
        {
            name ??= guid.ToString();
            var button = new Button
            {
                FlatStyle = FlatStyle.Flat,
                Image = icon,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                ForeColor = ForegroundColor,
                BackColor = active
                    ? SelectedButtonBackgroundColor
                    : ButtonBackgroundColor,
                Margin = Padding.Empty,
                Text = active ? "(Active) " + name : name,
                Font = new Font(Font.FontFamily, 12),
                Tag = guid,
                Dock = DockStyle.Fill,
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = FAMOBColor;

            button.Click += (_, _) =>
            {
                PowerManager.SetActivePowerScheme((Guid)button.Tag);
                Close();
            };

            return button;
        }

        protected override void OnLoad(EventArgs e)
        {
            BackColor = ButtonBackgroundColor;

            var activeSchemeGuid = PowerManager.GetActivePowerSchemeGuid();

            foreach (var (guid, name) in PowerManager.GetPowerSchemes())
            {
                var setting = PowerSchemeSettings.GetSetting(guid);
                if (setting is not null && !setting.Visible)
                {
                    continue;
                }

                _ = TlpPowerSchemes.RowStyles.Add(new RowStyle
                {
                    SizeType = SizeType.Percent,
                    Height = 50,
                });

                TlpPowerSchemes.Controls.Add(
                    CreateButton(
                        guid,
                        name,
                        setting?.Icon,
                        activeSchemeGuid == guid));
            }

            Height = TlpPowerSchemes.Controls.Count * ButtonHeight;
            Width = ButtonWidth;

            SetPositionToTaskbar();

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

            shownTriggered = true;

            base.OnShown(e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            // For some reason, with version .Net 8.0, the OnDeactivate
            // event is called when the dialog is shown for the first time.
            // It triggeres OnActivate, OnDeactivate and then OnActivate again.
            // Couldn't figure out why. Didn't seem to happen in .Net 5.0.
            // So we check if the Shown event was triggered before. Because
            // it cames last when showing the dialog.
            if (shownTriggered)
            {
                DialogResult = DialogResult.Cancel;
            }
            base.OnDeactivate(e);
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
    }
}
