namespace PowerPlanSwitcher;

using System;
using System.Drawing;
using System.Windows.Forms;
using PowerPlanSwitcher.Properties;
using SevenZip;

public partial class IconSelectionDlg : Form
{
    private class ImageCache
    {
        private readonly Dictionary<string, Bitmap?> cache = [];

        public IEnumerable<string> Keys => cache.Keys;
        public int Count => cache.Count;

        public ImageCache(IEnumerable<string> filenames)
        {
            foreach (var name in filenames)
            {
                cache[name] = null;
            }
        }

        public void CacheImage(string filename, Bitmap image)
        {
            lock (cache)
            {
                cache[filename] = image;
            }
        }

        public Bitmap? GetImage(string filename)
        {
            lock (cache)
            {
                if (cache.TryGetValue(filename, out var image))
                {
                    return image;
                }
                return null;
            }
        }

        public Bitmap? GetImage(int index)
        {
            lock (cache)
            {
                var key = cache.Keys.ElementAt(index);
                return cache[key];
            }
        }
    }

    private static string ArchivePath => @"Resources\fatcow-hosting-icons-3.9.2.zip";
    private static string ArchiveFolder => "FatCow_Icons32x32";

    private readonly ImageCache imageCache;
    private List<string> filteredMapping;

    public Image? SelectedIcon { get; set; }

    public IconSelectionDlg()
    {
        InitializeComponent();

        foreach (var _ in Enumerable.Range(0, (DgvIcons.Width / 64) - 0))
        {
            var i = DgvIcons.Columns.Add(new DataGridViewImageColumn
            {
                Name = "Icon",
                HeaderText = "",
                Width = 64,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            });
        }

        using var extractor = new SevenZipExtractor(ArchivePath);
        var files = extractor.ArchiveFileData
            .Where(f => !f.IsDirectory && f.FileName.StartsWith(ArchiveFolder, StringComparison.OrdinalIgnoreCase))
            .ToList();
        imageCache = new ImageCache(files.Select(f => f.FileName));
        filteredMapping = [.. imageCache.Keys];

        DgvIcons.RowCount = (imageCache.Count + DgvIcons.ColumnCount - 1)
            / DgvIcons.ColumnCount;
    }

    protected override void OnShown(EventArgs e)
    {
        _ = Task.Run(() =>
        {
            using var extractor = new SevenZipExtractor(ArchivePath);
            foreach (var key in imageCache.Keys)
            {
                using var ms = new MemoryStream();
                extractor.ExtractFile(key, ms);
                ms.Position = 0;
                using var original = Image.FromStream(ms, false, false);
                var bitmap = new Bitmap(original, new Size(32, 32));
                imageCache.CacheImage(key, bitmap);
                Invoke(() =>
                {
                    if (!filteredMapping.Contains(key))
                    {
                        return;
                    }

                    var index = filteredMapping.IndexOf(key);
                    var row = index / DgvIcons.ColumnCount;
                    var column = index % DgvIcons.ColumnCount;
                    DgvIcons.InvalidateCell(column, row);
                });
            }
        });

        base.OnShown(e);
    }

    private void BtnOk_Click(object sender, EventArgs e)
    {
        if (DgvIcons.SelectedCells.Count == 0)
        {
            return;
        }
        var selectedCell = DgvIcons.SelectedCells[0];
        var index = (selectedCell.RowIndex * DgvIcons.ColumnCount) + selectedCell.ColumnIndex;
        if (index >= filteredMapping.Count)
        {
            return;
        }

        SelectedIcon = imageCache.GetImage(index);
        if (SelectedIcon is null)
        {
            return;
        }

        DialogResult = DialogResult.OK;
    }

    private void TxtFilter_TextChanged(object sender, EventArgs e)
    {
        filteredMapping = [.. imageCache.Keys
            .Where(filename => filename.Contains(
                TxtFilter.Text,
                StringComparison.OrdinalIgnoreCase))];
        DgvIcons.RowCount = (filteredMapping.Count + DgvIcons.ColumnCount - 1)
            / DgvIcons.ColumnCount;
        DgvIcons.Refresh();
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

    private void DgvIcons_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
        var index = (e.RowIndex * DgvIcons.ColumnCount) + e.ColumnIndex;
        if (index >= filteredMapping.Count)
        {
            e.Value = new Bitmap(1, 1);
            return;
        }

        var filename = filteredMapping[index];
        var image = imageCache.GetImage(filename);
        if (image == null)
        {
            e.Value = Resources.loading;
            return;
        }

        e.Value = image;
    }

    private void DgvIcons_CellDoubleClick(
        object sender,
        DataGridViewCellEventArgs e) =>
        BtnOk_Click(sender, e);
}
