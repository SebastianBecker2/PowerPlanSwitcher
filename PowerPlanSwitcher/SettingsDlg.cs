namespace PowerPlanSwitcher;

using System.Data;
using Autofac;
using Newtonsoft.Json;
using PowerManagement;
using Properties;
using RuleManagement;
using RuleManagement.Dto;
using RuleManagement.Events;
using RuleManagement.Rules;
using System.Drawing;

public partial class SettingsDlg : Form
{
    private sealed class RuleWrapper(IRuleDto dto, int triggerCount = 0)
    {
        public IRuleDto Dto { get; set; } = dto;
        public int TriggerCount { get; set; } = triggerCount;
        public IRule? LiveRule { get; init; }

        public RuleWrapper(IRule rule) : this(rule.Dto, rule.TriggerCount) => LiveRule = rule;
    }

    private readonly List<IRule> triggerSubscribedRules = [];
    private EventHandler<TriggerChangedEventArgs>? ruleTriggerChangedHandler;

    private readonly List<(Guid guid, string name)> powerSchemes =
        [.. PowerSchemeOrder.Apply(
            PowerManager.Api.GetPowerSchemes()
                .Where(scheme => !string.IsNullOrWhiteSpace(scheme.name))
                .Select(scheme => (guid: scheme.guid, name: scheme.name!)),
            scheme => scheme.guid,
            guid => PowerSchemeSettings.GetSetting(guid)?.Order)];

    private readonly DpiImageScaler dpiImageScaler;

    private Size SettingsDlgOriginalSize { get; set; } = Size.Empty;

    public IEnumerable<IRuleDto> RuleDto => [.. Rules.Select(r => r.Dto)];
    private IEnumerable<RuleWrapper> Rules { get; init; }

    private RuleManager RuleManager { get; init; }
    private Func<HotkeySelectionDlg> HotkeySelectionDlgFactory { get; init; }

    public SettingsDlg(RuleManager ruleManager, Func<HotkeySelectionDlg> hotkeySelectionDlgFactory)
    {
        RuleManager = ruleManager;
        HotkeySelectionDlgFactory = hotkeySelectionDlgFactory;

        Rules = ruleManager.GetRules().Select(r => new RuleWrapper(r));

        InitializeComponent();
        ApplyLocalization();
        dpiImageScaler = new DpiImageScaler(this);
        dpiImageScaler.OverrideSource(BtnAddPowerRule, Resources.add);
        dpiImageScaler.OverrideSource(BtnEditPowerRule, Resources.pencil);
        dpiImageScaler.OverrideSource(BtnOpenPowerPlanSettings, Resources.control_panel);
        dpiImageScaler.OverrideSource(BtnAscentPowerRule, Resources.arrow_up);
        dpiImageScaler.OverrideSource(BtnDescentPowerRule, Resources.arrow_down);
        dpiImageScaler.OverrideSource(BtnAscentPowerScheme, Resources.arrow_up);
        dpiImageScaler.OverrideSource(BtnDescentPowerScheme, Resources.arrow_down);
        dpiImageScaler.OverrideSource(BtnDeletePowerRule, Resources.delete);
        dpiImageScaler.OverrideSource(BtnSetIcon, Resources.picture);
        dpiImageScaler.OverrideSource(BtnRemoveIcon, Resources.delete);
        dpiImageScaler.OverrideSource(BtnSetHotkey, Resources.keyboard);
        dpiImageScaler.OverrideSource(BtnRemoveHotkey, Resources.keyboard_delete);
        dpiImageScaler.OverrideSource(BtnSetCycleHotkey, Resources.keyboard);
        dpiImageScaler.OverrideSource(BtnRemoveCycleHotkey, Resources.keyboard_delete);
        TacSettingsCategories.SelectedIndexChanged +=
            TacSettingsCategories_SelectedIndexChanged;
        Size = Settings.Default.SettingsDlgSize;
        RestoreSelectedTab();

        TipHints.SetToolTip(PibLoggingInfo, Strings.SettingsDlg_LoggingHint);

        TipHints.SetToolTip(
            PibRulesOrderInfo,
            Strings.SettingsDlg_RulesOrderHint);
    }

