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
            RdbProcessRule.Checked = true;
            PrcProcessRule.Dto = processRuleDto;
        }

        if (RuleDto is PowerLineRuleDto powerLineRuleDto)
        {
            RdbPowerLineRule.Checked = true;
            PlcPowerLineRule.Dto = powerLineRuleDto;
        }

        if (RuleDto is IdleRuleDto idleRuleDto)
        {
            RdbIdleRule.Checked = true;
            IrcIdleRule.Dto = idleRuleDto;
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
        if (RdbProcessRule.Checked)
        {
            RuleDto = PrcProcessRule.Dto;
            RuleDto.SchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme));
            DialogResult = DialogResult.OK;
            return;
        }
        else if (RdbPowerLineRule.Checked)
        {
            RuleDto = PlcPowerLineRule.Dto;
            RuleDto.SchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme));
            DialogResult = DialogResult.OK;
            return;
        }
        else if (RdbIdleRule.Checked)
        {
            RuleDto = IrcIdleRule.Dto;
            RuleDto.SchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme));
            DialogResult = DialogResult.OK;
            return;
        }

        _ = MessageBox.Show(
            "Select a Rule Type!",
            "Invalid input",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private void RdbProcessRule_CheckedChanged(object sender, EventArgs e) =>
        PrcProcessRule.Visible = ((RadioButton)sender).Checked;

    private void RdbPowerLineRule_CheckedChanged(object sender, EventArgs e) =>
        PlcPowerLineRule.Visible = ((RadioButton)sender).Checked;

    private void RdbIdleRule_CheckedChanged(object sender, EventArgs e) =>
        IrcIdleRule.Visible = ((RadioButton)sender).Checked;
}
