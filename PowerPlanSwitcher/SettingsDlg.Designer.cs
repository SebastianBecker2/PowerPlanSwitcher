namespace PowerPlanSwitcher
{
    partial class SettingsDlg
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDlg));
            DgvPowerSchemes = new DataGridView();
            DgcVisible = new DataGridViewCheckBoxColumn();
            DgcIcon = new DataGridViewImageColumn();
            DgcName = new DataGridViewTextBoxColumn();
            DgcHotkey = new DataGridViewTextBoxColumn();
            BtnOk = new Button();
            BtnCancel = new Button();
            BtnCreateRuleFromProcess = new Button();
            DgvPowerRules = new DataGridView();
            DgcRuleIndex = new DataGridViewTextBoxColumn();
            DgcRuleType = new DataGridViewTextBoxColumn();
            DgcRulePath = new DataGridViewTextBoxColumn();
            DgcRuleSchemeIcon = new DataGridViewImageColumn();
            DgcRuleSchemeName = new DataGridViewTextBoxColumn();
            DgcActive = new DataGridViewCheckBoxColumn();
            BtnAddPowerRule = new Button();
            BtnEditPowerRule = new Button();
            BtnAscentPowerRule = new Button();
            BtnDescentPowerRule = new Button();
            BtnDeletePowerRule = new Button();
            ChbActivateInitialPowerScheme = new CheckBox();
            CmbInitialPowerScheme = new ComboBox();
            label1 = new Label();
            NudPowerRuleCheckInterval = new NumericUpDown();
            label2 = new Label();
            label5 = new Label();
            CmbColorTheme = new ComboBox();
            groupBox1 = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox2 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            BtnSetHotkey = new Button();
            BtnSetIcon = new Button();
            BtnRemoveHotkey = new Button();
            BtnRemoveIcon = new Button();
            groupBox3 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            BtnSetCycleHotkey = new Button();
            BtnRemoveCycleHotkey = new Button();
            RdbCycleAll = new RadioButton();
            RdbCycleVisible = new RadioButton();
            LblCycleHotkey = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)DgvPowerSchemes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DgvPowerRules).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NudPowerRuleCheckInterval).BeginInit();
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // DgvPowerSchemes
            // 
            DgvPowerSchemes.AllowUserToAddRows = false;
            DgvPowerSchemes.AllowUserToDeleteRows = false;
            DgvPowerSchemes.AllowUserToResizeColumns = false;
            DgvPowerSchemes.AllowUserToResizeRows = false;
            DgvPowerSchemes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DgvPowerSchemes.Columns.AddRange(new DataGridViewColumn[] { DgcVisible, DgcIcon, DgcName, DgcHotkey });
            DgvPowerSchemes.Dock = DockStyle.Fill;
            DgvPowerSchemes.Location = new Point(3, 3);
            DgvPowerSchemes.MultiSelect = false;
            DgvPowerSchemes.Name = "DgvPowerSchemes";
            DgvPowerSchemes.RowHeadersVisible = false;
            tableLayoutPanel2.SetRowSpan(DgvPowerSchemes, 2);
            DgvPowerSchemes.RowTemplate.Height = 26;
            DgvPowerSchemes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvPowerSchemes.Size = new Size(645, 155);
            DgvPowerSchemes.TabIndex = 0;
            DgvPowerSchemes.CellMouseDown += HandleDgvPowerSchemesCellMouseDown;
            // 
            // DgcVisible
            // 
            DgcVisible.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcVisible.Frozen = true;
            DgcVisible.HeaderText = "Visible";
            DgcVisible.Name = "DgcVisible";
            DgcVisible.ReadOnly = true;
            DgcVisible.Width = 47;
            // 
            // DgcIcon
            // 
            DgcIcon.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcIcon.Frozen = true;
            DgcIcon.HeaderText = "Icon";
            DgcIcon.Name = "DgcIcon";
            DgcIcon.ReadOnly = true;
            DgcIcon.Width = 36;
            // 
            // DgcName
            // 
            DgcName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgcName.HeaderText = "Name";
            DgcName.Name = "DgcName";
            DgcName.ReadOnly = true;
            // 
            // DgcHotkey
            // 
            DgcHotkey.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcHotkey.HeaderText = "Hotkey";
            DgcHotkey.Name = "DgcHotkey";
            DgcHotkey.ReadOnly = true;
            DgcHotkey.Width = 70;
            // 
            // BtnOk
            // 
            BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOk.Location = new Point(743, 657);
            BtnOk.Name = "BtnOk";
            BtnOk.Size = new Size(75, 23);
            BtnOk.TabIndex = 1;
            BtnOk.Text = "OK";
            BtnOk.UseVisualStyleBackColor = true;
            BtnOk.Click += HandleBtnOkClick;
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Location = new Point(824, 657);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnCreateRuleFromProcess
            // 
            BtnCreateRuleFromProcess.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnCreateRuleFromProcess.Image = (Image)resources.GetObject("BtnCreateRuleFromProcess.Image");
            BtnCreateRuleFromProcess.Location = new Point(654, 3);
            BtnCreateRuleFromProcess.Name = "BtnCreateRuleFromProcess";
            BtnCreateRuleFromProcess.Size = new Size(109, 74);
            BtnCreateRuleFromProcess.TabIndex = 6;
            BtnCreateRuleFromProcess.Text = "Create Rule from active Process";
            BtnCreateRuleFromProcess.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnCreateRuleFromProcess.UseVisualStyleBackColor = true;
            BtnCreateRuleFromProcess.Click += HandleBtnCreateRuleFromProcessClick;
            // 
            // DgvPowerRules
            // 
            DgvPowerRules.AllowUserToAddRows = false;
            DgvPowerRules.AllowUserToDeleteRows = false;
            DgvPowerRules.AllowUserToResizeColumns = false;
            DgvPowerRules.AllowUserToResizeRows = false;
            DgvPowerRules.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DgvPowerRules.Columns.AddRange(new DataGridViewColumn[] { DgcRuleIndex, DgcRuleType, DgcRulePath, DgcRuleSchemeIcon, DgcRuleSchemeName, DgcActive });
            DgvPowerRules.Dock = DockStyle.Fill;
            DgvPowerRules.Location = new Point(3, 3);
            DgvPowerRules.MultiSelect = false;
            DgvPowerRules.Name = "DgvPowerRules";
            DgvPowerRules.RowHeadersVisible = false;
            tableLayoutPanel1.SetRowSpan(DgvPowerRules, 3);
            DgvPowerRules.RowTemplate.Height = 26;
            DgvPowerRules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvPowerRules.Size = new Size(645, 235);
            DgvPowerRules.TabIndex = 7;
            // 
            // DgcRuleIndex
            // 
            DgcRuleIndex.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcRuleIndex.Frozen = true;
            DgcRuleIndex.HeaderText = "Index";
            DgcRuleIndex.Name = "DgcRuleIndex";
            DgcRuleIndex.ReadOnly = true;
            DgcRuleIndex.Width = 61;
            // 
            // DgcRuleType
            // 
            DgcRuleType.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcRuleType.Frozen = true;
            DgcRuleType.HeaderText = "Type";
            DgcRuleType.Name = "DgcRuleType";
            DgcRuleType.ReadOnly = true;
            DgcRuleType.Width = 56;
            // 
            // DgcRulePath
            // 
            DgcRulePath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgcRulePath.HeaderText = "Path/File";
            DgcRulePath.Name = "DgcRulePath";
            DgcRulePath.ReadOnly = true;
            // 
            // DgcRuleSchemeIcon
            // 
            DgcRuleSchemeIcon.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcRuleSchemeIcon.HeaderText = "Icon";
            DgcRuleSchemeIcon.Name = "DgcRuleSchemeIcon";
            DgcRuleSchemeIcon.ReadOnly = true;
            DgcRuleSchemeIcon.Width = 36;
            // 
            // DgcRuleSchemeName
            // 
            DgcRuleSchemeName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcRuleSchemeName.HeaderText = "Power Plan";
            DgcRuleSchemeName.Name = "DgcRuleSchemeName";
            DgcRuleSchemeName.ReadOnly = true;
            DgcRuleSchemeName.Width = 91;
            // 
            // DgcActive
            // 
            DgcActive.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcActive.HeaderText = "Active";
            DgcActive.Name = "DgcActive";
            DgcActive.ReadOnly = true;
            DgcActive.Width = 46;
            // 
            // BtnAddPowerRule
            // 
            BtnAddPowerRule.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnAddPowerRule.Image = (Image)resources.GetObject("BtnAddPowerRule.Image");
            BtnAddPowerRule.Location = new Point(769, 3);
            BtnAddPowerRule.Name = "BtnAddPowerRule";
            BtnAddPowerRule.Size = new Size(109, 74);
            BtnAddPowerRule.TabIndex = 8;
            BtnAddPowerRule.Text = "Create new Rule";
            BtnAddPowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnAddPowerRule.UseVisualStyleBackColor = true;
            BtnAddPowerRule.Click += HandleBtnAddPowerRuleClick;
            // 
            // BtnEditPowerRule
            // 
            BtnEditPowerRule.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnEditPowerRule.Image = (Image)resources.GetObject("BtnEditPowerRule.Image");
            BtnEditPowerRule.Location = new Point(769, 83);
            BtnEditPowerRule.Name = "BtnEditPowerRule";
            BtnEditPowerRule.Size = new Size(109, 74);
            BtnEditPowerRule.TabIndex = 9;
            BtnEditPowerRule.Text = "Edit selected Rule";
            BtnEditPowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnEditPowerRule.UseVisualStyleBackColor = true;
            BtnEditPowerRule.Click += HandleBtnEditPowerRuleClick;
            // 
            // BtnAscentPowerRule
            // 
            BtnAscentPowerRule.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnAscentPowerRule.Image = (Image)resources.GetObject("BtnAscentPowerRule.Image");
            BtnAscentPowerRule.Location = new Point(654, 83);
            BtnAscentPowerRule.Name = "BtnAscentPowerRule";
            BtnAscentPowerRule.Size = new Size(109, 74);
            BtnAscentPowerRule.TabIndex = 10;
            BtnAscentPowerRule.Text = "Move Rule up";
            BtnAscentPowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnAscentPowerRule.UseVisualStyleBackColor = true;
            BtnAscentPowerRule.Click += HandleBtnAscentPowerRuleClick;
            // 
            // BtnDescentPowerRule
            // 
            BtnDescentPowerRule.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnDescentPowerRule.Image = (Image)resources.GetObject("BtnDescentPowerRule.Image");
            BtnDescentPowerRule.Location = new Point(654, 163);
            BtnDescentPowerRule.Name = "BtnDescentPowerRule";
            BtnDescentPowerRule.Size = new Size(109, 74);
            BtnDescentPowerRule.TabIndex = 10;
            BtnDescentPowerRule.Text = "Move Rule down";
            BtnDescentPowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnDescentPowerRule.UseVisualStyleBackColor = true;
            BtnDescentPowerRule.Click += HandleBtnDescentPowerRuleClick;
            // 
            // BtnDeletePowerRule
            // 
            BtnDeletePowerRule.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnDeletePowerRule.Image = (Image)resources.GetObject("BtnDeletePowerRule.Image");
            BtnDeletePowerRule.Location = new Point(769, 163);
            BtnDeletePowerRule.Name = "BtnDeletePowerRule";
            BtnDeletePowerRule.Size = new Size(109, 74);
            BtnDeletePowerRule.TabIndex = 9;
            BtnDeletePowerRule.Text = "Delete selected Rule";
            BtnDeletePowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnDeletePowerRule.UseVisualStyleBackColor = true;
            BtnDeletePowerRule.Click += HandleBtnDeletePowerRuleClick;
            // 
            // ChbActivateInitialPowerScheme
            // 
            ChbActivateInitialPowerScheme.Anchor = AnchorStyles.Left;
            ChbActivateInitialPowerScheme.AutoSize = true;
            tableLayoutPanel3.SetColumnSpan(ChbActivateInitialPowerScheme, 2);
            ChbActivateInitialPowerScheme.Location = new Point(3, 10);
            ChbActivateInitialPowerScheme.Name = "ChbActivateInitialPowerScheme";
            ChbActivateInitialPowerScheme.Size = new Size(199, 19);
            ChbActivateInitialPowerScheme.TabIndex = 11;
            ChbActivateInitialPowerScheme.Text = "Activate this Power Plan on start:";
            ChbActivateInitialPowerScheme.UseVisualStyleBackColor = true;
            ChbActivateInitialPowerScheme.CheckedChanged += HandleChbActivateInitialPowerSchemeCheckedChanged;
            // 
            // CmbInitialPowerScheme
            // 
            CmbInitialPowerScheme.Anchor = AnchorStyles.Left;
            tableLayoutPanel3.SetColumnSpan(CmbInitialPowerScheme, 2);
            CmbInitialPowerScheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbInitialPowerScheme.FormattingEnabled = true;
            CmbInitialPowerScheme.Location = new Point(3, 48);
            CmbInitialPowerScheme.Name = "CmbInitialPowerScheme";
            CmbInitialPowerScheme.Size = new Size(269, 23);
            CmbInitialPowerScheme.TabIndex = 12;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left;
            label1.AutoSize = true;
            tableLayoutPanel3.SetColumnSpan(label1, 2);
            label1.Location = new Point(3, 92);
            label1.Name = "label1";
            label1.Size = new Size(169, 15);
            label1.TabIndex = 13;
            label1.Text = "Check for Rules to apply every:";
            // 
            // NudPowerRuleCheckInterval
            // 
            NudPowerRuleCheckInterval.Anchor = AnchorStyles.Left;
            NudPowerRuleCheckInterval.Location = new Point(3, 128);
            NudPowerRuleCheckInterval.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            NudPowerRuleCheckInterval.Name = "NudPowerRuleCheckInterval";
            NudPowerRuleCheckInterval.Size = new Size(84, 23);
            NudPowerRuleCheckInterval.TabIndex = 14;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(93, 132);
            label2.Name = "label2";
            label2.Size = new Size(51, 15);
            label2.TabIndex = 13;
            label2.Text = "Seconds";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Left;
            label5.AutoSize = true;
            label5.Location = new Point(694, 12);
            label5.Name = "label5";
            label5.Size = new Size(78, 15);
            label5.TabIndex = 16;
            label5.Text = "Color Theme:";
            // 
            // CmbColorTheme
            // 
            CmbColorTheme.Anchor = AnchorStyles.Left;
            CmbColorTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbColorTheme.FormattingEnabled = true;
            CmbColorTheme.Location = new Point(694, 48);
            CmbColorTheme.Name = "CmbColorTheme";
            CmbColorTheme.Size = new Size(181, 23);
            CmbColorTheme.TabIndex = 17;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.Location = new Point(12, 201);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(887, 262);
            groupBox1.TabIndex = 18;
            groupBox1.TabStop = false;
            groupBox1.Text = "Rules";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(DgvPowerRules, 0, 0);
            tableLayoutPanel1.Controls.Add(BtnCreateRuleFromProcess, 1, 0);
            tableLayoutPanel1.Controls.Add(BtnAscentPowerRule, 1, 1);
            tableLayoutPanel1.Controls.Add(BtnDescentPowerRule, 1, 2);
            tableLayoutPanel1.Controls.Add(BtnAddPowerRule, 2, 0);
            tableLayoutPanel1.Controls.Add(BtnEditPowerRule, 2, 1);
            tableLayoutPanel1.Controls.Add(BtnDeletePowerRule, 2, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 19);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(881, 240);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(tableLayoutPanel2);
            groupBox2.Location = new Point(12, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(887, 183);
            groupBox2.TabIndex = 19;
            groupBox2.TabStop = false;
            groupBox2.Text = "Power Plans";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(DgvPowerSchemes, 0, 0);
            tableLayoutPanel2.Controls.Add(BtnSetHotkey, 1, 1);
            tableLayoutPanel2.Controls.Add(BtnSetIcon, 1, 0);
            tableLayoutPanel2.Controls.Add(BtnRemoveHotkey, 1, 1);
            tableLayoutPanel2.Controls.Add(BtnRemoveIcon, 2, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(881, 161);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // BtnSetHotkey
            // 
            BtnSetHotkey.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnSetHotkey.Image = (Image)resources.GetObject("BtnSetHotkey.Image");
            BtnSetHotkey.Location = new Point(654, 83);
            BtnSetHotkey.Name = "BtnSetHotkey";
            BtnSetHotkey.Size = new Size(109, 74);
            BtnSetHotkey.TabIndex = 6;
            BtnSetHotkey.Text = "Set Hotkey";
            BtnSetHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnSetHotkey.UseVisualStyleBackColor = true;
            BtnSetHotkey.Click += BtnSetHotkey_Click;
            // 
            // BtnSetIcon
            // 
            BtnSetIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnSetIcon.Image = (Image)resources.GetObject("BtnSetIcon.Image");
            BtnSetIcon.Location = new Point(654, 3);
            BtnSetIcon.Name = "BtnSetIcon";
            BtnSetIcon.Size = new Size(109, 74);
            BtnSetIcon.TabIndex = 23;
            BtnSetIcon.Text = "Set Icon";
            BtnSetIcon.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnSetIcon.UseVisualStyleBackColor = true;
            BtnSetIcon.Click += BtnSetIcon_Click;
            // 
            // BtnRemoveHotkey
            // 
            BtnRemoveHotkey.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnRemoveHotkey.Image = (Image)resources.GetObject("BtnRemoveHotkey.Image");
            BtnRemoveHotkey.Location = new Point(769, 83);
            BtnRemoveHotkey.Name = "BtnRemoveHotkey";
            BtnRemoveHotkey.Size = new Size(109, 74);
            BtnRemoveHotkey.TabIndex = 22;
            BtnRemoveHotkey.Text = "Remove Hotkey";
            BtnRemoveHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRemoveHotkey.UseVisualStyleBackColor = true;
            BtnRemoveHotkey.Click += BtnRemoveHotkey_Click;
            // 
            // BtnRemoveIcon
            // 
            BtnRemoveIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnRemoveIcon.Image = (Image)resources.GetObject("BtnRemoveIcon.Image");
            BtnRemoveIcon.Location = new Point(769, 3);
            BtnRemoveIcon.Name = "BtnRemoveIcon";
            BtnRemoveIcon.Size = new Size(109, 74);
            BtnRemoveIcon.TabIndex = 21;
            BtnRemoveIcon.Text = "Remove Icon";
            BtnRemoveIcon.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRemoveIcon.UseVisualStyleBackColor = true;
            BtnRemoveIcon.Click += BtnRemoveIcon_Click;
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.Controls.Add(tableLayoutPanel3);
            groupBox3.Location = new Point(12, 469);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(884, 182);
            groupBox3.TabIndex = 20;
            groupBox3.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 6;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(label2, 1, 3);
            tableLayoutPanel3.Controls.Add(CmbColorTheme, 5, 1);
            tableLayoutPanel3.Controls.Add(BtnRemoveCycleHotkey, 4, 0);
            tableLayoutPanel3.Controls.Add(label5, 5, 0);
            tableLayoutPanel3.Controls.Add(label1, 0, 2);
            tableLayoutPanel3.Controls.Add(BtnSetCycleHotkey, 3, 0);
            tableLayoutPanel3.Controls.Add(LblCycleHotkey, 2, 1);
            tableLayoutPanel3.Controls.Add(NudPowerRuleCheckInterval, 0, 3);
            tableLayoutPanel3.Controls.Add(CmbInitialPowerScheme, 0, 1);
            tableLayoutPanel3.Controls.Add(ChbActivateInitialPowerScheme, 0, 0);
            tableLayoutPanel3.Controls.Add(label3, 2, 0);
            tableLayoutPanel3.Controls.Add(RdbCycleVisible, 2, 3);
            tableLayoutPanel3.Controls.Add(RdbCycleAll, 2, 2);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 4;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(878, 160);
            tableLayoutPanel3.TabIndex = 28;
            // 
            // BtnSetCycleHotkey
            // 
            BtnSetCycleHotkey.Anchor = AnchorStyles.None;
            BtnSetCycleHotkey.Image = (Image)resources.GetObject("BtnSetCycleHotkey.Image");
            BtnSetCycleHotkey.Location = new Point(464, 3);
            BtnSetCycleHotkey.Name = "BtnSetCycleHotkey";
            tableLayoutPanel3.SetRowSpan(BtnSetCycleHotkey, 2);
            BtnSetCycleHotkey.Size = new Size(109, 74);
            BtnSetCycleHotkey.TabIndex = 24;
            BtnSetCycleHotkey.Text = "Set Hotkey";
            BtnSetCycleHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnSetCycleHotkey.UseVisualStyleBackColor = true;
            BtnSetCycleHotkey.Click += BtnSetCycleHotkey_Click;
            // 
            // BtnRemoveCycleHotkey
            // 
            BtnRemoveCycleHotkey.Anchor = AnchorStyles.None;
            BtnRemoveCycleHotkey.Image = (Image)resources.GetObject("BtnRemoveCycleHotkey.Image");
            BtnRemoveCycleHotkey.Location = new Point(579, 3);
            BtnRemoveCycleHotkey.Name = "BtnRemoveCycleHotkey";
            tableLayoutPanel3.SetRowSpan(BtnRemoveCycleHotkey, 2);
            BtnRemoveCycleHotkey.Size = new Size(109, 74);
            BtnRemoveCycleHotkey.TabIndex = 25;
            BtnRemoveCycleHotkey.Text = "Remove Hotkey";
            BtnRemoveCycleHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRemoveCycleHotkey.UseVisualStyleBackColor = true;
            BtnRemoveCycleHotkey.Click += BtnRemoveCycleHotkey_Click;
            // 
            // RdbCycleAll
            // 
            RdbCycleAll.Anchor = AnchorStyles.None;
            RdbCycleAll.AutoSize = true;
            tableLayoutPanel3.SetColumnSpan(RdbCycleAll, 3);
            RdbCycleAll.Location = new Point(370, 90);
            RdbCycleAll.Name = "RdbCycleAll";
            RdbCycleAll.Size = new Size(226, 19);
            RdbCycleAll.TabIndex = 26;
            RdbCycleAll.TabStop = true;
            RdbCycleAll.Text = "Cycle through all existing Power Plans";
            RdbCycleAll.UseVisualStyleBackColor = true;
            // 
            // RdbCycleVisible
            // 
            RdbCycleVisible.Anchor = AnchorStyles.None;
            RdbCycleVisible.AutoSize = true;
            tableLayoutPanel3.SetColumnSpan(RdbCycleVisible, 3);
            RdbCycleVisible.Location = new Point(368, 130);
            RdbCycleVisible.Name = "RdbCycleVisible";
            RdbCycleVisible.Size = new Size(229, 19);
            RdbCycleVisible.TabIndex = 27;
            RdbCycleVisible.TabStop = true;
            RdbCycleVisible.Text = "Cycle only through visible Power Plans";
            RdbCycleVisible.UseVisualStyleBackColor = true;
            // 
            // LblCycleHotkey
            // 
            LblCycleHotkey.Anchor = AnchorStyles.None;
            LblCycleHotkey.AutoSize = true;
            LblCycleHotkey.Location = new Point(362, 52);
            LblCycleHotkey.Name = "LblCycleHotkey";
            LblCycleHotkey.Size = new Size(12, 15);
            LblCycleHotkey.TabIndex = 22;
            LblCycleHotkey.Text = "-";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Location = new Point(282, 5);
            label3.Name = "label3";
            label3.Size = new Size(171, 30);
            label3.TabIndex = 21;
            label3.Text = "Hotkey to cycle through Power Plans:";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SettingsDlg
            // 
            AcceptButton = BtnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(911, 692);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(639, 526);
            Name = "SettingsDlg";
            Text = "PowerPlanSwitcher - Settings";
            ((System.ComponentModel.ISupportInitialize)DgvPowerSchemes).EndInit();
            ((System.ComponentModel.ISupportInitialize)DgvPowerRules).EndInit();
            ((System.ComponentModel.ISupportInitialize)NudPowerRuleCheckInterval).EndInit();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView DgvPowerSchemes;
        private Button BtnOk;
        private Button BtnCancel;
        private Button BtnCreateRuleFromProcess;
        private DataGridView DgvPowerRules;
        private Button BtnAddPowerRule;
        private Button BtnEditPowerRule;
        private Button BtnAscentPowerRule;
        private Button BtnDescentPowerRule;
        private Button BtnDeletePowerRule;
        private CheckBox ChbActivateInitialPowerScheme;
        private ComboBox CmbInitialPowerScheme;
        private Label label1;
        private NumericUpDown NudPowerRuleCheckInterval;
        private Label label2;
        private DataGridViewTextBoxColumn DgcRuleIndex;
        private DataGridViewTextBoxColumn DgcRuleType;
        private DataGridViewTextBoxColumn DgcRulePath;
        private DataGridViewImageColumn DgcRuleSchemeIcon;
        private DataGridViewTextBoxColumn DgcRuleSchemeName;
        private DataGridViewCheckBoxColumn DgcActive;
        private Label label5;
        private ComboBox CmbColorTheme;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel2;
        private Button BtnRemoveIcon;
        private Button BtnSetHotkey;
        private Button BtnRemoveHotkey;
        private Button BtnSetIcon;
        private DataGridViewCheckBoxColumn DgcVisible;
        private DataGridViewImageColumn DgcIcon;
        private DataGridViewTextBoxColumn DgcName;
        private DataGridViewTextBoxColumn DgcHotkey;
        private Label label3;
        private Label LblCycleHotkey;
        private Button BtnSetCycleHotkey;
        private Button BtnRemoveCycleHotkey;
        private RadioButton RdbCycleAll;
        private RadioButton RdbCycleVisible;
        private TableLayoutPanel tableLayoutPanel3;
    }
}