    private void ApplyLocalization()
    {
        // Form title
        Text = Strings.SettingsDlg_Title;

        // Tab pages
        TapPowerSchemes.Text = Strings.SettingsDlg_TabPowerSchemes;
        TapRules.Text = Strings.SettingsDlg_TabRules;
        TapOtherSettings.Text = Strings.SettingsDlg_TabOtherSettings;

        // OK / Cancel
        BtnOk.Text = Strings.Common_OK;
        BtnCancel.Text = Strings.Common_Cancel;

        // Power schemes DataGridView columns
        DgcVisible.HeaderText = Strings.SettingsDlg_DgcVisible;
        DgcIcon.HeaderText = Strings.SettingsDlg_DgcIcon;
        DgcName.HeaderText = Strings.SettingsDlg_DgcName;
        DgcHotkey.HeaderText = Strings.SettingsDlg_DgcHotkey;

        // Rules DataGridView columns
        DgcRulePriority.HeaderText = Strings.SettingsDlg_DgcRulePriority;
        DgcRuleDescription.HeaderText = Strings.SettingsDlg_DgcRuleDescription;
        DgcRuleSchemeIcon.HeaderText = Strings.SettingsDlg_DgcIcon;
        DgcRuleSchemeName.HeaderText = Strings.SettingsDlg_DgcRuleSchemeName;
        DgcTriggerCount.HeaderText = Strings.SettingsDlg_DgcTriggerCount;

        // Rules tab buttons
        BtnAddPowerRule.Text = Strings.SettingsDlg_BtnAddPowerRule;
        BtnEditPowerRule.Text = Strings.SettingsDlg_BtnEditPowerRule;
        BtnDeletePowerRule.Text = Strings.SettingsDlg_BtnDeletePowerRule;
        BtnAscentPowerRule.Text = Strings.SettingsDlg_BtnAscentPowerRule;
        BtnDescentPowerRule.Text = Strings.SettingsDlg_BtnDescentPowerRule;

        // Rules order hint
        LblRulesOrderHint.Text = Strings.SettingsDlg_LblRulesOrderHint;

        // Power schemes tab buttons
        BtnOpenPowerPlanSettings.Text = Strings.SettingsDlg_BtnOpenPowerPlanSettings;
        BtnSetIcon.Text = Strings.SettingsDlg_BtnSetIcon;
        BtnRemoveIcon.Text = Strings.SettingsDlg_BtnRemoveIcon;
        BtnSetHotkey.Text = Strings.SettingsDlg_BtnSetHotkey;
        BtnRemoveHotkey.Text = Strings.SettingsDlg_BtnRemoveHotkey;
        BtnAscentPowerScheme.Text = Strings.SettingsDlg_BtnAscentPowerScheme;
        BtnDescentPowerScheme.Text = Strings.SettingsDlg_BtnDescentPowerScheme;

        // Other settings tab - group boxes
        groupBox4.Text = Strings.SettingsDlg_GroupHotkeyCycle;
        groupBox1.Text = Strings.SettingsDlg_GroupColorTheme;
        groupBox3.Text = Strings.SettingsDlg_GroupNotificationLocation;
        groupBox2.Text = Strings.SettingsDlg_GroupLogging;

        // Cycle hotkey group
        RdbCycleAll.Text = Strings.SettingsDlg_RdbCycleAll;
        RdbCycleVisible.Text = Strings.SettingsDlg_RdbCycleVisible;
        BtnSetCycleHotkey.Text = Strings.SettingsDlg_BtnSetCycleHotkey;
        BtnRemoveCycleHotkey.Text = Strings.SettingsDlg_BtnRemoveCycleHotkey;

        // Logging group
        ChbExtendedLogging.Text = Strings.SettingsDlg_ChbExtendedLogging;
        BtnOpenLogFolder.Text = Strings.SettingsDlg_BtnOpenLogFolder;
        BtnExportLog.Text = Strings.SettingsDlg_BtnExportLog;
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        UnsubscribeFromRuleTriggerChanges();

        if (TacSettingsCategories.SelectedTab != TapOtherSettings)
        {
            Settings.Default.SettingsDlgSize = Size;
        }
        else
        {
            Settings.Default.SettingsDlgSize = SettingsDlgOriginalSize;
        }
        Settings.Default.SettingsDlgSelectedTabIndex =
            TacSettingsCategories.SelectedIndex;
        Settings.Default.Save();
        base.OnFormClosing(e);
    }

