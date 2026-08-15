namespace RuleManagementTest;

using System.Xml.Linq;
using PowerManagement;

[TestClass]
public sealed class PowerSchemeUserPreferencesTest
{
    [TestMethod]
    public void LoadFromUserConfig_ReadsVisibilityAndCyclePreference()
    {
        var visibleGuid = Guid.NewGuid();
        var hiddenGuid = Guid.NewGuid();
        var json =
            "{" +
            $"\"{visibleGuid}\":{{\"Visible\":true}}," +
            $"\"{hiddenGuid}\":{{\"Visible\":false}}" +
            "}";

        var configPath = Path.Combine(
            Path.GetTempPath(),
            $"pps-user-config-{Guid.NewGuid():N}.config");

        try
        {
            WriteUserConfig(configPath, json, cycleOnlyVisible: true);

            var preferences = PowerSchemeUserPreferences.LoadFromUserConfig(configPath);

            Assert.IsTrue(preferences.CycleOnlyVisible);
            Assert.IsTrue(preferences.IsVisible(visibleGuid));
            Assert.IsFalse(preferences.IsVisible(hiddenGuid));
            Assert.IsTrue(preferences.IsVisible(Guid.NewGuid()));
            Assert.AreEqual(configPath, preferences.SourceConfigPath);
        }
        finally
        {
            if (File.Exists(configPath))
            {
                File.Delete(configPath);
            }
        }
    }

    [TestMethod]
    public void LoadFromUserConfig_MissingFileContent_UsesDefaults()
    {
        var configPath = Path.Combine(
            Path.GetTempPath(),
            $"pps-user-config-{Guid.NewGuid():N}.config");

        try
        {
            File.WriteAllText(configPath, "<configuration />");
            var preferences = PowerSchemeUserPreferences.LoadFromUserConfig(configPath);

            Assert.IsFalse(preferences.CycleOnlyVisible);
            Assert.IsTrue(preferences.IsVisible(Guid.NewGuid()));
        }
        finally
        {
            if (File.Exists(configPath))
            {
                File.Delete(configPath);
            }
        }
    }

    [TestMethod]
    public void Load_PrefersMatchingVersionOverNewerDifferentVersion()
    {
        var root = Path.Combine(Path.GetTempPath(), $"pps-localappdata-{Guid.NewGuid():N}");
        var company = PowerSchemeUserPreferences.ToSettingsCompanyFolderName(
            PowerSchemeUserPreferences.DefaultCompanyName);
        var app = Path.Combine(root, company, "PowerPlanSwitcher_Path_test");
        var oldVersion = Path.Combine(app, "1.2.9.0");
        var matchedVersion = Path.Combine(app, "1.2.10.0");
        Directory.CreateDirectory(oldVersion);
        Directory.CreateDirectory(matchedVersion);

        var oldConfig = Path.Combine(oldVersion, "user.config");
        var matchedConfig = Path.Combine(matchedVersion, "user.config");
        WriteUserConfig(oldConfig, "{}", cycleOnlyVisible: false);
        WriteUserConfig(matchedConfig, "{}", cycleOnlyVisible: true);

        // Make the non-matching version newer on disk.
        File.SetLastWriteTimeUtc(oldConfig, DateTime.UtcNow.AddMinutes(5));
        File.SetLastWriteTimeUtc(matchedConfig, DateTime.UtcNow);

        try
        {
            var preferences = PowerSchemeUserPreferences.Load(
                root,
                company,
                fileVersion: "1.2.10.0");

            Assert.IsTrue(preferences.CycleOnlyVisible);
            Assert.AreEqual(matchedConfig, preferences.SourceConfigPath);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [TestMethod]
    public void Load_FallsBackToNewestWhenVersionMissing()
    {
        var root = Path.Combine(Path.GetTempPath(), $"pps-localappdata-{Guid.NewGuid():N}");
        var company = PowerSchemeUserPreferences.ToSettingsCompanyFolderName(
            PowerSchemeUserPreferences.DefaultCompanyName);
        var app = Path.Combine(root, company, "PowerPlanSwitcher_Path_test");
        var older = Path.Combine(app, "1.2.9.0");
        var newer = Path.Combine(app, "1.2.8.0");
        Directory.CreateDirectory(older);
        Directory.CreateDirectory(newer);

        var olderConfig = Path.Combine(older, "user.config");
        var newerConfig = Path.Combine(newer, "user.config");
        WriteUserConfig(olderConfig, "{}", cycleOnlyVisible: false);
        WriteUserConfig(newerConfig, "{}", cycleOnlyVisible: true);
        File.SetLastWriteTimeUtc(olderConfig, DateTime.UtcNow);
        File.SetLastWriteTimeUtc(newerConfig, DateTime.UtcNow.AddMinutes(5));

        try
        {
            var preferences = PowerSchemeUserPreferences.Load(
                root,
                company,
                fileVersion: "9.9.9.0");

            Assert.IsTrue(preferences.CycleOnlyVisible);
            Assert.AreEqual(newerConfig, preferences.SourceConfigPath);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [TestMethod]
    public void ToSettingsCompanyFolderName_ReplacesSpaces()
    {
        Assert.AreEqual(
            "Sebastian_Becker",
            PowerSchemeUserPreferences.ToSettingsCompanyFolderName("Sebastian Becker"));
    }

    private static void WriteUserConfig(
        string configPath,
        string powerSchemeSettingsJson,
        bool cycleOnlyVisible)
    {
        var document = new XDocument(
            new XElement(
                "configuration",
                new XElement(
                    "userSettings",
                    new XElement(
                        "PowerPlanSwitcher.Properties.Settings",
                        new XElement(
                            "setting",
                            new XAttribute("name", "PowerSchemeSettings"),
                            new XAttribute("serializeAs", "String"),
                            new XElement("value", powerSchemeSettingsJson)),
                        new XElement(
                            "setting",
                            new XAttribute("name", "CycleOnlyVisible"),
                            new XAttribute("serializeAs", "String"),
                            new XElement("value", cycleOnlyVisible ? "True" : "False"))))));
        document.Save(configPath);
    }
}
