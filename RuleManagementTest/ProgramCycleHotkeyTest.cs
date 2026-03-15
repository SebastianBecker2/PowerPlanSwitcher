namespace RuleManagementTest;

using System.Reflection;

[TestClass]
public sealed class ProgramCycleHotkeyTest
{
    private static MethodInfo GetNextCycleSchemeIndexMethod()
    {
        var assembly = Assembly.Load("PowerPlanSwitcher");
        var type = assembly.GetType("PowerPlanSwitcher.Program");
        Assert.IsNotNull(type, "Program type should exist in PowerPlanSwitcher assembly");

        var method = type.GetMethod(
            "GetNextCycleSchemeIndex",
            BindingFlags.Static | BindingFlags.NonPublic);

        Assert.IsNotNull(method, "GetNextCycleSchemeIndex should exist");
        return method;
    }

    [TestMethod]
    public void GetNextCycleSchemeIndex_EmptySchemes_ReturnsMinusOne()
    {
        var method = GetNextCycleSchemeIndexMethod();

        var result = method.Invoke(null, [new List<Guid>(), Guid.Empty]);

        Assert.AreEqual(-1, result);
    }

    [TestMethod]
    public void GetNextCycleSchemeIndex_ActiveSchemeMissing_SelectsFirstScheme()
    {
        var method = GetNextCycleSchemeIndexMethod();
        var schemes = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var result = method.Invoke(null, [schemes, Guid.NewGuid()]);

        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void GetNextCycleSchemeIndex_ActiveSchemePresent_SelectsFollowingScheme()
    {
        var method = GetNextCycleSchemeIndexMethod();
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();
        var schemes = new List<Guid> { first, second };

        var result = method.Invoke(null, [schemes, first]);

        Assert.AreEqual(1, result);
    }
}
