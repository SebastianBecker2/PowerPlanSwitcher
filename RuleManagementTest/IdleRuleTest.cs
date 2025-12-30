namespace RuleManagementTest;

using System.Data;
using FakeItEasy;
using RuleManagement.Dto;
using RuleManagement.Rules;
using SystemManagement;

[TestClass]
public sealed class IdleRuleTest
{
    [TestMethod]
    public void InitialTriggerCount_IsZero()
    {
        var monitor = A.Fake<IIdleMonitor>();
        var dto = new IdleRuleDto { SchemeGuid = Guid.NewGuid(), IdleTimeThreshold = TimeSpan.FromSeconds(10) };

        var rule = new IdleRule(monitor, dto);
        rule.StartRuling();

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void IdleBelowThreshold_DoesNotActivate()
    {
        var monitor = A.Fake<IIdleMonitor>();
        var dto = new IdleRuleDto { SchemeGuid = Guid.NewGuid(), IdleTimeThreshold = TimeSpan.FromSeconds(10) };

        var rule = new IdleRule(monitor, dto);
        rule.StartRuling();

        monitor.IdleTimeChanged += Raise.With(new IdleTimeChangedEventArgs(TimeSpan.FromSeconds(5)));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void IdleMeetsThreshold_Activates()
    {
        var monitor = A.Fake<IIdleMonitor>();
        var dto = new IdleRuleDto { SchemeGuid = Guid.NewGuid(), IdleTimeThreshold = TimeSpan.FromSeconds(10) };

        var rule = new IdleRule(monitor, dto);
        rule.StartRuling();

        monitor.IdleTimeChanged += Raise.With(new IdleTimeChangedEventArgs(TimeSpan.FromSeconds(10)));

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void IdleAboveThreshold_DoesNotExceedOne()
    {
        var monitor = A.Fake<IIdleMonitor>();
        var dto = new IdleRuleDto { SchemeGuid = Guid.NewGuid(), IdleTimeThreshold = TimeSpan.FromSeconds(10) };

        var rule = new IdleRule(monitor, dto);
        rule.StartRuling();

        monitor.IdleTimeChanged += Raise.With(new IdleTimeChangedEventArgs(TimeSpan.FromSeconds(20)));
        monitor.IdleTimeChanged += Raise.With(new IdleTimeChangedEventArgs(TimeSpan.FromSeconds(30)));

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void IdleDropsBelowThreshold_Deactivates()
    {
        var monitor = A.Fake<IIdleMonitor>();
        var dto = new IdleRuleDto { SchemeGuid = Guid.NewGuid(), IdleTimeThreshold = TimeSpan.FromSeconds(10) };

        var rule = new IdleRule(monitor, dto);
        rule.StartRuling();

        // Activate
        monitor.IdleTimeChanged += Raise.With(new IdleTimeChangedEventArgs(TimeSpan.FromSeconds(15)));
        Assert.AreEqual(1, rule.TriggerCount);

        // Deactivate
        monitor.IdleTimeChanged += Raise.With(new IdleTimeChangedEventArgs(TimeSpan.FromSeconds(5)));
        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void TriggerCountNeverNegative()
    {
        var monitor = A.Fake<IIdleMonitor>();
        var dto = new IdleRuleDto { SchemeGuid = Guid.NewGuid(), IdleTimeThreshold = TimeSpan.FromSeconds(10) };

        var rule = new IdleRule(monitor, dto);
        rule.StartRuling();

        monitor.IdleTimeChanged += Raise.With(new IdleTimeChangedEventArgs(TimeSpan.Zero));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void ExposesSchemeGuidFromDto()
    {
        var guid = Guid.NewGuid();
        var monitor = A.Fake<IIdleMonitor>();
        var dto = new IdleRuleDto { SchemeGuid = guid, IdleTimeThreshold = TimeSpan.FromSeconds(10) };

        var rule = new IdleRule(monitor, dto);
        rule.StartRuling();

        Assert.AreEqual(guid, rule.SchemeGuid);
    }

    [TestMethod]
    public void SubscribesToIdleMonitor()
    {
        var monitor = A.Fake<IIdleMonitor>();
        var dto = new IdleRuleDto { SchemeGuid = Guid.NewGuid(), IdleTimeThreshold = TimeSpan.FromSeconds(10) };

        var rule = new IdleRule(monitor, dto);
        rule.StartRuling();

        _ = A.CallTo(monitor)
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
