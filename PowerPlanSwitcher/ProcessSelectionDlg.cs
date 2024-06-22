namespace PowerPlanSwitcher
{
    using System.ComponentModel;
    using System.Diagnostics;

    public partial class ProcessSelectionDlg : Form
    {
        private sealed class SortableProcess
        {
            public CachedProcess Process { get; }
            public DateTime StartTime { get; } = DateTime.MinValue;

            public SortableProcess(CachedProcess process)
            {
                Process = process;
                try
                {
                    StartTime = System.Diagnostics.Process
                        .GetProcessById(process.ProcessId)
                        .StartTime;
                }
                catch (ArgumentException) { }
                catch (Win32Exception) { }
                catch (InvalidOperationException) { }
            }
        }

        public CachedProcess? SelectedProcess { get; set; }

        public ProcessSelectionDlg() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {
            UpdateProcesses();
            base.OnLoad(e);
        }

        private static DataGridViewRow? ProcessWithStartTimeToRow(
            SortableProcess sortableProcess)
        {
            var row = new DataGridViewRow
            {
                Tag = sortableProcess,
            };

            var fileName = sortableProcess.Process.ExecutablePath;
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
                    Value = sortableProcess.Process.ProcessId,
                },
                new DataGridViewTextBoxCell
                {
                    Value = sortableProcess.Process.ProcessName,
                },
                new DataGridViewTextBoxCell
                {
                    Value = sortableProcess.StartTime,
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

            foreach (var sortableProcess in
                ProcessMonitor.GetUsersProcesses()
                .Select(p => new SortableProcess(p))
                .OrderByDescending(t => t.StartTime))
            {
                try
                {
                    var row = ProcessWithStartTimeToRow(sortableProcess);
                    if (row is null)
                    {
                        continue;
                    }
                    _ = DgvProcesses.Rows.Add(row);
                }
                catch (Win32Exception)
                {
                    Debug.Print($"Couldn't get process module of " +
                        $"{sortableProcess.Process.ProcessName}");
                }
                catch (InvalidOperationException)
                {
                    Debug.Print($"Process " +
                        $"{sortableProcess.Process.ProcessName} just exited");
                }
            }
        }

        private void HandleBtnOkClick(object sender, EventArgs e)
        {
            if (DgvProcesses.SelectedRows.Count == 0)
            {
                return;
            }

            SelectedProcess = DgvProcesses.SelectedRows[0].Tag as CachedProcess;
            DialogResult = DialogResult.OK;
        }

        private void HandleDgvProcessesSortCompare(
            object sender,
            DataGridViewSortCompareEventArgs e)
        {
            var process1 = DgvProcesses.Rows[e.RowIndex1].Tag as SortableProcess;
            Debug.Assert(process1 is not null);
            var process2 = DgvProcesses.Rows[e.RowIndex2].Tag as SortableProcess;
            Debug.Assert(process2 is not null);

            if (e.Column == DgcProcessStartTime)
            {
                e.Handled = true;
                e.SortResult = process1.StartTime < process2.StartTime ? -1 : 1;
            }
        }

        private void HandleDgvProcessesCellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e) => BtnOk.PerformClick();
    }
}
