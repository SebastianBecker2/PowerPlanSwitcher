namespace PowerPlanSwitcher
{
    public partial class PowerSchemeSelectorDlg : Form
    {
        private static readonly Color ButtonBackgroundColor =
            Color.FromArgb(0x15, 0x15, 0x14);
        private static readonly Color SelectedButtonBackgroundColor =
            Color.FromArgb(0x25, 0x25, 0x25);

        private const int ButtonHeight = 50;
        private const int ButtonWidth = 360;

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
                FlatStyle = FlatStyle.Popup,
                Image = icon,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                ForeColor = SystemColors.HighlightText,
                BackColor = active
                    ? SelectedButtonBackgroundColor
                    : ButtonBackgroundColor,
                Margin = Padding.Empty,
                Text = active ? "(Active) " + name : name,
                Font = new Font(Font.FontFamily, 12),
                Tag = guid,
                Dock = DockStyle.Fill,
            };

            button.Click += (_, _) =>
            {
                PowerManager.SetActivePowerScheme((Guid)button.Tag);
                button.BackColor = SelectedButtonBackgroundColor;
                foreach (Button b in TlpPowerSchemes.Controls)
                {
                    if (b == button)
                    {
                        continue;
                    }
                    b.BackColor = ButtonBackgroundColor;
                }
                Close();
            };

            return button;
        }

        protected override void OnLoad(EventArgs e)
        {
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
    }
}
