namespace PowerPlanSwitcher
{
    using System.Security.Cryptography;
    using System.Windows.Forms;

    public partial class CreatePasswordDlg : Form
    {
        private static readonly int RandomPasswordLength = 20;
        private static readonly char[] ValidChars =
            ("abcdefghijklmnopqrstuvwxyz" +
             "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
             "0123456789" +
             "!@#$%^&*()-_=+[]{}|;:,.<>?")
            .ToCharArray();

        public string Password => TxtPassword.Text;

        public CreatePasswordDlg() => InitializeComponent();

        private void BtnRandomize_Click(object sender, EventArgs e)
        {
            var password =
                string.Create(RandomPasswordLength, ValidChars, (span, chars) =>
                {
                    foreach (var i in Enumerable.Range(0, RandomPasswordLength))
                    {
                        var index = RandomNumberGenerator.GetInt32(chars.Length);
                        span[i] = chars[index];
                    }
                });

            TxtPassword.Text = password;
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPassword.Text))
            {
                return;
            }

            try
            {
                Clipboard.SetText(TxtPassword.Text);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(
                    $"Failed to copy to clipboard: {ex.Message}",
                    "Clipboard Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
