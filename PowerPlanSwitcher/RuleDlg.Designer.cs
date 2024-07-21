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
            CmbRuleType = new ComboBox();
            TxtPath = new TextBox();
            label1 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            label2 = new Label();
            label3 = new Label();
            CmbPowerScheme = new ComboBox();
            BtnSelectPath = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Location = new Point(481, 130);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 4;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnOk
            // 
            BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOk.Location = new Point(400, 130);
            BtnOk.Name = "BtnOk";
            BtnOk.Size = new Size(75, 23);
            BtnOk.TabIndex = 3;
            BtnOk.Text = "OK";
            BtnOk.UseVisualStyleBackColor = true;
            BtnOk.Click += BtnOk_Click;
            // 
            // CmbRuleType
            // 
            CmbRuleType.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            CmbRuleType.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbRuleType.FormattingEnabled = true;
            CmbRuleType.Location = new Point(109, 7);
            CmbRuleType.Name = "CmbRuleType";
            CmbRuleType.Size = new Size(401, 23);
            CmbRuleType.TabIndex = 5;
            // 
            // TxtPath
            // 
            TxtPath.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            TxtPath.Location = new Point(109, 44);
            TxtPath.Name = "TxtPath";
            TxtPath.Size = new Size(401, 23);
            TxtPath.TabIndex = 6;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 74);
            label1.Name = "label1";
            label1.Size = new Size(100, 38);
            label1.TabIndex = 7;
            label1.Text = "Power Plan:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(TxtPath, 1, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 2);
            tableLayoutPanel1.Controls.Add(CmbRuleType, 1, 0);
            tableLayoutPanel1.Controls.Add(label3, 0, 1);
            tableLayoutPanel1.Controls.Add(CmbPowerScheme, 1, 2);
            tableLayoutPanel1.Controls.Add(BtnSelectPath, 2, 1);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.Size = new Size(544, 112);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(3, 0);
            label2.Name = "label2";
            label2.Size = new Size(100, 37);
            label2.TabIndex = 7;
            label2.Text = "Type:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(3, 37);
            label3.Name = "label3";
            label3.Size = new Size(100, 37);
            label3.TabIndex = 7;
            label3.Text = "Path/File:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbPowerScheme
            // 
            CmbPowerScheme.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            CmbPowerScheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbPowerScheme.FormattingEnabled = true;
            CmbPowerScheme.Location = new Point(109, 81);
            CmbPowerScheme.Name = "CmbPowerScheme";
            CmbPowerScheme.Size = new Size(401, 23);
            CmbPowerScheme.TabIndex = 5;
            // 
            // BtnSelectPath
            // 
            BtnSelectPath.Anchor = AnchorStyles.Left;
            BtnSelectPath.Location = new Point(516, 44);
            BtnSelectPath.Name = "BtnSelectPath";
            BtnSelectPath.Size = new Size(25, 23);
            BtnSelectPath.TabIndex = 8;
            BtnSelectPath.Text = "...";
            BtnSelectPath.UseVisualStyleBackColor = true;
            BtnSelectPath.Click += HandleBtnSelectPathClick;
            // 
            // PowerRuleDlg
            // 
            AcceptButton = BtnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(568, 165);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "PowerRuleDlg";
            Text = "PowerPlanSwitcher - Rule";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button BtnCancel;
        private Button BtnOk;
        private ComboBox CmbRuleType;
        private TextBox TxtPath;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label2;
        private Label label3;
        private ComboBox CmbPowerScheme;
        private Button BtnSelectPath;
    }
}
