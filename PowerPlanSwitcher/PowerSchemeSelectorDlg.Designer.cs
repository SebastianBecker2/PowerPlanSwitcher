namespace PowerPlanSwitcher
{
    partial class PowerSchemeSelectorDlg
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(PowerSchemeSelectorDlg));
            TlpPowerSchemes = new TableLayoutPanel();
            SuspendLayout();
            // 
            // TlpPowerSchemes
            // 
            TlpPowerSchemes.ColumnCount = 1;
            TlpPowerSchemes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TlpPowerSchemes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TlpPowerSchemes.Dock = DockStyle.Fill;
            TlpPowerSchemes.Location = new Point(0, 0);
            TlpPowerSchemes.Margin = new Padding(0);
            TlpPowerSchemes.Name = "TlpPowerSchemes";
            TlpPowerSchemes.RowCount = 1;
            TlpPowerSchemes.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TlpPowerSchemes.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TlpPowerSchemes.Size = new Size(569, 344);
            TlpPowerSchemes.TabIndex = 0;
            // 
            // PowerSchemeSelectorDlg
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(569, 344);
            Controls.Add(TlpPowerSchemes);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "PowerSchemeSelectorDlg";
            ShowInTaskbar = false;
            Text = "PowerPlanSwitcher - Selector";
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel TlpPowerSchemes;
    }
}
