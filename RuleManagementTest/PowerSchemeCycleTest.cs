namespace RuleManagementTest;

using PowerManagement;

[TestClass]
public sealed class PowerSchemeCycleTest
{
    [TestMethod]
    public void GetNextSchemeIndex_EmptySchemes_ReturnsMinusOne()
    {
        Assert.AreEqual(-1, PowerSchemeCycle.GetNextSchemeIndex([], Guid.Empty));
    }

    [TestMethod]
    public void GetNextSchemeIndex_ActiveSchemeMissing_SelectsFirstScheme()
    {
        var schemes = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        Assert.AreEqual(0, PowerSchemeCycle.GetNextSchemeIndex(schemes, Guid.NewGuid()));
    }

    [TestMethod]
    public void GetNextSchemeIndex_ActiveSchemePresent_SelectsFollowingScheme()
    {
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();
        var schemes = new List<Guid> { first, second };

        Assert.AreEqual(1, PowerSchemeCycle.GetNextSchemeIndex(schemes, first));
    }

    [TestMethod]
    public void GetNextSchemeIndex_LastScheme_WrapsToFirst()
    {
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();
        var schemes = new List<Guid> { first, second };

        Assert.AreEqual(0, PowerSchemeCycle.GetNextSchemeIndex(schemes, second));
    }
}
