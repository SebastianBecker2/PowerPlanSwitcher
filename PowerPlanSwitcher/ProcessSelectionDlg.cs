namespace PowerPlanSwitcher
{
    using System.ComponentModel;
    using System.Diagnostics;

    public partial class ProcessSelectionDlg : Form
    {
        public CachingProcess? SelectedProcess { get; set; }

        public ProcessSelectionDlg() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {
            UpdateProcesses();
            base.OnLoad(e);
        }

        private void UpdateProcesses()
        {
            DgvProcesses.Rows.Clear();

            var processes = ProcessMonitor.GetOwnedProcesses()
                .OrderByDescending(p =>
                {
                    try
                    {
                        return p.StartTime;
                    }
                    catch (Win32Exception) { }
                    catch (InvalidOperationException) { }

                    return DateTime.MinValue;
                });

            foreach (var process in processes)
            {
                try
                {
                    var row = new DataGridViewRow { Tag = process, };
                    var fileName = process.FileName;
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        continue;
                    }

                    row.Cells.AddRange(
                        new DataGridViewImageCell
                        {
                            Value = Icon.ExtractAssociatedIcon(fileName),
                            ImageLayout = DataGridViewImageCellLayout.Zoom,
                        },
                        new DataGridViewTextBoxCell
                        {
                            Value = process.Id,
                        },
                        new DataGridViewTextBoxCell
                        {
                            Value = process.ProcessName,
                        },
                        new DataGridViewTextBoxCell
                        {
                            Value = process.StartTime,
                        },
                        new DataGridViewTextBoxCell
                        {
                            Value = fileName,
                        });

                    _ = DgvProcesses.Rows.Add(row);
                }
                catch (Win32Exception)
                {
                    Debug.Print($"Couldn't get process module of " +
                        $"{process.ProcessName}");
                }
                catch (InvalidOperationException)
                {
                    Debug.Print($"Process {process.ProcessName} just exited");
                }
            }
        }

        private void HandleBtnOkClick(object sender, EventArgs e)
        {
            if (DgvProcesses.SelectedRows.Count == 0)
            {
                return;
            }

            SelectedProcess = DgvProcesses.SelectedRows[0].Tag as CachingProcess;
            DialogResult = DialogResult.OK;
        }

        private void HandleDgvProcessesSortCompare(
            object sender,
            DataGridViewSortCompareEventArgs e)
        {
            var process1 = DgvProcesses.Rows[e.RowIndex1].Tag as CachingProcess;
            Debug.Assert(process1 is not null);
            var process2 = DgvProcesses.Rows[e.RowIndex2].Tag as CachingProcess;
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
