namespace PowerPlanSwitcher
{
    partial class HotkeySelectionDlg
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(HotkeySelectionDlg));
            BtnCancel = new Button();
            BtnOk = new Button();
            label1 = new Label();
            LblHotkeyPreview = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Location = new Point(168, 133);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 26);
            BtnCancel.TabIndex = 4;
            BtnCancel.TabStop = false;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnOk
            // 
            BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOk.DialogResult = DialogResult.OK;
            BtnOk.Location = new Point(87, 133);
            BtnOk.Name = "BtnOk";
            BtnOk.Size = new Size(75, 26);
            BtnOk.TabIndex = 3;
            BtnOk.TabStop = false;
            BtnOk.Text = "OK";
            BtnOk.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(225, 56);
            label1.TabIndex = 5;
            label1.Text = "Selected Hotkey:";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LblHotkeyPreview
            // 
            LblHotkeyPreview.AutoSize = true;
            LblHotkeyPreview.Dock = DockStyle.Fill;
            LblHotkeyPreview.Location = new Point(3, 56);
            LblHotkeyPreview.Name = "LblHotkeyPreview";
            LblHotkeyPreview.Size = new Size(225, 56);
            LblHotkeyPreview.TabIndex = 6;
            LblHotkeyPreview.Text = "[ ---------- ]";
            LblHotkeyPreview.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(LblHotkeyPreview, 0, 1);
            tableLayoutPanel1.Location = new Point(12, 14);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(231, 112);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // HotkeySelectionDlg
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(255, 172);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "HotkeySelectionDlg";
            Text = "PowerPlanSwitcher - Set Hotkey";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button BtnCancel;
        private Button BtnOk;
        private Label label1;
        private Label LblHotkeyPreview;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
