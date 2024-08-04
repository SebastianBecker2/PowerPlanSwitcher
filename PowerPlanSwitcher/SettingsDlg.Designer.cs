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
            DgvPowerRules = new DataGridView();
            DgcRuleIndex = new DataGridViewTextBoxColumn();
            DgcRuleDescription = new DataGridViewTextBoxColumn();
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
            groupBox3 = new GroupBox();
            tableLayoutPanel8 = new TableLayoutPanel();
            CmbPopUpWindowGlobal = new ComboBox();
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
            DgvPowerSchemes.Size = new Size(455, 195);
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
            BtnOk.Location = new Point(537, 284);
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
            BtnCancel.Location = new Point(618, 284);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // DgvPowerRules
            // 
            DgvPowerRules.AllowUserToAddRows = false;
            DgvPowerRules.AllowUserToDeleteRows = false;
            DgvPowerRules.AllowUserToResizeColumns = false;
            DgvPowerRules.AllowUserToResizeRows = false;
            DgvPowerRules.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DgvPowerRules.Columns.AddRange(new DataGridViewColumn[] { DgcRuleIndex, DgcRuleDescription, DgcRuleSchemeIcon, DgcRuleSchemeName, DgcActive });
            tableLayoutPanel1.SetColumnSpan(DgvPowerRules, 5);
            DgvPowerRules.Dock = DockStyle.Fill;
            DgvPowerRules.Location = new Point(3, 3);
            DgvPowerRules.MultiSelect = false;
            DgvPowerRules.Name = "DgvPowerRules";
            DgvPowerRules.RowHeadersVisible = false;
            DgvPowerRules.RowTemplate.Height = 26;
            DgvPowerRules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvPowerRules.Size = new Size(685, 158);
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
            // DgcRuleDescription
            // 
            DgcRuleDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgcRuleDescription.HeaderText = "Description";
            DgcRuleDescription.Name = "DgcRuleDescription";
            DgcRuleDescription.ReadOnly = true;
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
            BtnAddPowerRule.Location = new Point(3, 167);
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
            BtnEditPowerRule.Location = new Point(118, 167);
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
            BtnAscentPowerRule.Location = new Point(348, 167);
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
            BtnDescentPowerRule.Location = new Point(463, 167);
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
            BtnDeletePowerRule.Location = new Point(233, 167);
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
            CmbColorTheme.Location = new Point(81, 29);
            CmbColorTheme.Name = "CmbColorTheme";
            CmbColorTheme.Size = new Size(170, 23);
            CmbColorTheme.TabIndex = 17;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(DgvPowerRules, 0, 0);
            tableLayoutPanel1.Controls.Add(BtnDeletePowerRule, 2, 1);
            tableLayoutPanel1.Controls.Add(BtnDescentPowerRule, 4, 1);
            tableLayoutPanel1.Controls.Add(BtnAscentPowerRule, 3, 1);
            tableLayoutPanel1.Controls.Add(BtnEditPowerRule, 1, 1);
            tableLayoutPanel1.Controls.Add(BtnAddPowerRule, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(691, 244);
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
            tableLayoutPanel2.Size = new Size(691, 244);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // ChbActivateInitialPowerScheme
            // 
            ChbActivateInitialPowerScheme.AutoSize = true;
            ChbActivateInitialPowerScheme.Dock = DockStyle.Fill;
            ChbActivateInitialPowerScheme.Location = new Point(10, 211);
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
            CmbInitialPowerScheme.Location = new Point(229, 211);
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
            TacSettingsCategories.Size = new Size(705, 278);
            TacSettingsCategories.TabIndex = 21;
            // 
            // TapPowerSchemes
            // 
            TapPowerSchemes.Controls.Add(tableLayoutPanel2);
            TapPowerSchemes.Location = new Point(4, 24);
            TapPowerSchemes.Name = "TapPowerSchemes";
            TapPowerSchemes.Padding = new Padding(3);
            TapPowerSchemes.Size = new Size(697, 250);
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
            TapRules.Size = new Size(697, 250);
            TapRules.TabIndex = 1;
            TapRules.Text = "Rules";
            TapRules.UseVisualStyleBackColor = true;
            // 
            // TapOtherSettings
            // 
            TapOtherSettings.Controls.Add(tableLayoutPanel5);
            TapOtherSettings.Location = new Point(4, 24);
            TapOtherSettings.Name = "TapOtherSettings";
            TapOtherSettings.Padding = new Padding(3);
            TapOtherSettings.Size = new Size(697, 250);
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
            tableLayoutPanel5.Controls.Add(groupBox3, 1, 1);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tableLayoutPanel5.Size = new Size(691, 244);
            tableLayoutPanel5.TabIndex = 30;
            // 
            // groupBox4
            // 
            tableLayoutPanel5.SetColumnSpan(groupBox4, 2);
            groupBox4.Controls.Add(tableLayoutPanel4);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(685, 128);
            groupBox4.TabIndex = 29;
            groupBox4.TabStop = false;
            groupBox4.Text = "Hotkey to cycle through Power Plans";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 4;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
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
            tableLayoutPanel4.Size = new Size(679, 106);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // RdbCycleAll
            // 
            RdbCycleAll.AutoSize = true;
            RdbCycleAll.Dock = DockStyle.Left;
            RdbCycleAll.Location = new Point(433, 3);
            RdbCycleAll.Name = "RdbCycleAll";
            RdbCycleAll.Size = new Size(226, 47);
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
            LblCycleHotkey.Size = new Size(194, 106);
            LblCycleHotkey.TabIndex = 22;
            LblCycleHotkey.Text = "[ ---------- ]";
            LblCycleHotkey.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RdbCycleVisible
            // 
            RdbCycleVisible.AutoSize = true;
            RdbCycleVisible.Dock = DockStyle.Left;
            RdbCycleVisible.Location = new Point(433, 56);
            RdbCycleVisible.Name = "RdbCycleVisible";
            RdbCycleVisible.Size = new Size(229, 47);
            RdbCycleVisible.TabIndex = 27;
            RdbCycleVisible.TabStop = true;
            RdbCycleVisible.Text = "Cycle only through visible Power Plans";
            RdbCycleVisible.UseVisualStyleBackColor = true;
            // 
            // BtnRemoveCycleHotkey
            // 
            BtnRemoveCycleHotkey.Anchor = AnchorStyles.None;
            BtnRemoveCycleHotkey.Image = (Image)resources.GetObject("BtnRemoveCycleHotkey.Image");
            BtnRemoveCycleHotkey.Location = new Point(318, 16);
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
            BtnSetCycleHotkey.Location = new Point(203, 16);
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
            groupBox1.Location = new Point(3, 137);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(339, 104);
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
            tableLayoutPanel6.Size = new Size(333, 82);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tableLayoutPanel8);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(348, 137);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(340, 104);
            groupBox3.TabIndex = 33;
            groupBox3.TabStop = false;
            groupBox3.Text = "Notification Location";
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 1;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.Controls.Add(CmbPopUpWindowGlobal, 0, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(3, 19);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 1;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.Size = new Size(334, 82);
            tableLayoutPanel8.TabIndex = 0;
            // 
            // CmbPopUpWindowGlobal
            // 
            CmbPopUpWindowGlobal.Anchor = AnchorStyles.None;
            CmbPopUpWindowGlobal.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbPopUpWindowGlobal.FormattingEnabled = true;
            CmbPopUpWindowGlobal.Location = new Point(82, 29);
            CmbPopUpWindowGlobal.Name = "CmbPopUpWindowGlobal";
            CmbPopUpWindowGlobal.Size = new Size(170, 23);
            CmbPopUpWindowGlobal.TabIndex = 17;
            // 
            // SettingsDlg
            // 
            AcceptButton = BtnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(705, 319);
            Controls.Add(TacSettingsCategories);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(721, 358);
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
            groupBox3.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView DgvPowerSchemes;
        private Button BtnOk;
        private Button BtnCancel;
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
        private DataGridViewCheckBoxColumn DgcVisible;
        private DataGridViewImageColumn DgcIcon;
        private DataGridViewTextBoxColumn DgcName;
        private DataGridViewTextBoxColumn DgcHotkey;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel8;
        private DataGridViewTextBoxColumn DgcRuleIndex;
        private DataGridViewTextBoxColumn DgcRuleDescription;
        private DataGridViewImageColumn DgcRuleSchemeIcon;
        private DataGridViewTextBoxColumn DgcRuleSchemeName;
        private DataGridViewCheckBoxColumn DgcActive;
        private ComboBox CmbPopUpWindowGlobal;
    }
}
