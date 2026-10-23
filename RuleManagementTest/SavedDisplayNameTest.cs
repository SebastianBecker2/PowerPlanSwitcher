namespace RuleManagementTest;

using PowerPlanSwitcher;
using PowerPlanSwitcher.Properties;

[TestClass]
public sealed class SavedDisplayNameTest
{
    [TestMethod]
    public void GetSelectedPopUpWindowLocation_LegacyBottomRight_ReturnsBottomRight()
    {
        var location = PopUpWindowLocationHelper.GetSelectedPopUpWindowLocation(
            "Bottom Right");

        Assert.AreEqual(PopUpWindowLocation.BottomRight, location);
    }

    [TestMethod]
    public void GetSelectedColorTheme_LegacyDarkMode_ReturnsDark()
    {
        var previous = Settings.Default.ColorTheme;
        try
        {
            Settings.Default.ColorTheme = "Dark Mode";
            Assert.AreEqual(ColorTheme.Dark, ColorThemeHelper.GetSelectedColorTheme());
        }
        finally
        {
            Settings.Default.ColorTheme = previous;
        }
    }
}
