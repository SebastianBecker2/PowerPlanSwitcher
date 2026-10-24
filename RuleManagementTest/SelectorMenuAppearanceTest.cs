namespace RuleManagementTest;

using System.Drawing;
using PowerPlanSwitcher;

[TestClass]
public sealed class SelectorMenuAppearanceTest
{
    [TestMethod]
    public void ClampWidth_OutsideRange_ClampsToLimits()
    {
        Assert.AreEqual(
            SelectorMenuAppearance.MinWidth,
            SelectorMenuAppearance.ClampWidth(SelectorMenuAppearance.MinWidth - 1));
        Assert.AreEqual(
            SelectorMenuAppearance.MaxWidth,
            SelectorMenuAppearance.ClampWidth(5000));
        Assert.AreEqual(
            SelectorMenuAppearance.DefaultWidth,
            SelectorMenuAppearance.ClampWidth(SelectorMenuAppearance.DefaultWidth));
    }

    [TestMethod]
    public void ClampItemHeight_OutsideRange_ClampsToLimits()
    {
        Assert.AreEqual(
            SelectorMenuAppearance.MinItemHeight,
            SelectorMenuAppearance.ClampItemHeight(1));
        Assert.AreEqual(
            SelectorMenuAppearance.MaxItemHeight,
            SelectorMenuAppearance.ClampItemHeight(999));
        Assert.AreEqual(
            SelectorMenuAppearance.DefaultItemHeight,
            SelectorMenuAppearance.ClampItemHeight(
                SelectorMenuAppearance.DefaultItemHeight));
    }

    [TestMethod]
    public void CreateFont_UnknownFamily_UsesPrototypeFamilyAndDefaultSize()
    {
        using var prototype = new Font(
            FontFamily.GenericSansSerif,
            9f,
            FontStyle.Italic);
        using var font = SelectorMenuAppearance.CreateFont(
            "MissingFontXYZ, 14pt",
            prototype);

        Assert.AreEqual(prototype.FontFamily.Name, font.FontFamily.Name);
        Assert.AreEqual(SelectorMenuAppearance.DefaultFontSize, font.SizeInPoints);
        Assert.AreEqual(FontStyle.Regular, font.Style);
    }

    [TestMethod]
    public void CreateFont_BlankString_UsesPrototypeFamilyAndDefaultSize()
    {
        using var prototype = new Font(FontFamily.GenericSansSerif, 9f, FontStyle.Bold);
        using var font = SelectorMenuAppearance.CreateFont(" ", prototype);

        Assert.AreEqual(prototype.FontFamily.Name, font.FontFamily.Name);
        Assert.AreEqual(SelectorMenuAppearance.DefaultFontSize, font.SizeInPoints);
        Assert.AreEqual(FontStyle.Regular, font.Style);
    }

    [TestMethod]
    public void CreateFont_StoredFont_RoundTripsFamilySizeAndStyle()
    {
        using var original = new Font(
            FontFamily.GenericSansSerif,
            16f,
            FontStyle.Bold);
        using var prototype = new Font(FontFamily.GenericMonospace, 9f);
        using var loaded = SelectorMenuAppearance.CreateFont(
            SelectorMenuAppearance.ToSetting(original),
            prototype);

        Assert.AreEqual(original.FontFamily.Name, loaded.FontFamily.Name);
        Assert.AreEqual(original.SizeInPoints, loaded.SizeInPoints);
        Assert.AreEqual(FontStyle.Bold, loaded.Style);
    }

    [TestMethod]
    public void ParseTextColor_BlankOrUnknown_ReturnsNull()
    {
        Assert.IsNull(SelectorMenuAppearance.ParseTextColor(null));
        Assert.IsNull(SelectorMenuAppearance.ParseTextColor(" "));
        Assert.IsNull(SelectorMenuAppearance.ParseTextColor("not-a-color"));
    }

    [TestMethod]
    public void ParseTextColor_StoredColor_RoundTripsArgb()
    {
        var stored = SelectorMenuAppearance.ToColorSetting(Color.FromArgb(20, 40, 80));
        var loaded = SelectorMenuAppearance.ParseTextColor(stored);

        Assert.IsNotNull(loaded);
        Assert.AreEqual(Color.FromArgb(20, 40, 80).ToArgb(), loaded.Value.ToArgb());
    }

    [TestMethod]
    public void FitMenuWindow_ContentFits_ReturnsContent()
    {
        var content = new Size(360, 150);
        var fitted = SelectorMenuAppearance.FitMenuWindow(
            content,
            new Size(1920, 1000),
            verticalBar: 17,
            horizontalBar: 17);

        Assert.AreEqual(content, fitted);
    }

    [TestMethod]
    public void FitMenuWindow_TallerThanAvailable_CapsHeightAndReservesVerticalBar()
    {
        var fitted = SelectorMenuAppearance.FitMenuWindow(
            new Size(360, 1500),
            new Size(1920, 1000),
            verticalBar: 17,
            horizontalBar: 17);

        Assert.AreEqual(new Size(377, 1000), fitted);
    }

    [TestMethod]
    public void FitMenuWindow_WiderThanAvailable_CapsWidthAndReservesHorizontalBar()
    {
        var fitted = SelectorMenuAppearance.FitMenuWindow(
            new Size(1400, 150),
            new Size(1200, 1000),
            verticalBar: 17,
            horizontalBar: 17);

        Assert.AreEqual(new Size(1200, 167), fitted);
    }

    [TestMethod]
    public void FitMenuWindow_VerticalBarAlsoExceedsWidth_UsesAvailableSize()
    {
        var available = new Size(1200, 1000);
        var fitted = SelectorMenuAppearance.FitMenuWindow(
            new Size(1190, 1500),
            available,
            verticalBar: 17,
            horizontalBar: 17);

        Assert.AreEqual(available, fitted);
    }
}
