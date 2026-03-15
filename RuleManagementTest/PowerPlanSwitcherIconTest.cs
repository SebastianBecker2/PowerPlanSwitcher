namespace RuleManagementTest;

using System.Drawing;
using System.Drawing.Imaging;
using System.Xml.Linq;
using Newtonsoft.Json;
using PowerPlanSwitcher;

[TestClass]
public sealed class PowerPlanSwitcherIconTest
{
    [TestMethod]
    public void NormalizeForPowerSchemeIcon_NonSquareImage_Produces32x32()
    {
        using var source = new Bitmap(64, 21);
        using var normalized = IconUtilities.NormalizeForPowerSchemeIcon(source);

        Assert.AreEqual(32, normalized.Width);
        Assert.AreEqual(32, normalized.Height);
    }

    [TestMethod]
    public void CreateNotifyIcon_NonSquareImage_DoesNotThrow()
    {
        using var source = new Bitmap(42, 17);
        using var icon = IconUtilities.CreateNotifyIcon(source);

        Assert.IsGreaterThan(0, icon.Size.Width);
        Assert.IsGreaterThan(0, icon.Size.Height);
    }

    [TestMethod]
    public void DeserializeSettings_InvalidBase64Icon_ReturnsNullIcon()
    {
        var guid = Guid.NewGuid();
        var json = "{" +
            $"\"{guid}\":" +
            "{" +
            "\"Visible\":true," +
            "\"Icon\":\"%%%invalid%%%\"," +
            "\"Hotkey\":null" +
            "}" +
            "}";

        var result = JsonConvert.DeserializeObject<Dictionary<Guid, PowerSchemeSettings.Setting>>(json);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.ContainsKey(guid));
        Assert.IsNull(result[guid].Icon);
    }

    [TestMethod]
    public void RoundTripSettings_NonSquareIcon_IsNormalizedAfterDeserialize()
    {
        var guid = Guid.NewGuid();
        using var source = new Bitmap(96, 22);

        var settings = new Dictionary<Guid, PowerSchemeSettings.Setting>
        {
            [guid] = new PowerSchemeSettings.Setting
            {
                Visible = true,
                Icon = source,
                Hotkey = null,
            }
        };

        var serialized = JsonConvert.SerializeObject(settings);
        var deserialized = JsonConvert.DeserializeObject<Dictionary<Guid, PowerSchemeSettings.Setting>>(serialized);

        Assert.IsNotNull(deserialized);
        Assert.IsTrue(deserialized.ContainsKey(guid));
        Assert.IsNotNull(deserialized[guid].Icon);
        Assert.AreEqual(32, deserialized[guid].Icon!.Width);
        Assert.AreEqual(32, deserialized[guid].Icon!.Height);
    }

    [TestMethod]
    public void AttachedUserConfig_DeserializesAndCreatesNotifyIcons()
    {
        var filePath = @"c:\Users\Levithan\Downloads\New folder\user.config.txt";
        if (!File.Exists(filePath))
        {
            Assert.Inconclusive("Attached user config is not available on this machine.");
            return;
        }

        var xml = XDocument.Load(filePath);
        var settingElement = xml
            .Descendants("setting")
            .FirstOrDefault(e =>
                string.Equals(
                    (string?)e.Attribute("name"),
                    "PowerSchemeSettings",
                    StringComparison.Ordinal));

        Assert.IsNotNull(settingElement);
        var json = settingElement.Element("value")?.Value;
        Assert.IsFalse(string.IsNullOrWhiteSpace(json));
        Assert.IsNotNull(json);

        var settings = JsonConvert.DeserializeObject<Dictionary<Guid, PowerSchemeSettings.Setting>>(json);

        Assert.IsNotNull(settings);
        Assert.IsNotEmpty(settings);

        foreach (var setting in settings.Values)
        {
            if (setting.Icon is null)
            {
                continue;
            }

            using var icon = IconUtilities.CreateNotifyIcon(setting.Icon);
            Assert.IsGreaterThan(0, icon.Size.Width);
            Assert.IsGreaterThan(0, icon.Size.Height);
        }
    }

    [TestMethod]
    public void TryLoadUserIcon_InvalidFile_ReturnsFalseAndError()
    {
        var filePath = Path.Combine(
            Path.GetTempPath(),
            $"missing-icon-{Guid.NewGuid():N}.png");

        var result = IconUtilities.TryLoadDetachedImageFromFile(
            filePath,
            out var image,
            out var error);

        Assert.IsFalse(result);
        Assert.IsNull(image);
        Assert.IsNotNull(error);
    }

    [TestMethod]
    public void TryLoadUserIcon_ValidFile_ReturnsNormalizedImage()
    {
        var filePath = Path.Combine(
            Path.GetTempPath(),
            $"valid-icon-{Guid.NewGuid():N}.png");

        try
        {
            using (var source = new Bitmap(80, 18))
            {
                source.Save(filePath, ImageFormat.Png);
            }

            var result = IconUtilities.TryLoadDetachedImageFromFile(
                filePath,
                out var image,
                out var error);

            Assert.IsTrue(result);
            Assert.IsNotNull(image);
            Assert.IsNull(error);
            Assert.AreEqual(32, image.Width);
            Assert.AreEqual(32, image.Height);

            image.Dispose();
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
