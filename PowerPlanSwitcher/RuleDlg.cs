namespace PowerPlanSwitcher;

using PowerManagement;
using RuleManagement.Dto;

public partial class RuleDlg : Form
{
    public IRuleDto? RuleDto { get; set; }

    private static readonly List<(Guid guid, string name)> PowerSchemes =
        [.. PowerManager.Static.GetPowerSchemes()
            .Where(scheme => !string.IsNullOrWhiteSpace(scheme.name))
            .Cast<(Guid schemeGuid, string name)>()];

    private static readonly List<(string name, Type type)> RuleTypes =
        [
            ("Process Rule", typeof(ProcessRuleDto)),
            ("Power Line Rule", typeof(PowerLineRuleDto)),
            ("Idle Rule", typeof(IdleRuleDto)),
            ("Startup Rule", typeof(StartupRuleDto)),
            ("Shutdown Rule", typeof(ShutdownRuleDto)),
        ];

    private static string GetSelectedString(ComboBox cmb) =>
        cmb.Items[cmb.SelectedIndex]?.ToString() ?? string.Empty;

    private static Guid GetPowerSchemeGuid(string name) =>
            PowerSchemes.First(scheme => scheme.name == name).guid;

    public RuleDlg() => InitializeComponent();

    protected override void OnLoad(EventArgs e)
    {
        CmbPowerScheme.Items.AddRange([.. PowerSchemes
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
        else if (RuleDto is StartupRuleDto)
        {
            CmbRuleType.SelectedIndex = 3;
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
            CmbPowerScheme.SelectedIndex = PowerSchemes.FindIndex(
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
            RuleDto = new StartupRuleDto
            {
                SchemeGuid =
                    GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme))
            };
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
            "Select a Rule Type!",
            "Invalid input",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private void CmbRuleType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CmbRuleType.SelectedIndex == 0)
        {
            PrcProcessRule.Visible = true;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = false;
        }
        else if (CmbRuleType.SelectedIndex == 1)
        {
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = true;
            IrcIdleRule.Visible = false;
        }
        else if (CmbRuleType.SelectedIndex == 2)
        {
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = true;
        }
        else
        {
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = false;
        }
    }
}
