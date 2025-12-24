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
            var powerLineRuleDto1 = new RuleManagement.Dto.PowerLineRuleDto();
            var idleRuleDto1 = new RuleManagement.Dto.IdleRuleDto();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(RuleDlg));
            BtnCancel = new Button();
            BtnOk = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            LblPowerScheme = new Label();
            RdbPowerLineRule = new RadioButton();
            LblRuleType = new Label();
            RdbProcessRule = new RadioButton();
            CmbPowerScheme = new ComboBox();
            PrcProcessRule = new PowerPlanSwitcher.Rule.ProcessRuleControl();
            PlcPowerLineRule = new PowerPlanSwitcher.RuleControl.PowerLineRuleControl();
            IrcIdleRule = new PowerPlanSwitcher.RuleControl.IdleRuleControl();
            RdbIdleRule = new RadioButton();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Location = new Point(582, 315);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 4;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnOk
            // 
            BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOk.Location = new Point(501, 315);
            BtnOk.Name = "BtnOk";
            BtnOk.Size = new Size(75, 23);
            BtnOk.TabIndex = 3;
            BtnOk.Text = "OK";
            BtnOk.UseVisualStyleBackColor = true;
            BtnOk.Click += BtnOk_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(LblPowerScheme, 0, 6);
            tableLayoutPanel1.Controls.Add(RdbPowerLineRule, 1, 1);
            tableLayoutPanel1.Controls.Add(LblRuleType, 0, 0);
            tableLayoutPanel1.Controls.Add(RdbProcessRule, 1, 0);
            tableLayoutPanel1.Controls.Add(CmbPowerScheme, 1, 6);
            tableLayoutPanel1.Controls.Add(PrcProcessRule, 0, 3);
            tableLayoutPanel1.Controls.Add(PlcPowerLineRule, 0, 4);
            tableLayoutPanel1.Controls.Add(IrcIdleRule, 0, 5);
            tableLayoutPanel1.Controls.Add(RdbIdleRule, 1, 2);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(645, 297);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // LblPowerScheme
            // 
            LblPowerScheme.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LblPowerScheme.AutoSize = true;
            LblPowerScheme.Location = new Point(3, 261);
            LblPowerScheme.Name = "LblPowerScheme";
            LblPowerScheme.Size = new Size(95, 15);
            LblPowerScheme.TabIndex = 11;
            LblPowerScheme.Text = "Power Plan:";
            LblPowerScheme.TextAlign = ContentAlignment.MiddleRight;
            // 
            // RdbPowerLineRule
            // 
            RdbPowerLineRule.Anchor = AnchorStyles.Left;
            RdbPowerLineRule.AutoSize = true;
            RdbPowerLineRule.Location = new Point(104, 28);
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
            LblRuleType.Size = new Size(95, 25);
            LblRuleType.TabIndex = 9;
            LblRuleType.Text = "Select Rule Type:";
            LblRuleType.TextAlign = ContentAlignment.MiddleRight;
            // 
            // RdbProcessRule
            // 
            RdbProcessRule.Anchor = AnchorStyles.Left;
            RdbProcessRule.AutoSize = true;
            RdbProcessRule.Location = new Point(104, 3);
            RdbProcessRule.Name = "RdbProcessRule";
            RdbProcessRule.Size = new Size(91, 19);
            RdbProcessRule.TabIndex = 9;
            RdbProcessRule.TabStop = true;
            RdbProcessRule.Text = "Process Rule";
            RdbProcessRule.UseVisualStyleBackColor = true;
            RdbProcessRule.CheckedChanged += RdbProcessRule_CheckedChanged;
            // 
            // CmbPowerScheme
            // 
            CmbPowerScheme.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CmbPowerScheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbPowerScheme.FormattingEnabled = true;
            CmbPowerScheme.Location = new Point(104, 264);
            CmbPowerScheme.Name = "CmbPowerScheme";
            CmbPowerScheme.Size = new Size(538, 23);
            CmbPowerScheme.TabIndex = 5;
            // 
            // PrcProcessRule
            // 
            PrcProcessRule.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(PrcProcessRule, 2);
            PrcProcessRule.Location = new Point(3, 78);
            PrcProcessRule.Name = "PrcProcessRule";
            PrcProcessRule.Size = new Size(639, 93);
            PrcProcessRule.TabIndex = 13;
            PrcProcessRule.Visible = false;
            // 
            // PlcPowerLineRule
            // 
            PlcPowerLineRule.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(PlcPowerLineRule, 3);
            powerLineRuleDto1.PowerLineStatus = PowerManagement.PowerLineStatus.Offline;
            powerLineRuleDto1.SchemeGuid = new Guid("00000000-0000-0000-0000-000000000000");
            PlcPowerLineRule.Dto = powerLineRuleDto1;
            PlcPowerLineRule.Location = new Point(3, 177);
            PlcPowerLineRule.Name = "PlcPowerLineRule";
            PlcPowerLineRule.Size = new Size(639, 38);
            PlcPowerLineRule.TabIndex = 14;
            PlcPowerLineRule.Visible = false;
            // 
            // IrcIdleRule
            // 
            IrcIdleRule.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(IrcIdleRule, 2);
            idleRuleDto1.IdleTimeThreshold = TimeSpan.Parse("00:00:00");
            idleRuleDto1.SchemeGuid = new Guid("00000000-0000-0000-0000-000000000000");
            IrcIdleRule.Dto = idleRuleDto1;
            IrcIdleRule.Location = new Point(3, 221);
            IrcIdleRule.Name = "IrcIdleRule";
            IrcIdleRule.Size = new Size(639, 37);
            IrcIdleRule.TabIndex = 15;
            IrcIdleRule.Visible = false;
            // 
            // RdbIdleRule
            // 
            RdbIdleRule.Anchor = AnchorStyles.Left;
            RdbIdleRule.AutoSize = true;
            RdbIdleRule.Location = new Point(104, 53);
            RdbIdleRule.Name = "RdbIdleRule";
            RdbIdleRule.Size = new Size(70, 19);
            RdbIdleRule.TabIndex = 10;
            RdbIdleRule.TabStop = true;
            RdbIdleRule.Text = "Idle Rule";
            RdbIdleRule.UseVisualStyleBackColor = true;
            RdbIdleRule.CheckedChanged += RdbIdleRule_CheckedChanged;
            // 
            // RuleDlg
            // 
            AcceptButton = BtnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(669, 350);
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
        private TableLayoutPanel tableLayoutPanel1;
        private ComboBox CmbPowerScheme;
        private RadioButton RdbProcessRule;
        private RadioButton RdbPowerLineRule;
        private Label LblPowerScheme;
        private Label LblRuleType;
        private Rule.ProcessRuleControl PrcProcessRule;
        private RuleControl.PowerLineRuleControl PlcPowerLineRule;
        private RuleControl.IdleRuleControl IrcIdleRule;
        private RadioButton RdbIdleRule;
    }
}
