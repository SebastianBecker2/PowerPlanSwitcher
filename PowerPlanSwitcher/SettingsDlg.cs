namespace PowerPlanSwitcher;

using System.Data;
using System.Drawing.Drawing2D;
using Autofac;
using Newtonsoft.Json;
using PowerManagement;
using Properties;
using RuleManagement;
using RuleManagement.Dto;
using RuleManagement.Rules;

public partial class SettingsDlg : Form
{
    private class RuleWrapper(IRuleDto dto, int triggerCount)
    {
        public IRuleDto Dto { get; set; } = dto;
        public int TriggerCount { get; set; } = triggerCount;

        public RuleWrapper(IRuleDto dto) : this(dto, 0) { }
        public RuleWrapper(IRule rule) : this(rule.Dto, rule.TriggerCount) { }
    }

    private readonly List<(Guid guid, string name)> powerSchemes =
        [.. PowerManager.Static.GetPowerSchemes()
            .Where(scheme => !string.IsNullOrWhiteSpace(scheme.name))
            .Cast<(Guid schemeGuid, string name)>()];

    private Size SettingsDlgOriginalSize { get; set; } = Size.Empty;

    public IEnumerable<IRuleDto> RuleDto => [.. Rules.Select(r => r.Dto)];
    private IEnumerable<RuleWrapper> Rules { get; init; }

    private Func<HotkeySelectionDlg> HotkeySelectionDlgFactory { get; init; }

    public SettingsDlg(RuleManager ruleManager, Func<HotkeySelectionDlg> hotkeySelectionDlgFactory)
    {
        HotkeySelectionDlgFactory = hotkeySelectionDlgFactory;

        Rules = ruleManager.GetRules().Select(r => new RuleWrapper(r));

        InitializeComponent();
        TacSettingsCategories.SelectedIndexChanged +=
            TacSettingsCategories_SelectedIndexChanged;
        Size = Settings.Default.SettingsDlgSize;

        var text = $"Regular logging only captures critical errors, " +
            $"such as crash-related exceptions.{Environment.NewLine}" +
            $"Extended logging may have performance implication." +
            $"{Environment.NewLine}Extended log files will include basic " +
            $"information about running processes {Environment.NewLine}" +
            $"(process ID, executable path and start/stop times) " +
            $"{Environment.NewLine}which may be considered sensitive data.";
        TipHints.SetToolTip(PibLoggingInfo, text);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (TacSettingsCategories.SelectedTab != TapOtherSettings)
        {
            Settings.Default.SettingsDlgSize = Size;
        }
        else
        {
            Settings.Default.SettingsDlgSize = SettingsDlgOriginalSize;
        }
        Settings.Default.Save();
        base.OnFormClosing(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        DgvPowerSchemes.Rows.AddRange([.. powerSchemes.Select(SchemeToRow)]);

        UpdatePowerRules();

        ChbActivateInitialPowerScheme.Checked =
            Settings.Default.ActivateInitialPowerScheme;
        CmbInitialPowerScheme.Items.AddRange([.. powerSchemes
            .Select(scheme => scheme.name)
            .Cast<object>()]);
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

        CmbColorTheme.Items.AddRange([.. ColorThemeHelper.GetDisplayNames().Cast<object>()]);
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
            [.. PopUpWindowLocationHelper.GetDisplayNames().Cast<object>()]);
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

        ChbExtendedLogging.Checked = Settings.Default.ExtendedLogging;

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
            SettingsDlgOriginalSize = Size;
            MaximumSize = MinimumSize;
        }
        else if (TacSettingsCategories.SelectedTab != TapOtherSettings
            && MaximumSize != Size.Empty)
        {
            MaximumSize = Size.Empty;
            Size = SettingsDlgOriginalSize;
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
        // Update logging settings immediately to make sure we get all
        // log events from settings changes if it's activated.
        Program.UpdateLogLevelSwitch(ChbExtendedLogging.Checked);

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

        Settings.Default.ExtendedLogging = ChbExtendedLogging.Checked;

        Settings.Default.Save();

        DialogResult = DialogResult.OK;
    }

