namespace PowerPlanSwitcher
{
    public partial class Popup : Form
    {
        private static readonly Color ButtonBackgroundColor =
            Color.FromArgb(0x15, 0x15, 0x14);
        private static readonly Color SelectedButtonBackgroundColor =
            Color.FromArgb(0x25, 0x25, 0x25);

        private const int ButtonHeight = 50;
        private const int ButtonWidth = 360;

        public Popup() => InitializeComponent();

        private Button CreateButton(Guid guid, string? name, bool active)
        {
            var setting = PowerSchemeSettings.GetSetting(guid);

            var button = new Button
            {
                FlatStyle = FlatStyle.Popup,
                Image = setting?.Icon,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                ForeColor = SystemColors.HighlightText,
                BackColor = active
                    ? SelectedButtonBackgroundColor
                    : ButtonBackgroundColor,
                Margin = Padding.Empty,
                Text = name ?? guid.ToString(),
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
                _ = TlpPowerSchemes.RowStyles.Add(new RowStyle
                {
                    SizeType = SizeType.Percent,
                    Height = 50,
                });
                TlpPowerSchemes.Controls.Add(
                    CreateButton(guid, name, activeSchemeGuid == guid));
            }

            Height = TlpPowerSchemes.Controls.Count * ButtonHeight;
            Width = ButtonWidth;

            SetPositionToTaskbar();

            base.OnLoad(e);
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
