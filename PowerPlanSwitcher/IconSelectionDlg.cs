namespace PowerPlanSwitcher;

using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using PowerPlanSwitcher.Properties;
using Serilog;
using SevenZip;

public partial class IconSelectionDlg : Form
{
    private const string LoadingImageKey = "__loading__";

    private class ImageCache
    {
        private readonly Dictionary<string, Bitmap?> cache = [];

        public IEnumerable<string> Keys => cache.Keys;

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

    }

    private static string ArchivePath => @"Resources\fatcow-hosting-icons-3.9.2.zip";
    private static string ArchiveFolder => "FatCow_Icons32x32";

    private readonly CancellationTokenSource loadCancellation = new();
    private readonly ConcurrentQueue<string> loadQueue = new();
    private readonly ConcurrentQueue<int> redrawQueue = new();
    private readonly HashSet<string> queuedKeys = [];
    private readonly object queuedKeysLock = new();
    private readonly Dictionary<string, int> imageIndices = [];
    private readonly Dictionary<string, int> filteredIndexByKey = [];
    private readonly Dictionary<string, string> displayNameByKey = [];
    private readonly System.Windows.Forms.Timer refreshTimer = new();
    private readonly HashSet<int> redrawBatch = [];
    private readonly List<int> redrawBatchSorted = [];

    private readonly ImageCache imageCache;
    private List<string> filteredMapping;
    private int loadingImageIndex;
    private volatile bool isLoaderRunning;
    private volatile bool hasPendingRefresh;

    public Image? SelectedIcon { get; set; }

    public IconSelectionDlg()
    {
        InitializeComponent();

        using (var loadingMs = new MemoryStream(Resources.loading))
        using (var loadingImage = Image.FromStream(loadingMs, false, false))
        {
            ImgIcons.Images.Add(
                LoadingImageKey,
                IconUtilities.NormalizeForPowerSchemeIcon(loadingImage));
        }
        loadingImageIndex = ImgIcons.Images.IndexOfKey(LoadingImageKey);
        LvwIcons.VirtualMode = true;
        EnableDoubleBuffering(LvwIcons);

        refreshTimer.Interval = 120;
        refreshTimer.Tick += RefreshTimer_Tick;
        refreshTimer.Start();

        using var extractor = new SevenZipExtractor(ArchivePath);
        var files = extractor.ArchiveFileData
            .Where(f => !f.IsDirectory && f.FileName.StartsWith(ArchiveFolder, StringComparison.OrdinalIgnoreCase))
            .ToList();
        imageCache = new ImageCache(files.Select(f => f.FileName));
        foreach (var key in imageCache.Keys)
        {
            displayNameByKey[key] = Path.GetFileNameWithoutExtension(key);
        }
        filteredMapping = [.. imageCache.Keys];

        RebuildIconList();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        loadCancellation.Cancel();
        refreshTimer.Stop();
        refreshTimer.Dispose();
        base.OnFormClosed(e);
    }

    private void BtnOk_Click(object sender, EventArgs e)
    {
        if (LvwIcons.SelectedIndices.Count == 0)
        {
            return;
        }

        var selectedIndex = LvwIcons.SelectedIndices[0];
        if (selectedIndex < 0 || selectedIndex >= filteredMapping.Count)
        {
            return;
        }

        var key = filteredMapping[selectedIndex];

        SelectedIcon = imageCache.GetImage(key);
        if (SelectedIcon is null)
        {
            return;
        }

        DialogResult = DialogResult.OK;
    }

