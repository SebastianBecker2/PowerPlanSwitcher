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
        components = new System.ComponentModel.Container();
        tableLayoutPanel1 = new TableLayoutPanel();
        PibCheckFullscreenApp = new PictureBox();
        PibCheckExecutionState = new PictureBox();
        label3 = new Label();
        label2 = new Label();
        label1 = new Label();
        NudIdleTimeThreshold = new NumericUpDown();
        CmbUnit = new ComboBox();
        ChbCheckExecutionState = new CheckBox();
        ChbCheckFullscreenApp = new CheckBox();
        TipHints = new ToolTip(components);
        tableLayoutPanel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)PibCheckFullscreenApp).BeginInit();
        ((System.ComponentModel.ISupportInitialize)PibCheckExecutionState).BeginInit();
        ((System.ComponentModel.ISupportInitialize)NudIdleTimeThreshold).BeginInit();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 3;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        tableLayoutPanel1.Controls.Add(PibCheckFullscreenApp, 2, 2);
        tableLayoutPanel1.Controls.Add(PibCheckExecutionState, 2, 1);
        tableLayoutPanel1.Controls.Add(label3, 0, 2);
        tableLayoutPanel1.Controls.Add(label2, 0, 1);
        tableLayoutPanel1.Controls.Add(label1, 0, 0);
        tableLayoutPanel1.Controls.Add(NudIdleTimeThreshold, 1, 0);
        tableLayoutPanel1.Controls.Add(CmbUnit, 2, 0);
        tableLayoutPanel1.Controls.Add(ChbCheckExecutionState, 1, 1);
        tableLayoutPanel1.Controls.Add(ChbCheckFullscreenApp, 1, 2);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 3;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.Size = new Size(489, 127);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // PibCheckFullscreenApp
        // 
        PibCheckFullscreenApp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        PibCheckFullscreenApp.Cursor = Cursors.Help;
        PibCheckFullscreenApp.Image = Properties.Resources.info_rhombus;
        PibCheckFullscreenApp.Location = new Point(457, 84);
        PibCheckFullscreenApp.Margin = new Padding(0);
        PibCheckFullscreenApp.Name = "PibCheckFullscreenApp";
        PibCheckFullscreenApp.Size = new Size(32, 43);
        PibCheckFullscreenApp.SizeMode = PictureBoxSizeMode.AutoSize;
        PibCheckFullscreenApp.TabIndex = 7;
        PibCheckFullscreenApp.TabStop = false;
        PibCheckFullscreenApp.Click += PibCheckFullscreenApp_Click;
        // 
        // PibCheckExecutionState
        // 
        PibCheckExecutionState.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        PibCheckExecutionState.Cursor = Cursors.Help;
        PibCheckExecutionState.Image = Properties.Resources.info_rhombus;
        PibCheckExecutionState.Location = new Point(457, 42);
        PibCheckExecutionState.Margin = new Padding(0);
        PibCheckExecutionState.Name = "PibCheckExecutionState";
        PibCheckExecutionState.Size = new Size(32, 42);
        PibCheckExecutionState.SizeMode = PictureBoxSizeMode.AutoSize;
        PibCheckExecutionState.TabIndex = 6;
        PibCheckExecutionState.TabStop = false;
        PibCheckExecutionState.Click += PibCheckExecutionState_Click;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Dock = DockStyle.Fill;
        label3.Location = new Point(3, 84);
        label3.Name = "label3";
        label3.Size = new Size(129, 43);
        label3.TabIndex = 4;
        label3.Text = "Check Fullscreen Apps:";
        label3.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Dock = DockStyle.Fill;
        label2.Location = new Point(3, 42);
        label2.Name = "label2";
        label2.Size = new Size(129, 42);
        label2.TabIndex = 3;
        label2.Text = "Check Execution State:";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Dock = DockStyle.Fill;
        label1.Location = new Point(3, 0);
        label1.Name = "label1";
        label1.Size = new Size(129, 42);
        label1.TabIndex = 0;
        label1.Text = "Idle Time Threshold:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // NudIdleTimeThreshold
        // 
        NudIdleTimeThreshold.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        NudIdleTimeThreshold.Location = new Point(138, 9);
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
        CmbUnit.Location = new Point(264, 9);
        CmbUnit.Name = "CmbUnit";
        CmbUnit.Size = new Size(121, 23);
        CmbUnit.TabIndex = 2;
        // 
        // ChbCheckExecutionState
        // 
        ChbCheckExecutionState.Anchor = AnchorStyles.Left;
        ChbCheckExecutionState.AutoSize = true;
        ChbCheckExecutionState.Location = new Point(138, 56);
        ChbCheckExecutionState.Name = "ChbCheckExecutionState";
        ChbCheckExecutionState.Size = new Size(15, 14);
        ChbCheckExecutionState.TabIndex = 5;
        ChbCheckExecutionState.UseVisualStyleBackColor = true;
        // 
        // ChbCheckFullscreenApp
        // 
        ChbCheckFullscreenApp.Anchor = AnchorStyles.Left;
        ChbCheckFullscreenApp.AutoSize = true;
        ChbCheckFullscreenApp.Location = new Point(138, 98);
        ChbCheckFullscreenApp.Name = "ChbCheckFullscreenApp";
        ChbCheckFullscreenApp.Size = new Size(15, 14);
        ChbCheckFullscreenApp.TabIndex = 5;
        ChbCheckFullscreenApp.UseVisualStyleBackColor = true;
        // 
        // TipHints
        // 
        TipHints.ShowAlways = true;
        TipHints.UseAnimation = false;
        TipHints.UseFading = false;
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
        ((System.ComponentModel.ISupportInitialize)PibCheckFullscreenApp).EndInit();
        ((System.ComponentModel.ISupportInitialize)PibCheckExecutionState).EndInit();
        ((System.ComponentModel.ISupportInitialize)NudIdleTimeThreshold).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private Label label1;
    private NumericUpDown NudIdleTimeThreshold;
    private ComboBox CmbUnit;
    private Label label2;
    private Label label3;
    private CheckBox ChbCheckExecutionState;
    private CheckBox ChbCheckFullscreenApp;
    private PictureBox PibCheckFullscreenApp;
    private PictureBox PibCheckExecutionState;
    private ToolTip TipHints;
}
