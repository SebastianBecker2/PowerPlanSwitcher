namespace PowerPlanSwitcher;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

internal static class IconUtilities
{
    public static readonly Size DefaultPowerSchemeIconSize = new(32, 32);

    public static Bitmap NormalizeForPowerSchemeIcon(Image source) =>
        NormalizeImage(source, DefaultPowerSchemeIconSize);

    public static Bitmap NormalizeImage(Image source, Size targetSize)
    {
        var normalized = new Bitmap(
            targetSize.Width,
            targetSize.Height,
            PixelFormat.Format32bppArgb);

        using var graphics = Graphics.FromImage(normalized);
        graphics.Clear(Color.Transparent);
        graphics.CompositingMode = CompositingMode.SourceOver;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        var scale = Math.Min(
            targetSize.Width / (double)source.Width,
            targetSize.Height / (double)source.Height);

        var width = (int)Math.Round(source.Width * scale);
        var height = (int)Math.Round(source.Height * scale);
        var positionX = (targetSize.Width - width) / 2;
        var positionY = (targetSize.Height - height) / 2;

        graphics.DrawImage(source, positionX, positionY, width, height);
        return normalized;
    }

    public static Bitmap? LoadDetachedImageFromFile(string filePath)
    {
        using var fileStream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite);
        using var sourceImage = Image.FromStream(fileStream, true, true);
        return NormalizeForPowerSchemeIcon(sourceImage);
    }

    public static bool TryLoadDetachedImageFromFile(
        string filePath,
        out Bitmap? image,
        out Exception? error)
    {
        try
        {
            image = LoadDetachedImageFromFile(filePath);
            if (image is null)
            {
                throw new InvalidOperationException(
                    "Failed to load image from selected file.");
            }

            error = null;
            return true;
        }
        catch (Exception ex)
        {
            image = null;
            error = ex;
            return false;
        }
    }

    public static Icon CreateNotifyIcon(Image sourceImage)
    {
        var smallIconSize = SystemInformation.SmallIconSize;
        var iconSize = new Size(
            Math.Max(16, smallIconSize.Width),
            Math.Max(16, smallIconSize.Height));

        using var normalizedImage = NormalizeImage(sourceImage, iconSize);
        var iconHandle = normalizedImage.GetHicon();
        try
        {
            using var systemIcon = Icon.FromHandle(iconHandle);
            return (Icon)systemIcon.Clone();
        }
        finally
        {
            _ = Vanara.PInvoke.User32.DestroyIcon(iconHandle);
        }
    }
}