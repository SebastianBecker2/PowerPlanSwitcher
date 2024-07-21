namespace PowerPlanSwitcher
{
    partial class RuleDlg
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(RuleDlg));
            BtnCancel = new Button();
            BtnOk = new Button();
            CmbComparisonType = new ComboBox();
            TxtPath = new TextBox();
            LblPowerLineStatus = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            CmbPowerLineStatus = new ComboBox();
            LblPowerScheme = new Label();
            RdbPowerLineRule = new RadioButton();
            LblRuleType = new Label();
            RdbProcessRule = new RadioButton();
            LblComparisonType = new Label();
            LblPath = new Label();
            CmbPowerScheme = new ComboBox();
            BtnSelectFile = new Button();
            BtnSelectFromProcess = new Button();
            BtnSelectFolder = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Location = new Point(582, 257);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 4;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnOk
            // 
            BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOk.Location = new Point(501, 257);
            BtnOk.Name = "BtnOk";
            BtnOk.Size = new Size(75, 23);
            BtnOk.TabIndex = 3;
            BtnOk.Text = "OK";
            BtnOk.UseVisualStyleBackColor = true;
            BtnOk.Click += BtnOk_Click;
            // 
            // CmbComparisonType
            // 
            CmbComparisonType.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(CmbComparisonType, 3);
            CmbComparisonType.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbComparisonType.FormattingEnabled = true;
            CmbComparisonType.Location = new Point(112, 73);
            CmbComparisonType.Name = "CmbComparisonType";
            CmbComparisonType.Size = new Size(530, 23);
            CmbComparisonType.TabIndex = 5;
            // 
            // TxtPath
            // 
            TxtPath.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(TxtPath, 3);
            TxtPath.Location = new Point(112, 107);
            TxtPath.Name = "TxtPath";
            TxtPath.Size = new Size(530, 23);
            TxtPath.TabIndex = 6;
            // 
            // LblPowerLineStatus
            // 
            LblPowerLineStatus.AutoSize = true;
            LblPowerLineStatus.Dock = DockStyle.Fill;
            LblPowerLineStatus.Location = new Point(3, 170);
            LblPowerLineStatus.Name = "LblPowerLineStatus";
            LblPowerLineStatus.Size = new Size(103, 34);
            LblPowerLineStatus.TabIndex = 7;
            LblPowerLineStatus.Text = "Power Line Status:";
            LblPowerLineStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(CmbPowerLineStatus, 1, 5);
            tableLayoutPanel1.Controls.Add(LblPowerScheme, 0, 6);
            tableLayoutPanel1.Controls.Add(RdbPowerLineRule, 1, 1);
            tableLayoutPanel1.Controls.Add(LblRuleType, 0, 0);
            tableLayoutPanel1.Controls.Add(RdbProcessRule, 1, 0);
            tableLayoutPanel1.Controls.Add(LblComparisonType, 0, 2);
            tableLayoutPanel1.Controls.Add(TxtPath, 1, 3);
            tableLayoutPanel1.Controls.Add(LblPowerLineStatus, 0, 5);
            tableLayoutPanel1.Controls.Add(CmbComparisonType, 1, 2);
            tableLayoutPanel1.Controls.Add(LblPath, 0, 3);
            tableLayoutPanel1.Controls.Add(CmbPowerScheme, 1, 6);
            tableLayoutPanel1.Controls.Add(BtnSelectFile, 1, 4);
            tableLayoutPanel1.Controls.Add(BtnSelectFromProcess, 3, 4);
            tableLayoutPanel1.Controls.Add(BtnSelectFolder, 2, 4);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.Size = new Size(645, 239);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // CmbPowerLineStatus
            // 
            CmbPowerLineStatus.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(CmbPowerLineStatus, 3);
            CmbPowerLineStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbPowerLineStatus.FormattingEnabled = true;
            CmbPowerLineStatus.Location = new Point(112, 175);
            CmbPowerLineStatus.Name = "CmbPowerLineStatus";
            CmbPowerLineStatus.Size = new Size(530, 23);
            CmbPowerLineStatus.TabIndex = 12;
            // 
            // LblPowerScheme
            // 
            LblPowerScheme.AutoSize = true;
            LblPowerScheme.Dock = DockStyle.Fill;
            LblPowerScheme.Location = new Point(3, 204);
            LblPowerScheme.Name = "LblPowerScheme";
            LblPowerScheme.Size = new Size(103, 35);
            LblPowerScheme.TabIndex = 11;
            LblPowerScheme.Text = "Power Plan:";
            LblPowerScheme.TextAlign = ContentAlignment.MiddleRight;
            // 
            // RdbPowerLineRule
            // 
            RdbPowerLineRule.Anchor = AnchorStyles.Left;
            RdbPowerLineRule.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(RdbPowerLineRule, 3);
            RdbPowerLineRule.Location = new Point(112, 41);
            RdbPowerLineRule.Name = "RdbPowerLineRule";
            RdbPowerLineRule.Size = new Size(87, 19);
            RdbPowerLineRule.TabIndex = 10;
            RdbPowerLineRule.TabStop = true;
            RdbPowerLineRule.Text = "Energy Rule";
            RdbPowerLineRule.UseVisualStyleBackColor = true;
            RdbPowerLineRule.CheckedChanged += RdbPowerLineRule_CheckedChanged;
            // 
            // LblRuleType
            // 
            LblRuleType.AutoSize = true;
            LblRuleType.Dock = DockStyle.Fill;
            LblRuleType.Location = new Point(3, 0);
            LblRuleType.Name = "LblRuleType";
            LblRuleType.Size = new Size(103, 34);
            LblRuleType.TabIndex = 9;
            LblRuleType.Text = "Select Rule Type:";
            LblRuleType.TextAlign = ContentAlignment.MiddleRight;
            // 
            // RdbProcessRule
            // 
            RdbProcessRule.Anchor = AnchorStyles.Left;
            RdbProcessRule.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(RdbProcessRule, 3);
            RdbProcessRule.Location = new Point(112, 7);
            RdbProcessRule.Name = "RdbProcessRule";
            RdbProcessRule.Size = new Size(91, 19);
            RdbProcessRule.TabIndex = 9;
            RdbProcessRule.TabStop = true;
            RdbProcessRule.Text = "Process Rule";
            RdbProcessRule.UseVisualStyleBackColor = true;
            RdbProcessRule.CheckedChanged += RdbProcessRule_CheckedChanged;
            // 
            // LblComparisonType
            // 
            LblComparisonType.AutoSize = true;
            LblComparisonType.Dock = DockStyle.Fill;
            LblComparisonType.Location = new Point(3, 68);
            LblComparisonType.Name = "LblComparisonType";
            LblComparisonType.Size = new Size(103, 34);
            LblComparisonType.TabIndex = 7;
            LblComparisonType.Text = "Comparison:";
            LblComparisonType.TextAlign = ContentAlignment.MiddleRight;
            // 
            // LblPath
            // 
            LblPath.AutoSize = true;
            LblPath.Dock = DockStyle.Fill;
            LblPath.Location = new Point(3, 102);
            LblPath.Name = "LblPath";
            LblPath.Size = new Size(103, 34);
            LblPath.TabIndex = 7;
            LblPath.Text = "Path/File:";
            LblPath.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbPowerScheme
            // 
            CmbPowerScheme.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(CmbPowerScheme, 3);
            CmbPowerScheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbPowerScheme.FormattingEnabled = true;
            CmbPowerScheme.Location = new Point(112, 210);
            CmbPowerScheme.Name = "CmbPowerScheme";
            CmbPowerScheme.Size = new Size(530, 23);
            CmbPowerScheme.TabIndex = 5;
            // 
            // BtnSelectPath
            // 
            BtnSelectFile.Anchor = AnchorStyles.Left;
            BtnSelectFile.AutoSize = true;
            BtnSelectFile.Location = new Point(112, 140);
            BtnSelectFile.Name = "BtnSelectPath";
            BtnSelectFile.Size = new Size(78, 25);
            BtnSelectFile.TabIndex = 8;
            BtnSelectFile.Text = "Select File...";
            BtnSelectFile.UseVisualStyleBackColor = true;
            BtnSelectFile.Click += BtnSelectPath_Click;
            // 
            // BtnSelectFromProcess
            // 
            BtnSelectFromProcess.Anchor = AnchorStyles.Left;
            BtnSelectFromProcess.AutoSize = true;
            BtnSelectFromProcess.Location = new Point(295, 140);
            BtnSelectFromProcess.Name = "BtnSelectFromProcess";
            BtnSelectFromProcess.Size = new Size(163, 25);
            BtnSelectFromProcess.TabIndex = 8;
            BtnSelectFromProcess.Text = "Select from active Process...";
            BtnSelectFromProcess.UseVisualStyleBackColor = true;
            BtnSelectFromProcess.Click += BtnSelectFromProcess_Click;
            // 
            // BtnSelectFolder
            // 
            BtnSelectFolder.Anchor = AnchorStyles.Left;
            BtnSelectFolder.AutoSize = true;
            BtnSelectFolder.Location = new Point(196, 140);
            BtnSelectFolder.Name = "BtnSelectFolder";
            BtnSelectFolder.Size = new Size(93, 25);
            BtnSelectFolder.TabIndex = 13;
            BtnSelectFolder.Text = "Select Folder...";
            BtnSelectFolder.UseVisualStyleBackColor = true;
            BtnSelectFolder.Click += BtnSelectFolder_Click;
            // 
            // RuleDlg
            // 
            AcceptButton = BtnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(669, 292);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "RuleDlg";
            Text = "PowerPlanSwitcher - Rule";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button BtnCancel;
        private Button BtnOk;
        private ComboBox CmbComparisonType;
        private TextBox TxtPath;
        private Label LblPowerLineStatus;
        private TableLayoutPanel tableLayoutPanel1;
        private Label LblComparisonType;
        private Label LblPath;
        private ComboBox CmbPowerScheme;
        private Button BtnSelectFile;
        private RadioButton RdbProcessRule;
        private RadioButton RdbPowerLineRule;
        private ComboBox CmbPowerLineStatus;
        private Label LblPowerScheme;
        private Label LblRuleType;
        private Button BtnSelectFromProcess;
        private Button BtnSelectFolder;
    }
}
