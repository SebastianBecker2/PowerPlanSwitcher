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
        var ruleType = RuleTypes[CmbRuleType.SelectedIndex].type;
        if (ruleType == typeof(ProcessRuleDto))
        {
            TipHints.SetToolTip(
                PibRuleInfo,
                "A process rule switches power plans based on the presence of a specified process running on the system.");
            PrcProcessRule.Visible = true;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = false;
        }
        else if (ruleType == typeof(PowerLineRuleDto))
        {
            TipHints.SetToolTip(
                PibRuleInfo,
                "A power line rule switches power plans based on the current power line status (plugged in or on battery).");
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = true;
            IrcIdleRule.Visible = false;
        }
        else if (ruleType == typeof(IdleRuleDto))
        {
            TipHints.SetToolTip(
                PibRuleInfo,
                "An idle rule switches power plans based on the duration without user input.");
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = true;
        }
        else if (ruleType == typeof(StartupRuleDto))
        {
            TipHints.SetToolTip(
                PibRuleInfo,
                $"A startup rule switches power plans when the system starts up." +
                $"{Environment.NewLine}This rule is always triggered and should be the last rule in the list.");
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = false;
        }
        else if (ruleType == typeof(ShutdownRuleDto))
        {
            TipHints.SetToolTip(
                PibRuleInfo,
                "A shutdown rule switches power plans when the system is shutting down.");
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = false;
        }
        else
        {
            TipHints.SetToolTip(
                PibRuleInfo,
                "Select a Rule Type!");
            PrcProcessRule.Visible = false;
            PlcPowerLineRule.Visible = false;
            IrcIdleRule.Visible = false;
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
