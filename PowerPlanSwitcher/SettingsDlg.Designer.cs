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
            var dataGridViewCellStyle1 = new DataGridViewCellStyle();
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
            CmbColorTheme = new ComboBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            ChbActivateInitialPowerScheme = new CheckBox();
            BtnRemoveIcon = new Button();
            BtnSetIcon = new Button();
            BtnSetHotkey = new Button();
            BtnRemoveHotkey = new Button();
            CmbInitialPowerScheme = new ComboBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            tableLayoutPanel5 = new TableLayoutPanel();
            groupBox4 = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            RdbCycleAll = new RadioButton();
            LblCycleHotkey = new Label();
            RdbCycleVisible = new RadioButton();
            BtnRemoveCycleHotkey = new Button();
            BtnSetCycleHotkey = new Button();
            PopUpWindowLocation = new GroupBox();
            ComPopUpWindow = new ComboBox();
            groupBox1 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            GrbBatteryManagement = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            CmbAcPowerScheme = new ComboBox();
            label1 = new Label();
            label3 = new Label();
            CmbBatteryPowerScheme = new ComboBox();
            groupBox3 = new GroupBox();
            tableLayoutPanel8 = new TableLayoutPanel();
            ChbShowToastNotifications = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)DgvPowerSchemes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DgvPowerRules).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            PopUpWindowLocation.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            GrbBatteryManagement.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
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
            tableLayoutPanel2.SetColumnSpan(DgvPowerSchemes, 2);
            DgvPowerSchemes.Dock = DockStyle.Fill;
            DgvPowerSchemes.Location = new Point(3, 3);
            DgvPowerSchemes.MultiSelect = false;
            DgvPowerSchemes.Name = "DgvPowerSchemes";
            DgvPowerSchemes.RowHeadersVisible = false;
            tableLayoutPanel2.SetRowSpan(DgvPowerSchemes, 2);
            DgvPowerSchemes.RowTemplate.Height = 26;
            DgvPowerSchemes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvPowerSchemes.Size = new Size(455, 226);
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
            DgcVisible.Width = 52;
            // 
            // DgcIcon
            // 
            DgcIcon.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcIcon.Frozen = true;
            DgcIcon.HeaderText = "Icon";
            DgcIcon.Name = "DgcIcon";
            DgcIcon.ReadOnly = true;
            DgcIcon.Width = 39;
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
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgcHotkey.DefaultCellStyle = dataGridViewCellStyle1;
            DgcHotkey.HeaderText = "Hotkey";
            DgcHotkey.Name = "DgcHotkey";
            DgcHotkey.ReadOnly = true;
            DgcHotkey.Width = 74;
            // 
            // BtnOk
            // 
            BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOk.Location = new Point(537, 322);
            BtnOk.Name = "BtnOk";
            BtnOk.Size = new Size(75, 26);
            BtnOk.TabIndex = 1;
            BtnOk.Text = "OK";
            BtnOk.UseVisualStyleBackColor = true;
            BtnOk.Click += HandleBtnOkClick;
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnCancel.DialogResult = DialogResult.Cancel;
            BtnCancel.Location = new Point(618, 322);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 26);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnCreateRuleFromProcess
            // 
            BtnCreateRuleFromProcess.Image = (Image)resources.GetObject("BtnCreateRuleFromProcess.Image");
            BtnCreateRuleFromProcess.Location = new Point(118, 192);
            BtnCreateRuleFromProcess.Name = "BtnCreateRuleFromProcess";
            BtnCreateRuleFromProcess.Size = new Size(109, 84);
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
            tableLayoutPanel1.SetColumnSpan(DgvPowerRules, 6);
            DgvPowerRules.Dock = DockStyle.Fill;
            DgvPowerRules.Location = new Point(3, 3);
            DgvPowerRules.MultiSelect = false;
            DgvPowerRules.Name = "DgvPowerRules";
            DgvPowerRules.RowHeadersVisible = false;
            DgvPowerRules.RowTemplate.Height = 26;
            DgvPowerRules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvPowerRules.Size = new Size(685, 183);
            DgvPowerRules.TabIndex = 7;
            DgvPowerRules.CellContentDoubleClick += DgvPowerRules_CellContentDoubleClick;
            // 
            // DgcRuleIndex
            // 
            DgcRuleIndex.Frozen = true;
            DgcRuleIndex.HeaderText = "ID";
            DgcRuleIndex.Name = "DgcRuleIndex";
            DgcRuleIndex.ReadOnly = true;
            DgcRuleIndex.Width = 30;
            // 
            // DgcRuleType
            // 
            DgcRuleType.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcRuleType.Frozen = true;
            DgcRuleType.HeaderText = "Type";
            DgcRuleType.Name = "DgcRuleType";
            DgcRuleType.ReadOnly = true;
            DgcRuleType.Width = 61;
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
            DgcRuleSchemeIcon.Width = 39;
            // 
            // DgcRuleSchemeName
            // 
            DgcRuleSchemeName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcRuleSchemeName.HeaderText = "Power Plan";
            DgcRuleSchemeName.Name = "DgcRuleSchemeName";
            DgcRuleSchemeName.ReadOnly = true;
            DgcRuleSchemeName.Width = 97;
            // 
            // DgcActive
            // 
            DgcActive.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcActive.HeaderText = "Active";
            DgcActive.Name = "DgcActive";
            DgcActive.ReadOnly = true;
            DgcActive.Width = 48;
            // 
            // BtnAddPowerRule
            // 
            BtnAddPowerRule.Image = (Image)resources.GetObject("BtnAddPowerRule.Image");
            BtnAddPowerRule.Location = new Point(3, 192);
            BtnAddPowerRule.Name = "BtnAddPowerRule";
            BtnAddPowerRule.Size = new Size(109, 84);
            BtnAddPowerRule.TabIndex = 8;
            BtnAddPowerRule.Text = "Create new Rule";
            BtnAddPowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnAddPowerRule.UseVisualStyleBackColor = true;
            BtnAddPowerRule.Click += HandleBtnAddPowerRuleClick;
            // 
            // BtnEditPowerRule
            // 
            BtnEditPowerRule.Image = (Image)resources.GetObject("BtnEditPowerRule.Image");
            BtnEditPowerRule.Location = new Point(233, 192);
            BtnEditPowerRule.Name = "BtnEditPowerRule";
            BtnEditPowerRule.Size = new Size(109, 84);
            BtnEditPowerRule.TabIndex = 9;
            BtnEditPowerRule.Text = "Edit selected Rule";
            BtnEditPowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnEditPowerRule.UseVisualStyleBackColor = true;
            BtnEditPowerRule.Click += HandleBtnEditPowerRuleClick;
            // 
            // BtnAscentPowerRule
            // 
            BtnAscentPowerRule.Image = (Image)resources.GetObject("BtnAscentPowerRule.Image");
            BtnAscentPowerRule.Location = new Point(463, 192);
            BtnAscentPowerRule.Name = "BtnAscentPowerRule";
            BtnAscentPowerRule.Size = new Size(109, 84);
            BtnAscentPowerRule.TabIndex = 10;
            BtnAscentPowerRule.Text = "Move Rule up";
            BtnAscentPowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnAscentPowerRule.UseVisualStyleBackColor = true;
            BtnAscentPowerRule.Click += HandleBtnAscentPowerRuleClick;
            // 
            // BtnDescentPowerRule
            // 
            BtnDescentPowerRule.Image = (Image)resources.GetObject("BtnDescentPowerRule.Image");
            BtnDescentPowerRule.Location = new Point(578, 192);
            BtnDescentPowerRule.Name = "BtnDescentPowerRule";
            BtnDescentPowerRule.Size = new Size(109, 84);
            BtnDescentPowerRule.TabIndex = 10;
            BtnDescentPowerRule.Text = "Move Rule down";
            BtnDescentPowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnDescentPowerRule.UseVisualStyleBackColor = true;
            BtnDescentPowerRule.Click += HandleBtnDescentPowerRuleClick;
            // 
            // BtnDeletePowerRule
            // 
            BtnDeletePowerRule.Image = (Image)resources.GetObject("BtnDeletePowerRule.Image");
            BtnDeletePowerRule.Location = new Point(348, 192);
            BtnDeletePowerRule.Name = "BtnDeletePowerRule";
            BtnDeletePowerRule.Size = new Size(109, 84);
            BtnDeletePowerRule.TabIndex = 9;
            BtnDeletePowerRule.Text = "Delete selected Rule";
            BtnDeletePowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnDeletePowerRule.UseVisualStyleBackColor = true;
            BtnDeletePowerRule.Click += HandleBtnDeletePowerRuleClick;
            // 
            // CmbColorTheme
            // 
            CmbColorTheme.Anchor = AnchorStyles.None;
            CmbColorTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbColorTheme.FormattingEnabled = true;
            CmbColorTheme.Location = new Point(81, 11);
            CmbColorTheme.Name = "CmbColorTheme";
            CmbColorTheme.Size = new Size(170, 25);
            CmbColorTheme.TabIndex = 17;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 6;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(DgvPowerRules, 0, 0);
            tableLayoutPanel1.Controls.Add(BtnDeletePowerRule, 3, 1);
            tableLayoutPanel1.Controls.Add(BtnDescentPowerRule, 5, 1);
            tableLayoutPanel1.Controls.Add(BtnAscentPowerRule, 4, 1);
            tableLayoutPanel1.Controls.Add(BtnEditPowerRule, 2, 1);
            tableLayoutPanel1.Controls.Add(BtnCreateRuleFromProcess, 1, 1);
            tableLayoutPanel1.Controls.Add(BtnAddPowerRule, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            tableLayoutPanel1.Size = new Size(691, 279);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(ChbActivateInitialPowerScheme, 0, 2);
            tableLayoutPanel2.Controls.Add(DgvPowerSchemes, 0, 0);
            tableLayoutPanel2.Controls.Add(BtnRemoveIcon, 3, 0);
            tableLayoutPanel2.Controls.Add(BtnSetIcon, 2, 0);
            tableLayoutPanel2.Controls.Add(BtnSetHotkey, 2, 1);
            tableLayoutPanel2.Controls.Add(BtnRemoveHotkey, 3, 1);
            tableLayoutPanel2.Controls.Add(CmbInitialPowerScheme, 1, 2);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(691, 279);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // ChbActivateInitialPowerScheme
            // 
            ChbActivateInitialPowerScheme.AutoSize = true;
            ChbActivateInitialPowerScheme.Dock = DockStyle.Fill;
            ChbActivateInitialPowerScheme.Location = new Point(10, 243);
            ChbActivateInitialPowerScheme.Margin = new Padding(10, 11, 10, 11);
            ChbActivateInitialPowerScheme.Name = "ChbActivateInitialPowerScheme";
            ChbActivateInitialPowerScheme.Size = new Size(216, 25);
            ChbActivateInitialPowerScheme.TabIndex = 24;
            ChbActivateInitialPowerScheme.Text = "Activate this Power Plan on start:";
            ChbActivateInitialPowerScheme.UseVisualStyleBackColor = true;
            ChbActivateInitialPowerScheme.CheckedChanged += HandleChbActivateInitialPowerSchemeCheckedChanged;
            // 
            // BtnRemoveIcon
            // 
            BtnRemoveIcon.Image = (Image)resources.GetObject("BtnRemoveIcon.Image");
            BtnRemoveIcon.Location = new Point(579, 3);
            BtnRemoveIcon.Name = "BtnRemoveIcon";
            BtnRemoveIcon.Size = new Size(109, 84);
            BtnRemoveIcon.TabIndex = 21;
            BtnRemoveIcon.Text = "Remove Icon";
            BtnRemoveIcon.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRemoveIcon.UseVisualStyleBackColor = true;
            BtnRemoveIcon.Click += BtnRemoveIcon_Click;
            // 
            // BtnSetIcon
            // 
            BtnSetIcon.Image = (Image)resources.GetObject("BtnSetIcon.Image");
            BtnSetIcon.Location = new Point(464, 3);
            BtnSetIcon.Name = "BtnSetIcon";
            BtnSetIcon.Size = new Size(109, 84);
            BtnSetIcon.TabIndex = 23;
            BtnSetIcon.Text = "Set Icon";
            BtnSetIcon.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnSetIcon.UseVisualStyleBackColor = true;
            BtnSetIcon.Click += BtnSetIcon_Click;
            // 
            // BtnSetHotkey
            // 
            BtnSetHotkey.Image = (Image)resources.GetObject("BtnSetHotkey.Image");
            BtnSetHotkey.Location = new Point(464, 93);
            BtnSetHotkey.Name = "BtnSetHotkey";
            BtnSetHotkey.Size = new Size(109, 84);
            BtnSetHotkey.TabIndex = 6;
            BtnSetHotkey.Text = "Set Hotkey";
            BtnSetHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnSetHotkey.UseVisualStyleBackColor = true;
            BtnSetHotkey.Click += BtnSetHotkey_Click;
            // 
            // BtnRemoveHotkey
            // 
            BtnRemoveHotkey.Image = (Image)resources.GetObject("BtnRemoveHotkey.Image");
            BtnRemoveHotkey.Location = new Point(579, 93);
            BtnRemoveHotkey.Name = "BtnRemoveHotkey";
            BtnRemoveHotkey.Size = new Size(109, 84);
            BtnRemoveHotkey.TabIndex = 22;
            BtnRemoveHotkey.Text = "Remove Hotkey";
            BtnRemoveHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRemoveHotkey.UseVisualStyleBackColor = true;
            BtnRemoveHotkey.Click += BtnRemoveHotkey_Click;
            // 
            // CmbInitialPowerScheme
            // 
            CmbInitialPowerScheme.Anchor = AnchorStyles.Left;
            tableLayoutPanel2.SetColumnSpan(CmbInitialPowerScheme, 2);
            CmbInitialPowerScheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbInitialPowerScheme.FormattingEnabled = true;
            CmbInitialPowerScheme.Location = new Point(246, 243);
            CmbInitialPowerScheme.Margin = new Padding(10, 11, 10, 11);
            CmbInitialPowerScheme.Name = "CmbInitialPowerScheme";
            CmbInitialPowerScheme.Size = new Size(269, 25);
            CmbInitialPowerScheme.TabIndex = 25;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(705, 315);
            tabControl1.TabIndex = 21;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tableLayoutPanel2);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(697, 285);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Power Plans";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tableLayoutPanel1);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(697, 285);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Rules";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.AutoScroll = true;
            tabPage3.Controls.Add(tableLayoutPanel5);
            tabPage3.Location = new Point(4, 26);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(697, 285);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Other Settings";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Controls.Add(groupBox4, 0, 0);
            tableLayoutPanel5.Controls.Add(groupBox1, 0, 1);
            tableLayoutPanel5.Controls.Add(GrbBatteryManagement, 0, 2);
            tableLayoutPanel5.Controls.Add(groupBox3, 1, 1);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 3);
            tableLayoutPanel5.MaximumSize = new Size(691, 279);
            tableLayoutPanel5.MinimumSize = new Size(691, 279);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 3;
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.Size = new Size(691, 279);
            tableLayoutPanel5.TabIndex = 30;
            // 
            // groupBox4
            // 
            tableLayoutPanel5.SetColumnSpan(groupBox4, 2);
            groupBox4.Controls.Add(tableLayoutPanel4);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(685, 116);
            groupBox4.TabIndex = 29;
            groupBox4.TabStop = false;
            groupBox4.Text = "Hotkey to cycle through Power Plans";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 5;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(RdbCycleAll, 3, 0);
            tableLayoutPanel4.Controls.Add(LblCycleHotkey, 0, 0);
            tableLayoutPanel4.Controls.Add(RdbCycleVisible, 3, 1);
            tableLayoutPanel4.Controls.Add(BtnRemoveCycleHotkey, 2, 0);
            tableLayoutPanel4.Controls.Add(BtnSetCycleHotkey, 1, 0);
            tableLayoutPanel4.Controls.Add(PopUpWindowLocation, 4, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 19);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(679, 94);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // RdbCycleAll
            // 
            RdbCycleAll.AutoSize = true;
            RdbCycleAll.Dock = DockStyle.Left;
            RdbCycleAll.Location = new Point(328, 3);
            RdbCycleAll.Name = "RdbCycleAll";
            RdbCycleAll.Size = new Size(174, 41);
            RdbCycleAll.TabIndex = 26;
            RdbCycleAll.TabStop = true;
            RdbCycleAll.Text = "Cycle through all existing Power Plans";
            RdbCycleAll.UseVisualStyleBackColor = true;
            // 
            // LblCycleHotkey
            // 
            LblCycleHotkey.AutoSize = true;
            LblCycleHotkey.BackColor = Color.Transparent;
            LblCycleHotkey.Dock = DockStyle.Fill;
            LblCycleHotkey.ForeColor = SystemColors.ControlText;
            LblCycleHotkey.Location = new Point(3, 0);
            LblCycleHotkey.Name = "LblCycleHotkey";
            tableLayoutPanel4.SetRowSpan(LblCycleHotkey, 2);
            LblCycleHotkey.Size = new Size(89, 94);
            LblCycleHotkey.TabIndex = 22;
            LblCycleHotkey.Text = "[ ---------- ]";
            LblCycleHotkey.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RdbCycleVisible
            // 
            RdbCycleVisible.AutoSize = true;
            RdbCycleVisible.Dock = DockStyle.Left;
            RdbCycleVisible.Location = new Point(328, 50);
            RdbCycleVisible.Name = "RdbCycleVisible";
            RdbCycleVisible.Size = new Size(174, 41);
            RdbCycleVisible.TabIndex = 27;
            RdbCycleVisible.TabStop = true;
            RdbCycleVisible.Text = "Cycle only through visible Power Plans";
            RdbCycleVisible.UseVisualStyleBackColor = true;
            // 
            // BtnRemoveCycleHotkey
            // 
            BtnRemoveCycleHotkey.Anchor = AnchorStyles.None;
            BtnRemoveCycleHotkey.Image = (Image)resources.GetObject("BtnRemoveCycleHotkey.Image");
            BtnRemoveCycleHotkey.Location = new Point(213, 5);
            BtnRemoveCycleHotkey.Name = "BtnRemoveCycleHotkey";
            tableLayoutPanel4.SetRowSpan(BtnRemoveCycleHotkey, 2);
            BtnRemoveCycleHotkey.Size = new Size(109, 84);
            BtnRemoveCycleHotkey.TabIndex = 25;
            BtnRemoveCycleHotkey.Text = "Remove Hotkey";
            BtnRemoveCycleHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRemoveCycleHotkey.UseVisualStyleBackColor = true;
            BtnRemoveCycleHotkey.Click += BtnRemoveCycleHotkey_Click;
            // 
            // BtnSetCycleHotkey
            // 
            BtnSetCycleHotkey.Anchor = AnchorStyles.None;
            BtnSetCycleHotkey.Image = (Image)resources.GetObject("BtnSetCycleHotkey.Image");
            BtnSetCycleHotkey.Location = new Point(98, 5);
            BtnSetCycleHotkey.Name = "BtnSetCycleHotkey";
            tableLayoutPanel4.SetRowSpan(BtnSetCycleHotkey, 2);
            BtnSetCycleHotkey.Size = new Size(109, 84);
            BtnSetCycleHotkey.TabIndex = 24;
            BtnSetCycleHotkey.Text = "Set Hotkey";
            BtnSetCycleHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnSetCycleHotkey.UseVisualStyleBackColor = true;
            BtnSetCycleHotkey.Click += BtnSetCycleHotkey_Click;
            // 
            // PopUpWindowLocation
            // 
            PopUpWindowLocation.Controls.Add(ComPopUpWindow);
            PopUpWindowLocation.Location = new Point(508, 3);
            PopUpWindowLocation.Name = "PopUpWindowLocation";
            tableLayoutPanel4.SetRowSpan(PopUpWindowLocation, 2);
            PopUpWindowLocation.Size = new Size(168, 88);
            PopUpWindowLocation.TabIndex = 28;
            PopUpWindowLocation.TabStop = false;
            PopUpWindowLocation.Text = "Pop-up window location";
            // 
            // ComPopUpWindow
            // 
            ComPopUpWindow.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            ComPopUpWindow.DropDownStyle = ComboBoxStyle.DropDownList;
            ComPopUpWindow.FormattingEnabled = true;
            ComPopUpWindow.Location = new Point(9, 33);
            ComPopUpWindow.Name = "ComPopUpWindow";
            ComPopUpWindow.Size = new Size(152, 25);
            ComPopUpWindow.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel6);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 125);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(339, 70);
            groupBox1.TabIndex = 30;
            groupBox1.TabStop = false;
            groupBox1.Text = "Color Theme";
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Controls.Add(CmbColorTheme, 0, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 19);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Size = new Size(333, 48);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // GrbBatteryManagement
            // 
            tableLayoutPanel5.SetColumnSpan(GrbBatteryManagement, 2);
            GrbBatteryManagement.Controls.Add(tableLayoutPanel3);
            GrbBatteryManagement.Dock = DockStyle.Fill;
            GrbBatteryManagement.Location = new Point(3, 201);
            GrbBatteryManagement.Name = "GrbBatteryManagement";
            GrbBatteryManagement.Size = new Size(685, 75);
            GrbBatteryManagement.TabIndex = 32;
            GrbBatteryManagement.TabStop = false;
            GrbBatteryManagement.Text = "Battery Management";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.Controls.Add(CmbAcPowerScheme, 1, 0);
            tableLayoutPanel3.Controls.Add(label1, 0, 0);
            tableLayoutPanel3.Controls.Add(label3, 2, 0);
            tableLayoutPanel3.Controls.Add(CmbBatteryPowerScheme, 3, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel3.Size = new Size(679, 53);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // CmbAcPowerScheme
            // 
            CmbAcPowerScheme.Anchor = AnchorStyles.Left;
            CmbAcPowerScheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbAcPowerScheme.FormattingEnabled = true;
            CmbAcPowerScheme.Location = new Point(179, 14);
            CmbAcPowerScheme.Margin = new Padding(10, 11, 10, 11);
            CmbAcPowerScheme.Name = "CmbAcPowerScheme";
            CmbAcPowerScheme.Size = new Size(149, 25);
            CmbAcPowerScheme.TabIndex = 26;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(15, 9);
            label1.Name = "label1";
            label1.Size = new Size(151, 34);
            label1.TabIndex = 0;
            label1.Text = "Default Power Plan when on AC:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(353, 9);
            label3.Name = "label3";
            label3.Size = new Size(151, 34);
            label3.TabIndex = 0;
            label3.Text = "Default Power Plan when on Battery:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbBatteryPowerScheme
            // 
            CmbBatteryPowerScheme.Anchor = AnchorStyles.Left;
            CmbBatteryPowerScheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbBatteryPowerScheme.FormattingEnabled = true;
            CmbBatteryPowerScheme.Location = new Point(517, 14);
            CmbBatteryPowerScheme.Margin = new Padding(10, 11, 10, 11);
            CmbBatteryPowerScheme.Name = "CmbBatteryPowerScheme";
            CmbBatteryPowerScheme.Size = new Size(152, 25);
            CmbBatteryPowerScheme.TabIndex = 26;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tableLayoutPanel8);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(348, 125);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(340, 70);
            groupBox3.TabIndex = 33;
            groupBox3.TabStop = false;
            groupBox3.Text = "Notifications";
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 1;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.Controls.Add(ChbShowToastNotifications, 0, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(3, 19);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 1;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.Size = new Size(334, 48);
            tableLayoutPanel8.TabIndex = 0;
            // 
            // ChbShowToastNotifications
            // 
            ChbShowToastNotifications.Anchor = AnchorStyles.None;
            ChbShowToastNotifications.AutoSize = true;
            ChbShowToastNotifications.Location = new Point(24, 13);
            ChbShowToastNotifications.Name = "ChbShowToastNotifications";
            ChbShowToastNotifications.Size = new Size(285, 21);
            ChbShowToastNotifications.TabIndex = 0;
            ChbShowToastNotifications.Text = "Show notification when switching Power Plan";
            ChbShowToastNotifications.UseVisualStyleBackColor = true;
            // 
            // SettingsDlg
            // 
            AcceptButton = BtnOk;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(705, 362);
            Controls.Add(tabControl1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(721, 401);
            Name = "SettingsDlg";
            Text = "PowerPlanSwitcher - Settings";
            ((System.ComponentModel.ISupportInitialize)DgvPowerSchemes).EndInit();
            ((System.ComponentModel.ISupportInitialize)DgvPowerRules).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            PopUpWindowLocation.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            GrbBatteryManagement.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            groupBox3.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel8.PerformLayout();
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
        private ComboBox CmbColorTheme;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button BtnRemoveIcon;
        private Button BtnSetHotkey;
        private Button BtnRemoveHotkey;
        private Button BtnSetIcon;
        private DataGridViewTextBoxColumn DgcRuleIndex;
        private DataGridViewTextBoxColumn DgcRuleType;
        private DataGridViewTextBoxColumn DgcRulePath;
        private DataGridViewImageColumn DgcRuleSchemeIcon;
        private DataGridViewTextBoxColumn DgcRuleSchemeName;
        private DataGridViewCheckBoxColumn DgcActive;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel4;
        private RadioButton RdbCycleAll;
        private Label LblCycleHotkey;
        private RadioButton RdbCycleVisible;
        private Button BtnRemoveCycleHotkey;
        private Button BtnSetCycleHotkey;
        private CheckBox ChbActivateInitialPowerScheme;
        private ComboBox CmbInitialPowerScheme;
        private TableLayoutPanel tableLayoutPanel5;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel6;
        private GroupBox GrbBatteryManagement;
        private TableLayoutPanel tableLayoutPanel3;
        private ComboBox CmbAcPowerScheme;
        private Label label1;
        private Label label3;
        private ComboBox CmbBatteryPowerScheme;
        private DataGridViewCheckBoxColumn DgcVisible;
        private DataGridViewImageColumn DgcIcon;
        private DataGridViewTextBoxColumn DgcName;
        private DataGridViewTextBoxColumn DgcHotkey;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel8;
        private CheckBox ChbShowToastNotifications;
        private GroupBox PopUpWindowLocation;
        private ComboBox ComPopUpWindow;
    }
}
