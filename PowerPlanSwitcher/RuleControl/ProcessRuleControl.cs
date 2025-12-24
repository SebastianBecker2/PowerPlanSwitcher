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
            dto.FilePath = TxtPath.Text;
            return dto;
        }

        set
        {
            dto = value;
            TxtPath.Text = dto.FilePath;
            CmbComparisonType.SelectedIndex = ComparisonTypes.IndexOf(dto.Type);
        }
    }
    private ProcessRuleDto dto = new();

    public ProcessRuleControl()
    {
        InitializeComponent();

        CmbComparisonType.Items.AddRange([.. ComparisonTypes
            .Select(ProcessRuleDto.ComparisonTypeToText)
            .Cast<object>()]);
        CmbComparisonType.SelectedIndex = 0;
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
}
