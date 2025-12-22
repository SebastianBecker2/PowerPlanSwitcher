namespace PowerPlanSwitcher;

using Microsoft.WindowsAPICodePack.Dialogs;
using PowerManagement;
using RuleManagement;
using RuleManagement.Rules;

public partial class RuleDlg : Form
{
    public IRuleDto? RuleDto { get; set; }

    private static readonly List<(Guid guid, string name)> PowerSchemes =
        [.. PowerManager.Static.GetPowerSchemes()
            .Where(scheme => !string.IsNullOrWhiteSpace(scheme.name))
            .Cast<(Guid schemeGuid, string name)>()];

    private static readonly List<ComparisonType> ComparisonTypes =
        [.. Enum.GetValues(typeof(ComparisonType)).Cast<ComparisonType>()];

    private static readonly List<PowerLineStatus> PowerLineStatuses =
        [.. Enum.GetValues(typeof(PowerLineStatus)).Cast<PowerLineStatus>()];

    private static string GetSelectedString(ComboBox cmb) =>
        cmb.Items[cmb.SelectedIndex]?.ToString() ?? string.Empty;

    private static Guid GetPowerSchemeGuid(string name) =>
            PowerSchemes.First(scheme => scheme.name == name).guid;

    public RuleDlg() => InitializeComponent();

    protected override void OnLoad(EventArgs e)
    {
        LblComparisonType.Visible = false;
        CmbComparisonType.Visible = false;
        LblPath.Visible = false;
        TxtPath.Visible = false;
        BtnSelectFile.Visible = false;
        BtnSelectFolder.Visible = false;
        BtnSelectFromProcess.Visible = false;
        LblPowerLineStatus.Visible = false;
        CmbPowerLineStatus.Visible = false;

        CmbComparisonType.Items.AddRange([.. ComparisonTypes
            .Select(ProcessRuleDto.ComparisonTypeToText)
            .Cast<object>()]);

        CmbPowerLineStatus.Items.AddRange([.. PowerLineStatuses
            .Select(PowerLineRuleDto.PowerLineStatusToText)
            .Cast<object>()]);

        CmbPowerScheme.Items.AddRange([.. PowerSchemes
            .Select(scheme => scheme.name)
            .Cast<object>()]);

        if (RuleDto is ProcessRuleDto processRuleDto)
        {
            RdbProcessRule.Checked = true;
            CmbComparisonType.SelectedIndex =
                ComparisonTypes.IndexOf(processRuleDto.Type);
            TxtPath.Text = processRuleDto.FilePath;
        }
        else
        {
            CmbComparisonType.SelectedIndex = 0;
        }

        if (RuleDto is PowerLineRuleDto powerLineRuleDto)
        {
            RdbPowerLineRule.Checked = true;
            CmbPowerLineStatus.SelectedIndex =
                PowerLineStatuses.IndexOf(powerLineRuleDto.PowerLineStatus);
        }
        else
        {
            CmbPowerLineStatus.SelectedIndex = 0;
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
            if (ApplyProcessRule())
            {
                DialogResult = DialogResult.OK;
            }
            return;
        }
        else if (RdbPowerLineRule.Checked)
        {
            if (ApplyPowerLineRule())
            {
                DialogResult = DialogResult.OK;
            }
            return;
        }

        _ = MessageBox.Show(
            "Select a Rule Type!",
            "Invalid input",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private bool ApplyPowerLineRule()
    {
        RuleDto = new PowerLineRuleDto()
        {
            PowerLineStatus =
                PowerLineRuleDto.TextToPowerLineStatus(
                    GetSelectedString(CmbPowerLineStatus)),
            SchemeGuid =
                GetPowerSchemeGuid(
                    GetSelectedString(CmbPowerScheme))
        };
        return true;
    }

    private bool ApplyProcessRule()
    {
        if (string.IsNullOrWhiteSpace(TxtPath.Text))
        {
            _ = MessageBox.Show(
                "Path/File must not be empty!",
                "Invalid input",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }

        RuleDto = new ProcessRuleDto
        {
            Type = ProcessRuleDto.TextToComparisonType(GetSelectedString(CmbComparisonType)),
            FilePath = TxtPath.Text,
            SchemeGuid = GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme))
        };
        return true;
    }

    private void BtnSelectPath_Click(object sender, EventArgs e)
    {
        using var dlg = new CommonOpenFileDialog
        {
            IsFolderPicker = false,
        };

        if (dlg.ShowDialog() != CommonFileDialogResult.Ok)
        {
            return;
        }

        TxtPath.Text = dlg.FileName;
    }

    private void RdbProcessRule_CheckedChanged(object sender, EventArgs e)
    {
        LblComparisonType.Visible = RdbProcessRule.Checked;
        CmbComparisonType.Visible = RdbProcessRule.Checked;
        LblPath.Visible = RdbProcessRule.Checked;
        TxtPath.Visible = RdbProcessRule.Checked;
        BtnSelectFile.Visible = RdbProcessRule.Checked;
        BtnSelectFolder.Visible = RdbProcessRule.Checked;
        BtnSelectFromProcess.Visible = RdbProcessRule.Checked;
    }

    private void RdbPowerLineRule_CheckedChanged(object sender, EventArgs e)
    {
        LblPowerLineStatus.Visible = RdbPowerLineRule.Checked;
        CmbPowerLineStatus.Visible = RdbPowerLineRule.Checked;
    }

    private void BtnSelectFromProcess_Click(object sender, EventArgs e)
    {
        using var processSelectionDlg = new ProcessSelectionDlg();
        if (processSelectionDlg.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        TxtPath.Text = processSelectionDlg.SelectedProcess!.ExecutablePath;
    }

    private void BtnSelectFolder_Click(object sender, EventArgs e)
    {
        using var dlg = new CommonOpenFileDialog
        {
            IsFolderPicker = true,
        };

        if (dlg.ShowDialog() != CommonFileDialogResult.Ok)
        {
            return;
        }

        TxtPath.Text = dlg.FileName;
    }
}
