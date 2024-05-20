namespace PowerPlanSwitcher
{
    using Microsoft.WindowsAPICodePack.Dialogs;

    public partial class PowerRuleDlg : Form
    {
        public PowerRule? PowerRule { get; set; }
        private readonly List<(Guid guid, string name)> powerSchemes =
            PowerManager.GetPowerSchemes()
                .Where(scheme => !string.IsNullOrWhiteSpace(scheme.name))
                .Cast<(Guid schemeGuid, string name)>()
                .ToList();

        private readonly List<RuleType> ruleTypes =
            Enum.GetValues(typeof(RuleType)).Cast<RuleType>().ToList();
        // 添加新的成员变量来存储是否显示电池和电源供应的规则
        private bool ListAC;
        private bool ListBattery;
        private bool NotEdit = true;
        
        public PowerRuleDlg(bool SDListAC,bool SDListBattery,bool SDNotEdit)
        {
            InitializeComponent();
            ListAC = SDListAC;
            ListBattery = SDListBattery;
            NotEdit = SDNotEdit;
        }
        
        private void SetControlsEnabledState(bool TxtPathEnabled,bool CmbRuleTypeEnabled)
        {

            TxtPath.Enabled = TxtPathEnabled; // 启用或禁用TxtPath文本框
            TxtPath.BackColor = 
                TxtPathEnabled 
                ? SystemColors.Control 
                : Color.Silver;// 设置背景色为灰色（如果锁定）或默认色（如果未锁定）
            BtnSelectPath.Enabled = TxtPathEnabled; // 启用或禁用BtnSelectPath按钮
            BtnSelectPath.BackColor = 
                TxtPathEnabled 
                ? SystemColors.Control 
                : Color.Silver;
            CmbRuleType.Enabled = CmbRuleTypeEnabled; // 启用或禁用TxtPath文本框
            CmbRuleType.BackColor = 
                CmbRuleTypeEnabled
                ? SystemColors.Control 
                : Color.Gray;// 设置背景色为灰色（如果锁定）或默认色（如果未锁定）
        }
        
        private void CmbRuleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            static string GetSelectedString(ComboBox cmb) =>
                cmb.Items[cmb.SelectedIndex]?.ToString() ?? string.Empty;
            RuleType selectedRuleType = PowerRule.TextToRuleType(GetSelectedString(CmbRuleType));
            bool shouldLock = selectedRuleType == RuleType.Battery || selectedRuleType == RuleType.PowerSupply;
            
            SetControlsEnabledState(!shouldLock, NotEdit); // 如果应该锁定，则禁用控件；否则启用它们
        }

        protected override void OnLoad(EventArgs e)
        {
            //方法01
            // 首先，填充CmbRuleType下拉框
            CmbRuleType.Items.AddRange(ruleTypes
                .Select(PowerRule.RuleTypeToText)
                .Cast<object>()
                .ToArray());
            // 然后，根据条件移除特定的项
            if (NotEdit == true)
            {
                int acIndex = CmbRuleType.Items.IndexOf("Power AC");
                int batteryIndex = CmbRuleType.Items.IndexOf("Battery");
                if (ListAC && acIndex != -1) // 如果不想显示 AC，则移除 "Power AC" 项
                {
                    CmbRuleType.Items.RemoveAt(acIndex);
                }

                if (ListBattery && batteryIndex != -1) // 如果不想显示 Battery，则移除 "Battery" 项
                {
                    CmbRuleType.Items.RemoveAt(batteryIndex);
                }
            }
            else
            {
                SetControlsEnabledState(false, NotEdit);
            }
            
            // //方法02
            // List<string> ruleTypeTexts = ruleTypes
                // .Select(PowerRule.RuleTypeToText)
                // .ToList();
                
            // // 根据条件移除 "Power AC" 文本
            // if (ListAC)
            // {
                // ruleTypeTexts.RemoveAll(text => text == "Power AC");
            // }
            // // 根据条件移除 "Battery" 文本
            // if (ListBattery)
            // {
                // ruleTypeTexts.RemoveAll(text => text == "Battery");
            // }
            // // 将筛选后的文本添加到下拉列表中
            // CmbRuleType.Items.AddRange(ruleTypeTexts.ToArray());
            
            
            if (PowerRule is not null)
            {
                CmbRuleType.SelectedIndex = ruleTypes.IndexOf(PowerRule.Type);
            }
            else
            {
                CmbRuleType.SelectedIndex = 0;
            }

            TxtPath.Text = PowerRule?.FilePath;

            CmbPowerScheme.Items.AddRange(powerSchemes
                .Select(scheme => scheme.name)
                .Cast<object>()
                .ToArray());
            if (PowerRule is not null && PowerRule.SchemeGuid != Guid.Empty)
            {
                CmbPowerScheme.SelectedIndex = powerSchemes.FindIndex(
                    scheme => scheme.guid == PowerRule.SchemeGuid);
            }
            else
            {
                CmbPowerScheme.SelectedIndex = 0;
            }

            base.OnLoad(e);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            static string GetSelectedString(ComboBox cmb) =>
                cmb.Items[cmb.SelectedIndex]?.ToString() ?? string.Empty;
                
            RuleType selectedRuleType = PowerRule.TextToRuleType(GetSelectedString(CmbRuleType));
            if (selectedRuleType != RuleType.Battery && selectedRuleType != RuleType.PowerSupply)
            {
                if (string.IsNullOrWhiteSpace(TxtPath.Text))
                {
                    _ = MessageBox.Show(
                        "Path/File must not be empty!",
                        "Invalid input",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }

            Guid GetPowerSchemeGuid(string name) =>
                powerSchemes.First(scheme => scheme.name == name).guid;

            PowerRule ??= new PowerRule();
            PowerRule.Type =
                PowerRule.TextToRuleType(GetSelectedString(CmbRuleType));
            PowerRule.FilePath = TxtPath.Text;
            PowerRule.SchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme));

            DialogResult = DialogResult.OK;
        }

        private void HandleBtnSelectPathClick(object sender, EventArgs e)
        {
            static string GetSelectedString(ComboBox cmb) =>
                cmb.Items[cmb.SelectedIndex]?.ToString() ?? string.Empty;

            var type = PowerRule.TextToRuleType(GetSelectedString(CmbRuleType));

            using var dlg = new CommonOpenFileDialog
            {
                IsFolderPicker = type == RuleType.StartsWith,
            };

            if (dlg.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            if (type == RuleType.EndsWith)
            {
                TxtPath.Text = Path.GetFileName(dlg.FileName);
                return;
            }

            TxtPath.Text = dlg.FileName;
        }
    }
}
