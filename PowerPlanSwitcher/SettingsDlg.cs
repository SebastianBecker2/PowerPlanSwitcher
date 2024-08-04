namespace PowerPlanSwitcher
{
    using System.Data;
    using System.Drawing.Drawing2D;
    using Newtonsoft.Json;
    using PowerPlanSwitcher.PowerManagement;
    using PowerPlanSwitcher.RuleManagement.Rules;
    using Properties;

    public partial class SettingsDlg : Form
    {
        private readonly List<(Guid guid, string name)> powerSchemes =
            PowerManager.Static.GetPowerSchemes()
                .Where(scheme => !string.IsNullOrWhiteSpace(scheme.name))
                .Cast<(Guid schemeGuid, string name)>()
                .ToList();

        private Size settingsDlgOriginalSize = Size.Empty;

        public SettingsDlg()
        {
            InitializeComponent();
            TacSettingsCategories.SelectedIndexChanged +=
                TacSettingsCategories_SelectedIndexChanged;
            Size = Settings.Default.SettingsDlgSize;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (TacSettingsCategories.SelectedTab != TapOtherSettings)
            {
                Settings.Default.SettingsDlgSize = Size;
            }
            else
            {
                Settings.Default.SettingsDlgSize = settingsDlgOriginalSize;
            }
            Settings.Default.Save();
            base.OnFormClosing(e);
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

            CmbPopUpWindowGlobal.Items.AddRange(
                PopUpWindowLocationHelper.GetDisplayNames()
                    .Cast<object>()
                    .ToArray());
            index = CmbPopUpWindowGlobal.Items.IndexOf(
                Settings.Default.PopUpWindowLocationGlobal);
            if (index != -1 && index < CmbPopUpWindowGlobal.Items.Count)
            {
                CmbPopUpWindowGlobal.SelectedIndex = index;
            }
            else
            {
                CmbPopUpWindowGlobal.SelectedIndex = 0;
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

        private static Bitmap ResizeImage(Image original,
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

        private static Bitmap ResizeImage(
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

        private void TacSettingsCategories_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            if (TacSettingsCategories.SelectedTab == TapOtherSettings
                && MaximumSize == Size.Empty)
            {
                settingsDlgOriginalSize = Size;
                MaximumSize = MinimumSize;
            }
            else if (TacSettingsCategories.SelectedTab != TapOtherSettings
                && MaximumSize != Size.Empty)
            {
                MaximumSize = Size.Empty;
                Size = settingsDlgOriginalSize;
            }
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
                        Visible = (bool)row.Cells["DgcVisible"].Value,
                        Icon = row.Cells["DgcIcon"].Value as Image,
                        Hotkey = row.Cells["DgcHotkey"].Tag as Hotkey,
                    });
            }
            PowerSchemeSettings.SaveSettings();

            Rules.SetRules(DgvPowerRules.Rows
                .Cast<DataGridViewRow>()
                .Select(r => r.Tag as IRule)
                .Cast<IRule>());

            static string GetSelectedString(ComboBox cmb)
            {
                if (cmb.SelectedIndex == -1)
                {
                    return string.Empty;
                }
                return cmb.Items[cmb.SelectedIndex]?.ToString() ?? string.Empty;
            }

            Guid GetPowerSchemeGuid(string name) =>
                powerSchemes.FirstOrDefault(scheme => scheme.name == name).guid;

            Settings.Default.ActivateInitialPowerScheme =
                ChbActivateInitialPowerScheme.Checked;
            Settings.Default.InitialPowerSchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbInitialPowerScheme));

            Settings.Default.CyclePowerSchemeHotkey =
                JsonConvert.SerializeObject(LblCycleHotkey.Tag);
            Settings.Default.CycleOnlyVisible = RdbCycleVisible.Checked;

            Settings.Default.ColorTheme = CmbColorTheme.SelectedItem as string;

            Settings.Default.PopUpWindowLocationGlobal =
                CmbPopUpWindowGlobal.SelectedItem as string;

            Settings.Default.Save();

            DialogResult = DialogResult.OK;
        }

        private static DataGridViewRow RuleToRow(IRule rule)
        {
            var row = new DataGridViewRow { Tag = rule, };
            var setting = PowerSchemeSettings.GetSetting(rule.SchemeGuid);
            row.Cells.AddRange(
                new DataGridViewTextBoxCell
                {
                    Value = rule.Index + 1,
                },
                new DataGridViewTextBoxCell
                {
                    Value = rule.GetDescription(),
                },
                new DataGridViewImageCell
                {
                    Value = setting?.Icon,
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                },
                new DataGridViewTextBoxCell
                {
                    Value =
                        PowerManager.Static.GetPowerSchemeName(
                            rule.SchemeGuid)
                        ?? rule.SchemeGuid.ToString(),
                },
                new DataGridViewCheckBoxCell
                {
                    Value = rule.ActivationCount,
                });

            return row;
        }

        private void UpdatePowerRules() =>
            DgvPowerRules.Rows.AddRange(Rules.GetRules()
                .OrderBy(r => r.Index)
                .Select(RuleToRow)
                .ToArray());

        private void HandleBtnAddPowerRuleClick(object sender, EventArgs e)
        {
            using var dlg = new RuleDlg();
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            dlg.Rule!.Index = DgvPowerRules.RowCount;
            _ = DgvPowerRules.Rows.Add(RuleToRow(dlg.Rule));
        }

        private void HandleBtnEditPowerRuleClick(object sender, EventArgs e)
        {
            if (DgvPowerRules.SelectedRows.Count == 0)
            {
                return;
            }

            using var dlg = new RuleDlg
            {
                Rule = DgvPowerRules.SelectedRows[0].Tag as IRule,
            };
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            DgvPowerRules.Rows.RemoveAt(dlg.Rule!.Index);
            DgvPowerRules.Rows.Insert(
                dlg.Rule!.Index,
                RuleToRow(dlg.Rule));
            DgvPowerRules.Rows[dlg.Rule!.Index].Selected = true;
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
                (row.Tag as IRule)!.Index = index;
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
            var powerRule = row.Tag as IRule;
            if (powerRule!.Index == 0)
            {
                return;
            }

            DgvPowerRules.Rows.Remove(row);
            DgvPowerRules.Rows.Insert(powerRule!.Index - 1, row);

            var otherRow = DgvPowerRules.Rows[powerRule.Index];
            otherRow.Cells["DgcRuleIndex"].Value =
                ++(otherRow.Tag as IRule)!.Index;
            row.Cells["DgcRuleIndex"].Value = --(row.Tag as IRule)!.Index;

            row.Selected = true;
        }

        private void HandleBtnDescentPowerRuleClick(object sender, EventArgs e)
        {
            if (DgvPowerRules.SelectedRows.Count == 0)
            {
                return;
            }

            var row = DgvPowerRules.SelectedRows[0];
            var powerRule = row.Tag as IRule;
            if (powerRule!.Index == DgvPowerRules.RowCount - 1)
            {
                return;
            }

            DgvPowerRules.Rows.Remove(row);
            DgvPowerRules.Rows.Insert(powerRule!.Index + 1, row);

            var otherRow = DgvPowerRules.Rows[powerRule.Index];
            otherRow.Cells["DgcRuleIndex"].Value =
                --(otherRow.Tag as IRule)!.Index;
            row.Cells["DgcRuleIndex"].Value = ++(row.Tag as IRule)!.Index;

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
                .Where(r => (r.Tag as IRule)!.SchemeGuid == guid))
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
                .Where(r => (r.Tag as IRule)!.SchemeGuid == guid))
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

        private void DgvPowerRules_CellContentDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= DgvPowerRules.RowCount)
            {
                return;
            }

            HandleBtnEditPowerRuleClick(sender, e);
        }
    }
}
