namespace PowerPlanSwitcher
{
    partial class Popup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Popup));
            this.TlpPowerSchemes = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // TlpPowerSchemes
            // 
            this.TlpPowerSchemes.ColumnCount = 1;
            this.TlpPowerSchemes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlpPowerSchemes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlpPowerSchemes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TlpPowerSchemes.Location = new System.Drawing.Point(0, 0);
            this.TlpPowerSchemes.Margin = new System.Windows.Forms.Padding(0);
            this.TlpPowerSchemes.Name = "TlpPowerSchemes";
            this.TlpPowerSchemes.RowCount = 1;
            this.TlpPowerSchemes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlpPowerSchemes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlpPowerSchemes.Size = new System.Drawing.Size(569, 344);
            this.TlpPowerSchemes.TabIndex = 0;
            // 
            // Popup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(569, 344);
            this.Controls.Add(this.TlpPowerSchemes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Popup";
            this.ShowInTaskbar = false;
            this.Text = "Popup";
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel TlpPowerSchemes;
    }
}