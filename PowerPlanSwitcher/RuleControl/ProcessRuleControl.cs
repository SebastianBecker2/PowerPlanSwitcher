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
            ComparisonType.Exact =>
                $"The processes execution path has to match the pattern completely." +
                $"{Environment.NewLine}Ignoring case.",
            ComparisonType.StartsWith =>
                $"The processes execution path has to start with the provided pattern." +
                $"{Environment.NewLine}Ignoring case.",
            ComparisonType.EndsWith =>
                $"The processes execution path has to end with the provided pattern." +
                $"{Environment.NewLine}Ignoring case.",
            ComparisonType.Wildcard =>
                $"The process’s execution path must match the provided wildcard pattern, using glob‑style matching." +
                $"{Environment.NewLine}Supports * (match within one folder), ? (single character), and ** (match across any number of folders)." +
                $"{Environment.NewLine}C:\\Program Files\\*.exe → matches any exe files directly in C:\\Program Files\\" +
                $"{Environment.NewLine}C:\\Program Files\\**\\*.exe → matches any exe files in any subfolder of C:\\Program Files\\" +
                $"{Environment.NewLine}C:\\Program Files\\My?pp.exe → matches MyApp.exe and MySpp.exe in C:\\Program Files\\" +
                $"{Environment.NewLine}Ignoring case.",
            _ => string.Empty,
        };

        TipHints.SetToolTip(PibComparisonInfo, text);
    }
}