    private void TxtFilter_TextChanged(object sender, EventArgs e)
    {
        var filterText = TxtFilter.Text;
        if (string.IsNullOrWhiteSpace(filterText))
        {
            filteredMapping = [.. imageCache.Keys];
            RebuildIconList();
            return;
        }

        filteredMapping = [.. imageCache.Keys
            .Where(filename =>
                filename.Contains(filterText, StringComparison.OrdinalIgnoreCase)
                || displayNameByKey[filename].Contains(filterText, StringComparison.OrdinalIgnoreCase))];
        RebuildIconList();
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

        if (!IconUtilities.TryLoadDetachedImageFromFile(
            dlg.FileName,
            out var loadedImage,
            out var error))
        {
            Log.Warning(
                error,
                "Failed to load user-selected icon file {FilePath}",
                dlg.FileName);
            _ = MessageBox.Show(
                "The selected file could not be loaded as an icon image. " +
                "Please choose a valid PNG, JPG, BMP, TIFF, or GIF image.",
                "Invalid image file",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        SelectedIcon = loadedImage;

        DialogResult = DialogResult.OK;
    }

    private void RebuildIconList()
    {
        filteredIndexByKey.Clear();
        for (var index = 0; index < filteredMapping.Count; index++)
        {
            filteredIndexByKey[filteredMapping[index]] = index;
        }

        LvwIcons.BeginUpdate();
        try
        {
            LvwIcons.VirtualListSize = filteredMapping.Count;
        }
        finally
        {
            LvwIcons.EndUpdate();
        }

        if (LvwIcons.VirtualListSize > 0)
        {
            LvwIcons.RedrawItems(0, LvwIcons.VirtualListSize - 1, false);
        }
    }

    private void LvwIcons_ItemActivate(object? sender, EventArgs e)
    {
        BtnOk_Click(sender ?? this, e);
    }

    private void LvwIcons_RetrieveVirtualItem(
        object? sender,
        RetrieveVirtualItemEventArgs e)
    {
        if (e.ItemIndex < 0 || e.ItemIndex >= filteredMapping.Count)
        {
            e.Item = new ListViewItem(string.Empty, loadingImageIndex);
            return;
        }

        var key = filteredMapping[e.ItemIndex];
        var image = imageCache.GetImage(key);
        if (image is null)
        {
            QueueIconLoad(key);
            e.Item = new ListViewItem(displayNameByKey[key], loadingImageIndex);
            return;
        }

        var imageIndex = GetOrAddImageIndex(key, image);
        e.Item = new ListViewItem(displayNameByKey[key], imageIndex);
    }

    private void LvwIcons_CacheVirtualItems(
        object? sender,
        CacheVirtualItemsEventArgs e)
    {
        if (filteredMapping.Count == 0)
        {
            return;
        }

        var first = Math.Max(0, e.StartIndex);
        var last = Math.Min(filteredMapping.Count - 1, e.EndIndex);

        for (var i = first; i <= last; i++)
        {
            QueueIconLoad(filteredMapping[i]);
        }
    }

    private int GetOrAddImageIndex(string key, Bitmap image)
    {
        if (imageIndices.TryGetValue(key, out var index))
        {
            return index;
        }

        ImgIcons.Images.Add(key, image);
        index = ImgIcons.Images.IndexOfKey(key);
        imageIndices[key] = index;
        return index;
    }

    private void QueueIconLoad(string key)
    {
        lock (queuedKeysLock)
        {
            if (!queuedKeys.Add(key))
            {
                return;
            }
        }

        loadQueue.Enqueue(key);
        StartLoaderIfNeeded();
    }

    private void StartLoaderIfNeeded()
    {
        if (isLoaderRunning)
        {
            return;
        }

        isLoaderRunning = true;
        _ = Task.Run(ProcessLoadQueueAsync, loadCancellation.Token);
    }

    private async Task ProcessLoadQueueAsync()
    {
        try
        {
            using var extractor = new SevenZipExtractor(ArchivePath);
            while (!loadCancellation.IsCancellationRequested)
            {
                if (!loadQueue.TryDequeue(out var key))
                {
                    await Task.Delay(30, loadCancellation.Token);
                    continue;
                }

                Bitmap? bitmap = null;
                Exception? error = null;

                try
                {
                    using var ms = new MemoryStream();
                    extractor.ExtractFile(key, ms);
                    ms.Position = 0;
                    using var original = Image.FromStream(ms, false, false);
                    bitmap = IconUtilities.NormalizeForPowerSchemeIcon(original);
                    imageCache.CacheImage(key, bitmap);
                }
                catch (Exception ex)
                {
                    error = ex;
                }
                finally
                {
                    lock (queuedKeysLock)
                    {
                        _ = queuedKeys.Remove(key);
                    }
                }

                if (error is not null)
                {
                    Log.Warning(
                        error,
                        "Failed to decode icon from archive entry {ArchiveEntry}",
                        key);
                    continue;
                }

                if (bitmap is null || IsDisposed)
                {
                    continue;
                }

                if (filteredIndexByKey.TryGetValue(key, out var index))
                {
                    redrawQueue.Enqueue(index);
                }

                hasPendingRefresh = true;
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            isLoaderRunning = false;
            if (!loadQueue.IsEmpty && !loadCancellation.IsCancellationRequested)
            {
                StartLoaderIfNeeded();
            }
        }
    }

    private void RefreshTimer_Tick(object? sender, EventArgs e)
    {
        if (!hasPendingRefresh)
        {
            return;
        }

        hasPendingRefresh = false;
        if (IsDisposed)
        {
            return;
        }

        redrawBatch.Clear();
        while (redrawBatch.Count < 512 && redrawQueue.TryDequeue(out var index))
        {
            if (index >= 0 && index < LvwIcons.VirtualListSize)
            {
                _ = redrawBatch.Add(index);
            }
        }

        if (redrawBatch.Count == 0)
        {
            hasPendingRefresh = !redrawQueue.IsEmpty;
            return;
        }

        redrawBatchSorted.Clear();
        redrawBatchSorted.AddRange(redrawBatch);
        redrawBatchSorted.Sort();

        var rangeStart = redrawBatchSorted[0];
        var previous = redrawBatchSorted[0];

        for (var i = 1; i < redrawBatchSorted.Count; i++)
        {
            var current = redrawBatchSorted[i];
            if (current == previous + 1)
            {
                previous = current;
                continue;
            }

            LvwIcons.RedrawItems(rangeStart, previous, false);
            rangeStart = current;
            previous = current;
        }

        LvwIcons.RedrawItems(rangeStart, previous, false);
        hasPendingRefresh = !redrawQueue.IsEmpty;
    }

    private static void EnableDoubleBuffering(Control control)
    {
        var property = typeof(Control).GetProperty(
            "DoubleBuffered",
            BindingFlags.Instance | BindingFlags.NonPublic);
        property?.SetValue(control, true, null);
    }
}
