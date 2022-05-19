namespace PowerPlanSwitcher
{
    partial class SettingsDlg
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDlg));
            this.DgvPowerSchemes = new System.Windows.Forms.DataGridView();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.DgcVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DgcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgcIcon = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DgvPowerSchemes)).BeginInit();
            this.SuspendLayout();
            // 
            // DgvPowerSchemes
            // 
            this.DgvPowerSchemes.AllowUserToAddRows = false;
            this.DgvPowerSchemes.AllowUserToDeleteRows = false;
            this.DgvPowerSchemes.AllowUserToResizeColumns = false;
            this.DgvPowerSchemes.AllowUserToResizeRows = false;
            this.DgvPowerSchemes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvPowerSchemes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvPowerSchemes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DgcVisible,
            this.DgcName,
            this.DgcIcon});
            this.DgvPowerSchemes.Location = new System.Drawing.Point(12, 12);
            this.DgvPowerSchemes.MultiSelect = false;
            this.DgvPowerSchemes.Name = "DgvPowerSchemes";
            this.DgvPowerSchemes.RowHeadersVisible = false;
            this.DgvPowerSchemes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvPowerSchemes.Size = new System.Drawing.Size(434, 203);
            this.DgvPowerSchemes.TabIndex = 0;
            this.DgvPowerSchemes.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.HandleDgvPowerSchemesCellMouseDown);
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.Location = new System.Drawing.Point(290, 221);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 1;
            this.BtnOk.Text = "OK";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(371, 221);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // DgcVisible
            // 
            this.DgcVisible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DgcVisible.Frozen = true;
            this.DgcVisible.HeaderText = "Visible";
            this.DgcVisible.Name = "DgcVisible";
            this.DgcVisible.ReadOnly = true;
            this.DgcVisible.Width = 47;
            // 
            // DgcName
            // 
            this.DgcName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DgcName.HeaderText = "Name";
            this.DgcName.Name = "DgcName";
            this.DgcName.ReadOnly = true;
            // 
            // DgcIcon
            // 
            this.DgcIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DgcIcon.HeaderText = "Icon";
            this.DgcIcon.Name = "DgcIcon";
            this.DgcIcon.ReadOnly = true;
            this.DgcIcon.Width = 36;
            // 
            // SettingsDlg
            // 
            this.AcceptButton = this.BtnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(458, 256);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.DgvPowerSchemes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsDlg";
            this.Text = "PowerPlanSwitcher - Settings";
            ((System.ComponentModel.ISupportInitialize)(this.DgvPowerSchemes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView DgvPowerSchemes;
        private Button BtnOk;
        private Button BtnCancel;
        private DataGridViewCheckBoxColumn DgcVisible;
        private DataGridViewTextBoxColumn DgcName;
        private DataGridViewImageColumn DgcIcon;
    }
}
