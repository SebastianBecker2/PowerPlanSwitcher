namespace PowerPlanSwitcher;

using System.ComponentModel;

internal static class SelectorMenuAppearance
{
    public const int DefaultWidth = 360;
    public const int DefaultItemHeight = 50;
    public const float DefaultFontSize = 12f;

    public const int MinWidth = 180;
    public const int MaxWidth = 1200;
    public const int MinItemHeight = 28;
    public const int MaxItemHeight = 240;

    public static int ClampWidth(int width) =>
        Math.Clamp(width, MinWidth, MaxWidth);

    public static int ClampItemHeight(int height) =>
        Math.Clamp(height, MinItemHeight, MaxItemHeight);

    public static Font CreateFont(string? stored, Font prototype)
    {
        if (!string.IsNullOrWhiteSpace(stored))
        {
            try
            {
                var converter = new FontConverter();
                if (converter.ConvertFromInvariantString(stored) is Font font)
                {
                    // An unknown family is substituted instead of throwing.
                    if (font.SizeInPoints > 0
                        && font.FontFamily.Name.Equals(
                            font.OriginalFontName,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        return font;
                    }

                    font.Dispose();
                }
            }
            catch (Exception ex) when (
                ex is ArgumentException or FormatException or NotSupportedException)
            {
            }
        }

        return new Font(
            prototype.FontFamily.Name,
            DefaultFontSize,
            FontStyle.Regular,
            GraphicsUnit.Point);
    }

    public static string ToSetting(Font font) =>
        new FontConverter().ConvertToInvariantString(font) ?? "";

    public static Color? ParseTextColor(string? stored)
    {
        if (string.IsNullOrWhiteSpace(stored))
        {
            return null;
        }

        try
        {
            if (new ColorConverter().ConvertFromInvariantString(stored) is Color color
                && !color.IsEmpty)
            {
                return color;
            }
        }
        catch (Exception ex) when (
            ex is ArgumentException or FormatException or NotSupportedException)
        {
        }

        return null;
    }

    public static string ToColorSetting(Color color) =>
        new ColorConverter().ConvertToInvariantString(color) ?? "";

    public static Color ThemeTextColor() =>
        ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
            ? Color.Black
            : Color.White;

    internal static Size FitMenuWindow(
        Size content,
        Size available,
        int verticalBar,
        int horizontalBar)
    {
        if (content.Width <= available.Width && content.Height <= available.Height)
        {
            return content;
        }

        var needsVertical = content.Height > available.Height;
        var needsHorizontal = content.Width > available.Width;
        if (needsVertical && content.Width + verticalBar > available.Width)
        {
            needsHorizontal = true;
        }

        if (needsHorizontal && content.Height + horizontalBar > available.Height)
        {
            needsVertical = true;
        }

        if (needsVertical && needsHorizontal)
        {
            return available;
        }

        if (needsVertical)
        {
            return new Size(content.Width + verticalBar, available.Height);
        }

        return new Size(available.Width, content.Height + horizontalBar);
    }
}
