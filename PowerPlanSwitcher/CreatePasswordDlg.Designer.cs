namespace PowerPlanSwitcher
{
    partial class CreatePasswordDlg
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(CreatePasswordDlg));
            tableLayoutPanel1 = new TableLayoutPanel();
            TxtPassword = new TextBox();
            BtnRandomize = new Button();
            BtnCopy = new Button();
            label1 = new Label();
            BtnOkay = new Button();
            btnCancel = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(TxtPassword, 0, 1);
            tableLayoutPanel1.Controls.Add(BtnRandomize, 1, 1);
            tableLayoutPanel1.Controls.Add(BtnCopy, 2, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(437, 84);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // TxtPassword
            // 
            TxtPassword.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            TxtPassword.Location = new Point(3, 45);
            TxtPassword.Name = "TxtPassword";
            TxtPassword.Size = new Size(343, 23);
            TxtPassword.TabIndex = 0;
            // 
            // BtnRandomize
            // 
            BtnRandomize.Anchor = AnchorStyles.None;
            BtnRandomize.AutoSize = true;
            BtnRandomize.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BtnRandomize.Image = Properties.Resources.dice;
            BtnRandomize.Location = new Point(352, 38);
            BtnRandomize.Name = "BtnRandomize";
            BtnRandomize.Size = new Size(38, 38);
            BtnRandomize.TabIndex = 1;
            BtnRandomize.UseVisualStyleBackColor = true;
            BtnRandomize.Click += BtnRandomize_Click;
            // 
            // BtnCopy
            // 
            BtnCopy.Anchor = AnchorStyles.None;
            BtnCopy.AutoSize = true;
            BtnCopy.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BtnCopy.Image = Properties.Resources.page_white_copy;
            BtnCopy.Location = new Point(396, 38);
            BtnCopy.Name = "BtnCopy";
            BtnCopy.Size = new Size(38, 38);
            BtnCopy.TabIndex = 2;
            BtnCopy.UseVisualStyleBackColor = true;
            BtnCopy.Click += BtnCopy_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(label1, 3);
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(431, 30);
            label1.TabIndex = 3;
            label1.Text = "To protect the ZIP file with encryption, enter a password.\r\nLeave the field empty to create an unencrypted archive.";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // BtnOkay
            // 
            BtnOkay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOkay.DialogResult = DialogResult.OK;
            BtnOkay.Location = new Point(293, 102);
            BtnOkay.Name = "BtnOkay";
            BtnOkay.Size = new Size(75, 23);
            BtnOkay.TabIndex = 3;
            BtnOkay.Text = "OK";
            BtnOkay.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(374, 102);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // CreatePasswordDlg
            // 
            AcceptButton = BtnOkay;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(461, 137);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(btnCancel);
            Controls.Add(BtnOkay);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "CreatePasswordDlg";
            Text = "Set Password";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TextBox TxtPassword;
        private Button BtnRandomize;
        private Button BtnCopy;
        private Button BtnOkay;
        private Button btnCancel;
        private Label label1;
    }
}