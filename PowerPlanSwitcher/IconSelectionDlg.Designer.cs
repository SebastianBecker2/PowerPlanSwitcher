namespace PowerPlanSwitcher
{
    partial class IconSelectionDlg
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(IconSelectionDlg));
            BtnCancel = new Button();
            BtnOk = new Button();
            TxtFilter = new TextBox();
            label1 = new Label();
            BtnSelectFile = new Button();
            DgvIcons = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)DgvIcons).BeginInit();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Location = new Point(639, 402);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 6;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnOk
            // 
            BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOk.Location = new Point(558, 402);
            BtnOk.Name = "BtnOk";
            BtnOk.Size = new Size(75, 23);
            BtnOk.TabIndex = 5;
            BtnOk.Text = "OK";
            BtnOk.UseVisualStyleBackColor = true;
            BtnOk.Click += BtnOk_Click;
            // 
            // TxtFilter
            // 
            TxtFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            TxtFilter.Location = new Point(54, 12);
            TxtFilter.Name = "TxtFilter";
            TxtFilter.Size = new Size(131, 23);
            TxtFilter.TabIndex = 8;
            TxtFilter.TextChanged += TxtFilter_TextChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(36, 15);
            label1.TabIndex = 9;
            label1.Text = "Filter:";
            // 
            // BtnSelectFile
            // 
            BtnSelectFile.Location = new Point(594, 12);
            BtnSelectFile.Name = "BtnSelectFile";
            BtnSelectFile.Size = new Size(120, 23);
            BtnSelectFile.TabIndex = 10;
            BtnSelectFile.Text = "Select from file...";
            BtnSelectFile.UseVisualStyleBackColor = true;
            BtnSelectFile.Click += BtnSelectFile_Click;
            // 
            // DgvIcons
            // 
            DgvIcons.AllowUserToAddRows = false;
            DgvIcons.AllowUserToDeleteRows = false;
            DgvIcons.AllowUserToResizeColumns = false;
            DgvIcons.AllowUserToResizeRows = false;
            DgvIcons.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DgvIcons.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DgvIcons.EditMode = DataGridViewEditMode.EditProgrammatically;
            DgvIcons.Location = new Point(12, 41);
            DgvIcons.MultiSelect = false;
            DgvIcons.Name = "DgvIcons";
            DgvIcons.RowHeadersVisible = false;
            DgvIcons.RowTemplate.Height = 64;
            DgvIcons.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DgvIcons.Size = new Size(702, 355);
            DgvIcons.TabIndex = 11;
            DgvIcons.VirtualMode = true;
            DgvIcons.CellDoubleClick += DgvIcons_CellDoubleClick;
            DgvIcons.CellValueNeeded += DgvIcons_CellValueNeeded;
            // 
            // IconSelectionDlg
            // 
            AcceptButton = BtnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(726, 437);
            Controls.Add(DgvIcons);
            Controls.Add(BtnSelectFile);
            Controls.Add(label1);
            Controls.Add(TxtFilter);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "IconSelectionDlg";
            Text = "PowerPlanSwitcher - Icon selection";
            ((System.ComponentModel.ISupportInitialize)DgvIcons).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button BtnCancel;
        private Button BtnOk;
        private TextBox TxtFilter;
        private Label label1;
        private Button BtnSelectFile;
        private DataGridView DgvIcons;
    }
}