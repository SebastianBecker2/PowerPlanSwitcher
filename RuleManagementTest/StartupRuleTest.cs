namespace RuleManagementTest;

using System.Diagnostics;
using System.Threading;
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
    public void InitialTriggerCount_IsOneEvenWhenDelayConfiguredOnDto()
    {
        var dto = new StartupRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            Delay = TimeSpan.FromSeconds(1)
        };

        var rule = new StartupRule(dto);

        Assert.AreEqual(1, rule.TriggerCount, "Delay on the DTO is not applied by StartupRule.");
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

    [TestMethod]
    public void NoDuration_RuleStaysTriggered()
    {
        var dto = new StartupRuleDto { SchemeGuid = Guid.NewGuid(), Duration = null };
        var rule = new StartupRule(dto);

        rule.StartRuling();
        Thread.Sleep(500); // Wait to ensure timer would have fired
        rule.StopRuling();

        Assert.AreEqual(1, rule.TriggerCount, "Rule should remain triggered when no duration is set");
    }

    [TestMethod]
    [Timeout(5000, CooperativeCancellation = true)]
    public void WithDuration_RuleUntriggersAfterElapsed()
    {
        var dto = new StartupRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            Duration = TimeSpan.FromMilliseconds(200) // Very short duration for testing
        };
        var rule = new StartupRule(dto);

        rule.StartRuling();
        Assert.AreEqual(1, rule.TriggerCount);

        WaitUntil(
            () => rule.TriggerCount == 0,
            TimeSpan.FromSeconds(3),
            "Rule should be untriggered after duration elapses.");

        Assert.AreEqual(0, rule.TriggerCount, "Rule should be untriggered after duration elapses");
        rule.StopRuling();
    }

    [TestMethod]
    [Timeout(5000, CooperativeCancellation = true)]
    public void StopRuling_CancelsDurationTimer()
    {
        var dto = new StartupRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            Duration = TimeSpan.FromSeconds(10) // Long duration
        };
        var rule = new StartupRule(dto);

        rule.StartRuling();
        Assert.AreEqual(1, rule.TriggerCount);

        rule.StopRuling();

        // Wait a bit (but less than the 10-second duration)
        Thread.Sleep(200);

        // Rule should still be triggered (no auto-untrigger after StopRuling)
        Assert.AreEqual(1, rule.TriggerCount, "Rule should remain triggered after StopRuling without waiting for full duration");
    }

    [TestMethod]
    public void GetDescription_WithDurationSeconds()
    {
        var dto = new StartupRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            Duration = TimeSpan.FromSeconds(30)
        };

        var description = dto.GetDescription();

        Assert.AreEqual("Startup Rule (duration 30 seconds)", description);
    }

    [TestMethod]
    public void GetDescription_WithDurationMinutes()
    {
        var dto = new StartupRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            Duration = TimeSpan.FromMinutes(5)
        };

        var description = dto.GetDescription();

        Assert.AreEqual("Startup Rule (duration 5 minutes)", description);
    }

    [TestMethod]
    public void GetDescription_WithDurationHours()
    {
        var dto = new StartupRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            Duration = TimeSpan.FromHours(2)
        };

        var description = dto.GetDescription();

        Assert.AreEqual("Startup Rule (duration 2 hours)", description);
    }

    [TestMethod]
    public void GetDescription_WithDelayOnly()
    {
        var dto = new StartupRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            Delay = TimeSpan.FromSeconds(45)
        };

        Assert.AreEqual("Startup Rule (delay 45 seconds)", dto.GetDescription());
    }

    [TestMethod]
    public void GetDescription_WithDelayAndDuration()
    {
        var dto = new StartupRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            Delay = TimeSpan.FromMinutes(2),
            Duration = TimeSpan.FromMinutes(5)
        };

        Assert.AreEqual("Startup Rule (delay 2 minutes, duration 5 minutes)", dto.GetDescription());
    }

    [TestMethod]
    [Timeout(5000, CooperativeCancellation = true)]
    public void Dispose_StopsRule()
    {
        var dto = new StartupRuleDto
        {
            SchemeGuid = Guid.NewGuid(),
            Duration = TimeSpan.FromSeconds(10)
        };
        var rule = new StartupRule(dto);

        rule.StartRuling();
        rule.Dispose();

        // Should not throw and should clean up resources
        Assert.AreEqual(1, rule.TriggerCount);
    }

    private static void WaitUntil(Func<bool> condition, TimeSpan timeout, string timeoutMessage)
    {
        var sw = Stopwatch.StartNew();
        while (sw.Elapsed < timeout)
        {
            if (condition())
            {
                return;
            }

            Thread.Sleep(25);
        }

        Assert.Fail(timeoutMessage);
    }
}
