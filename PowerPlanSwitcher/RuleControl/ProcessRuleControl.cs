namespace PowerPlanSwitcher.Rule;

using Microsoft.WindowsAPICodePack.Dialogs;
using RuleManagement;
using RuleManagement.Dto;

public partial class ProcessRuleControl : UserControl
{
    private static readonly List<ComparisonType> ComparisonTypes =
        [.. Enum.GetValues(typeof(ComparisonType)).Cast<ComparisonType>()];

    public ProcessRuleDto Dto
    {
        get
        {
            dto.Type = ComparisonTypes[CmbComparisonType.SelectedIndex];
            dto.Pattern = TxtPath.Text.ToLowerInvariant();
            return dto;
        }

        set
        {
            dto = value;
            TxtPath.Text = dto.Pattern;
            CmbComparisonType.SelectedIndex = ComparisonTypes.IndexOf(dto.Type);
        }
    }
    private ProcessRuleDto dto = new();

    public ProcessRuleControl()
    {
        InitializeComponent();
        ApplyLocalization();

        CmbComparisonType.Items.AddRange([.. ComparisonTypes
            .Select(ProcessRuleDto.ComparisonTypeToText)
            .Cast<object>()]);
        CmbComparisonType.SelectedIndex = 0;
    }

    private void ApplyLocalization()
    {
        label1.Text = Strings.ProcessRuleControl_LblComparison;
        label2.Text = Strings.ProcessRuleControl_LblPattern;
        BtnSelectFile.Text = Strings.ProcessRuleControl_BtnSelectFile;
        BtnSelectFolder.Text = Strings.ProcessRuleControl_BtnSelectFolder;
        BtnSelectFromProcess.Text = Strings.ProcessRuleControl_BtnSelectFromProcess;
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

    private void PibComparisonInfo_Click(object sender, EventArgs e) =>
        TipHints.Show(
            TipHints.GetToolTip(PibComparisonInfo),
            PibComparisonInfo,
            0,
            PibComparisonInfo.Height,
            3000);

    private void CmbComparisonType_SelectedIndexChanged(object sender, EventArgs e)
    {
        var comparisonType = ComparisonTypes[CmbComparisonType.SelectedIndex];

        var text = comparisonType switch
        {
            ComparisonType.Exact => Strings.ProcessRuleControl_CmpExactTip,
            ComparisonType.StartsWith => Strings.ProcessRuleControl_CmpStartsWithTip,
            ComparisonType.EndsWith => Strings.ProcessRuleControl_CmpEndsWithTip,
            ComparisonType.Wildcard => Strings.ProcessRuleControl_CmpWildcardTip,
            _ => string.Empty,
        };

        TipHints.SetToolTip(PibComparisonInfo, text);
    }
}
