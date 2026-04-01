namespace PowerPlanSwitcher.RuleControl;

partial class StartupRuleControl
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
        components = new System.ComponentModel.Container();
        tableLayoutPanel1 = new TableLayoutPanel();
        PibDurationHint = new PictureBox();
        label1 = new Label();
        ChbEnableDuration = new CheckBox();
        label2 = new Label();
        NudDuration = new NumericUpDown();
        CmbUnit = new ComboBox();
        TipHints = new ToolTip(components);
        tableLayoutPanel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)PibDurationHint).BeginInit();
        ((System.ComponentModel.ISupportInitialize)NudDuration).BeginInit();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 4;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        tableLayoutPanel1.Controls.Add(PibDurationHint, 3, 0);
        tableLayoutPanel1.Controls.Add(label1, 0, 0);
        tableLayoutPanel1.Controls.Add(ChbEnableDuration, 1, 0);
        tableLayoutPanel1.Controls.Add(label2, 0, 1);
        tableLayoutPanel1.Controls.Add(NudDuration, 1, 1);
        tableLayoutPanel1.Controls.Add(CmbUnit, 2, 1);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel1.Size = new Size(489, 84);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // PibDurationHint
        // 
        PibDurationHint.Anchor = AnchorStyles.Right;
        PibDurationHint.Cursor = Cursors.Help;
        PibDurationHint.Image = Properties.Resources.info_rhombus;
        PibDurationHint.Location = new Point(457, 5);
        PibDurationHint.Margin = new Padding(0);
        PibDurationHint.Name = "PibDurationHint";
        PibDurationHint.Size = new Size(32, 32);
        PibDurationHint.SizeMode = PictureBoxSizeMode.AutoSize;
        PibDurationHint.TabIndex = 7;
        PibDurationHint.TabStop = false;
        PibDurationHint.Click += PibDurationHint_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Dock = DockStyle.Fill;
        label1.Location = new Point(3, 0);
        label1.Name = "label1";
        label1.Size = new Size(94, 42);
        label1.TabIndex = 0;
        label1.Text = "Enable Duration:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // ChbEnableDuration
        // 
        ChbEnableDuration.Anchor = AnchorStyles.Left;
        ChbEnableDuration.AutoSize = true;
        ChbEnableDuration.Location = new Point(103, 14);
        ChbEnableDuration.Name = "ChbEnableDuration";
        ChbEnableDuration.Size = new Size(15, 14);
        ChbEnableDuration.TabIndex = 1;
        ChbEnableDuration.UseVisualStyleBackColor = true;
        ChbEnableDuration.CheckedChanged += ChbEnableDuration_CheckedChanged;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Dock = DockStyle.Fill;
        label2.Location = new Point(3, 42);
        label2.Name = "label2";
        label2.Size = new Size(94, 42);
        label2.TabIndex = 2;
        label2.Text = "Duration:";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // NudDuration
        // 
        NudDuration.Enabled = false;
        NudDuration.Location = new Point(103, 45);
        NudDuration.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
        NudDuration.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        NudDuration.Name = "NudDuration";
        NudDuration.Size = new Size(120, 23);
        NudDuration.TabIndex = 3;
        NudDuration.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // CmbUnit
        // 
        CmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        CmbUnit.Enabled = false;
        CmbUnit.FormattingEnabled = true;
        CmbUnit.Items.AddRange(new object[] { "Seconds", "Minutes", "Hours" });
        CmbUnit.Location = new Point(229, 45);
        CmbUnit.Name = "CmbUnit";
        CmbUnit.Size = new Size(121, 23);
        CmbUnit.TabIndex = 4;
        // 
        // StartupRuleControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel1);
        Name = "StartupRuleControl";
        Size = new Size(489, 84);
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)PibDurationHint).EndInit();
        ((System.ComponentModel.ISupportInitialize)NudDuration).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private PictureBox PibDurationHint;
    private Label label1;
    private CheckBox ChbEnableDuration;
    private Label label2;
    private NumericUpDown NudDuration;
    private ComboBox CmbUnit;
    private ToolTip TipHints;
}
