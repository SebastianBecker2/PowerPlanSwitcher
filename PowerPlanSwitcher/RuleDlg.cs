namespace PowerPlanSwitcher
{
    using Microsoft.WindowsAPICodePack.Dialogs;
    using PowerPlanSwitcher.PowerManagement;
    using PowerPlanSwitcher.RuleManagement;
    using PowerPlanSwitcher.RuleManagement.Rules;

    public partial class RuleDlg : Form
    {
        public ProcessRule? PowerRule { get; set; }
        private readonly List<(Guid guid, string name)> powerSchemes =
            PowerManager.Static.GetPowerSchemes()
                .Where(scheme => !string.IsNullOrWhiteSpace(scheme.name))
                .Cast<(Guid schemeGuid, string name)>()
                .ToList();

        private static readonly List<ComparisonType> ComparisonTypes =
            Enum.GetValues(typeof(ComparisonType)).Cast<ComparisonType>().ToList();

        public RuleDlg() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {
            CmbRuleType.Items.AddRange(ComparisonTypes
                .Select(ProcessRule.ComparisonTypeToText)
                .Cast<object>()
                .ToArray());
            if (PowerRule is not null)
            {
                CmbRuleType.SelectedIndex = ComparisonTypes.IndexOf(PowerRule.Type);
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
            if (string.IsNullOrWhiteSpace(TxtPath.Text))
            {
                _ = MessageBox.Show(
                    "Path/File must not be empty!",
                    "Invalid input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            static string GetSelectedString(ComboBox cmb) =>
                cmb.Items[cmb.SelectedIndex]?.ToString() ?? string.Empty;

            Guid GetPowerSchemeGuid(string name) =>
                powerSchemes.First(scheme => scheme.name == name).guid;

            PowerRule ??= new ProcessRule();
            PowerRule.Type =
                ProcessRule.TextToComparisonType(GetSelectedString(CmbRuleType));
            PowerRule.FilePath = TxtPath.Text;
            PowerRule.SchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme));

            DialogResult = DialogResult.OK;
        }

        private void HandleBtnSelectPathClick(object sender, EventArgs e)
        {
            static string GetSelectedString(ComboBox cmb) =>
                cmb.Items[cmb.SelectedIndex]?.ToString() ?? string.Empty;

            var type = ProcessRule.TextToComparisonType(GetSelectedString(CmbRuleType));

            using var dlg = new CommonOpenFileDialog
            {
                IsFolderPicker = type == ComparisonType.StartsWith,
            };

            if (dlg.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            if (type == ComparisonType.EndsWith)
            {
                TxtPath.Text = Path.GetFileName(dlg.FileName);
                return;
            }

            TxtPath.Text = dlg.FileName;
        }
    }
}
