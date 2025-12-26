namespace PowerPlanSwitcher.RuleControl;

partial class IdleRuleControl
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
        NudIdleTimeThreshold = new NumericUpDown();
        CmbUnit = new ComboBox();
        tableLayoutPanel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)NudIdleTimeThreshold).BeginInit();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 3;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        tableLayoutPanel1.Controls.Add(label1, 0, 0);
        tableLayoutPanel1.Controls.Add(NudIdleTimeThreshold, 1, 0);
        tableLayoutPanel1.Controls.Add(CmbUnit, 2, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 1;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Size = new Size(489, 127);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Dock = DockStyle.Fill;
        label1.Location = new Point(3, 0);
        label1.Name = "label1";
        label1.Size = new Size(115, 127);
        label1.TabIndex = 0;
        label1.Text = "Idle Time Threshold:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // NudIdleTimeThreshold
        // 
        NudIdleTimeThreshold.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        NudIdleTimeThreshold.Location = new Point(124, 52);
        NudIdleTimeThreshold.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
        NudIdleTimeThreshold.Name = "NudIdleTimeThreshold";
        NudIdleTimeThreshold.Size = new Size(120, 23);
        NudIdleTimeThreshold.TabIndex = 1;
        // 
        // CmbUnit
        // 
        CmbUnit.Anchor = AnchorStyles.Left;
        CmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        CmbUnit.FormattingEnabled = true;
        CmbUnit.Items.AddRange(new object[] { "Seconds", "Minutes", "Hours" });
        CmbUnit.Location = new Point(250, 52);
        CmbUnit.Name = "CmbUnit";
        CmbUnit.Size = new Size(121, 23);
        CmbUnit.TabIndex = 2;
        // 
        // IdleRuleControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel1);
        Name = "IdleRuleControl";
        Size = new Size(489, 127);
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)NudIdleTimeThreshold).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private Label label1;
    private NumericUpDown NudIdleTimeThreshold;
    private ComboBox CmbUnit;
}
