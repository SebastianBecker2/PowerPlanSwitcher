namespace PowerPlanSwitcher.RuleControl;

partial class PowerLineRuleControl
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        tableLayoutPanel1 = new TableLayoutPanel();
        label1 = new Label();
        CmbPowerLineStatus = new ComboBox();
        tableLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(label1, 0, 0);
        tableLayoutPanel1.Controls.Add(CmbPowerLineStatus, 1, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 1;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Size = new Size(682, 50);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Dock = DockStyle.Fill;
        label1.Location = new Point(3, 0);
        label1.Name = "label1";
        label1.Size = new Size(103, 50);
        label1.TabIndex = 0;
        label1.Text = "Power Line Status:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // CmbPowerLineStatus
        // 
        CmbPowerLineStatus.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        CmbPowerLineStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        CmbPowerLineStatus.FormattingEnabled = true;
        CmbPowerLineStatus.Location = new Point(112, 13);
        CmbPowerLineStatus.Name = "CmbPowerLineStatus";
        CmbPowerLineStatus.Size = new Size(567, 23);
        CmbPowerLineStatus.TabIndex = 1;
        // 
        // PowerLineRuleControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel1);
        Name = "PowerLineRuleControl";
        Size = new Size(682, 50);
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel1.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private Label label1;
    private ComboBox CmbPowerLineStatus;
}
