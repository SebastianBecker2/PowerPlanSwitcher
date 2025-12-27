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
            components = new System.ComponentModel.Container();
            var processRuleDto1 = new RuleManagement.Dto.ProcessRuleDto();
            var powerLineRuleDto1 = new RuleManagement.Dto.PowerLineRuleDto();
            var idleRuleDto1 = new RuleManagement.Dto.IdleRuleDto();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(RuleDlg));
            BtnCancel = new Button();
            BtnOk = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            PibRuleInfo = new PictureBox();
            LblPowerScheme = new Label();
            LblRuleType = new Label();
            CmbPowerScheme = new ComboBox();
            PrcProcessRule = new PowerPlanSwitcher.Rule.ProcessRuleControl();
            PlcPowerLineRule = new PowerPlanSwitcher.RuleControl.PowerLineRuleControl();
            IrcIdleRule = new PowerPlanSwitcher.RuleControl.IdleRuleControl();
            CmbRuleType = new ComboBox();
            TipHints = new ToolTip(components);
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PibRuleInfo).BeginInit();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Location = new Point(582, 281);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 4;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnOk
            // 
            BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOk.Location = new Point(501, 281);
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
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(PibRuleInfo, 2, 0);
            tableLayoutPanel1.Controls.Add(LblPowerScheme, 0, 4);
            tableLayoutPanel1.Controls.Add(LblRuleType, 0, 0);
            tableLayoutPanel1.Controls.Add(CmbPowerScheme, 1, 4);
            tableLayoutPanel1.Controls.Add(PrcProcessRule, 0, 1);
            tableLayoutPanel1.Controls.Add(PlcPowerLineRule, 0, 2);
            tableLayoutPanel1.Controls.Add(IrcIdleRule, 0, 3);
            tableLayoutPanel1.Controls.Add(CmbRuleType, 1, 0);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(645, 263);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // PibRuleInfo
            // 
            PibRuleInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PibRuleInfo.Image = Properties.Resources.info_rhombus;
            PibRuleInfo.Location = new Point(613, 0);
            PibRuleInfo.Margin = new Padding(0);
            PibRuleInfo.Name = "PibRuleInfo";
            PibRuleInfo.Size = new Size(32, 32);
            PibRuleInfo.SizeMode = PictureBoxSizeMode.AutoSize;
            PibRuleInfo.TabIndex = 17;
            PibRuleInfo.TabStop = false;
            PibRuleInfo.Click += PibRuleInfo_Click;
            // 
            // LblPowerScheme
            // 
            LblPowerScheme.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LblPowerScheme.AutoSize = true;
            LblPowerScheme.Location = new Point(3, 200);
            LblPowerScheme.Name = "LblPowerScheme";
            LblPowerScheme.Size = new Size(95, 15);
            LblPowerScheme.TabIndex = 11;
            LblPowerScheme.Text = "Power Plan:";
            LblPowerScheme.TextAlign = ContentAlignment.MiddleRight;
            // 
            // LblRuleType
            // 
            LblRuleType.AutoSize = true;
            LblRuleType.Dock = DockStyle.Fill;
            LblRuleType.Location = new Point(3, 0);
            LblRuleType.Name = "LblRuleType";
            LblRuleType.Size = new Size(95, 32);
            LblRuleType.TabIndex = 9;
            LblRuleType.Text = "Select Rule Type:";
            LblRuleType.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbPowerScheme
            // 
            CmbPowerScheme.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(CmbPowerScheme, 2);
            CmbPowerScheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbPowerScheme.FormattingEnabled = true;
            CmbPowerScheme.Location = new Point(104, 203);
            CmbPowerScheme.Name = "CmbPowerScheme";
            CmbPowerScheme.Size = new Size(538, 23);
            CmbPowerScheme.TabIndex = 5;
            // 
            // PrcProcessRule
            // 
            PrcProcessRule.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(PrcProcessRule, 3);
            processRuleDto1.Pattern = "";
            processRuleDto1.SchemeGuid = new Guid("00000000-0000-0000-0000-000000000000");
            processRuleDto1.Type = RuleManagement.ComparisonType.StartsWith;
            PrcProcessRule.Dto = processRuleDto1;
            PrcProcessRule.Location = new Point(0, 32);
            PrcProcessRule.Margin = new Padding(0);
            PrcProcessRule.Name = "PrcProcessRule";
            PrcProcessRule.Size = new Size(645, 93);
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
            PlcPowerLineRule.Location = new Point(0, 125);
            PlcPowerLineRule.Margin = new Padding(0);
            PlcPowerLineRule.Name = "PlcPowerLineRule";
            PlcPowerLineRule.Size = new Size(645, 38);
            PlcPowerLineRule.TabIndex = 14;
            PlcPowerLineRule.Visible = false;
            // 
            // IrcIdleRule
            // 
            IrcIdleRule.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(IrcIdleRule, 3);
            idleRuleDto1.IdleTimeThreshold = TimeSpan.Parse("00:00:00");
            idleRuleDto1.SchemeGuid = new Guid("00000000-0000-0000-0000-000000000000");
            IrcIdleRule.Dto = idleRuleDto1;
            IrcIdleRule.Location = new Point(0, 163);
            IrcIdleRule.Margin = new Padding(0);
            IrcIdleRule.Name = "IrcIdleRule";
            IrcIdleRule.Size = new Size(645, 37);
            IrcIdleRule.TabIndex = 15;
            IrcIdleRule.Visible = false;
            // 
            // CmbRuleType
            // 
            CmbRuleType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CmbRuleType.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbRuleType.FormattingEnabled = true;
            CmbRuleType.Items.AddRange(new object[] { "Process Rule", "Power Line Rule", "Idle Rule", "Startup Rule", "Shutdown Rule" });
            CmbRuleType.Location = new Point(104, 3);
            CmbRuleType.Name = "CmbRuleType";
            CmbRuleType.Size = new Size(506, 23);
            CmbRuleType.TabIndex = 16;
            CmbRuleType.SelectedIndexChanged += CmbRuleType_SelectedIndexChanged;
            // 
            // TipHints
            // 
            TipHints.ShowAlways = true;
            TipHints.UseAnimation = false;
            TipHints.UseFading = false;
            // 
            // RuleDlg
            // 
            AcceptButton = BtnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(669, 316);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "RuleDlg";
            Text = "PowerPlanSwitcher - Rule";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PibRuleInfo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button BtnCancel;
        private Button BtnOk;
        private TableLayoutPanel tableLayoutPanel1;
        private ComboBox CmbPowerScheme;
        private Label LblPowerScheme;
        private Label LblRuleType;
        private Rule.ProcessRuleControl PrcProcessRule;
        private RuleControl.PowerLineRuleControl PlcPowerLineRule;
        private RuleControl.IdleRuleControl IrcIdleRule;
        private ComboBox CmbRuleType;
        private PictureBox PibRuleInfo;
        private ToolTip TipHints;
    }
}
