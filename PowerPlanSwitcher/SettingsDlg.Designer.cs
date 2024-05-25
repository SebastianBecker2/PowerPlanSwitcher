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
            var dataGridViewCellStyle5 = new DataGridViewCellStyle();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDlg));
            DgvPowerSchemes = new DataGridView();
            DgcVisible = new DataGridViewCheckBoxColumn();
            DgcIcon = new DataGridViewImageColumn();
            DgcName = new DataGridViewTextBoxColumn();
            AcPowerCheckBox = new DataGridViewCheckBoxColumn();
            BatteryCheckBox = new DataGridViewCheckBoxColumn();
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
            NudPowerRuleCheckInterval = new NumericUpDown();
            label2 = new Label();
            CmbColorTheme = new ComboBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            BtnSetHotkey = new Button();
            BtnSetIcon = new Button();
            BtnRemoveHotkey = new Button();
            BtnRemoveIcon = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            groupBox4 = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            RdbCycleAll = new RadioButton();
            LblCycleHotkey = new Label();
            RdbCycleVisible = new RadioButton();
            BtnRemoveCycleHotkey = new Button();
            BtnSetCycleHotkey = new Button();
            ChbActivateInitialPowerScheme = new CheckBox();
            CmbInitialPowerScheme = new ComboBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            groupBox2 = new GroupBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)DgvPowerSchemes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DgvPowerRules).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NudPowerRuleCheckInterval).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            SuspendLayout();
            // 
            // DgvPowerSchemes
            // 
            DgvPowerSchemes.AllowUserToAddRows = false;
            DgvPowerSchemes.AllowUserToDeleteRows = false;
            DgvPowerSchemes.AllowUserToResizeColumns = false;
            DgvPowerSchemes.AllowUserToResizeRows = false;
            DgvPowerSchemes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DgvPowerSchemes.Columns.AddRange(new DataGridViewColumn[] { DgcVisible, DgcIcon, DgcName, AcPowerCheckBox, BatteryCheckBox, DgcHotkey });
            tableLayoutPanel2.SetColumnSpan(DgvPowerSchemes, 2);
            DgvPowerSchemes.Dock = DockStyle.Fill;
            DgvPowerSchemes.Location = new Point(3, 3);
            DgvPowerSchemes.MultiSelect = false;
            DgvPowerSchemes.Name = "DgvPowerSchemes";
            DgvPowerSchemes.RowHeadersVisible = false;
            tableLayoutPanel2.SetRowSpan(DgvPowerSchemes, 2);
            DgvPowerSchemes.RowTemplate.Height = 26;
            DgvPowerSchemes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvPowerSchemes.Size = new Size(455, 154);
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
            // AcPowerCheckBox
            // 
            AcPowerCheckBox.HeaderText = "AC";
            AcPowerCheckBox.Name = "AcPowerCheckBox";
            AcPowerCheckBox.Resizable = DataGridViewTriState.True;
            AcPowerCheckBox.SortMode = DataGridViewColumnSortMode.Automatic;
            AcPowerCheckBox.Width = 30;
            // 
            // BatteryCheckBox
            // 
            BatteryCheckBox.HeaderText = "BA";
            BatteryCheckBox.Name = "BatteryCheckBox";
            BatteryCheckBox.Resizable = DataGridViewTriState.True;
            BatteryCheckBox.SortMode = DataGridViewColumnSortMode.Automatic;
            BatteryCheckBox.Width = 30;
            // 
            // DgcHotkey
            // 
            DgcHotkey.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgcHotkey.DefaultCellStyle = dataGridViewCellStyle5;
            DgcHotkey.HeaderText = "Hotkey";
            DgcHotkey.Name = "DgcHotkey";
            DgcHotkey.ReadOnly = true;
            DgcHotkey.Width = 70;
            // 
            // BtnOk
            // 
            BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOk.Location = new Point(537, 243);
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
            BtnCancel.Location = new Point(618, 243);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnCreateRuleFromProcess
            // 
            BtnCreateRuleFromProcess.Image = (Image)resources.GetObject("BtnCreateRuleFromProcess.Image");
            BtnCreateRuleFromProcess.Location = new Point(118, 126);
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
            DgvPowerRules.Size = new Size(685, 117);
            DgvPowerRules.TabIndex = 7;
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
            BtnAddPowerRule.Location = new Point(3, 126);
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
            BtnEditPowerRule.Location = new Point(233, 126);
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
            BtnAscentPowerRule.Location = new Point(463, 126);
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
            BtnDescentPowerRule.Location = new Point(578, 126);
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
            BtnDeletePowerRule.Location = new Point(348, 126);
            BtnDeletePowerRule.Name = "BtnDeletePowerRule";
            BtnDeletePowerRule.Size = new Size(109, 74);
            BtnDeletePowerRule.TabIndex = 9;
            BtnDeletePowerRule.Text = "Delete selected Rule";
            BtnDeletePowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnDeletePowerRule.UseVisualStyleBackColor = true;
            BtnDeletePowerRule.Click += HandleBtnDeletePowerRuleClick;
            // 
            // NudPowerRuleCheckInterval
            // 
            NudPowerRuleCheckInterval.Anchor = AnchorStyles.Right;
            NudPowerRuleCheckInterval.Location = new Point(80, 22);
            NudPowerRuleCheckInterval.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            NudPowerRuleCheckInterval.Name = "NudPowerRuleCheckInterval";
            NudPowerRuleCheckInterval.Size = new Size(84, 23);
            NudPowerRuleCheckInterval.TabIndex = 14;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(170, 26);
            label2.Name = "label2";
            label2.Size = new Size(51, 15);
            label2.TabIndex = 13;
            label2.Text = "Seconds";
            // 
            // CmbColorTheme
            // 
            CmbColorTheme.Anchor = AnchorStyles.None;
            CmbColorTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbColorTheme.FormattingEnabled = true;
            CmbColorTheme.Location = new Point(65, 22);
            CmbColorTheme.Name = "CmbColorTheme";
            CmbColorTheme.Size = new Size(202, 23);
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
            tableLayoutPanel1.Size = new Size(691, 203);
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
            tableLayoutPanel2.Size = new Size(691, 203);
            tableLayoutPanel2.TabIndex = 1;
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
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(705, 237);
            tabControl1.TabIndex = 21;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tableLayoutPanel2);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(697, 209);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Power Plans";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tableLayoutPanel1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(697, 209);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Rules";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(tableLayoutPanel5);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(697, 209);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Other Settings";
            tabPage3.UseVisualStyleBackColor = true;
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
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.Size = new Size(679, 80);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // RdbCycleAll
            // 
            RdbCycleAll.AutoSize = true;
            RdbCycleAll.Dock = DockStyle.Left;
            RdbCycleAll.Location = new Point(433, 3);
            RdbCycleAll.Name = "RdbCycleAll";
            RdbCycleAll.Size = new Size(226, 19);
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
            LblCycleHotkey.Size = new Size(194, 80);
            LblCycleHotkey.TabIndex = 22;
            LblCycleHotkey.Text = "[ ---------- ]";
            LblCycleHotkey.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RdbCycleVisible
            // 
            RdbCycleVisible.AutoSize = true;
            RdbCycleVisible.Dock = DockStyle.Left;
            RdbCycleVisible.Location = new Point(433, 28);
            RdbCycleVisible.Name = "RdbCycleVisible";
            RdbCycleVisible.Size = new Size(229, 49);
            RdbCycleVisible.TabIndex = 27;
            RdbCycleVisible.TabStop = true;
            RdbCycleVisible.Text = "Cycle only through visible Power Plans";
            RdbCycleVisible.UseVisualStyleBackColor = true;
            // 
            // BtnRemoveCycleHotkey
            // 
            BtnRemoveCycleHotkey.Anchor = AnchorStyles.None;
            BtnRemoveCycleHotkey.Image = (Image)resources.GetObject("BtnRemoveCycleHotkey.Image");
            BtnRemoveCycleHotkey.Location = new Point(318, 3);
            BtnRemoveCycleHotkey.Name = "BtnRemoveCycleHotkey";
            tableLayoutPanel4.SetRowSpan(BtnRemoveCycleHotkey, 2);
            BtnRemoveCycleHotkey.Size = new Size(109, 74);
            BtnRemoveCycleHotkey.TabIndex = 25;
            BtnRemoveCycleHotkey.Text = "Remove Hotkey";
            BtnRemoveCycleHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRemoveCycleHotkey.UseVisualStyleBackColor = true;
            // 
            // BtnSetCycleHotkey
            // 
            BtnSetCycleHotkey.Anchor = AnchorStyles.None;
            BtnSetCycleHotkey.Image = (Image)resources.GetObject("BtnSetCycleHotkey.Image");
            BtnSetCycleHotkey.Location = new Point(203, 3);
            BtnSetCycleHotkey.Name = "BtnSetCycleHotkey";
            tableLayoutPanel4.SetRowSpan(BtnSetCycleHotkey, 2);
            BtnSetCycleHotkey.Size = new Size(109, 74);
            BtnSetCycleHotkey.TabIndex = 24;
            BtnSetCycleHotkey.Text = "Set Hotkey";
            BtnSetCycleHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnSetCycleHotkey.UseVisualStyleBackColor = true;
            // 
            // ChbActivateInitialPowerScheme
            // 
            ChbActivateInitialPowerScheme.AutoSize = true;
            ChbActivateInitialPowerScheme.Dock = DockStyle.Fill;
            ChbActivateInitialPowerScheme.Location = new Point(10, 170);
            ChbActivateInitialPowerScheme.Margin = new Padding(10);
            ChbActivateInitialPowerScheme.Name = "ChbActivateInitialPowerScheme";
            ChbActivateInitialPowerScheme.Size = new Size(199, 23);
            ChbActivateInitialPowerScheme.TabIndex = 24;
            ChbActivateInitialPowerScheme.Text = "Activate this Power Plan on start:";
            ChbActivateInitialPowerScheme.UseVisualStyleBackColor = true;
            // 
            // CmbInitialPowerScheme
            // 
            CmbInitialPowerScheme.Anchor = AnchorStyles.Left;
            tableLayoutPanel2.SetColumnSpan(CmbInitialPowerScheme, 2);
            CmbInitialPowerScheme.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbInitialPowerScheme.FormattingEnabled = true;
            CmbInitialPowerScheme.Location = new Point(229, 170);
            CmbInitialPowerScheme.Margin = new Padding(10);
            CmbInitialPowerScheme.Name = "CmbInitialPowerScheme";
            CmbInitialPowerScheme.Size = new Size(269, 23);
            CmbInitialPowerScheme.TabIndex = 25;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(groupBox4, 0, 0);
            tableLayoutPanel5.Controls.Add(groupBox1, 0, 1);
            tableLayoutPanel5.Controls.Add(groupBox2, 1, 1);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(691, 203);
            tableLayoutPanel5.TabIndex = 30;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel6);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 111);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(339, 89);
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
            tableLayoutPanel6.Size = new Size(333, 67);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel7);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(348, 111);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(340, 89);
            groupBox2.TabIndex = 31;
            groupBox2.TabStop = false;
            groupBox2.Text = "Check for Rules to apply every";
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.Controls.Add(NudPowerRuleCheckInterval, 0, 0);
            tableLayoutPanel7.Controls.Add(label2, 1, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(3, 19);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 1;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.Size = new Size(334, 67);
            tableLayoutPanel7.TabIndex = 0;
            // 
            // SettingsDlg
            // 
            AcceptButton = BtnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(705, 278);
            Controls.Add(tabControl1);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(721, 317);
            Name = "SettingsDlg";
            Text = "PowerPlanSwitcher - Settings";
            Load += SettingsDlg_Load;
            ((System.ComponentModel.ISupportInitialize)DgvPowerSchemes).EndInit();
            ((System.ComponentModel.ISupportInitialize)DgvPowerRules).EndInit();
            ((System.ComponentModel.ISupportInitialize)NudPowerRuleCheckInterval).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
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
        private NumericUpDown NudPowerRuleCheckInterval;
        private Label label2;
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
        private DataGridViewCheckBoxColumn DgcVisible;
        private DataGridViewImageColumn DgcIcon;
        private DataGridViewTextBoxColumn DgcName;
        private DataGridViewCheckBoxColumn AcPowerCheckBox;
        private DataGridViewCheckBoxColumn BatteryCheckBox;
        private DataGridViewTextBoxColumn DgcHotkey;
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
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel7;
    }
}
