namespace PowerPlanSwitcher
{
    public partial class PowerRuleDlg : Form
    {
        public PowerRule? PowerRule { get; set; }
        private readonly List<KeyValuePair<Guid, string?>> powerSchemes =
            PowerManager.GetPowerSchemes()
                .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
                .ToList();

        private readonly List<RuleType> ruleTypes =
            Enum.GetValues(typeof(RuleType)).Cast<RuleType>().ToList();

        public PowerRuleDlg() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {
            CmbRuleType.Items.AddRange(ruleTypes
                .Select(PowerRule.RuleTypeToText)
                .Cast<object>()
                .ToArray());
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
                .Select(kvp => kvp.Value)
                .Cast<object>()
                .ToArray());
            if (PowerRule is not null && PowerRule.SchemeGuid != Guid.Empty)
            {
                CmbPowerScheme.SelectedIndex = powerSchemes.FindIndex(
                    kvp => kvp.Key == PowerRule.SchemeGuid);
            }
            else
            {
                CmbPowerScheme.SelectedIndex = 0;
            }

            base.OnLoad(e);
        }

        private void BtnOk_Click(object sender, EventArgs e)
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

            string GetSelectedString(ComboBox cmb) =>
                cmb.Items[cmb.SelectedIndex].ToString() ?? string.Empty;

            Guid GetPowerSchemeGuid(string name) =>
                powerSchemes.First(kvp => kvp.Value == name).Key;

            PowerRule ??= new PowerRule();
            PowerRule.Type =
                PowerRule.TextToRuleType(GetSelectedString(CmbRuleType));
            PowerRule.FilePath = TxtPath.Text;
            PowerRule.SchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme));

            DialogResult = DialogResult.OK;
        }
    }
}
