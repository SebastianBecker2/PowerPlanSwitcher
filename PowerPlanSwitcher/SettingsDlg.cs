namespace PowerPlanSwitcher
{
    using System.Drawing.Drawing2D;
    using Properties;

    public partial class SettingsDlg : Form
    {
        private readonly List<(Guid guid, string name)> powerSchemes =
            PowerManager.GetPowerSchemes()
                .Where(scheme => !string.IsNullOrWhiteSpace(scheme.name))
                .Cast<(Guid schemeGuid, string name)>()
                .ToList();

        public SettingsDlg() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {
            DgvPowerSchemes.Rows.AddRange(powerSchemes
                .Select(SchemeToRow)
                .ToArray());

            UpdatePowerRules();

            ChbActivateInitialPowerScheme.Checked =
                Settings.Default.ActivateInitialPowerScheme;
            CmbInitialPowerScheme.Items.AddRange(powerSchemes
                .Select(scheme => scheme.name)
                .Cast<object>()
                .ToArray());
            if (Settings.Default.InitialPowerSchemeGuid == Guid.Empty)
            {
                CmbInitialPowerScheme.SelectedIndex = 0;
            }
            else
            {
                CmbInitialPowerScheme.SelectedIndex = powerSchemes.FindIndex(
                    scheme => scheme.guid
                        == Settings.Default.InitialPowerSchemeGuid);
            }
            CmbInitialPowerScheme.Enabled =
                ChbActivateInitialPowerScheme.Checked;

            base.OnLoad(e);
        }

        private DataGridViewRow SchemeToRow((Guid guid, string name) scheme)
        {
            var setting = PowerSchemeSettings.GetSetting(scheme.guid);

            var row = new DataGridViewRow { Tag = scheme.guid, };

            row.Cells.AddRange(
                new DataGridViewCheckBoxCell
                {
                    Value = setting is null || setting.Visible,
                },
                new DataGridViewTextBoxCell { Value = scheme.guid, },
                new DataGridViewImageCell
                {
                    Value = setting?.Icon,
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                });

            return row;
        }

        private void HandleDlgPowerSchemesImageCellClick(
            object sender,
            DataGridViewCellMouseEventArgs e)
        {
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

                var image = Image.FromFile(dlg.FileName);
                if (image.Size.Height > 32 || image.Size.Width > 32)
                {
                    image = ResizeImage(
                        image,
                        Rescale(image.Size, new Size(32, 32)));
                }

                DgvPowerSchemes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value =
                    image;
            }
        }

        private static Size Rescale(
            Size originalSize,
            Size containerSize)
        {
            var width = originalSize.Width;
            var height = originalSize.Height;

            if (width > height)
            {
                height = (int)(height / ((double)width / containerSize.Width));
                return containerSize with { Height = height };
            }

            width = (int)(width / ((double)height / containerSize.Height));
            return containerSize with { Width = width };
        }

        private static Image ResizeImage(Image original,
            Size size,
            InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic,
            SmoothingMode smoothingMode = SmoothingMode.HighQuality,
            PixelOffsetMode pixelOffsetMode = PixelOffsetMode.HighQuality) =>
            ResizeImage(
                original,
                size.Width,
                size.Height,
                interpolationMode,
                smoothingMode,
                pixelOffsetMode);

        public static Image ResizeImage(
            Image original,
            int width,
            int height,
            InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic,
            SmoothingMode smoothingMode = SmoothingMode.HighQuality,
            PixelOffsetMode pixelOffsetMode = PixelOffsetMode.HighQuality)
        {
            var result = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(result);
            graphics.SmoothingMode = smoothingMode;
            graphics.InterpolationMode = interpolationMode;
            graphics.PixelOffsetMode = pixelOffsetMode;
            graphics.DrawImage(original, 0, 0, width, height);
            return result;
        }

        private void HandleDlgPowerSchemesVisibleCellClick(
            object sender,
            DataGridViewCellMouseEventArgs e)
        {
            var cell = DgvPowerSchemes.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.Value = !(bool)cell.Value;
        }

        private void HandleDgvPowerSchemesCellMouseDown(
            object sender,
            DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= DgvPowerSchemes.RowCount)
            {
                return;
            }

            // Handle image cell click
            if (e.ColumnIndex == 2)
            {
                HandleDlgPowerSchemesImageCellClick(sender, e);
                return;
            }

            // Handle visible checkbox cell click
            if (e.ColumnIndex == 0)
            {
                HandleDlgPowerSchemesVisibleCellClick(sender, e);
            }
        }

        private void HandleBtnOkClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DgvPowerSchemes.Rows)
            {
                var schemeGuid = (Guid)row.Tag!;
                PowerSchemeSettings.SetSetting(schemeGuid,
                    new PowerSchemeSettings.Setting
                    {
                        Visible = (bool)row.Cells[0].Value,
                        Icon = row.Cells[2].Value as Image,
                    });
            }
            PowerSchemeSettings.SaveSettings();

            PowerRule.SetPowerRules(DgvPowerRules.Rows
                .Cast<DataGridViewRow>()
                .Select(r => r.Tag as PowerRule)
                .Cast<PowerRule>());
            PowerRule.SavePowerRules();

            string GetSelectedString(ComboBox cmb) =>
                cmb.Items[cmb.SelectedIndex].ToString() ?? string.Empty;

            Guid GetPowerSchemeGuid(string name) =>
                powerSchemes.First(scheme => scheme.name == name).guid;

            Settings.Default.ActivateInitialPowerScheme =
                ChbActivateInitialPowerScheme.Checked;
            Settings.Default.InitialPowerSchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbInitialPowerScheme));
            Settings.Default.Save();

            DialogResult = DialogResult.OK;
        }

        private void HandleBtnCreateRuleFromProcessClick(
            object sender,
            EventArgs e)
        {
            using var processSelectionDlg = new ProcessSelectionDlg();
            if (processSelectionDlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using var powerRuleDlg = new PowerRuleDlg
            {
                PowerRule = new PowerRule
                {
                    FilePath = processSelectionDlg.SelectedProcess!.FileName,
                    Type = RuleType.Exact,
                },
            };
            if (powerRuleDlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            powerRuleDlg.PowerRule!.Index = DgvPowerRules.RowCount;
            _ = DgvPowerRules.Rows.Add(PowerRuleToRow(powerRuleDlg.PowerRule));
        }

        private static DataGridViewRow PowerRuleToRow(PowerRule powerRule)
        {
            var row = new DataGridViewRow { Tag = powerRule, };
            var setting = PowerSchemeSettings.GetSetting(powerRule.SchemeGuid);
            row.Cells.AddRange(
                new DataGridViewTextBoxCell
                {
                    Value = powerRule.Index,
                },
                new DataGridViewTextBoxCell
                {
                    Value = PowerRule.RuleTypeToText(powerRule.Type),
                },
                new DataGridViewTextBoxCell
                {
                    Value = powerRule.FilePath,
                },
                new DataGridViewImageCell
                {
                    Value = setting?.Icon,
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                },
                new DataGridViewTextBoxCell
                {
                    Value = PowerManager.GetPowerSchemeName(powerRule.SchemeGuid)
                        ?? "Power Plan is missing!",
                });

            return row;
        }

        private void UpdatePowerRules() =>
            DgvPowerRules.Rows.AddRange(PowerRule.GetPowerRules()
                .OrderBy(r => r.Index)
                .Select(PowerRuleToRow)
                .ToArray());

        private void HandleBtnAddPowerRuleClick(object sender, EventArgs e)
        {
            using var dlg = new PowerRuleDlg();
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            dlg.PowerRule!.Index = DgvPowerRules.RowCount;
            _ = DgvPowerRules.Rows.Add(PowerRuleToRow(dlg.PowerRule));
        }

        private void HandleBtnEditPowerRuleClick(object sender, EventArgs e)
        {
            if (DgvPowerRules.SelectedRows.Count == 0)
            {
                return;
            }

            using var dlg = new PowerRuleDlg
            {
                PowerRule = DgvPowerRules.SelectedRows[0].Tag as PowerRule,
            };
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            DgvPowerRules.Rows.RemoveAt(dlg.PowerRule!.Index);
            DgvPowerRules.Rows.Insert(
                dlg.PowerRule!.Index,
                PowerRuleToRow(dlg.PowerRule));
            DgvPowerRules.Rows[dlg.PowerRule!.Index].Selected = true;
        }

        private void HandleBtnDeletePowerRuleClick(object sender, EventArgs e)
        {
            if (DgvPowerRules.SelectedRows.Count == 0)
            {
                return;
            }

            var index = DgvPowerRules.SelectedRows[0].Index;
            DgvPowerRules.Rows.RemoveAt(index);
            for (; index < DgvPowerRules.Rows.Count; index++)
            {
                var row = DgvPowerRules.Rows[index];
                (row.Tag as PowerRule)!.Index = index;
                row.Cells[0].Value = index;
            }
        }

        private void HandleBtnAscentPowerRuleClick(object sender, EventArgs e)
        {
            if (DgvPowerRules.SelectedRows.Count == 0)
            {
                return;
            }

            var row = DgvPowerRules.SelectedRows[0];
            var powerRule = row.Tag as PowerRule;
            if (powerRule!.Index == 0)
            {
                return;
            }

            DgvPowerRules.Rows.Remove(row);
            DgvPowerRules.Rows.Insert(powerRule!.Index - 1, row);

            var otherRow = DgvPowerRules.Rows[powerRule.Index];
            otherRow.Cells[0].Value = ++(otherRow.Tag as PowerRule)!.Index;
            row.Cells[0].Value = --(row.Tag as PowerRule)!.Index;

            row.Selected = true;
        }

        private void HandleBtnDescentPowerRuleClick(object sender, EventArgs e)
        {
            if (DgvPowerRules.SelectedRows.Count == 0)
            {
                return;
            }

            var row = DgvPowerRules.SelectedRows[0];
            var powerRule = row.Tag as PowerRule;
            if (powerRule!.Index == DgvPowerRules.RowCount - 1)
            {
                return;
            }

            DgvPowerRules.Rows.Remove(row);
            DgvPowerRules.Rows.Insert(powerRule!.Index + 1, row);

            var otherRow = DgvPowerRules.Rows[powerRule.Index];
            otherRow.Cells[0].Value = --(otherRow.Tag as PowerRule)!.Index;
            row.Cells[0].Value = ++(row.Tag as PowerRule)!.Index;

            row.Selected = true;
        }

        private void HandleChbActivateInitialPowerSchemeCheckedChanged(
            object sender,
            EventArgs e) =>
            CmbInitialPowerScheme.Enabled =
                ChbActivateInitialPowerScheme.Checked;
    }
}
