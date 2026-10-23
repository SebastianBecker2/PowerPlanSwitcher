namespace PowerPlanSwitcher;

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

    public CreatePasswordDlg()
    {
        InitializeComponent();
        ApplyLocalization();
        _ = new DpiImageScaler(this);
    }

    private void ApplyLocalization()
    {
        Text = Strings.CreatePasswordDlg_Title;
        BtnOkay.Text = Strings.CreatePasswordDlg_BtnOk;
        btnCancel.Text = Strings.CreatePasswordDlg_BtnCancel;
        label1.Text = Strings.CreatePasswordDlg_Hint;
    }

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
                string.Format(Strings.CreatePasswordDlg_MsgCopyFailed, ex.Message),
                Strings.CreatePasswordDlg_TitleClipboardError,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
