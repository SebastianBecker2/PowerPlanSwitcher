namespace RuleManagementTest;

using System.Data;
using FakeItEasy;
using RuleManagement.Dto;
using SystemManagement;

[TestClass]
public sealed class IdleRuleTest
{
    [TestMethod]
    public void InitialTriggerCount_IsZero()
    {
        var dto = new IdleRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            IdleTimeThreshold = TimeSpan.FromSeconds(10)
        };
        var harness = new IdleRuleHarness(dto);
        harness.IdleRule.StartRuling();

        Assert.AreEqual(0, harness.IdleRule.TriggerCount);
    }

    [TestMethod]
    public void IdleBelowThreshold_DoesNotActivate()
    {
        var dto = new IdleRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            IdleTimeThreshold = TimeSpan.FromSeconds(10)
        };
        var harness = new IdleRuleHarness(dto);
        harness.IdleRule.StartRuling();

        harness.IdleMonitor.IdleTimeChanged += Raise.With(
            new IdleTimeChangedEventArgs(TimeSpan.FromSeconds(5)));

        Assert.AreEqual(0, harness.IdleRule.TriggerCount);
    }

    [TestMethod]
    public void IdleMeetsThreshold_Activates()
    {
        var dto = new IdleRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            IdleTimeThreshold = TimeSpan.FromSeconds(10)
        };
        var harness = new IdleRuleHarness(dto);
        harness.IdleRule.StartRuling();

        harness.IdleMonitor.IdleTimeChanged += Raise.With(
            new IdleTimeChangedEventArgs(TimeSpan.FromSeconds(10)));

        Assert.AreEqual(1, harness.IdleRule.TriggerCount);
    }

    [TestMethod]
    public void IdleAboveThreshold_DoesNotExceedOne()
    {
        var dto = new IdleRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            IdleTimeThreshold = TimeSpan.FromSeconds(10)
        };
        var harness = new IdleRuleHarness(dto);
        harness.IdleRule.StartRuling();

        harness.IdleMonitor.IdleTimeChanged += Raise.With(new IdleTimeChangedEventArgs(TimeSpan.FromSeconds(20)));
        harness.IdleMonitor.IdleTimeChanged += Raise.With(new IdleTimeChangedEventArgs(TimeSpan.FromSeconds(30)));

        Assert.AreEqual(1, harness.IdleRule.TriggerCount);
    }

    [TestMethod]
    public void IdleDropsBelowThreshold_Deactivates()
    {
        var dto = new IdleRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            IdleTimeThreshold = TimeSpan.FromSeconds(10)
        };
        var harness = new IdleRuleHarness(dto);
        harness.IdleRule.StartRuling();

        // Activate
        harness.IdleMonitor.IdleTimeChanged += Raise.With(new IdleTimeChangedEventArgs(TimeSpan.FromSeconds(15)));
        Assert.AreEqual(1, harness.IdleRule.TriggerCount);

        // Deactivate
        harness.IdleMonitor.IdleTimeChanged += Raise.With(new IdleTimeChangedEventArgs(TimeSpan.FromSeconds(5)));
        Assert.AreEqual(0, harness.IdleRule.TriggerCount);
    }

    [TestMethod]
    public void TriggerCountNeverNegative()
    {
        var dto = new IdleRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            IdleTimeThreshold = TimeSpan.FromSeconds(10)
        };
        var harness = new IdleRuleHarness(dto);
        harness.IdleRule.StartRuling();

        harness.IdleMonitor.IdleTimeChanged += Raise.With(new IdleTimeChangedEventArgs(TimeSpan.Zero));

        Assert.AreEqual(0, harness.IdleRule.TriggerCount);
    }

    [TestMethod]
    public void ExposesSchemeGuidFromDto()
    {
        var guid = Guid.NewGuid();
        var dto = new IdleRuleDto
        {
            SchemeGuid = guid,
            IdleTimeThreshold = TimeSpan.FromSeconds(10)
        };
        var harness = new IdleRuleHarness(dto);
        harness.IdleRule.StartRuling();

        Assert.AreEqual(guid, harness.IdleRule.SchemeGuid);
    }

    [TestMethod]
    public void SubscribesToIdleMonitor()
    {
        var dto = new IdleRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            IdleTimeThreshold = TimeSpan.FromSeconds(10)
        };
        var harness = new IdleRuleHarness(dto);
        harness.IdleRule.StartRuling();

        _ = A.CallTo(harness.IdleMonitor)
            .Where(call => call.Method.Name == "add_IdleTimeChanged")
            .MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public void GetDescription_ReturnsExpectedText()
    {
        var dto = new IdleRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            IdleTimeThreshold = TimeSpan.FromSeconds(10)
        };

        Assert.AreEqual("Idle Time -> 00:00:10", dto.GetDescription());
    }
}
