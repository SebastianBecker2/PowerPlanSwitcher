namespace PowerPlanSwitcher
{
    using Microsoft.WindowsAPICodePack.Dialogs;
    using PowerPlanSwitcher.PowerManagement;
    using PowerPlanSwitcher.RuleManagement;
    using PowerPlanSwitcher.RuleManagement.Rules;

    public partial class RuleDlg : Form
    {
        public IRule? Rule { get; set; }

        private static readonly List<(Guid guid, string name)> PowerSchemes =
            PowerManager.Static.GetPowerSchemes()
                .Where(scheme => !string.IsNullOrWhiteSpace(scheme.name))
                .Cast<(Guid schemeGuid, string name)>()
                .ToList();

        private static readonly List<ComparisonType> ComparisonTypes =
            Enum.GetValues(typeof(ComparisonType))
                .Cast<ComparisonType>()
                .ToList();

        private static readonly List<PowerLineStatus> PowerLineStatuses =
            Enum.GetValues(typeof(PowerLineStatus))
                .Cast<PowerLineStatus>()
                .ToList();

        private static string GetSelectedString(ComboBox cmb) =>
            cmb.Items[cmb.SelectedIndex]?.ToString() ?? string.Empty;

        private static Guid GetPowerSchemeGuid(string name) =>
                PowerSchemes.First(scheme => scheme.name == name).guid;

        public RuleDlg() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {
            LblComparisonType.Visible = false;
            CmbComparisonType.Visible = false;
            LblPath.Visible = false;
            TxtPath.Visible = false;
            BtnSelectFile.Visible = false;
            BtnSelectFolder.Visible = false;
            BtnSelectFromProcess.Visible = false;
            LblPowerLineStatus.Visible = false;
            CmbPowerLineStatus.Visible = false;

            CmbComparisonType.Items.AddRange(ComparisonTypes
                .Select(ProcessRule.ComparisonTypeToText)
                .Cast<object>()
                .ToArray());

            CmbPowerLineStatus.Items.AddRange(PowerLineStatuses
                .Select(PowerLineRule.PowerLineStatusToText)
                .Cast<object>()
                .ToArray());

            CmbPowerScheme.Items.AddRange(PowerSchemes
                .Select(scheme => scheme.name)
                .Cast<object>()
                .ToArray());

            if (Rule is ProcessRule processRule)
            {
                RdbProcessRule.Checked = true;
                CmbComparisonType.SelectedIndex =
                    ComparisonTypes.IndexOf(processRule.Type);
                TxtPath.Text = processRule.FilePath;
            }
            else
            {
                CmbComparisonType.SelectedIndex = 0;
            }

            if (Rule is PowerLineRule powerLineRule)
            {
                RdbPowerLineRule.Checked = true;
                CmbPowerLineStatus.SelectedIndex =
                    PowerLineStatuses.IndexOf(powerLineRule.PowerLineStatus);
            }
            else
            {
                CmbPowerLineStatus.SelectedIndex = 0;
            }

            if (Rule is not null && Rule.SchemeGuid != Guid.Empty)
            {
                CmbPowerScheme.SelectedIndex = PowerSchemes.FindIndex(
                    scheme => scheme.guid == Rule.SchemeGuid);
            }
            else
            {
                CmbPowerScheme.SelectedIndex = 0;
            }

            base.OnLoad(e);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (RdbProcessRule.Checked)
            {
                if (ApplyProcessRule())
                {
                    DialogResult = DialogResult.OK;
                }
                return;
            }
            else if (RdbPowerLineRule.Checked)
            {
                if (ApplyPowerLineRule())
                {
                    DialogResult = DialogResult.OK;
                }
                return;
            }

            _ = MessageBox.Show(
                "Select a Rule Type!",
                "Invalid input",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private bool ApplyPowerLineRule()
        {
            var powerLineRule = Rule as PowerLineRule ?? new PowerLineRule();
            powerLineRule.Index = Rule?.Index ?? 0;

            powerLineRule.PowerLineStatus =
                PowerLineRule.TextToPowerLineStatus(
                    GetSelectedString(CmbPowerLineStatus));
            powerLineRule.SchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme));

            Rule = powerLineRule;
            return true;
        }

        private bool ApplyProcessRule()
        {
            if (string.IsNullOrWhiteSpace(TxtPath.Text))
            {
                _ = MessageBox.Show(
                    "Path/File must not be empty!",
                    "Invalid input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            var processRule = Rule as ProcessRule ?? new ProcessRule();
            processRule.Index = Rule?.Index ?? 0;

            processRule.Type =
                ProcessRule.TextToComparisonType(
                    GetSelectedString(CmbComparisonType));
            processRule.FilePath = TxtPath.Text;
            processRule.SchemeGuid =
                GetPowerSchemeGuid(GetSelectedString(CmbPowerScheme));

            Rule = processRule;
            return true;
        }

        private void BtnSelectPath_Click(object sender, EventArgs e)
        {
            using var dlg = new CommonOpenFileDialog
            {
                IsFolderPicker = false,
            };

            if (dlg.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            TxtPath.Text = dlg.FileName;
        }

        private void RdbProcessRule_CheckedChanged(object sender, EventArgs e)
        {
            LblComparisonType.Visible = RdbProcessRule.Checked;
            CmbComparisonType.Visible = RdbProcessRule.Checked;
            LblPath.Visible = RdbProcessRule.Checked;
            TxtPath.Visible = RdbProcessRule.Checked;
            BtnSelectFile.Visible = RdbProcessRule.Checked;
            BtnSelectFolder.Visible = RdbProcessRule.Checked;
            BtnSelectFromProcess.Visible = RdbProcessRule.Checked;
        }

        private void RdbPowerLineRule_CheckedChanged(object sender, EventArgs e)
        {
            LblPowerLineStatus.Visible = RdbPowerLineRule.Checked;
            CmbPowerLineStatus.Visible = RdbPowerLineRule.Checked;
        }

        private void BtnSelectFromProcess_Click(object sender, EventArgs e)
        {
            using var processSelectionDlg = new ProcessSelectionDlg();
            if (processSelectionDlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            TxtPath.Text = processSelectionDlg.SelectedProcess!.ExecutablePath;
        }

        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            using var dlg = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
            };

            if (dlg.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            TxtPath.Text = dlg.FileName;
        }
    }
}
