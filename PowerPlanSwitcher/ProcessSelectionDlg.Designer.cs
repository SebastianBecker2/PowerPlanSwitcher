namespace PowerPlanSwitcher
{
    partial class ProcessSelectionDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessSelectionDlg));
            DgvProcesses = new DataGridView();
            BtnCancel = new Button();
            BtnOk = new Button();
            DgcProcessIcon = new DataGridViewImageColumn();
            DgcProcessId = new DataGridViewTextBoxColumn();
            DgcProcessName = new DataGridViewTextBoxColumn();
            DgcProcessStartTime = new DataGridViewTextBoxColumn();
            DgcProcessPath = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)DgvProcesses).BeginInit();
            SuspendLayout();
            // 
            // DgvProcesses
            // 
            DgvProcesses.AllowUserToAddRows = false;
            DgvProcesses.AllowUserToDeleteRows = false;
            DgvProcesses.AllowUserToResizeRows = false;
            DgvProcesses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DgvProcesses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DgvProcesses.Columns.AddRange(new DataGridViewColumn[] { DgcProcessIcon, DgcProcessId, DgcProcessName, DgcProcessStartTime, DgcProcessPath });
            DgvProcesses.Location = new Point(12, 12);
            DgvProcesses.MultiSelect = false;
            DgvProcesses.Name = "DgvProcesses";
            DgvProcesses.RowHeadersVisible = false;
            DgvProcesses.RowTemplate.Height = 26;
            DgvProcesses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvProcesses.Size = new Size(776, 397);
            DgvProcesses.TabIndex = 4;
            DgvProcesses.CellDoubleClick += HandleDgvProcessesCellDoubleClick;
            DgvProcesses.SortCompare += HandleDgvProcessesSortCompare;
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Location = new Point(713, 415);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 6;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnOk
            // 
            BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOk.Location = new Point(632, 415);
            BtnOk.Name = "BtnOk";
            BtnOk.Size = new Size(75, 23);
            BtnOk.TabIndex = 5;
            BtnOk.Text = "OK";
            BtnOk.UseVisualStyleBackColor = true;
            BtnOk.Click += HandleBtnOkClick;
            // 
            // DgcProcessIcon
            // 
            DgcProcessIcon.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcProcessIcon.HeaderText = "";
            DgcProcessIcon.Name = "DgcProcessIcon";
            DgcProcessIcon.ReadOnly = true;
            DgcProcessIcon.Width = 5;
            // 
            // DgcProcessId
            // 
            DgcProcessId.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcProcessId.HeaderText = "PID";
            DgcProcessId.Name = "DgcProcessId";
            DgcProcessId.ReadOnly = true;
            DgcProcessId.Width = 50;
            // 
            // DgcProcessName
            // 
            DgcProcessName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcProcessName.HeaderText = "Name";
            DgcProcessName.Name = "DgcProcessName";
            DgcProcessName.ReadOnly = true;
            DgcProcessName.Width = 64;
            // 
            // DgcProcessStartTime
            // 
            DgcProcessStartTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcProcessStartTime.HeaderText = "Start Time";
            DgcProcessStartTime.Name = "DgcProcessStartTime";
            DgcProcessStartTime.ReadOnly = true;
            DgcProcessStartTime.Width = 86;
            // 
            // DgcProcessPath
            // 
            DgcProcessPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcProcessPath.HeaderText = "Path";
            DgcProcessPath.Name = "DgcProcessPath";
            DgcProcessPath.ReadOnly = true;
            DgcProcessPath.Width = 56;
            // 
            // ProcessSelectionDlg
            // 
            AcceptButton = BtnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(800, 450);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOk);
            Controls.Add(DgvProcesses);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ProcessSelectionDlg";
            Text = "PowerPlanSwitcher - Select Process";
            ((System.ComponentModel.ISupportInitialize)DgvProcesses).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView DgvProcesses;
        private Button BtnCancel;
        private Button BtnOk;
        private DataGridViewImageColumn DgcProcessIcon;
        private DataGridViewTextBoxColumn DgcProcessId;
        private DataGridViewTextBoxColumn DgcProcessName;
        private DataGridViewTextBoxColumn DgcProcessStartTime;
        private DataGridViewTextBoxColumn DgcProcessPath;
    }
}