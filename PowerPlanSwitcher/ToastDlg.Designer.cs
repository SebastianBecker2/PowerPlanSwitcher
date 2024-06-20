namespace PowerPlanSwitcher
{
    partial class ToastDlg
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ToastDlg));
            DisplayTimer = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1 = new TableLayoutPanel();
            LblTitle = new Label();
            PibAppIcon = new PictureBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            LblPowerSchemeName = new Label();
            PibPowerSchemeIcon = new PictureBox();
            LblReason = new Label();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PibAppIcon).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PibPowerSchemeIcon).BeginInit();
            SuspendLayout();
            // 
            // DisplayTimer
            // 
            DisplayTimer.Interval = 2000;
            DisplayTimer.Tick += DisplayTimer_Tick;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(LblTitle, 2, 0);
            tableLayoutPanel1.Controls.Add(PibAppIcon, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(LblReason, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(270, 84);
            tableLayoutPanel1.TabIndex = 1;
            tableLayoutPanel1.Click += Any_Click;
            // 
            // LblTitle
            // 
            LblTitle.AutoSize = true;
            LblTitle.Dock = DockStyle.Fill;
            LblTitle.Location = new Point(27, 0);
            LblTitle.Name = "LblTitle";
            LblTitle.Size = new Size(237, 22);
            LblTitle.TabIndex = 1;
            LblTitle.Text = "PowerPlanSwitcher switched Power Plan to:";
            LblTitle.TextAlign = ContentAlignment.MiddleCenter;
            LblTitle.Click += Any_Click;
            // 
            // PibAppIcon
            // 
            PibAppIcon.Dock = DockStyle.Fill;
            PibAppIcon.Image = Properties.Resources.power_surge;
            PibAppIcon.Location = new Point(5, 3);
            PibAppIcon.Name = "PibAppIcon";
            PibAppIcon.Size = new Size(16, 16);
            PibAppIcon.SizeMode = PictureBoxSizeMode.Zoom;
            PibAppIcon.TabIndex = 2;
            PibAppIcon.TabStop = false;
            PibAppIcon.Click += Any_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.None;
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel1.SetColumnSpan(tableLayoutPanel2, 4);
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(LblPowerSchemeName, 1, 0);
            tableLayoutPanel2.Controls.Add(PibPowerSchemeIcon, 0, 0);
            tableLayoutPanel2.Location = new Point(33, 22);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(203, 41);
            tableLayoutPanel2.TabIndex = 5;
            tableLayoutPanel2.Click += Any_Click;
            // 
            // LblPowerSchemeName
            // 
            LblPowerSchemeName.Anchor = AnchorStyles.Left;
            LblPowerSchemeName.AutoSize = true;
            LblPowerSchemeName.Font = new Font("Segoe UI", 12F);
            LblPowerSchemeName.Location = new Point(42, 10);
            LblPowerSchemeName.Name = "LblPowerSchemeName";
            LblPowerSchemeName.Size = new Size(158, 21);
            LblPowerSchemeName.TabIndex = 4;
            LblPowerSchemeName.Text = "Power Scheme Name";
            LblPowerSchemeName.Click += Any_Click;
            // 
            // PibPowerSchemeIcon
            // 
            PibPowerSchemeIcon.Anchor = AnchorStyles.None;
            PibPowerSchemeIcon.Image = Properties.Resources.green;
            PibPowerSchemeIcon.Location = new Point(0, 0);
            PibPowerSchemeIcon.Margin = new Padding(0);
            PibPowerSchemeIcon.Name = "PibPowerSchemeIcon";
            PibPowerSchemeIcon.Size = new Size(39, 41);
            PibPowerSchemeIcon.SizeMode = PictureBoxSizeMode.CenterImage;
            PibPowerSchemeIcon.TabIndex = 2;
            PibPowerSchemeIcon.TabStop = false;
            PibPowerSchemeIcon.Click += Any_Click;
            // 
            // LblReason
            // 
            LblReason.Anchor = AnchorStyles.None;
            LblReason.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(LblReason, 4);
            LblReason.Location = new Point(116, 66);
            LblReason.Name = "LblReason";
            LblReason.Size = new Size(38, 15);
            LblReason.TabIndex = 6;
            LblReason.Text = "label1";
            // 
            // ToastDlg
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(270, 84);
            ControlBox = false;
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ToastDlg";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "ToastDlg";
            TopMost = true;
            Click += Any_Click;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PibAppIcon).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PibPowerSchemeIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Timer DisplayTimer;
        private TableLayoutPanel tableLayoutPanel1;
        private Label LblTitle;
        private PictureBox PibAppIcon;
        private PictureBox PibPowerSchemeIcon;
        private Label LblPowerSchemeName;
        private TableLayoutPanel tableLayoutPanel2;
        private Label LblReason;
    }
}