    private void RestoreSelectedTab()
    {
        var index = Settings.Default.SettingsDlgSelectedTabIndex;
        if (index < 0 || index >= TacSettingsCategories.TabCount)
        {
            index = 0;
        }

        TacSettingsCategories.SelectedIndex = index;
    }

    protected override void OnLoad(EventArgs e)
    {
        ColorThemeHelper.ApplyToDataGridView(DgvPowerSchemes);
        ColorThemeHelper.ApplyToDataGridView(DgvRules);

        DgvPowerSchemes.Rows.AddRange([.. powerSchemes.Select(SchemeToRow)]);

        UpdatePowerRules();
        SubscribeToRuleTriggerChanges();

        var cycleHotkey = JsonConvert.DeserializeObject<Hotkey>(
            Settings.Default.CyclePowerSchemeHotkey);
        LblCycleHotkey.Text = cycleHotkey?.ToString() ?? "[ ---------- ]";
        LblCycleHotkey.Tag = cycleHotkey;

        RdbCycleAll.Checked = !Settings.Default.CycleOnlyVisible;
        RdbCycleVisible.Checked = Settings.Default.CycleOnlyVisible;

        // Color theme combo: populate with localized display names,
        // select by enum index so save/load is language independent.
        var themes = ColorThemeHelper.GetColorThemes();
        CmbColorTheme.Items.Clear();
        CmbColorTheme.Items.AddRange(
            [.. themes.Select(ColorThemeHelper.GetDisplayName).Cast<object>()]);
        var selectedTheme = ColorThemeHelper.GetSelectedColorTheme();
        var themeIndex = themes.IndexOf(selectedTheme);
        CmbColorTheme.SelectedIndex = themeIndex >= 0 ? themeIndex : 0;

        // Pop-up window location combo: same approach.
        var locations = PopUpWindowLocationHelper.GetPopUpWindowLocations();
        CmbPopUpWindowGlobal.Items.Clear();
        CmbPopUpWindowGlobal.Items.AddRange(
            [.. locations.Select(PopUpWindowLocationHelper.GetDisplayName).Cast<object>()]);
        var selectedLocation =
            PopUpWindowLocationHelper.GetSelectedPopUpWindowLocation(
                Settings.Default.PopUpWindowLocationGlobal);
        var locationIndex = locations.IndexOf(selectedLocation);
        CmbPopUpWindowGlobal.SelectedIndex = locationIndex >= 0 ? locationIndex : 0;

        ChbExtendedLogging.Checked = Settings.Default.ExtendedLogging;

        DgvRules.SelectionChanged += (_, _) => UpdateRuleSelectionButtons();
        UpdateRuleSelectionButtons();

        DgvPowerSchemes.SelectionChanged += (_, _) => UpdatePowerSchemeSelectionButtons();
        UpdatePowerSchemeSelectionButtons();

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
                    Icon = row.Cells["DgcIcon"].Value is not Image image
                        ? null
                        : IconUtilities.NormalizeForPowerSchemeIcon(image),
                    Hotkey = row.Cells["DgcHotkey"].Tag as Hotkey,
                    Order = row.Index,
                });
        }
        PowerSchemeSettings.SaveSettings();

        RuleManager.SetRules(DgvRules.Rows
            .Cast<DataGridViewRow>()
            .Select(r => (r.Tag as RuleWrapper)!.Dto));

        Settings.Default.CyclePowerSchemeHotkey =
            JsonConvert.SerializeObject(LblCycleHotkey.Tag);
        Settings.Default.CycleOnlyVisible = RdbCycleVisible.Checked;

        // Save color theme by enum name, language independent.
        var themes = ColorThemeHelper.GetColorThemes();
        var themeIndex = CmbColorTheme.SelectedIndex;
        if (themeIndex >= 0 && themeIndex < themes.Count)
        {
            ColorThemeHelper.SetSelectedColorTheme(themes[themeIndex]);
        }

        // Save pop-up window location by enum name, language independent.
        var locations = PopUpWindowLocationHelper.GetPopUpWindowLocations();
        var locationIndex = CmbPopUpWindowGlobal.SelectedIndex;
        if (locationIndex >= 0 && locationIndex < locations.Count)
        {
            PopUpWindowLocationHelper.SetSelectedPopUpWindowLocation(
                locations[locationIndex],
                batteryManagement: false);
        }

        Settings.Default.ExtendedLogging = ChbExtendedLogging.Checked;

        Settings.Default.Save();
        ColorThemeHelper.ApplyToApplication();

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
            new DataGridViewTextBoxCell
            {
                Value = 0,
            },
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
                    PowerManager.Api.GetPowerSchemeName(
                        dto.SchemeGuid)
                    ?? dto.SchemeGuid.ToString(),
            },
            new DataGridViewTextBoxCell
            {
                Value = rule.TriggerCount,
            });

        return row;
    }

    private void SubscribeToRuleTriggerChanges()
    {
        ruleTriggerChangedHandler ??= Rule_TriggerChanged;

        foreach (var rule in RuleManager.GetRules())
        {
            rule.TriggerChanged += ruleTriggerChangedHandler;
            triggerSubscribedRules.Add(rule);
        }
    }

    private void UnsubscribeFromRuleTriggerChanges()
    {
        if (ruleTriggerChangedHandler is null)
        {
            return;
        }

        foreach (var rule in triggerSubscribedRules)
        {
            rule.TriggerChanged -= ruleTriggerChangedHandler;
        }

        triggerSubscribedRules.Clear();
    }

    private void Rule_TriggerChanged(object? sender, TriggerChangedEventArgs e)
    {
        void UpdateRow()
        {
            foreach (DataGridViewRow row in DgvRules.Rows)
            {
                if (row.Tag is not RuleWrapper wrapper
                    || !ReferenceEquals(wrapper.LiveRule, e.Rule))
                {
                    continue;
                }

                wrapper.TriggerCount = e.Rule.TriggerCount;
                row.Cells["DgcTriggerCount"].Value = e.Rule.TriggerCount;
                return;
            }
        }

        if (InvokeRequired)
        {
            BeginInvoke(UpdateRow);
        }
        else
        {
            UpdateRow();
        }
    }

    private void UpdatePowerRules()
    {
        DgvRules.Rows.AddRange([.. Rules.Select(RuleWrapperToRow)]);
        UpdateRulePriorities();
    }

    private void UpdateRulePriorities()
    {
        for (var index = 0; index < DgvRules.Rows.Count; index++)
        {
            DgvRules.Rows[index].Cells["DgcRulePriority"].Value = index + 1;
        }
    }

    private void HandleBtnAddPowerRuleClick(object sender, EventArgs e)
    {
        using var dlg = new RuleDlg();
        if (dlg.ShowDialog() != DialogResult.OK || dlg.RuleDto is null)
        {
            return;
        }

        _ = DgvRules.Rows.Add(RuleDtoToRow(dlg.RuleDto));
        UpdateRulePriorities();
    }

    private void UpdateRuleSelectionButtons()
    {
        var hasSelection = DgvRules.SelectedRows.Count > 0;
        BtnEditPowerRule.Enabled = hasSelection;
        BtnDeletePowerRule.Enabled = hasSelection;
    }

    private void UpdatePowerSchemeSelectionButtons()
    {
        BtnOpenPowerPlanSettings.Enabled = DgvPowerSchemes.SelectedRows.Count > 0;
    }

    private void HandleBtnOpenPowerPlanSettingsClick(object sender, EventArgs e)
    {
        if (DgvPowerSchemes.SelectedRows.Count == 0)
        {
            _ = MessageBox.Show(
                Strings.SettingsDlg_MsgSelectPowerPlanFirst,
                Strings.SettingsDlg_TitleOpenPowerPlanSettings,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        var schemeGuid = (Guid)DgvPowerSchemes.SelectedRows[0].Tag!;

        if (!PowerSchemeSettingsOpener.IsKnownPowerScheme(schemeGuid))
        {
            _ = MessageBox.Show(
                Strings.SettingsDlg_MsgNotValidPowerPlan,
                Strings.SettingsDlg_TitleOpenPowerPlanSettings,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (!PowerSchemeSettingsOpener.TryOpenPowerOptions())
        {
            _ = MessageBox.Show(
                Strings.SettingsDlg_MsgCouldNotOpenSettings,
                Strings.SettingsDlg_TitleOpenPowerPlanSettings,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
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
        UpdateRulePriorities();
    }

    private void HandleBtnDeletePowerRuleClick(object sender, EventArgs e)
    {
        if (DgvRules.SelectedRows.Count == 0)
        {
            return;
        }

        var index = DgvRules.SelectedRows[0].Index;
        DgvRules.Rows.RemoveAt(index);
        UpdateRulePriorities();
    }

    private void HandleBtnAscentPowerRuleClick(object sender, EventArgs e)
    {
        if (DgvRules.SelectedRows.Count == 0)
        {
            return;
        }

        var row = DgvRules.SelectedRows[0];
        var index = Math.Max(row.Index - 1, 0);
        DgvRules.Rows.Remove(row);
        DgvRules.Rows.Insert(index, row);
        row.Selected = true;
        UpdateRulePriorities();
    }

    private void HandleBtnDescentPowerRuleClick(object sender, EventArgs e)
    {
        if (DgvRules.SelectedRows.Count == 0)
        {
            return;
        }

        var row = DgvRules.SelectedRows[0];
        var index = Math.Min(row.Index + 1, DgvRules.RowCount - 1);
        DgvRules.Rows.Remove(row);
        DgvRules.Rows.Insert(index, row);
        row.Selected = true;
        UpdateRulePriorities();
    }

    private void HandleBtnAscentPowerSchemeClick(object sender, EventArgs e)
    {
        if (DgvPowerSchemes.SelectedRows.Count == 0)
        {
            return;
        }

        var row = DgvPowerSchemes.SelectedRows[0];
        var index = Math.Max(row.Index - 1, 0);
        DgvPowerSchemes.Rows.Remove(row);
        DgvPowerSchemes.Rows.Insert(index, row);
        row.Selected = true;
    }

    private void HandleBtnDescentPowerSchemeClick(object sender, EventArgs e)
    {
        if (DgvPowerSchemes.SelectedRows.Count == 0)
        {
            return;
        }

        var row = DgvPowerSchemes.SelectedRows[0];
        var index = Math.Min(row.Index + 1, DgvPowerSchemes.RowCount - 1);
        DgvPowerSchemes.Rows.Remove(row);
        DgvPowerSchemes.Rows.Insert(index, row);
        row.Selected = true;
    }

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

        var image = IconUtilities.NormalizeForPowerSchemeIcon(dlg.SelectedIcon);

        var row = DgvPowerSchemes.SelectedRows[0];
        row.Cells["DgcIcon"].Value = image;
        var guid = (Guid)row.Tag!;

        foreach (var r in DgvRules.Rows
            .Cast<DataGridViewRow>()
            .Where(r => (r.Tag as RuleWrapper)!.Dto.SchemeGuid == guid))
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
                string.Format(
                    Strings.SettingsDlg_MsgHotkeyAlreadyAssignedToPlan,
                    duplicateName,
                    name),
                Strings.SettingsDlg_TitleHotkeyAlreadyInUse,
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
                string.Format(
                    Strings.SettingsDlg_MsgHotkeyAlreadyAssignedToCycle,
                    name),
                Strings.SettingsDlg_TitleHotkeyAlreadyInUse,
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
                string.Format(
                    Strings.SettingsDlg_MsgHotkeyRebindToCycle,
                    duplicateName),
                Strings.SettingsDlg_TitleHotkeyAlreadyInUse,
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

    private void PibRulesOrderInfo_Click(object sender, EventArgs e) =>
        TipHints.Show(
            TipHints.GetToolTip(PibRulesOrderInfo),
            PibRulesOrderInfo,
            0,
            PibRulesOrderInfo.Height,
            3000);

    private void BtnOpenLogFolder_Click(object sender, EventArgs e) =>
        Program.OpenLogPath();

    private void BtnExportLog_Click(object sender, EventArgs e) =>
        Program.ExportLog();
}

