namespace RuleManagementTest;

using PowerManagement;

[TestClass]
public sealed class PowerSchemeSettingsOpenerTest
{
    [TestMethod]
    public void IsKnownPowerScheme_ReturnsFalseForEmptyGuid()
    {
        Assert.IsFalse(PowerSchemeSettingsOpener.IsKnownPowerScheme(Guid.Empty));
    }

    [TestMethod]
    public void IsKnownPowerScheme_ReturnsTrueForAtLeastOneSystemPlan()
    {
        var schemes = PowerManager.Api.GetPowerSchemes().ToList();

        if (schemes.Count == 0)
        {
            Assert.Inconclusive("No power schemes are available on this system.");
        }

        var knownScheme = schemes.First(s => !string.IsNullOrWhiteSpace(s.name));
        Assert.IsTrue(PowerSchemeSettingsOpener.IsKnownPowerScheme(knownScheme.guid));
    }

    [TestMethod]
    public void IsKnownPowerScheme_ReturnsFalseForUnknownGuid()
    {
        Assert.IsFalse(PowerSchemeSettingsOpener.IsKnownPowerScheme(Guid.NewGuid()));
    }
}
