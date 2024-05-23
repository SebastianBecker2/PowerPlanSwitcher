namespace PowerPlanSwitcher
{
    using System.Data;
    using System.Drawing.Drawing2D;
    using Newtonsoft.Json;
    using Properties;

    public partial class SettingsDlg : Form
    {
        private readonly List<(Guid guid, string name)> powerSchemes =
            PowerManager.GetPowerSchemes()
                .Where(scheme => !string.IsNullOrWhiteSpace(scheme.name))
                .Cast<(Guid schemeGuid, string name)>()
                .ToList();

        public SettingsDlg()
        {
            InitializeComponent();
            this.DgvPowerSchemes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPowerSchemes_CellClick);
        }

        private void dgvPowerSchemes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 确保点击的是复选框列
            if (e.ColumnIndex == this.DgvPowerSchemes.Columns["AcPowerCheckBox"].Index ||
                e.ColumnIndex == this.DgvPowerSchemes.Columns["BatteryCheckBox"].Index)
            {
                // 获取被点击的列名
                string columnName = this.DgvPowerSchemes.Columns[e.ColumnIndex].Name;

                // 遍历该列的所有单元格
                foreach (DataGridViewRow row in this.DgvPowerSchemes.Rows)
                {
                    // 忽略未绑定的行或Header行
                    if (!row.IsNewRow && e.RowIndex != row.Index)
                    {
                        // 检查单元格是否在同一个列，并且是否被选中
                        if (row.Cells[columnName] is DataGridViewCheckBoxCell checkBoxCell &&
                            (bool)checkBoxCell.FormattedValue == true)
                        {
                            // 取消选中除了被点击的单元格之外的所有单元格
                            row.Cells[columnName].Value = false;
                        }
                    }
                }

                // 确保被点击的单元格被选中
                this.DgvPowerSchemes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
            }
        }

        private void SettingsDlg_Load(object sender, EventArgs e)
        {
            // 检查电池状态并隐藏复选框列
            if (BatteryMonitor.BatteryNull.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery)
            {
                // 隐藏 AcPowerCheckBox 列
                this.DgvPowerSchemes.Columns["AcPowerCheckBox"].Visible = false;
                // 隐藏 BatteryCheckBox 列
                this.DgvPowerSchemes.Columns["BatteryCheckBox"].Visible = false;

                // foreach (DataGridViewRow row in this.DgvPowerSchemes.Rows)
                // {
                // // 忽略未绑定的行或Header行
                // if (!row.IsNewRow)
                // {
                // DataGridViewCheckBoxCell acCheckBox = (DataGridViewCheckBoxCell)row.Cells["AcPowerCheckBox"];
                // acCheckBox.Value = false; // 取消勾选

                // DataGridViewCheckBoxCell batteryCheckBox = (DataGridViewCheckBoxCell)row.Cells["BatteryCheckBox"];
                // batteryCheckBox.Value = false; // 取消勾选
                // }
                // }
            }
        }

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

            NudPowerRuleCheckInterval.Value =
                Settings.Default.PowerRuleCheckInterval;

            var cycleHotkey = JsonConvert.DeserializeObject<Hotkey>(
                Settings.Default.CyclePowerSchemeHotkey);
            LblCycleHotkey.Text = cycleHotkey?.ToString() ?? "[ ---------- ]";
            LblCycleHotkey.Tag = cycleHotkey;

            RdbCycleAll.Checked = !Settings.Default.CycleOnlyVisible;
            RdbCycleVisible.Checked = Settings.Default.CycleOnlyVisible;

            CmbColorTheme.Items.AddRange(ColorThemeHelper.GetDisplayNames()
                .Cast<object>()
                .ToArray());
            var index = CmbColorTheme.Items.IndexOf(Settings.Default.ColorTheme);
            if (index != -1 && index < CmbColorTheme.Items.Count)
            {
                CmbColorTheme.SelectedIndex = index;
            }
            else
            {
                CmbColorTheme.SelectedIndex = 0;
            }

            base.OnLoad(e);
        }

        private DataGridViewRow SchemeToRow((Guid guid, string name) scheme)
        {
            var setting = PowerSchemeSettings.GetSetting(scheme.guid);
            static string getHotkeyText(PowerSchemeSettings.Setting? setting)
            {
                if (setting?.Hotkey is null)
                {
                    return "[ ---------- ]";
                }
                return setting!.Hotkey.ToString();
            }

            var row = new DataGridViewRow { Tag = scheme.guid, };

            row.Cells.AddRange(
                new DataGridViewCheckBoxCell
                {
                    Value = setting is null || setting.Visible,
                },
                new DataGridViewImageCell
                {
                    Value = setting?.Icon,
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                },
                new DataGridViewTextBoxCell { Value = scheme.name, },

                new DataGridViewCheckBoxCell
                {
                    Value = setting is null || setting.AcPowerVisible,
                },
                new DataGridViewCheckBoxCell
                {
                    Value = setting is null || setting.BatteryVisible,
                },

                new DataGridViewTextBoxCell
                {
                    Value = getHotkeyText(setting),
                    Tag = setting?.Hotkey
                });

            return row;
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
                        AcPowerVisible = (bool)row.Cells["AcPowerCheckBox"].Value,
                        BatteryVisible = (bool)row.Cells["BatteryCheckBox"].Value,
                        Visible = (bool)row.Cells["DgcVisible"].Value,
                        Icon = row.Cells["DgcIcon"].Value as Image,
                        Hotkey = row.Cells["DgcHotkey"].Tag as Hotkey,
                    });
            }
            PowerSchemeSettings.SaveSettings();

            PowerRule.SetPowerRules(DgvPowerRules.Rows
                .Cast<DataGridViewRow>()
                .Select(r => r.Tag as PowerRule)
                .Cast<PowerRule>());
            PowerRule.SavePowerRules();

            static string GetSelectedString(ComboBox cmb) =>
                cmb.Items[cmb.SelectedIndex]?.ToString() ?? string.Empty;

            Guid GetPowerSchemeGuid(string name) =>
                powerSchemes.First(scheme => scheme.name == name).guid;

            Settings.Default.ActivateInitialPowerScheme =
                ChbActivateInitialPowerScheme.Checked;
            Settings.Default.InitialPowerSchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbInitialPowerScheme));

            Settings.Default.PowerRuleCheckInterval =
                (int)NudPowerRuleCheckInterval.Value;

            Settings.Default.CyclePowerSchemeHotkey =
                JsonConvert.SerializeObject(LblCycleHotkey.Tag);
            Settings.Default.CycleOnlyVisible = RdbCycleVisible.Checked;

            Settings.Default.ColorTheme = CmbColorTheme.SelectedItem as string;

            Settings.Default.Save();

            BatteryMonitor.BatteryMonitorInitialization();

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
                        ?? powerRule.SchemeGuid.ToString(),
                },
                new DataGridViewCheckBoxCell
                {
                    Value = powerRule.Active,
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
                row.Cells["DgcRuleIndex"].Value = index;
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
            otherRow.Cells["DgcRuleIndex"].Value =
                ++(otherRow.Tag as PowerRule)!.Index;
            row.Cells["DgcRuleIndex"].Value = --(row.Tag as PowerRule)!.Index;

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
            otherRow.Cells["DgcRuleIndex"].Value =
                --(otherRow.Tag as PowerRule)!.Index;
            row.Cells["DgcRuleIndex"].Value = ++(row.Tag as PowerRule)!.Index;

            row.Selected = true;
        }

        private void HandleChbActivateInitialPowerSchemeCheckedChanged(
            object sender,
            EventArgs e) =>
            CmbInitialPowerScheme.Enabled =
                ChbActivateInitialPowerScheme.Checked;

        private void BtnRemoveIcon_Click(object sender, EventArgs e)
        {
            if (DgvPowerSchemes.SelectedRows.Count == 0)
            {
                return;
            }

            var row = DgvPowerSchemes.SelectedRows[0];
            row.Cells["DgcIcon"].Value = null;
            var guid = (Guid)row.Tag!;

            foreach (var r in DgvPowerRules.Rows
                .Cast<DataGridViewRow>()
                .Where(r => (r.Tag as PowerRule)!.SchemeGuid == guid))
            {
                r.Cells["DgcRuleSchemeIcon"].Value = null;
            }
        }

        private void BtnSetIcon_Click(object sender, EventArgs e)
        {
            if (DgvPowerSchemes.SelectedRows.Count == 0)
            {
                return;
            }

            using var dlg = new IconSelectionDlg();
            if (dlg.ShowDialog() != DialogResult.OK
                || dlg.SelectedIcon is null)
            {
                return;
            }

            var image = dlg.SelectedIcon;
            if (image.Size.Height > 32 || image.Size.Width > 32)
            {
                image = ResizeImage(
                    image,
                    Rescale(image.Size, new Size(32, 32)));
            }

            var row = DgvPowerSchemes.SelectedRows[0];
            row.Cells["DgcIcon"].Value = image;
            var guid = (Guid)row.Tag!;

            foreach (var r in DgvPowerRules.Rows
                .Cast<DataGridViewRow>()
                .Where(r => (r.Tag as PowerRule)!.SchemeGuid == guid))
            {
                r.Cells["DgcRuleSchemeIcon"].Value = image;
            }
        }

        private void BtnSetHotkey_Click(object sender, EventArgs e)
        {
            if (DgvPowerSchemes.SelectedRows.Count == 0)
            {
                return;
            }
            var row = DgvPowerSchemes.SelectedRows[0];
            var cell = row.Cells["DgcHotkey"];
            var name = row.Cells["DgcName"].Value;

            using var dlg = new HotkeySelectionDlg();
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (dlg.Hotkey is null)
            {
                cell.Value = "[ ---------- ]";
                cell.Tag = null;
                return;
            }

            var duplicate = DgvPowerSchemes.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r =>
                    r != row
                    && dlg.Hotkey.Equals(r.Cells["DgcHotkey"].Tag));
            if (duplicate is not null)
            {
                var duplicateName = duplicate.Cells["DgcName"].Value;
                if (MessageBox.Show(
                    "Hotkey already assigned to Power Plan " +
                    $"'{duplicateName}'.{Environment.NewLine}Do you want to " +
                    $"rebind to '{name}'?",
                    "Hotkey already in use",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                var duplicateCell = duplicate.Cells["DgcHotkey"];
                duplicateCell.Value = "[ ---------- ]";
                duplicateCell.Tag = null;
            }
            else if (dlg.Hotkey.Equals(LblCycleHotkey.Tag))
            {
                if (MessageBox.Show(
                    "Hotkey already assigned to cycle through Power Plans." +
                    $"{Environment.NewLine}Do you want to " +
                    $"rebind to '{name}'?",
                    "Hotkey already in use",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                LblCycleHotkey.Text = "[ ---------- ]";
                LblCycleHotkey.Tag = null;
            }

            cell.Value = dlg.Hotkey.ToString();
            cell.Tag = dlg.Hotkey;
        }

        private void BtnRemoveHotkey_Click(object sender, EventArgs e)
        {
            if (DgvPowerSchemes.SelectedRows.Count == 0)
            {
                return;
            }

            var cell = DgvPowerSchemes.SelectedRows[0].Cells["DgcHotkey"];

            cell.Tag = null;
            cell.Value = "[ ---------- ]";
        }

        private void BtnSetCycleHotkey_Click(object sender, EventArgs e)
        {
            using var dlg = new HotkeySelectionDlg();
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (dlg.Hotkey is null)
            {
                LblCycleHotkey.Text = "[ ---------- ]";
                LblCycleHotkey.Tag = null;
                return;
            }

            var duplicate = DgvPowerSchemes.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r => dlg.Hotkey.Equals(r.Cells["DgcHotkey"].Tag));
            if (duplicate is not null)
            {
                var duplicateName = duplicate.Cells["DgcName"].Value;
                if (MessageBox.Show(
                    "Hotkey already assigned to Power Plan " +
                    $"'{duplicateName}'.{Environment.NewLine}Do you want to " +
                    $"rebind to cycle through Power Plans?",
                    "Hotkey already in use",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                var duplicateCell = duplicate.Cells["DgcHotkey"];
                duplicateCell.Value = "[ ---------- ]";
                duplicateCell.Tag = null;
            }

            LblCycleHotkey.Tag = dlg.Hotkey;
            LblCycleHotkey.Text = dlg.Hotkey?.ToString() ?? "[ ---------- ]";
        }

        private void BtnRemoveCycleHotkey_Click(object sender, EventArgs e)
        {
            LblCycleHotkey.Text = "[ ---------- ]";
            LblCycleHotkey.Tag = null;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
