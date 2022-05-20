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
            this.DgvProcesses = new System.Windows.Forms.DataGridView();
            this.DgcProcessIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.DgcProcessId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgcProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgcProcessStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgcProcessPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DgvProcesses)).BeginInit();
            this.SuspendLayout();
            // 
            // DgvProcesses
            // 
            this.DgvProcesses.AllowUserToAddRows = false;
            this.DgvProcesses.AllowUserToDeleteRows = false;
            this.DgvProcesses.AllowUserToResizeRows = false;
            this.DgvProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvProcesses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvProcesses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DgcProcessIcon,
            this.DgcProcessId,
            this.DgcProcessName,
            this.DgcProcessStartTime,
            this.DgcProcessPath});
            this.DgvProcesses.Location = new System.Drawing.Point(12, 12);
            this.DgvProcesses.MultiSelect = false;
            this.DgvProcesses.Name = "DgvProcesses";
            this.DgvProcesses.RowHeadersVisible = false;
            this.DgvProcesses.RowTemplate.Height = 26;
            this.DgvProcesses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvProcesses.Size = new System.Drawing.Size(776, 397);
            this.DgvProcesses.TabIndex = 4;
            this.DgvProcesses.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleDgvProcessesCellDoubleClick);
            this.DgvProcesses.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.HandleDgvProcessesSortCompare);
            // 
            // DgcProcessIcon
            // 
            this.DgcProcessIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DgcProcessIcon.HeaderText = "";
            this.DgcProcessIcon.Name = "DgcProcessIcon";
            this.DgcProcessIcon.ReadOnly = true;
            this.DgcProcessIcon.Width = 5;
            // 
            // DgcProcessId
            // 
            this.DgcProcessId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DgcProcessId.HeaderText = "PID";
            this.DgcProcessId.Name = "DgcProcessId";
            this.DgcProcessId.ReadOnly = true;
            this.DgcProcessId.Width = 50;
            // 
            // DgcProcessName
            // 
            this.DgcProcessName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DgcProcessName.HeaderText = "Name";
            this.DgcProcessName.Name = "DgcProcessName";
            this.DgcProcessName.ReadOnly = true;
            // 
            // DgcProcessStartTime
            // 
            this.DgcProcessStartTime.HeaderText = "Start Time";
            this.DgcProcessStartTime.Name = "DgcProcessStartTime";
            this.DgcProcessStartTime.ReadOnly = true;
            // 
            // DgcProcessPath
            // 
            this.DgcProcessPath.HeaderText = "Path";
            this.DgcProcessPath.Name = "DgcProcessPath";
            this.DgcProcessPath.ReadOnly = true;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(713, 415);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 6;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.Location = new System.Drawing.Point(632, 415);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 5;
            this.BtnOk.Text = "OK";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.HandleBtnOkClick);
            // 
            // ProcessSelectionDlg
            // 
            this.AcceptButton = this.BtnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.DgvProcesses);
            this.Name = "ProcessSelectionDlg";
            this.Text = "PowerPlanSwitcher - Select Process";
            ((System.ComponentModel.ISupportInitialize)(this.DgvProcesses)).EndInit();
            this.ResumeLayout(false);

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