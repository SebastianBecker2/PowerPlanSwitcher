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
            if (disposing)
            {
                DisposeSelectorPreview();
            }
            base.Dispose(disposing);
            if (disposing)
            {
                DisposeSelectorMenuFont();
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var dataGridViewCellStyle3 = new DataGridViewCellStyle();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDlg));
            DgvPowerSchemes = new DataGridView();
            DgcVisible = new DataGridViewCheckBoxColumn();
            DgcIcon = new DataGridViewImageColumn();
            DgcName = new DataGridViewTextBoxColumn();
            DgcHotkey = new DataGridViewTextBoxColumn();
            BtnOk = new Button();
            BtnCancel = new Button();
            DgvRules = new DataGridView();
            DgcRulePriority = new DataGridViewTextBoxColumn();
            DgcRuleDescription = new DataGridViewTextBoxColumn();
            DgcRuleSchemeIcon = new DataGridViewImageColumn();
            DgcRuleSchemeName = new DataGridViewTextBoxColumn();
            DgcTriggerCount = new DataGridViewTextBoxColumn();
            BtnAddPowerRule = new Button();
            BtnEditPowerRule = new Button();
            BtnOpenPowerPlanSettings = new Button();
            BtnAscentPowerScheme = new Button();
            BtnDescentPowerScheme = new Button();
            BtnAscentPowerRule = new Button();
            BtnDescentPowerRule = new Button();
            BtnDeletePowerRule = new Button();
            CmbColorTheme = new ComboBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            LblRulesOrderHint = new Label();
            PibRulesOrderInfo = new PictureBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            BtnRemoveIcon = new Button();
            BtnSetIcon = new Button();
            BtnSetHotkey = new Button();
            BtnRemoveHotkey = new Button();
            TacSettingsCategories = new ThemedTabControl();
            TapPowerSchemes = new TabPage();
            TapRules = new TabPage();
            TapOtherSettings = new TabPage();
            tableLayoutPanel5 = new TableLayoutPanel();
            tableLayoutPanel9 = new TableLayoutPanel();
            groupBox4 = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            tableLayoutPanel12 = new TableLayoutPanel();
            RdbCycleAll = new RadioButton();
            LblCycleHotkey = new Label();
            RdbCycleVisible = new RadioButton();
            BtnRemoveCycleHotkey = new Button();
            BtnSetCycleHotkey = new Button();
            groupBox5 = new GroupBox();
            tableLayoutPanel11 = new TableLayoutPanel();
            tableLayoutPanel13 = new TableLayoutPanel();
            BtnSelectorFont = new Button();
            LblSelectorFont = new Label();
            LblSelectorWidth = new Label();
            NudSelectorWidth = new NumericUpDown();
            LblSelectorItemHeight = new Label();
            NudSelectorItemHeight = new NumericUpDown();
            BtnSelectorReset = new Button();
            tableLayoutPanel10 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            groupBox3 = new GroupBox();
            tableLayoutPanel8 = new TableLayoutPanel();
            CmbPopUpWindowGlobal = new ComboBox();
            groupBox2 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            PibLoggingInfo = new PictureBox();
            ChbExtendedLogging = new CheckBox();
            BtnOpenLogFolder = new Button();
            BtnExportLog = new Button();
            TipHints = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)DgvPowerSchemes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DgvRules).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PibRulesOrderInfo).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            TacSettingsCategories.SuspendLayout();
            TapPowerSchemes.SuspendLayout();
            TapRules.SuspendLayout();
            TapOtherSettings.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel9.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel12.SuspendLayout();
            groupBox5.SuspendLayout();
            tableLayoutPanel11.SuspendLayout();
            tableLayoutPanel13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NudSelectorWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NudSelectorItemHeight).BeginInit();
            tableLayoutPanel10.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PibLoggingInfo).BeginInit();
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
            tableLayoutPanel2.SetRowSpan(DgvPowerSchemes, 4);
            DgvPowerSchemes.RowTemplate.Height = 26;
            DgvPowerSchemes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvPowerSchemes.Size = new Size(455, 431);
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
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgcHotkey.DefaultCellStyle = dataGridViewCellStyle3;
            DgcHotkey.HeaderText = "Hotkey";
            DgcHotkey.Name = "DgcHotkey";
            DgcHotkey.ReadOnly = true;
            DgcHotkey.Width = 74;
            // 
            // BtnOk
            // 
            BtnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnOk.Location = new Point(537, 479);
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
            BtnCancel.Location = new Point(618, 479);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 26);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // DgvRules
            // 
            DgvRules.AllowUserToAddRows = false;
            DgvRules.AllowUserToDeleteRows = false;
            DgvRules.AllowUserToResizeColumns = false;
            DgvRules.AllowUserToResizeRows = false;
            DgvRules.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DgvRules.Columns.AddRange(new DataGridViewColumn[] { DgcRulePriority, DgcRuleDescription, DgcRuleSchemeIcon, DgcRuleSchemeName, DgcTriggerCount });
            tableLayoutPanel1.SetColumnSpan(DgvRules, 6);
            DgvRules.Dock = DockStyle.Fill;
            DgvRules.Location = new Point(3, 32);
            DgvRules.MultiSelect = false;
            DgvRules.Name = "DgvRules";
            DgvRules.RowHeadersVisible = false;
            DgvRules.RowTemplate.Height = 26;
            DgvRules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvRules.Size = new Size(685, 312);
            DgvRules.TabIndex = 7;
            DgvRules.CellContentDoubleClick += DgvPowerRules_CellContentDoubleClick;
            // 
            // DgcRulePriority
            // 
            DgcRulePriority.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcRulePriority.HeaderText = "#";
            DgcRulePriority.Name = "DgcRulePriority";
            DgcRulePriority.ReadOnly = true;
            DgcRulePriority.Width = 41;
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
            // DgcTriggerCount
            // 
            DgcTriggerCount.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgcTriggerCount.HeaderText = "Trigger";
            DgcTriggerCount.Name = "DgcTriggerCount";
            DgcTriggerCount.ReadOnly = true;
            DgcTriggerCount.Width = 76;
            // 
            // BtnAddPowerRule
            // 
            BtnAddPowerRule.Image = (Image)resources.GetObject("BtnAddPowerRule.Image");
            BtnAddPowerRule.Location = new Point(3, 350);
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
            BtnEditPowerRule.Location = new Point(118, 350);
            BtnEditPowerRule.Name = "BtnEditPowerRule";
            BtnEditPowerRule.Size = new Size(109, 84);
            BtnEditPowerRule.TabIndex = 9;
            BtnEditPowerRule.Text = "Edit selected Rule";
            BtnEditPowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnEditPowerRule.UseVisualStyleBackColor = true;
            BtnEditPowerRule.Click += HandleBtnEditPowerRuleClick;
            // 
            // BtnOpenPowerPlanSettings
            // 
            tableLayoutPanel2.SetColumnSpan(BtnOpenPowerPlanSettings, 2);
            BtnOpenPowerPlanSettings.Image = Properties.Resources.control_panel;
            BtnOpenPowerPlanSettings.Location = new Point(464, 350);
            BtnOpenPowerPlanSettings.Name = "BtnOpenPowerPlanSettings";
            BtnOpenPowerPlanSettings.Size = new Size(224, 84);
            BtnOpenPowerPlanSettings.TabIndex = 24;
            BtnOpenPowerPlanSettings.Text = "Open power plan\r\nsettings";
            BtnOpenPowerPlanSettings.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnOpenPowerPlanSettings.UseVisualStyleBackColor = true;
            BtnOpenPowerPlanSettings.Click += HandleBtnOpenPowerPlanSettingsClick;
            // 
            // BtnAscentPowerScheme
            // 
            BtnAscentPowerScheme.Image = Properties.Resources.arrow_up;
            BtnAscentPowerScheme.Location = new Point(464, 260);
            BtnAscentPowerScheme.Name = "BtnAscentPowerScheme";
            BtnAscentPowerScheme.Size = new Size(109, 84);
            BtnAscentPowerScheme.TabIndex = 25;
            BtnAscentPowerScheme.Text = "Move Plan up";
            BtnAscentPowerScheme.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnAscentPowerScheme.UseVisualStyleBackColor = true;
            BtnAscentPowerScheme.Click += HandleBtnAscentPowerSchemeClick;
            // 
            // BtnDescentPowerScheme
            // 
            BtnDescentPowerScheme.Image = Properties.Resources.arrow_down;
            BtnDescentPowerScheme.Location = new Point(579, 260);
            BtnDescentPowerScheme.Name = "BtnDescentPowerScheme";
            BtnDescentPowerScheme.Size = new Size(109, 84);
            BtnDescentPowerScheme.TabIndex = 26;
            BtnDescentPowerScheme.Text = "Move Plan down";
            BtnDescentPowerScheme.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnDescentPowerScheme.UseVisualStyleBackColor = true;
            BtnDescentPowerScheme.Click += HandleBtnDescentPowerSchemeClick;
            // 
            // BtnAscentPowerRule
            // 
            BtnAscentPowerRule.Image = (Image)resources.GetObject("BtnAscentPowerRule.Image");
            BtnAscentPowerRule.Location = new Point(348, 350);
            BtnAscentPowerRule.Name = "BtnAscentPowerRule";
            BtnAscentPowerRule.Size = new Size(109, 84);
            BtnAscentPowerRule.TabIndex = 10;
            BtnAscentPowerRule.Text = "Move Rule up\r\n(higher priority)";
            BtnAscentPowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnAscentPowerRule.UseVisualStyleBackColor = true;
            BtnAscentPowerRule.Click += HandleBtnAscentPowerRuleClick;
            // 
            // BtnDescentPowerRule
            // 
            BtnDescentPowerRule.Image = (Image)resources.GetObject("BtnDescentPowerRule.Image");
            BtnDescentPowerRule.Location = new Point(463, 350);
            BtnDescentPowerRule.Name = "BtnDescentPowerRule";
            BtnDescentPowerRule.Size = new Size(109, 84);
            BtnDescentPowerRule.TabIndex = 10;
            BtnDescentPowerRule.Text = "Move Rule down\r\n(lower priority)";
            BtnDescentPowerRule.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnDescentPowerRule.UseVisualStyleBackColor = true;
            BtnDescentPowerRule.Click += HandleBtnDescentPowerRuleClick;
            // 
            // BtnDeletePowerRule
            // 
            BtnDeletePowerRule.Image = (Image)resources.GetObject("BtnDeletePowerRule.Image");
            BtnDeletePowerRule.Location = new Point(233, 350);
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
            CmbColorTheme.Location = new Point(24, 72);
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
            tableLayoutPanel1.Controls.Add(LblRulesOrderHint, 0, 0);
            tableLayoutPanel1.Controls.Add(DgvRules, 0, 1);
            tableLayoutPanel1.Controls.Add(BtnDeletePowerRule, 2, 2);
            tableLayoutPanel1.Controls.Add(BtnAscentPowerRule, 3, 2);
            tableLayoutPanel1.Controls.Add(BtnEditPowerRule, 1, 2);
            tableLayoutPanel1.Controls.Add(BtnAddPowerRule, 0, 2);
            tableLayoutPanel1.Controls.Add(PibRulesOrderInfo, 5, 2);
            tableLayoutPanel1.Controls.Add(BtnDescentPowerRule, 4, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(691, 437);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // LblRulesOrderHint
            // 
            LblRulesOrderHint.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(LblRulesOrderHint, 6);
            LblRulesOrderHint.Dock = DockStyle.Fill;
            LblRulesOrderHint.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblRulesOrderHint.Location = new Point(3, 0);
            LblRulesOrderHint.Name = "LblRulesOrderHint";
            LblRulesOrderHint.Padding = new Padding(0, 7, 0, 7);
            LblRulesOrderHint.Size = new Size(685, 29);
            LblRulesOrderHint.TabIndex = 12;
            LblRulesOrderHint.Text = "Rule order matters: rules are evaluated top to bottom, and the first triggered rule is applied.";
            LblRulesOrderHint.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // PibRulesOrderInfo
            // 
            PibRulesOrderInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PibRulesOrderInfo.Image = (Image)resources.GetObject("PibRulesOrderInfo.Image");
            PibRulesOrderInfo.Location = new Point(656, 350);
            PibRulesOrderInfo.Name = "PibRulesOrderInfo";
            PibRulesOrderInfo.Size = new Size(32, 32);
            PibRulesOrderInfo.SizeMode = PictureBoxSizeMode.AutoSize;
            PibRulesOrderInfo.TabIndex = 11;
            PibRulesOrderInfo.TabStop = false;
            PibRulesOrderInfo.Click += PibRulesOrderInfo_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(DgvPowerSchemes, 0, 0);
            tableLayoutPanel2.Controls.Add(BtnRemoveIcon, 3, 0);
            tableLayoutPanel2.Controls.Add(BtnSetIcon, 2, 0);
            tableLayoutPanel2.Controls.Add(BtnSetHotkey, 2, 1);
            tableLayoutPanel2.Controls.Add(BtnRemoveHotkey, 3, 1);
            tableLayoutPanel2.Controls.Add(BtnAscentPowerScheme, 2, 2);
            tableLayoutPanel2.Controls.Add(BtnDescentPowerScheme, 3, 2);
            tableLayoutPanel2.Controls.Add(BtnOpenPowerPlanSettings, 2, 3);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(691, 437);
            tableLayoutPanel2.TabIndex = 1;
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
            // TacSettingsCategories
            // 
            TacSettingsCategories.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TacSettingsCategories.Controls.Add(TapPowerSchemes);
            TacSettingsCategories.Controls.Add(TapRules);
            TacSettingsCategories.Controls.Add(TapOtherSettings);
            TacSettingsCategories.Location = new Point(0, 0);
            TacSettingsCategories.Name = "TacSettingsCategories";
            TacSettingsCategories.SelectedIndex = 0;
            TacSettingsCategories.Size = new Size(705, 473);
            TacSettingsCategories.TabIndex = 21;
            // 
            // TapPowerSchemes
            // 
            TapPowerSchemes.Controls.Add(tableLayoutPanel2);
            TapPowerSchemes.Location = new Point(4, 26);
            TapPowerSchemes.Name = "TapPowerSchemes";
            TapPowerSchemes.Padding = new Padding(3);
            TapPowerSchemes.Size = new Size(697, 443);
            TapPowerSchemes.TabIndex = 0;
            TapPowerSchemes.Text = "Power Plans";
            TapPowerSchemes.UseVisualStyleBackColor = true;
            // 
            // TapRules
            // 
            TapRules.Controls.Add(tableLayoutPanel1);
            TapRules.Location = new Point(4, 26);
            TapRules.Name = "TapRules";
            TapRules.Padding = new Padding(3);
            TapRules.Size = new Size(697, 443);
            TapRules.TabIndex = 1;
            TapRules.Text = "Rules";
            TapRules.UseVisualStyleBackColor = true;
            // 
            // TapOtherSettings
            // 
            TapOtherSettings.Controls.Add(tableLayoutPanel5);
            TapOtherSettings.Location = new Point(4, 26);
            TapOtherSettings.Name = "TapOtherSettings";
            TapOtherSettings.Padding = new Padding(3);
            TapOtherSettings.Size = new Size(697, 443);
            TapOtherSettings.TabIndex = 2;
            TapOtherSettings.Text = "Other Settings";
            TapOtherSettings.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Controls.Add(tableLayoutPanel9, 0, 0);
            tableLayoutPanel5.Controls.Add(tableLayoutPanel10, 0, 1);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tableLayoutPanel5.Size = new Size(691, 437);
            tableLayoutPanel5.TabIndex = 30;
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.ColumnCount = 2;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel9.Controls.Add(groupBox4, 0, 0);
            tableLayoutPanel9.Controls.Add(groupBox5, 1, 0);
            tableLayoutPanel9.Dock = DockStyle.Fill;
            tableLayoutPanel9.Location = new Point(3, 3);
            tableLayoutPanel9.Margin = new Padding(0);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 1;
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel9.Size = new Size(685, 234);
            tableLayoutPanel9.TabIndex = 35;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(tableLayoutPanel4);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(405, 228);
            groupBox4.TabIndex = 29;
            groupBox4.TabStop = false;
            groupBox4.Text = "Hotkey to cycle through Power Plans";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 5;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(BtnSetCycleHotkey, 1, 0);
            tableLayoutPanel4.Controls.Add(LblCycleHotkey, 2, 0);
            tableLayoutPanel4.Controls.Add(BtnRemoveCycleHotkey, 3, 0);
            tableLayoutPanel4.Controls.Add(tableLayoutPanel12, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 19);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.Size = new Size(399, 206);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // LblCycleHotkey
            // 
            LblCycleHotkey.AutoSize = false;
            LblCycleHotkey.BackColor = Color.Transparent;
            LblCycleHotkey.Dock = DockStyle.Fill;
            LblCycleHotkey.Location = new Point(145, 0);
            LblCycleHotkey.Margin = new Padding(4, 0, 4, 0);
            LblCycleHotkey.Name = "LblCycleHotkey";
            LblCycleHotkey.Size = new Size(112, 80);
            LblCycleHotkey.TabIndex = 22;
            LblCycleHotkey.Text = "[ ---------- ]";
            LblCycleHotkey.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // BtnSetCycleHotkey
            // 
            BtnSetCycleHotkey.Anchor = AnchorStyles.None;
            BtnSetCycleHotkey.Image = (Image)resources.GetObject("BtnSetCycleHotkey.Image");
            BtnSetCycleHotkey.Location = new Point(3, 3);
            BtnSetCycleHotkey.Name = "BtnSetCycleHotkey";
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
            BtnRemoveCycleHotkey.Location = new Point(287, 3);
            BtnRemoveCycleHotkey.Name = "BtnRemoveCycleHotkey";
            BtnRemoveCycleHotkey.Size = new Size(109, 74);
            BtnRemoveCycleHotkey.TabIndex = 25;
            BtnRemoveCycleHotkey.Text = "Remove Hotkey";
            BtnRemoveCycleHotkey.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnRemoveCycleHotkey.UseVisualStyleBackColor = true;
            BtnRemoveCycleHotkey.Click += BtnRemoveCycleHotkey_Click;
            // 
            // tableLayoutPanel12
            // 
            tableLayoutPanel12.Anchor = AnchorStyles.None;
            tableLayoutPanel12.AutoSize = true;
            tableLayoutPanel12.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel12.ColumnCount = 1;
            tableLayoutPanel4.SetColumnSpan(tableLayoutPanel12, 5);
            tableLayoutPanel12.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel12.Controls.Add(RdbCycleAll, 0, 0);
            tableLayoutPanel12.Controls.Add(RdbCycleVisible, 0, 1);
            tableLayoutPanel12.Location = new Point(78, 83);
            tableLayoutPanel12.Name = "tableLayoutPanel12";
            tableLayoutPanel12.RowCount = 2;
            tableLayoutPanel12.RowStyles.Add(new RowStyle());
            tableLayoutPanel12.RowStyles.Add(new RowStyle());
            tableLayoutPanel12.Size = new Size(249, 54);
            tableLayoutPanel12.TabIndex = 28;
            // 
            // RdbCycleAll
            // 
            RdbCycleAll.Anchor = AnchorStyles.Left;
            RdbCycleAll.AutoSize = true;
            RdbCycleAll.Location = new Point(3, 3);
            RdbCycleAll.Margin = new Padding(3, 3, 3, 0);
            RdbCycleAll.Name = "RdbCycleAll";
            RdbCycleAll.Size = new Size(243, 21);
            RdbCycleAll.TabIndex = 26;
            RdbCycleAll.TabStop = true;
            RdbCycleAll.Text = "Cycle through all existing Power Plans";
            RdbCycleAll.UseVisualStyleBackColor = true;
            // 
            // RdbCycleVisible
            // 
            RdbCycleVisible.Anchor = AnchorStyles.Left;
            RdbCycleVisible.AutoSize = true;
            RdbCycleVisible.Location = new Point(3, 27);
            RdbCycleVisible.Margin = new Padding(3, 3, 3, 3);
            RdbCycleVisible.Name = "RdbCycleVisible";
            RdbCycleVisible.Size = new Size(243, 21);
            RdbCycleVisible.TabIndex = 27;
            RdbCycleVisible.TabStop = true;
            RdbCycleVisible.Text = "Cycle only through visible Power Plans";
            RdbCycleVisible.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(tableLayoutPanel11);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(414, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(268, 228);
            groupBox5.TabIndex = 30;
            groupBox5.TabStop = false;
            groupBox5.Text = "Appearance of Power Plan selection";
            // 
            // tableLayoutPanel11
            // 
            tableLayoutPanel11.ColumnCount = 1;
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel11.Controls.Add(tableLayoutPanel13, 0, 0);
            tableLayoutPanel11.Dock = DockStyle.Fill;
            tableLayoutPanel11.Location = new Point(3, 19);
            tableLayoutPanel11.Name = "tableLayoutPanel11";
            tableLayoutPanel11.RowCount = 1;
            tableLayoutPanel11.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel11.Size = new Size(262, 206);
            tableLayoutPanel11.TabIndex = 0;
            // 
            // tableLayoutPanel13
            // 
            tableLayoutPanel13.Anchor = AnchorStyles.None;
            tableLayoutPanel13.AutoSize = true;
            tableLayoutPanel13.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel13.ColumnCount = 2;
            tableLayoutPanel13.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel13.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel13.Controls.Add(BtnSelectorFont, 0, 0);
            tableLayoutPanel13.Controls.Add(LblSelectorFont, 1, 0);
            tableLayoutPanel13.Controls.Add(LblSelectorWidth, 0, 1);
            tableLayoutPanel13.Controls.Add(NudSelectorWidth, 1, 1);
            tableLayoutPanel13.Controls.Add(LblSelectorItemHeight, 0, 2);
            tableLayoutPanel13.Controls.Add(NudSelectorItemHeight, 1, 2);
            tableLayoutPanel13.Controls.Add(BtnSelectorReset, 0, 3);
            tableLayoutPanel13.Location = new Point(40, 40);
            tableLayoutPanel13.Name = "tableLayoutPanel13";
            tableLayoutPanel13.RowCount = 4;
            tableLayoutPanel13.RowStyles.Add(new RowStyle());
            tableLayoutPanel13.RowStyles.Add(new RowStyle());
            tableLayoutPanel13.RowStyles.Add(new RowStyle());
            tableLayoutPanel13.RowStyles.Add(new RowStyle());
            tableLayoutPanel13.Size = new Size(180, 130);
            tableLayoutPanel13.TabIndex = 1;
            // 
            // BtnSelectorFont
            // 
            BtnSelectorFont.Anchor = AnchorStyles.Left;
            BtnSelectorFont.AutoSize = true;
            BtnSelectorFont.Location = new Point(3, 8);
            BtnSelectorFont.Name = "BtnSelectorFont";
            BtnSelectorFont.Size = new Size(62, 27);
            BtnSelectorFont.TabIndex = 0;
            BtnSelectorFont.Text = "Font...";
            BtnSelectorFont.UseVisualStyleBackColor = true;
            BtnSelectorFont.Click += BtnSelectorFont_Click;
            // 
            // LblSelectorFont
            // 
            LblSelectorFont.Anchor = AnchorStyles.Left;
            LblSelectorFont.AutoSize = true;
            LblSelectorFont.Location = new Point(71, 11);
            LblSelectorFont.MaximumSize = new Size(180, 0);
            LblSelectorFont.Name = "LblSelectorFont";
            LblSelectorFont.Size = new Size(90, 17);
            LblSelectorFont.TabIndex = 1;
            LblSelectorFont.Text = "Segoe UI, 12pt";
            // 
            // LblSelectorWidth
            // 
            LblSelectorWidth.Anchor = AnchorStyles.Left;
            LblSelectorWidth.AutoSize = true;
            LblSelectorWidth.Location = new Point(3, 48);
            LblSelectorWidth.Name = "LblSelectorWidth";
            LblSelectorWidth.Size = new Size(42, 17);
            LblSelectorWidth.TabIndex = 2;
            LblSelectorWidth.Text = "Width";
            // 
            // NudSelectorWidth
            // 
            NudSelectorWidth.Anchor = AnchorStyles.Left;
            NudSelectorWidth.Location = new Point(71, 45);
            NudSelectorWidth.Maximum = new decimal(new int[] { 1200, 0, 0, 0 });
            NudSelectorWidth.Minimum = new decimal(new int[] { 180, 0, 0, 0 });
            NudSelectorWidth.Name = "NudSelectorWidth";
            NudSelectorWidth.Size = new Size(90, 23);
            NudSelectorWidth.TabIndex = 3;
            NudSelectorWidth.Value = new decimal(new int[] { 360, 0, 0, 0 });
            NudSelectorWidth.ValueChanged += SelectorMenuSize_ValueChanged;
            // 
            // LblSelectorItemHeight
            // 
            LblSelectorItemHeight.Anchor = AnchorStyles.Left;
            LblSelectorItemHeight.AutoSize = true;
            LblSelectorItemHeight.Location = new Point(3, 80);
            LblSelectorItemHeight.Name = "LblSelectorItemHeight";
            LblSelectorItemHeight.Size = new Size(70, 17);
            LblSelectorItemHeight.TabIndex = 4;
            LblSelectorItemHeight.Text = "Item height";
            // 
            // NudSelectorItemHeight
            // 
            NudSelectorItemHeight.Anchor = AnchorStyles.Left;
            NudSelectorItemHeight.Location = new Point(71, 77);
            NudSelectorItemHeight.Maximum = new decimal(new int[] { 240, 0, 0, 0 });
            NudSelectorItemHeight.Minimum = new decimal(new int[] { 28, 0, 0, 0 });
            NudSelectorItemHeight.Name = "NudSelectorItemHeight";
            NudSelectorItemHeight.Size = new Size(90, 23);
            NudSelectorItemHeight.TabIndex = 5;
            NudSelectorItemHeight.Value = new decimal(new int[] { 50, 0, 0, 0 });
            NudSelectorItemHeight.ValueChanged += SelectorMenuSize_ValueChanged;
            // 
            // BtnSelectorReset
            // 
            BtnSelectorReset.Anchor = AnchorStyles.None;
            BtnSelectorReset.AutoSize = true;
            tableLayoutPanel13.SetColumnSpan(BtnSelectorReset, 2);
            BtnSelectorReset.Location = new Point(3, 109);
            BtnSelectorReset.Name = "BtnSelectorReset";
            BtnSelectorReset.Size = new Size(58, 27);
            BtnSelectorReset.TabIndex = 6;
            BtnSelectorReset.Text = "Reset";
            BtnSelectorReset.UseVisualStyleBackColor = true;
            BtnSelectorReset.Click += BtnSelectorReset_Click;
            // 
            // tableLayoutPanel10
            // 
            tableLayoutPanel10.ColumnCount = 3;
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel10.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel10.Controls.Add(groupBox3, 1, 0);
            tableLayoutPanel10.Controls.Add(groupBox2, 2, 0);
            tableLayoutPanel10.Dock = DockStyle.Fill;
            tableLayoutPanel10.Location = new Point(3, 243);
            tableLayoutPanel10.Margin = new Padding(0);
            tableLayoutPanel10.Name = "tableLayoutPanel10";
            tableLayoutPanel10.RowCount = 1;
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel10.Size = new Size(685, 191);
            tableLayoutPanel10.TabIndex = 36;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel6);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 243);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(224, 191);
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
            tableLayoutPanel6.Size = new Size(218, 169);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tableLayoutPanel8);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(233, 243);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(224, 191);
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
            tableLayoutPanel8.Size = new Size(218, 169);
            tableLayoutPanel8.TabIndex = 0;
            // 
            // CmbPopUpWindowGlobal
            // 
            CmbPopUpWindowGlobal.Anchor = AnchorStyles.None;
            CmbPopUpWindowGlobal.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbPopUpWindowGlobal.FormattingEnabled = true;
            CmbPopUpWindowGlobal.Location = new Point(24, 72);
            CmbPopUpWindowGlobal.Name = "CmbPopUpWindowGlobal";
            CmbPopUpWindowGlobal.Size = new Size(170, 25);
            CmbPopUpWindowGlobal.TabIndex = 17;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel3);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(463, 243);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(225, 191);
            groupBox2.TabIndex = 34;
            groupBox2.TabStop = false;
            groupBox2.Text = "Logging";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(PibLoggingInfo, 2, 0);
            tableLayoutPanel3.Controls.Add(ChbExtendedLogging, 1, 0);
            tableLayoutPanel3.Controls.Add(BtnOpenLogFolder, 1, 1);
            tableLayoutPanel3.Controls.Add(BtnExportLog, 1, 2);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.Size = new Size(219, 169);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // PibLoggingInfo
            // 
            PibLoggingInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PibLoggingInfo.Image = Properties.Resources.info_rhombus;
            PibLoggingInfo.Location = new Point(165, 0);
            PibLoggingInfo.Margin = new Padding(0);
            PibLoggingInfo.Name = "PibLoggingInfo";
            tableLayoutPanel3.SetRowSpan(PibLoggingInfo, 3);
            PibLoggingInfo.Size = new Size(32, 32);
            PibLoggingInfo.SizeMode = PictureBoxSizeMode.AutoSize;
            PibLoggingInfo.TabIndex = 2;
            PibLoggingInfo.TabStop = false;
            PibLoggingInfo.Click += PibLoggingInfo_Click;
            // 
            // ChbExtendedLogging
            // 
            ChbExtendedLogging.Anchor = AnchorStyles.None;
            ChbExtendedLogging.AutoSize = true;
            ChbExtendedLogging.Location = new Point(26, 17);
            ChbExtendedLogging.Name = "ChbExtendedLogging";
            ChbExtendedLogging.Size = new Size(133, 21);
            ChbExtendedLogging.TabIndex = 0;
            ChbExtendedLogging.Text = "Extended Logging";
            ChbExtendedLogging.UseVisualStyleBackColor = true;
            // 
            // BtnOpenLogFolder
            // 
            BtnOpenLogFolder.Anchor = AnchorStyles.None;
            BtnOpenLogFolder.Location = new Point(24, 72);
            BtnOpenLogFolder.Name = "BtnOpenLogFolder";
            BtnOpenLogFolder.Size = new Size(138, 24);
            BtnOpenLogFolder.TabIndex = 1;
            BtnOpenLogFolder.Text = "Open log folder";
            BtnOpenLogFolder.UseVisualStyleBackColor = true;
            BtnOpenLogFolder.Click += BtnOpenLogFolder_Click;
            // 
            // BtnExportLog
            // 
            BtnExportLog.Anchor = AnchorStyles.None;
            BtnExportLog.Location = new Point(24, 128);
            BtnExportLog.Name = "BtnExportLog";
            BtnExportLog.Size = new Size(138, 25);
            BtnExportLog.TabIndex = 1;
            BtnExportLog.Text = "Export log to zip";
            BtnExportLog.UseVisualStyleBackColor = true;
            BtnExportLog.Click += BtnExportLog_Click;
            // 
            // TipHints
            // 
            TipHints.ShowAlways = true;
            TipHints.UseAnimation = false;
            TipHints.UseFading = false;
            // 
            // SettingsDlg
            // 
            AcceptButton = BtnOk;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = BtnCancel;
            ClientSize = new Size(705, 519);
            Controls.Add(TacSettingsCategories);
            Controls.Add(BtnCancel);
            Controls.Add(BtnOk);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(721, 491);
            Name = "SettingsDlg";
            Text = "PowerPlanSwitcher - Settings";
            ((System.ComponentModel.ISupportInitialize)DgvPowerSchemes).EndInit();
            ((System.ComponentModel.ISupportInitialize)DgvRules).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PibRulesOrderInfo).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            TacSettingsCategories.ResumeLayout(false);
            TapPowerSchemes.ResumeLayout(false);
            TapRules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)NudSelectorWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)NudSelectorItemHeight).EndInit();
            TapOtherSettings.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel9.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            tableLayoutPanel12.ResumeLayout(false);
            tableLayoutPanel12.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            groupBox5.ResumeLayout(false);
            tableLayoutPanel13.ResumeLayout(false);
            tableLayoutPanel13.PerformLayout();
            tableLayoutPanel11.ResumeLayout(false);
            tableLayoutPanel11.PerformLayout();
            tableLayoutPanel10.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PibLoggingInfo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView DgvPowerSchemes;
        private Button BtnOk;
        private Button BtnCancel;
        private DataGridView DgvRules;
        private Button BtnAddPowerRule;
        private Button BtnEditPowerRule;
        private Button BtnOpenPowerPlanSettings;
        private Button BtnAscentPowerScheme;
        private Button BtnDescentPowerScheme;
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
        private ThemedTabControl TacSettingsCategories;
        private TabPage TapPowerSchemes;
        private TabPage TapRules;
        private TabPage TapOtherSettings;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel12;
        private RadioButton RdbCycleAll;
        private Label LblCycleHotkey;
        private RadioButton RdbCycleVisible;
        private Button BtnRemoveCycleHotkey;
        private Button BtnSetCycleHotkey;
        private GroupBox groupBox5;
        private TableLayoutPanel tableLayoutPanel11;
        private TableLayoutPanel tableLayoutPanel13;
        private Button BtnSelectorFont;
        private Label LblSelectorFont;
        private Label LblSelectorWidth;
        private NumericUpDown NudSelectorWidth;
        private Label LblSelectorItemHeight;
        private NumericUpDown NudSelectorItemHeight;
        private Button BtnSelectorReset;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel9;
        private TableLayoutPanel tableLayoutPanel10;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel6;
        private DataGridViewCheckBoxColumn DgcVisible;
        private DataGridViewImageColumn DgcIcon;
        private DataGridViewTextBoxColumn DgcName;
        private DataGridViewTextBoxColumn DgcHotkey;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel8;
        private ComboBox CmbPopUpWindowGlobal;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel3;
        private CheckBox ChbExtendedLogging;
        private Button BtnOpenLogFolder;
        private Button BtnExportLog;
        private PictureBox PibLoggingInfo;
        private ToolTip TipHints;
        private DataGridViewTextBoxColumn DgcRulePriority;
        private DataGridViewTextBoxColumn DgcRuleDescription;
        private DataGridViewImageColumn DgcRuleSchemeIcon;
        private DataGridViewTextBoxColumn DgcRuleSchemeName;
        private DataGridViewTextBoxColumn DgcTriggerCount;
        private PictureBox PibRulesOrderInfo;
        private Label LblRulesOrderHint;
    }
}