    private static DataGridViewRow RuleDtoToRow(IRuleDto dto) =>
        RuleWrapperToRow(new RuleWrapper(dto));

    private static DataGridViewRow RuleWrapperToRow(RuleWrapper rule)
    {
        var dto = rule.Dto;
        var row = new DataGridViewRow { Tag = rule, };
        var setting = PowerSchemeSettings.GetSetting(dto.SchemeGuid);
        row.Cells.AddRange(
            new DataGridViewTextBoxCell(),
            new DataGridViewTextBoxCell
            {
                Value = dto.GetDescription(),
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
                        dto.SchemeGuid)
                    ?? dto.SchemeGuid.ToString(),
            },
            new DataGridViewTextBoxCell
            {
                Value = rule.TriggerCount,
            });

        return row;
    }

    private void UpdatePowerRules() =>
        DgvRules.Rows.AddRange([.. Rules.Select(RuleWrapperToRow)]);

    private void HandleBtnAddPowerRuleClick(object sender, EventArgs e)
    {
        using var dlg = new RuleDlg();
        if (dlg.ShowDialog() != DialogResult.OK || dlg.RuleDto is null)
        {
            return;
        }

        _ = DgvRules.Rows.Add(RuleDtoToRow(dlg.RuleDto));
    }

    private void HandleBtnEditPowerRuleClick(object sender, EventArgs e)
    {
        if (DgvRules.SelectedRows.Count == 0)
        {
            return;
        }

        var index = DgvRules.SelectedRows[0].Index;
        var rule = DgvRules.SelectedRows[0].Tag as RuleWrapper
            ?? throw new InvalidOperationException(
                "Selected row does not have a valid Rule tag.");

        using var dlg = new RuleDlg
        {
            RuleDto = rule.Dto,
        };
        if (dlg.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        DgvRules.Rows.RemoveAt(index);
        DgvRules.Rows.Insert(index, RuleDtoToRow(dlg.RuleDto));
        DgvRules.Rows[index].Selected = true;
    }

    private void HandleBtnDeletePowerRuleClick(object sender, EventArgs e)
    {
        if (DgvRules.SelectedRows.Count == 0)
        {
            return;
        }

        var index = DgvRules.SelectedRows[0].Index;
        DgvRules.Rows.RemoveAt(index);
    }

    private void HandleBtnAscentPowerRuleClick(object sender, EventArgs e)
    {
        if (DgvRules.SelectedRows.Count == 0)
        {
            return;
        }

        var row = DgvRules.SelectedRows[0];
        DgvRules.Rows.Remove(row);
        DgvRules.Rows.Insert(row.Index - 1, row);
        row.Selected = true;
    }

    private void HandleBtnDescentPowerRuleClick(object sender, EventArgs e)
    {
        if (DgvRules.SelectedRows.Count == 0)
        {
            return;
        }

        var row = DgvRules.SelectedRows[0];
        DgvRules.Rows.Remove(row);
        DgvRules.Rows.Insert(row.Index + 1, row);
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

        foreach (var r in DgvRules.Rows
            .Cast<DataGridViewRow>()
            .Where(r => (r.Tag as RuleWrapper)!.Dto.SchemeGuid == guid))
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

        foreach (var r in DgvRules.Rows
            .Cast<DataGridViewRow>()
            .Where(r => (r.Tag as IRule)!.Dto.SchemeGuid == guid))
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

        using var dlg = HotkeySelectionDlgFactory();
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
        using var dlg = HotkeySelectionDlgFactory();
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
        if (e.RowIndex < 0 || e.RowIndex >= DgvRules.RowCount)
        {
            return;
        }

        HandleBtnEditPowerRuleClick(sender, e);
    }

    private void PibLoggingInfo_Click(object sender, EventArgs e) =>
        TipHints.Show(
            TipHints.GetToolTip(PibLoggingInfo),
            PibLoggingInfo,
            0,
            PibLoggingInfo.Height,
            3000);

    private void BtnOpenLogFolder_Click(object sender, EventArgs e) =>
        Program.OpenLogPath();

    private void BtnExportLog_Click(object sender, EventArgs e) =>
        Program.ExportLog();
}
