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
            this.DgcVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DgcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgcIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnCreateRuleFromProcess = new System.Windows.Forms.Button();
            this.DgvPowerRules = new System.Windows.Forms.DataGridView();
            this.DgcRuleIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgcRuleType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgcRulePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgcRuleSchemeIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.DgcRuleSchemeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnAddPowerRule = new System.Windows.Forms.Button();
            this.BtnEditPowerRule = new System.Windows.Forms.Button();
            this.BtnAscentPowerRule = new System.Windows.Forms.Button();
            this.BtnDescentPowerRule = new System.Windows.Forms.Button();
            this.BtnDeletePowerRule = new System.Windows.Forms.Button();
            this.ChbActivateInitialPowerScheme = new System.Windows.Forms.CheckBox();
            this.CmbInitialPowerScheme = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NudPowerRuleCheckInterval = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DgvPowerSchemes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvPowerRules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudPowerRuleCheckInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // DgvPowerSchemes
            // 
            this.DgvPowerSchemes.AllowUserToAddRows = false;
            this.DgvPowerSchemes.AllowUserToDeleteRows = false;
            this.DgvPowerSchemes.AllowUserToResizeColumns = false;
            this.DgvPowerSchemes.AllowUserToResizeRows = false;
            this.DgvPowerSchemes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
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
            this.DgvPowerSchemes.RowTemplate.Height = 26;
            this.DgvPowerSchemes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvPowerSchemes.Size = new System.Drawing.Size(473, 229);
            this.DgvPowerSchemes.TabIndex = 0;
            this.DgvPowerSchemes.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.HandleDgvPowerSchemesCellMouseDown);
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
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.Location = new System.Drawing.Point(928, 518);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 1;
            this.BtnOk.Text = "OK";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.HandleBtnOkClick);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(1009, 518);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnCreateRuleFromProcess
            // 
            this.BtnCreateRuleFromProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCreateRuleFromProcess.Location = new System.Drawing.Point(766, 12);
            this.BtnCreateRuleFromProcess.Name = "BtnCreateRuleFromProcess";
            this.BtnCreateRuleFromProcess.Size = new System.Drawing.Size(156, 59);
            this.BtnCreateRuleFromProcess.TabIndex = 6;
            this.BtnCreateRuleFromProcess.Text = "Create Rule from active Process";
            this.BtnCreateRuleFromProcess.UseVisualStyleBackColor = true;
            this.BtnCreateRuleFromProcess.Click += new System.EventHandler(this.HandleBtnCreateRuleFromProcessClick);
            // 
            // DgvPowerRules
            // 
            this.DgvPowerRules.AllowUserToAddRows = false;
            this.DgvPowerRules.AllowUserToDeleteRows = false;
            this.DgvPowerRules.AllowUserToResizeColumns = false;
            this.DgvPowerRules.AllowUserToResizeRows = false;
            this.DgvPowerRules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvPowerRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvPowerRules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DgcRuleIndex,
            this.DgcRuleType,
            this.DgcRulePath,
            this.DgcRuleSchemeIcon,
            this.DgcRuleSchemeName});
            this.DgvPowerRules.Location = new System.Drawing.Point(12, 247);
            this.DgvPowerRules.MultiSelect = false;
            this.DgvPowerRules.Name = "DgvPowerRules";
            this.DgvPowerRules.RowHeadersVisible = false;
            this.DgvPowerRules.RowTemplate.Height = 26;
            this.DgvPowerRules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvPowerRules.Size = new System.Drawing.Size(1072, 265);
            this.DgvPowerRules.TabIndex = 7;
            // 
            // DgcRuleIndex
            // 
            this.DgcRuleIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DgcRuleIndex.HeaderText = "Index";
            this.DgcRuleIndex.Name = "DgcRuleIndex";
            this.DgcRuleIndex.ReadOnly = true;
            this.DgcRuleIndex.Width = 61;
            // 
            // DgcRuleType
            // 
            this.DgcRuleType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DgcRuleType.HeaderText = "Type";
            this.DgcRuleType.Name = "DgcRuleType";
            this.DgcRuleType.ReadOnly = true;
            this.DgcRuleType.Width = 56;
            // 
            // DgcRulePath
            // 
            this.DgcRulePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DgcRulePath.HeaderText = "Path/File";
            this.DgcRulePath.Name = "DgcRulePath";
            this.DgcRulePath.ReadOnly = true;
            // 
            // DgcRuleSchemeIcon
            // 
            this.DgcRuleSchemeIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DgcRuleSchemeIcon.HeaderText = "Icon";
            this.DgcRuleSchemeIcon.Name = "DgcRuleSchemeIcon";
            this.DgcRuleSchemeIcon.ReadOnly = true;
            this.DgcRuleSchemeIcon.Width = 36;
            // 
            // DgcRuleSchemeName
            // 
            this.DgcRuleSchemeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DgcRuleSchemeName.HeaderText = "Power Plan";
            this.DgcRuleSchemeName.Name = "DgcRuleSchemeName";
            this.DgcRuleSchemeName.ReadOnly = true;
            this.DgcRuleSchemeName.Width = 91;
            // 
            // BtnAddPowerRule
            // 
            this.BtnAddPowerRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAddPowerRule.Location = new System.Drawing.Point(928, 12);
            this.BtnAddPowerRule.Name = "BtnAddPowerRule";
            this.BtnAddPowerRule.Size = new System.Drawing.Size(156, 59);
            this.BtnAddPowerRule.TabIndex = 8;
            this.BtnAddPowerRule.Text = "Create new Rule";
            this.BtnAddPowerRule.UseVisualStyleBackColor = true;
            this.BtnAddPowerRule.Click += new System.EventHandler(this.HandleBtnAddPowerRuleClick);
            // 
            // BtnEditPowerRule
            // 
            this.BtnEditPowerRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnEditPowerRule.Location = new System.Drawing.Point(928, 77);
            this.BtnEditPowerRule.Name = "BtnEditPowerRule";
            this.BtnEditPowerRule.Size = new System.Drawing.Size(156, 59);
            this.BtnEditPowerRule.TabIndex = 9;
            this.BtnEditPowerRule.Text = "Edit selected Rule";
            this.BtnEditPowerRule.UseVisualStyleBackColor = true;
            this.BtnEditPowerRule.Click += new System.EventHandler(this.HandleBtnEditPowerRuleClick);
            // 
            // BtnAscentPowerRule
            // 
            this.BtnAscentPowerRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAscentPowerRule.Location = new System.Drawing.Point(766, 77);
            this.BtnAscentPowerRule.Name = "BtnAscentPowerRule";
            this.BtnAscentPowerRule.Size = new System.Drawing.Size(156, 59);
            this.BtnAscentPowerRule.TabIndex = 10;
            this.BtnAscentPowerRule.Text = "Move Rule up";
            this.BtnAscentPowerRule.UseVisualStyleBackColor = true;
            this.BtnAscentPowerRule.Click += new System.EventHandler(this.HandleBtnAscentPowerRuleClick);
            // 
            // BtnDescentPowerRule
            // 
            this.BtnDescentPowerRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDescentPowerRule.Location = new System.Drawing.Point(766, 142);
            this.BtnDescentPowerRule.Name = "BtnDescentPowerRule";
            this.BtnDescentPowerRule.Size = new System.Drawing.Size(156, 59);
            this.BtnDescentPowerRule.TabIndex = 10;
            this.BtnDescentPowerRule.Text = "Move Rule down";
            this.BtnDescentPowerRule.UseVisualStyleBackColor = true;
            this.BtnDescentPowerRule.Click += new System.EventHandler(this.HandleBtnDescentPowerRuleClick);
            // 
            // BtnDeletePowerRule
            // 
            this.BtnDeletePowerRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDeletePowerRule.Location = new System.Drawing.Point(928, 142);
            this.BtnDeletePowerRule.Name = "BtnDeletePowerRule";
            this.BtnDeletePowerRule.Size = new System.Drawing.Size(156, 59);
            this.BtnDeletePowerRule.TabIndex = 9;
            this.BtnDeletePowerRule.Text = "Delete selected Rule";
            this.BtnDeletePowerRule.UseVisualStyleBackColor = true;
            this.BtnDeletePowerRule.Click += new System.EventHandler(this.HandleBtnDeletePowerRuleClick);
            // 
            // ChbActivateInitialPowerScheme
            // 
            this.ChbActivateInitialPowerScheme.AutoSize = true;
            this.ChbActivateInitialPowerScheme.Location = new System.Drawing.Point(491, 12);
            this.ChbActivateInitialPowerScheme.Name = "ChbActivateInitialPowerScheme";
            this.ChbActivateInitialPowerScheme.Size = new System.Drawing.Size(199, 19);
            this.ChbActivateInitialPowerScheme.TabIndex = 11;
            this.ChbActivateInitialPowerScheme.Text = "Activate this Power Plan on start:";
            this.ChbActivateInitialPowerScheme.UseVisualStyleBackColor = true;
            this.ChbActivateInitialPowerScheme.CheckedChanged += new System.EventHandler(this.HandleChbActivateInitialPowerSchemeCheckedChanged);
            // 
            // CmbInitialPowerScheme
            // 
            this.CmbInitialPowerScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbInitialPowerScheme.FormattingEnabled = true;
            this.CmbInitialPowerScheme.Location = new System.Drawing.Point(491, 37);
            this.CmbInitialPowerScheme.Name = "CmbInitialPowerScheme";
            this.CmbInitialPowerScheme.Size = new System.Drawing.Size(269, 23);
            this.CmbInitialPowerScheme.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(491, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 15);
            this.label1.TabIndex = 13;
            this.label1.Text = "Check for Rules to apply every:";
            // 
            // NudPowerRuleCheckInterval
            // 
            this.NudPowerRuleCheckInterval.Location = new System.Drawing.Point(491, 81);
            this.NudPowerRuleCheckInterval.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.NudPowerRuleCheckInterval.Name = "NudPowerRuleCheckInterval";
            this.NudPowerRuleCheckInterval.Size = new System.Drawing.Size(84, 23);
            this.NudPowerRuleCheckInterval.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(581, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Seconds";
            // 
            // SettingsDlg
            // 
            this.AcceptButton = this.BtnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(1096, 553);
            this.Controls.Add(this.NudPowerRuleCheckInterval);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CmbInitialPowerScheme);
            this.Controls.Add(this.ChbActivateInitialPowerScheme);
            this.Controls.Add(this.BtnDescentPowerRule);
            this.Controls.Add(this.BtnAscentPowerRule);
            this.Controls.Add(this.BtnDeletePowerRule);
            this.Controls.Add(this.BtnEditPowerRule);
            this.Controls.Add(this.BtnAddPowerRule);
            this.Controls.Add(this.DgvPowerRules);
            this.Controls.Add(this.BtnCreateRuleFromProcess);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.DgvPowerSchemes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsDlg";
            this.Text = "PowerPlanSwitcher - Settings";
            ((System.ComponentModel.ISupportInitialize)(this.DgvPowerSchemes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvPowerRules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudPowerRuleCheckInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView DgvPowerSchemes;
        private Button BtnOk;
        private Button BtnCancel;
        private DataGridViewCheckBoxColumn DgcVisible;
        private DataGridViewTextBoxColumn DgcName;
        private DataGridViewImageColumn DgcIcon;
        private Button BtnCreateRuleFromProcess;
        private DataGridView DgvPowerRules;
        private Button BtnAddPowerRule;
        private Button BtnEditPowerRule;
        private Button BtnAscentPowerRule;
        private Button BtnDescentPowerRule;
        private Button BtnDeletePowerRule;
        private CheckBox ChbActivateInitialPowerScheme;
        private ComboBox CmbInitialPowerScheme;
        private Label label1;
        private NumericUpDown NudPowerRuleCheckInterval;
        private Label label2;
        private DataGridViewTextBoxColumn DgcRuleIndex;
        private DataGridViewTextBoxColumn DgcRuleType;
        private DataGridViewTextBoxColumn DgcRulePath;
        private DataGridViewImageColumn DgcRuleSchemeIcon;
        private DataGridViewTextBoxColumn DgcRuleSchemeName;
    }
}
