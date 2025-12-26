namespace RuleManagementTest;

using RuleManagement.Dto;
using RuleManagement.Rules;

[TestClass]
public sealed class StartupRuleTest
{
    [TestMethod]
    public void InitialTriggerCount_IsOne()
    {
        var dto = new StartupRuleDto { SchemeGuid = Guid.NewGuid() };

        var rule = new StartupRule(dto);

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void GetDescription_ReturnsExpectedText()
    {
        var dto = new StartupRuleDto { SchemeGuid = Guid.NewGuid() };
        var description = dto.GetDescription();

        Assert.AreEqual("Startup Rule", description);
    }

    [TestMethod]
    public void ExposesSchemeGuidFromDto()
    {
        var guid = Guid.NewGuid();
        var dto = new StartupRuleDto { SchemeGuid = guid };

        var rule = new StartupRule(dto);

        Assert.AreEqual(guid, rule.SchemeGuid);
    }
}
