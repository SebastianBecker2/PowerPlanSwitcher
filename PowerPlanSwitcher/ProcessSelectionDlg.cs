namespace PowerPlanSwitcher;

using ProcessManagement;

public partial class ProcessSelectionDlg : Form
{
    public IProcess? SelectedProcess { get; set; }

    public ProcessSelectionDlg() => InitializeComponent();

    protected override void OnLoad(EventArgs e)
    {
        UpdateProcesses();
        base.OnLoad(e);
    }

    private static DataGridViewRow? ProcessToRow(IProcess process)
    {
        var row = new DataGridViewRow
        {
            Tag = process,
        };

        var fileName = process.ExecutablePath;
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return null;
        }

        row.Cells.AddRange(
            new DataGridViewImageCell
            {
                Value = Icon.ExtractAssociatedIcon(fileName),
                ImageLayout = DataGridViewImageCellLayout.Zoom,
            },
            new DataGridViewTextBoxCell
            {
                Value = process.ProcessId,
            },
            new DataGridViewTextBoxCell
            {
                Value = process.ProcessName,
            },
            //new DataGridViewTextBoxCell
            //{
            //    Value = process.MainWindowTitle,
            //},
            //new DataGridViewTextBoxCell
            //{
            //    Value = process.BasePriority,
            //},
            //new DataGridViewTextBoxCell
            //{
            //    Value = process.PriorityClass,
            //},
            new DataGridViewTextBoxCell
            {
                Value = process.StartTime,
            },
            new DataGridViewTextBoxCell
            {
                Value = fileName,
            });
        return row;
    }

    private void UpdateProcesses()
    {
        DgvProcesses.Rows.Clear();

        foreach (var process in
            ProcessMonitor.Static.GetUsersProcesses()
            .OrderByDescending(p => p.StartTime))
        {
            var row = ProcessToRow(process);
            if (row is null)
            {
                continue;
            }
            _ = DgvProcesses.Rows.Add(row);
        }
    }

    private void HandleBtnOkClick(object sender, EventArgs e)
    {
        if (DgvProcesses.SelectedRows.Count == 0)
        {
            return;
        }

        SelectedProcess =
            (DgvProcesses.SelectedRows[0].Tag as IProcess)!;
        DialogResult = DialogResult.OK;
    }

    private void HandleDgvProcessesSortCompare(
        object sender,
        DataGridViewSortCompareEventArgs e)
    {
        var process1 = DgvProcesses.Rows[e.RowIndex1].Tag as IProcess;
        System.Diagnostics.Debug.Assert(process1 is not null);
        var process2 = DgvProcesses.Rows[e.RowIndex2].Tag as IProcess;
        System.Diagnostics.Debug.Assert(process2 is not null);

        if (e.Column == DgcProcessStartTime)
        {
            e.Handled = true;
            e.SortResult =
                process1.StartTime < process2.StartTime
                ? -1
                : 1;
        }
    }

    private void HandleDgvProcessesCellDoubleClick(
        object sender,
        DataGridViewCellEventArgs e) => BtnOk.PerformClick();
}
