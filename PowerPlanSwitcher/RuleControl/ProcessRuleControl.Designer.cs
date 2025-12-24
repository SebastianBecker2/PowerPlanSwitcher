namespace PowerPlanSwitcher.Rule;

partial class ProcessRuleControl
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
        BtnSelectFile = new Button();
        BtnSelectFolder = new Button();
        BtnSelectFromProcess = new Button();
        label1 = new Label();
        label2 = new Label();
        TxtPath = new TextBox();
        CmbComparisonType = new ComboBox();
        tableLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 4;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
        tableLayoutPanel1.Controls.Add(BtnSelectFile, 1, 2);
        tableLayoutPanel1.Controls.Add(BtnSelectFolder, 2, 2);
        tableLayoutPanel1.Controls.Add(BtnSelectFromProcess, 3, 2);
        tableLayoutPanel1.Controls.Add(label1, 0, 0);
        tableLayoutPanel1.Controls.Add(label2, 0, 1);
        tableLayoutPanel1.Controls.Add(TxtPath, 1, 1);
        tableLayoutPanel1.Controls.Add(CmbComparisonType, 1, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 3;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.Size = new Size(642, 107);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // BtnSelectFile
        // 
        BtnSelectFile.Location = new Point(84, 73);
        BtnSelectFile.Name = "BtnSelectFile";
        BtnSelectFile.Size = new Size(108, 23);
        BtnSelectFile.TabIndex = 1;
        BtnSelectFile.Text = "Select File...";
        BtnSelectFile.UseVisualStyleBackColor = true;
        BtnSelectFile.Click += BtnSelectPath_Click;
        // 
        // BtnSelectFolder
        // 
        BtnSelectFolder.Location = new Point(198, 73);
        BtnSelectFolder.Name = "BtnSelectFolder";
        BtnSelectFolder.Size = new Size(104, 23);
        BtnSelectFolder.TabIndex = 1;
        BtnSelectFolder.Text = "Select Folder...";
        BtnSelectFolder.UseVisualStyleBackColor = true;
        BtnSelectFolder.Click += BtnSelectFolder_Click;
        // 
        // BtnSelectFromProcess
        // 
        BtnSelectFromProcess.Location = new Point(308, 73);
        BtnSelectFromProcess.Name = "BtnSelectFromProcess";
        BtnSelectFromProcess.Size = new Size(171, 23);
        BtnSelectFromProcess.TabIndex = 1;
        BtnSelectFromProcess.Text = "Select from active Process...";
        BtnSelectFromProcess.UseVisualStyleBackColor = true;
        BtnSelectFromProcess.Click += BtnSelectFromProcess_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Dock = DockStyle.Fill;
        label1.Location = new Point(3, 0);
        label1.Name = "label1";
        label1.Size = new Size(75, 35);
        label1.TabIndex = 0;
        label1.Text = "Comparison:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Dock = DockStyle.Fill;
        label2.Location = new Point(3, 35);
        label2.Name = "label2";
        label2.Size = new Size(75, 35);
        label2.TabIndex = 0;
        label2.Text = "Path/File:";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // TxtPath
        // 
        TxtPath.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        tableLayoutPanel1.SetColumnSpan(TxtPath, 3);
        TxtPath.Location = new Point(84, 41);
        TxtPath.Name = "TxtPath";
        TxtPath.Size = new Size(555, 23);
        TxtPath.TabIndex = 2;
        // 
        // CmbComparisonType
        // 
        CmbComparisonType.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        tableLayoutPanel1.SetColumnSpan(CmbComparisonType, 3);
        CmbComparisonType.DropDownStyle = ComboBoxStyle.DropDownList;
        CmbComparisonType.FormattingEnabled = true;
        CmbComparisonType.Location = new Point(84, 6);
        CmbComparisonType.Name = "CmbComparisonType";
        CmbComparisonType.Size = new Size(555, 23);
        CmbComparisonType.TabIndex = 3;
        // 
        // ProcessRuleControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel1);
        Name = "ProcessRuleControl";
        Size = new Size(642, 107);
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel1.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private Label label2;
    private Label label1;
    private Button BtnSelectFile;
    private Button BtnSelectFolder;
    private Button BtnSelectFromProcess;
    private TextBox TxtPath;
    private ComboBox CmbComparisonType;
}
