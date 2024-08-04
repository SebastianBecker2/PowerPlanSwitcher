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
            TacSettingsCategories = new TabControl();
            TapPowerSchemes = new TabPage();
            TapRules = new TabPage();
            TapOtherSettings = new TabPage();
            tableLayoutPanel5 = new TableLayoutPanel();
            groupBox4 = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            RdbCycleAll = new RadioButton();
            LblCycleHotkey = new Label();
            RdbCycleVisible = new RadioButton();
            BtnRemoveCycleHotkey = new Button();
            BtnSetCycleHotkey = new Button();
            groupBox1 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            GrbBatteryManagement = new GroupBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            groupBox2 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            CmbAcPowerScheme = new ComboBox();
            label1 = new Label();
            CmbBatteryPowerScheme = new ComboBox();
            label3 = new Label();
            groupBox5 = new GroupBox();
            CmbPopUpWindowBM = new ComboBox();
            PopUpWindowLocation = new GroupBox();
            CmbPopUpWindowGlobal = new ComboBox();
            tableLayoutPanel8 = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)DgvPowerSchemes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DgvPowerRules).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            TacSettingsCategories.SuspendLayout();
            TapPowerSchemes.SuspendLayout();
            TapRules.SuspendLayout();
            TapOtherSettings.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            GrbBatteryManagement.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox5.SuspendLayout();
            PopUpWindowLocation.SuspendLayout();
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
            DgvPowerSchemes.Size = new Size(455, 206);
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
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgcHotkey.DefaultCellStyle = dataGridViewCellStyle1;
            DgcHotkey.HeaderText = "Hotkey";
            DgcHotkey.Name = "DgcHotkey";
            DgcHotkey.ReadOnly = true;
            DgcHotkey.Width = 70;
            // 
            // BtnOk
            // 
            BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOk.Location = new Point(537, 295);
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
            BtnCancel.Location = new Point(618, 295);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnCreateRuleFromProcess
            // 
            BtnCreateRuleFromProcess.Image = (Image)resources.GetObject("BtnCreateRuleFromProcess.Image");
            BtnCreateRuleFromProcess.Location = new Point(118, 178);
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
            tableLayoutPanel1.SetColumnSpan(DgvPowerRules, 6);
            DgvPowerRules.Dock = DockStyle.Fill;
            DgvPowerRules.Location = new Point(3, 3);
            DgvPowerRules.MultiSelect = false;
            DgvPowerRules.Name = "DgvPowerRules";
            DgvPowerRules.RowHeadersVisible = false;
            DgvPowerRules.RowTemplate.Height = 26;
            DgvPowerRules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvPowerRules.Size = new Size(685, 169);
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
            BtnAddPowerRule.Image = (Image)resources.GetObject("BtnAddPowerRule.Image");
            BtnAddPowerRule.Location = new Point(3, 178);
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
            BtnEditPowerRule.Image = (Image)resources.GetObject("BtnEditPowerRule.Image");
            BtnEditPowerRule.Location = new Point(233, 178);
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
            BtnAscentPowerRule.Image = (Image)resources.GetObject("BtnAscentPowerRule.Image");
            BtnAscentPowerRule.Location = new Point(463, 178);
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
            BtnDescentPowerRule.Image = (Image)resources.GetObject("BtnDescentPowerRule.Image");
            BtnDescentPowerRule.Location = new Point(578, 178);
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
            BtnDeletePowerRule.Image = (Image)resources.GetObject("BtnDeletePowerRule.Image");
            BtnDeletePowerRule.Location = new Point(348, 178);
            BtnDeletePowerRule.Name = "BtnDeletePowerRule";
            BtnDeletePowerRule.Size = new Size(109, 74);
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
            CmbColorTheme.Location = new Point(81, 8);
            CmbColorTheme.Name = "CmbColorTheme";
            CmbColorTheme.Size = new Size(170, 23);
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
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(691, 255);
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
            tableLayoutPanel2.Size = new Size(691, 255);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // ChbActivateInitialPowerScheme
            // 
            ChbActivateInitialPowerScheme.AutoSize = true;
            ChbActivateInitialPowerScheme.Dock = DockStyle.Fill;
            ChbActivateInitialPowerScheme.Location = new Point(10, 222);
            ChbActivateInitialPowerScheme.Margin = new Padding(10);
            ChbActivateInitialPowerScheme.Name = "ChbActivateInitialPowerScheme";
            ChbActivateInitialPowerScheme.Size = new Size(199, 23);
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
            BtnRemoveIcon.Size = new Size(109, 74);
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
            BtnSetIcon.Size = new Size(109, 74);
            BtnSetIcon.TabIndex = 23;
            BtnSetIcon.Text = "Set Icon";
            BtnSetIcon.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnSetIcon.UseVisualStyleBackColor = true;
            BtnSetIcon.Click += BtnSetIcon_Click;
            // 
            // BtnSetHotkey
            // 
            BtnSetHotkey.Image = (Image)resources.GetObject("BtnSetHotkey.Image");
            BtnSetHotkey.Location = new Point(464, 83);
            BtnSetHotkey.Name = "BtnSetHotkey";
            BtnSetHotkey.Size = new Size(109, 74);
            BtnSetHotkey.TabIndex = 6;
            BtnSetHotkey.Text = "Set Hotkey";
            BtnSetHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnSetHotkey.UseVisualStyleBackColor = true;
            BtnSetHotkey.Click += BtnSetHotkey_Click;
            // 
            // BtnRemoveHotkey
            // 
            BtnRemoveHotkey.Image = (Image)resources.GetObject("BtnRemoveHotkey.Image");
            BtnRemoveHotkey.Location = new Point(579, 83);
            BtnRemoveHotkey.Name = "BtnRemoveHotkey";
            BtnRemoveHotkey.Size = new Size(109, 74);
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
            CmbInitialPowerScheme.Location = new Point(229, 222);
            CmbInitialPowerScheme.Margin = new Padding(10);
            CmbInitialPowerScheme.Name = "CmbInitialPowerScheme";
            CmbInitialPowerScheme.Size = new Size(269, 23);
            CmbInitialPowerScheme.TabIndex = 25;
            // 
            // TacSettingsCategories
            // 
            TacSettingsCategories.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TacSettingsCategories.Controls.Add(TapPowerSchemes);
            TacSettingsCategories.Controls.Add(TapRules);
            TacSettingsCategories.Controls.Add(TapOtherSettings);
            TacSettingsCategories.Location = new Point(0, 0);
            TacSettingsCategories.Name = "TacSettingsCategories";
            TacSettingsCategories.SelectedIndex = 0;
            TacSettingsCategories.Size = new Size(705, 289);
            TacSettingsCategories.TabIndex = 21;
            // 
            // TapPowerSchemes
            // 
            TapPowerSchemes.Controls.Add(tableLayoutPanel2);
            TapPowerSchemes.Location = new Point(4, 24);
            TapPowerSchemes.Name = "TapPowerSchemes";
            TapPowerSchemes.Padding = new Padding(3);
            TapPowerSchemes.Size = new Size(697, 261);
            TapPowerSchemes.TabIndex = 0;
            TapPowerSchemes.Text = "Power Plans";
            TapPowerSchemes.UseVisualStyleBackColor = true;
            // 
            // TapRules
            // 
            TapRules.Controls.Add(tableLayoutPanel1);
            TapRules.Location = new Point(4, 24);
            TapRules.Name = "TapRules";
            TapRules.Padding = new Padding(3);
            TapRules.Size = new Size(697, 261);
            TapRules.TabIndex = 1;
            TapRules.Text = "Rules";
            TapRules.UseVisualStyleBackColor = true;
            // 
            // TapOtherSettings
            // 
            TapOtherSettings.AutoScroll = true;
            TapOtherSettings.Controls.Add(tableLayoutPanel5);
            TapOtherSettings.Location = new Point(4, 24);
            TapOtherSettings.Name = "TapOtherSettings";
            TapOtherSettings.Padding = new Padding(3);
            TapOtherSettings.Size = new Size(697, 261);
            TapOtherSettings.TabIndex = 2;
            TapOtherSettings.Text = "Other Settings";
            TapOtherSettings.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(groupBox4, 0, 0);
            tableLayoutPanel5.Controls.Add(groupBox1, 0, 1);
            tableLayoutPanel5.Controls.Add(GrbBatteryManagement, 0, 2);
            tableLayoutPanel5.Controls.Add(PopUpWindowLocation, 1, 1);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 3);
            tableLayoutPanel5.MaximumSize = new Size(691, 257);
            tableLayoutPanel5.MinimumSize = new Size(691, 257);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 3;
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(691, 257);
            tableLayoutPanel5.TabIndex = 30;
            // 
            // groupBox4
            // 
            tableLayoutPanel5.SetColumnSpan(groupBox4, 2);
            groupBox4.Controls.Add(tableLayoutPanel4);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(685, 102);
            groupBox4.TabIndex = 29;
            groupBox4.TabStop = false;
            groupBox4.Text = "Hotkey to cycle through Power Plans";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 4;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.Controls.Add(RdbCycleAll, 3, 0);
            tableLayoutPanel4.Controls.Add(LblCycleHotkey, 0, 0);
            tableLayoutPanel4.Controls.Add(RdbCycleVisible, 3, 1);
            tableLayoutPanel4.Controls.Add(BtnRemoveCycleHotkey, 2, 0);
            tableLayoutPanel4.Controls.Add(BtnSetCycleHotkey, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 19);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(679, 80);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // RdbCycleAll
            // 
            RdbCycleAll.AutoSize = true;
            RdbCycleAll.Dock = DockStyle.Left;
            RdbCycleAll.Location = new Point(447, 3);
            RdbCycleAll.Name = "RdbCycleAll";
            RdbCycleAll.Size = new Size(226, 34);
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
            LblCycleHotkey.Size = new Size(208, 80);
            LblCycleHotkey.TabIndex = 22;
            LblCycleHotkey.Text = "[ ---------- ]";
            LblCycleHotkey.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RdbCycleVisible
            // 
            RdbCycleVisible.AutoSize = true;
            RdbCycleVisible.Dock = DockStyle.Left;
            RdbCycleVisible.Location = new Point(447, 43);
            RdbCycleVisible.Name = "RdbCycleVisible";
            RdbCycleVisible.Size = new Size(229, 34);
            RdbCycleVisible.TabIndex = 27;
            RdbCycleVisible.TabStop = true;
            RdbCycleVisible.Text = "Cycle only through visible Power Plans";
            RdbCycleVisible.UseVisualStyleBackColor = true;
            // 
            // BtnRemoveCycleHotkey
            // 
            BtnRemoveCycleHotkey.Anchor = AnchorStyles.None;
            BtnRemoveCycleHotkey.Image = (Image)resources.GetObject("BtnRemoveCycleHotkey.Image");
            BtnRemoveCycleHotkey.Location = new Point(332, 3);
            BtnRemoveCycleHotkey.Name = "BtnRemoveCycleHotkey";
            tableLayoutPanel4.SetRowSpan(BtnRemoveCycleHotkey, 2);
            BtnRemoveCycleHotkey.Size = new Size(109, 74);
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
            BtnSetCycleHotkey.Location = new Point(217, 3);
            BtnSetCycleHotkey.Name = "BtnSetCycleHotkey";
            tableLayoutPanel4.SetRowSpan(BtnSetCycleHotkey, 2);
            BtnSetCycleHotkey.Size = new Size(109, 74);
            BtnSetCycleHotkey.TabIndex = 24;
            BtnSetCycleHotkey.Text = "Set Hotkey";
            BtnSetCycleHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnSetCycleHotkey.UseVisualStyleBackColor = true;
            BtnSetCycleHotkey.Click += BtnSetCycleHotkey_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel6);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 111);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(339, 62);
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
            tableLayoutPanel6.Size = new Size(333, 40);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // GrbBatteryManagement
            // 
            tableLayoutPanel5.SetColumnSpan(GrbBatteryManagement, 2);
            GrbBatteryManagement.Controls.Add(tableLayoutPanel7);
            GrbBatteryManagement.Dock = DockStyle.Fill;
            GrbBatteryManagement.Location = new Point(3, 179);
            GrbBatteryManagement.Name = "GrbBatteryManagement";
            GrbBatteryManagement.Size = new Size(685, 75);
            GrbBatteryManagement.TabIndex = 32;
            GrbBatteryManagement.TabStop = false;
            GrbBatteryManagement.Text = "Battery Management";
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Controls.Add(groupBox2, 0, 0);
            tableLayoutPanel7.Controls.Add(groupBox5, 1, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(3, 19);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 1;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 57F));
            tableLayoutPanel7.Size = new Size(679, 53);
            tableLayoutPanel7.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel3);
            groupBox2.Dock = DockStyle.Left;
            groupBox2.Location = new Point(3, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(499, 47);
            groupBox2.TabIndex = 22;
            groupBox2.TabStop = false;
            groupBox2.Text = "Default Power Plan when on ";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.Controls.Add(CmbAcPowerScheme, 1, 0);
            tableLayoutPanel3.Controls.Add(label1, 0, 0);
            tableLayoutPanel3.Controls.Add(CmbBatteryPowerScheme, 3, 0);
            tableLayoutPanel3.Controls.Add(label3, 2, 0);
            tableLayoutPanel3.Location = new Point(3, 8);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(493, 42);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // CmbAcPowerScheme
            // 
            CmbAcPowerScheme.Anchor = AnchorStyles.Left;
            CmbAcPowerScheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbAcPowerScheme.FormattingEnabled = true;
            CmbAcPowerScheme.Items.AddRange(new object[] { "Off" });
            CmbAcPowerScheme.Location = new Point(42, 10);
            CmbAcPowerScheme.Margin = new Padding(10);
            CmbAcPowerScheme.Name = "CmbAcPowerScheme";
            CmbAcPowerScheme.Size = new Size(149, 23);
            CmbAcPowerScheme.TabIndex = 26;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(3, 13);
            label1.Name = "label1";
            label1.Size = new Size(26, 15);
            label1.TabIndex = 0;
            label1.Text = "AC:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbBatteryPowerScheme
            // 
            CmbBatteryPowerScheme.Anchor = AnchorStyles.Left;
            CmbBatteryPowerScheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbBatteryPowerScheme.FormattingEnabled = true;
            CmbBatteryPowerScheme.Items.AddRange(new object[] { "Off" });
            CmbBatteryPowerScheme.Location = new Point(264, 10);
            CmbBatteryPowerScheme.Margin = new Padding(10);
            CmbBatteryPowerScheme.Name = "CmbBatteryPowerScheme";
            CmbBatteryPowerScheme.Size = new Size(152, 23);
            CmbBatteryPowerScheme.TabIndex = 26;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(204, 13);
            label3.Name = "label3";
            label3.Size = new Size(47, 15);
            label3.TabIndex = 0;
            label3.Text = "Battery:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // groupBox5
            // 
            groupBox5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox5.Controls.Add(CmbPopUpWindowBM);
            groupBox5.Location = new Point(508, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(168, 47);
            groupBox5.TabIndex = 29;
            groupBox5.TabStop = false;
            groupBox5.Text = "Pop-up window location";
            // 
            // CmbPopUpWindowBM
            // 
            CmbPopUpWindowBM.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            CmbPopUpWindowBM.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbPopUpWindowBM.FormattingEnabled = true;
            CmbPopUpWindowBM.Location = new Point(9, 16);
            CmbPopUpWindowBM.Name = "CmbPopUpWindowBM";
            CmbPopUpWindowBM.Size = new Size(152, 23);
            CmbPopUpWindowBM.TabIndex = 0;
            // 
            // PopUpWindowLocation
            // 
            PopUpWindowLocation.Controls.Add(tableLayoutPanel8);
            PopUpWindowLocation.Dock = DockStyle.Fill;
            PopUpWindowLocation.Location = new Point(348, 111);
            PopUpWindowLocation.Name = "PopUpWindowLocation";
            PopUpWindowLocation.Size = new Size(340, 62);
            PopUpWindowLocation.TabIndex = 28;
            PopUpWindowLocation.TabStop = false;
            PopUpWindowLocation.Text = "Global Pop-up window";
            // 
            // CmbPopUpWindowGlobal
            // 
            CmbPopUpWindowGlobal.Anchor = AnchorStyles.None;
            CmbPopUpWindowGlobal.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbPopUpWindowGlobal.FormattingEnabled = true;
            CmbPopUpWindowGlobal.Location = new Point(84, 8);
            CmbPopUpWindowGlobal.Name = "CmbPopUpWindowGlobal";
            CmbPopUpWindowGlobal.Size = new Size(165, 23);
            CmbPopUpWindowGlobal.TabIndex = 0;
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 1;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.Controls.Add(CmbPopUpWindowGlobal, 0, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(3, 19);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 1;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.Size = new Size(334, 40);
            tableLayoutPanel8.TabIndex = 1;
            // 
            // SettingsDlg
            // 
            AcceptButton = BtnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(705, 330);
            Controls.Add(TacSettingsCategories);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(721, 369);
            Name = "SettingsDlg";
            Text = "PowerPlanSwitcher - Settings";
            ((System.ComponentModel.ISupportInitialize)DgvPowerSchemes).EndInit();
            ((System.ComponentModel.ISupportInitialize)DgvPowerRules).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            TacSettingsCategories.ResumeLayout(false);
            TapPowerSchemes.ResumeLayout(false);
            TapRules.ResumeLayout(false);
            TapOtherSettings.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            GrbBatteryManagement.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            groupBox5.ResumeLayout(false);
            PopUpWindowLocation.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
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
        private TabControl TacSettingsCategories;
        private TabPage TapPowerSchemes;
        private TabPage TapRules;
        private TabPage TapOtherSettings;
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
        private GroupBox PopUpWindowLocation;
        private ComboBox CmbPopUpWindowGlobal;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel7;
        private GroupBox groupBox5;
        private ComboBox CmbPopUpWindowBM;
        private TableLayoutPanel tableLayoutPanel8;
    }
}
