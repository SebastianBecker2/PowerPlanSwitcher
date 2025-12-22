namespace PowerPlanSwitcher;

using System;
using System.Drawing;
using System.Windows.Forms;

public partial class IconSelectionDlg : Form
{
    public Image? SelectedIcon { get; set; }

    public IconSelectionDlg() => InitializeComponent();

    protected override void OnLoad(EventArgs e) =>
        UpdateLsvIcons();

    private void BtnOk_Click(object sender, EventArgs e)
    {
        if (LsvIcons.SelectedItems.Count == 0)
        {
            return;
        }

        SelectedIcon = ImlIcons.Images[LsvIcons.SelectedItems[0].ImageIndex];

        DialogResult = DialogResult.OK;
    }

    private void TxtFilter_TextChanged(object sender, EventArgs e) =>
        UpdateLsvIcons();

    private void UpdateLsvIcons()
    {
        LsvIcons.Items.Clear();
        LsvIcons.Items.AddRange([.. ImlIcons.Images.Keys
            .Cast<string>()
            .Where(k => k.Contains(TxtFilter.Text, StringComparison.InvariantCultureIgnoreCase))
            .Select(k => new ListViewItem
            {
                ImageIndex = ImlIcons.Images.IndexOfKey(k),
                Text = k,
            })]);
    }

    private void BtnSelectFile_Click(object sender, EventArgs e)
    {
        var typeFilters = new[]
        {
            "All image types " +
            "(*.png; *.jpg; *.jpeg; *.bmp; *.tiff; *.tif; *.gif)" +
            "|*.png;*.jpg;*.jpeg;*.bmp;*.tiff;*.tif;*.gif",
            "PNG (*.png)|*.png",
            "JPEG (*.jpg; *.jpeg)|*.jpg;*.jpeg",
            "BMP (*.bmp)|*.bmp",
            "TIFF (*.tiff; *.tif)|*.tiff;*.tif",
            "GIF (*.gif)|*.gif",
            "All files (*.*)|*.*",
        };

        using var dlg = new OpenFileDialog
        {
            Filter = string.Join("|", typeFilters),
            FilterIndex = 0,
            RestoreDirectory = true,
        };
        if (dlg.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        SelectedIcon = Image.FromFile(dlg.FileName);
        DialogResult = DialogResult.OK;
    }

    private void LsvIcons_MouseDoubleClick(object sender, MouseEventArgs e) =>
        BtnOk_Click(sender, e);
}
