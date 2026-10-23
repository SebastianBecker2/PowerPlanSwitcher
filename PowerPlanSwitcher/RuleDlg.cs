namespace PowerPlanSwitcher;

using PowerManagement;
using RuleManagement.Dto;

public partial class RuleDlg : Form
{
    private readonly RuleControl.StartupRuleControl srcStartupRule = new();

    public IRuleDto? RuleDto { get; set; }

    private List<(Guid guid, string name)> powerSchemes = [];

    private static readonly List<(string nameKey, Type type)> RuleTypes =
        [
            ("RuleDlg_TypeProcess", typeof(ProcessRuleDto)),
            ("RuleDlg_TypePowerLine", typeof(PowerLineRuleDto)),
            ("RuleDlg_TypeIdle", typeof(IdleRuleDto)),
            ("RuleDlg_TypeStartup", typeof(StartupRuleDto)),
            ("RuleDlg_TypeShutdown", typeof(ShutdownRuleDto)),
        ];

    private static string GetSelectedString(ComboBox cmb) =>
        cmb.Items[cmb.SelectedIndex]?.ToString() ?? string.Empty;

    private Guid GetPowerSchemeGuid(string name) =>
            powerSchemes.First(scheme => scheme.name == name).guid;

    public RuleDlg()
    {
        InitializeComponent();
        ApplyLocalization();
        _ = new DpiImageScaler(this);
        srcStartupRule.Dock = DockStyle.Fill;
        tableLayoutPanel1.Controls.Add(srcStartupRule, 0, 3);
        tableLayoutPanel1.SetColumnSpan(srcStartupRule, 3);
    }

    private void ApplyLocalization()
    {
        Text = Strings.RuleDlg_Title;
        LblRuleType.Text = Strings.RuleDlg_LblRuleType;
        LblPowerScheme.Text = Strings.RuleDlg_LblPowerScheme;
        BtnOk.Text = Strings.RuleDlg_BtnOk;
        BtnCancel.Text = Strings.RuleDlg_BtnCancel;

        CmbRuleType.Items.Clear();
        CmbRuleType.Items.AddRange([.. RuleTypes
            .Select(rt => (object)Strings.ResourceManager.GetString(rt.nameKey)!)
        ]);
    }

    protected override void OnLoad(EventArgs e)
    {
        powerSchemes =
        [
            .. PowerSchemeOrder.Apply(
                PowerManager.Api.GetPowerSchemes()
                    .Where(scheme => !string.IsNullOrWhiteSpace(scheme.name))
                    .Select(scheme => (guid: scheme.guid, name: scheme.name!)),
                scheme => scheme.guid,
                guid => PowerSchemeSettings.GetSetting(guid)?.Order)
        ];

        CmbPowerScheme.Items.AddRange([.. powerSchemes
            .Select(scheme => scheme.name)
            .Cast<object>()]);

        if (RuleDto is ProcessRuleDto processRuleDto)
        {
            CmbRuleType.SelectedIndex = 0;
            PrcProcessRule.Dto = processRuleDto;
        }
        else if (RuleDto is PowerLineRuleDto powerLineRuleDto)
        {
            CmbRuleType.SelectedIndex = 1;
            PlcPowerLineRule.Dto = powerLineRuleDto;
        }
        else if (RuleDto is IdleRuleDto idleRuleDto)
        {
            CmbRuleType.SelectedIndex = 2;
            IrcIdleRule.Dto = idleRuleDto;
        }
        else if (RuleDto is StartupRuleDto startupRuleDto)
        {
            CmbRuleType.SelectedIndex = 3;
            srcStartupRule.Dto = startupRuleDto;
        }
        else if (RuleDto is ShutdownRuleDto)
        {
            CmbRuleType.SelectedIndex = 4;
        }
        else
        {
            CmbRuleType.SelectedIndex = 0;
        }

        if (RuleDto is not null && RuleDto.SchemeGuid != Guid.Empty)
        {
            CmbPowerScheme.SelectedIndex = powerSchemes.FindIndex(
                scheme => scheme.guid == RuleDto.SchemeGuid);
        }
        else
        {
            CmbPowerScheme.SelectedIndex = 0;
        }

        base.OnLoad(e);
    }

    private void BtnOk_Click(object sender, EventArgs e)
    {
        var ruleType = RuleTypes[CmbRuleType.SelectedIndex].type;
        if (ruleType == typeof(ProcessRuleDto))
        {
            RuleDto = PrcProcessRule.Dto;
            RuleDto.SchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme));
            DialogResult = DialogResult.OK;
            return;
        }
        else if (ruleType == typeof(PowerLineRuleDto))
        {
            RuleDto = PlcPowerLineRule.Dto;
            RuleDto.SchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme));
            DialogResult = DialogResult.OK;
            return;
        }
        else if (ruleType == typeof(IdleRuleDto))
        {
            RuleDto = IrcIdleRule.Dto;
            RuleDto.SchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme));
            DialogResult = DialogResult.OK;
            return;
        }
        else if (ruleType == typeof(StartupRuleDto))
        {
            RuleDto = srcStartupRule.Dto;
            RuleDto.SchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme));
            DialogResult = DialogResult.OK;
            return;
        }
        else if (ruleType == typeof(ShutdownRuleDto))
        {
            RuleDto = new ShutdownRuleDto
            {
                SchemeGuid =
                    GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme))
            };
            DialogResult = DialogResult.OK;
            return;
        }

        _ = MessageBox.Show(
            Strings.RuleDlg_MsgSelectRuleType,
            Strings.RuleDlg_MsgInvalidInputTitle,
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private void CmbRuleType_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ruleType = RuleTypes[CmbRuleType.SelectedIndex].type;
        if (ruleType == typeof(ProcessRuleDto))
        {
            TipHints.SetToolTip(
                PibRuleInfo,
                Strings.RuleDlg_TipProcess);
            PrcProcessRule.Visible = true;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = false;
            srcStartupRule.Visible = false;
        }
        else if (ruleType == typeof(PowerLineRuleDto))
        {
            TipHints.SetToolTip(
                PibRuleInfo,
                Strings.RuleDlg_TipPowerLine);
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = true;
            IrcIdleRule.Visible = false;
            srcStartupRule.Visible = false;
        }
        else if (ruleType == typeof(IdleRuleDto))
        {
            TipHints.SetToolTip(
                PibRuleInfo,
                Strings.RuleDlg_TipIdle);
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = true;
            srcStartupRule.Visible = false;
        }
        else if (ruleType == typeof(StartupRuleDto))
        {
            TipHints.SetToolTip(
                PibRuleInfo,
                Strings.RuleDlg_TipStartup);
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = false;
            srcStartupRule.Visible = true;
        }
        else if (ruleType == typeof(ShutdownRuleDto))
        {
            TipHints.SetToolTip(
                PibRuleInfo,
                Strings.RuleDlg_TipShutdown);
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = false;
            srcStartupRule.Visible = false;
        }
        else
        {
            TipHints.SetToolTip(
                PibRuleInfo,
                Strings.RuleDlg_MsgSelectRuleType);
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = false;
            srcStartupRule.Visible = false;
        }
    }

    private void PibRuleInfo_Click(object sender, EventArgs e) =>
        TipHints.Show(
            TipHints.GetToolTip(PibRuleInfo),
            PibRuleInfo,
            0,
            PibRuleInfo.Height,
            3000);
}

