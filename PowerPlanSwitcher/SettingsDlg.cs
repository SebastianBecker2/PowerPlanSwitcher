namespace PowerPlanSwitcher
{
    public partial class SettingsDlg : Form
    {
        public SettingsDlg() => InitializeComponent();

        //private PowerManager pm;

        protected override void OnLoad(EventArgs e)
        {
            DgvPowerSchemes.Rows.AddRange(PowerManager.GetPowerSchemes()
                .Select(SchemeToRow)
                .ToArray());

            base.OnLoad(e);
        }

        private DataGridViewRow SchemeToRow(KeyValuePair<Guid, string?> scheme)
        {
            var (guid, name) = scheme;
            var setting = PowerSchemeSettings.GetSetting(guid);

            var row = new DataGridViewRow { Tag = guid, };

            row.Cells.AddRange(
                new DataGridViewCheckBoxCell
                {
                    Value = setting is null || setting.Visible,
                },
                new DataGridViewTextBoxCell { Value = name, },
                new DataGridViewImageCell
                {
                    Value = setting?.Icon,
                });

            return row;
        }

        private void HandleDgvPowerSchemesCellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= DgvPowerSchemes.RowCount)
            {
                return;
            }

            // Has to be the icon column
            if (e.ColumnIndex != 2)
            {
                return;
            }

            var cell = DgvPowerSchemes.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (e.Button == MouseButtons.Right)
            {
                cell.Value = null;
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                var typeFilters = new[]
                {
                    "All image types " +
                        "(*.png; *.jpg; *.jpeg; *.bmp; *.tiff; *.tif; *.gif)" +
                        "|*.png;*.jpg;*.jpeg;*.bmp;*.tiff;*.tif;*.gif",
                    "PNG (*.png)|*.png",
                    "JPEG (*.jpg; *.jpeg)|*.jpg;*.jpeg",
                    "BMP (*.bmp)|*.bmp",
                    "TIFF (*.tiff; *.tif)|*.tiff;*.tif",
                    "GIF (*.gif)|*.gif",
                    "All files (*.*)|*.*",
                };

                using var dlg = new OpenFileDialog
                {
                    Filter = string.Join("|", typeFilters),
                    FilterIndex = 0,
                    RestoreDirectory = true,
                };
                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                DgvPowerSchemes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value =
                    Image.FromFile(dlg.FileName);
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DgvPowerSchemes.Rows)
            {
                var schemeGuid = (Guid)row.Tag!;
                PowerSchemeSettings.SetSetting(schemeGuid, new PowerSchemeSettings.Setting
                {
                    Visible = (bool)row.Cells[0].Value,
                    Icon = row.Cells[2].Value as Image,
                });
            }
            PowerSchemeSettings.SaveSettings();
            DialogResult = DialogResult.OK;
        }
    }
}
