namespace RuleManagementTest;

using FakeItEasy;
using RuleManagement.Dto;
using RuleManagement.Rules;
using SystemManagement;

[TestClass]
public sealed class ShutdownRuleTest
{
    [TestMethod]
    public void InitialTriggerCount_IsZero()
    {
        var monitor = A.Fake<IWindowMessageMonitor>();
        var dto = new ShutdownRuleDto { SchemeGuid = Guid.NewGuid() };

        var rule = new ShutdownRule(monitor, dto);

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void QueryEndSession_SetsTriggerCountToOne()
    {
        var monitor = A.Fake<IWindowMessageMonitor>();
        var dto = new ShutdownRuleDto { SchemeGuid = Guid.NewGuid() };
        var rule = new ShutdownRule(monitor, dto);

        var args = new WindowMessageEventArgs(WindowMessage.QueryEndSession, 0, 0);

        monitor.WindowMessageReceived += Raise.With(args);

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void EndSessionWithZero_SetsTriggerCountToZero()
    {
        var monitor = A.Fake<IWindowMessageMonitor>();
        var dto = new ShutdownRuleDto { SchemeGuid = Guid.NewGuid() };
        var rule = new ShutdownRule(monitor, dto);

        // First simulate a shutdown request
        monitor.WindowMessageReceived += Raise.With(
            new WindowMessageEventArgs(WindowMessage.QueryEndSession, 0, 0));

        Assert.AreEqual(1, rule.TriggerCount);

        // Now simulate cancellation
        monitor.WindowMessageReceived += Raise.With(
            new WindowMessageEventArgs(WindowMessage.EndSession, 0, 0));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void EndSessionWithNonZero_DoesNotResetTriggerCount()
    {
        var monitor = A.Fake<IWindowMessageMonitor>();
        var dto = new ShutdownRuleDto { SchemeGuid = Guid.NewGuid() };
        var rule = new ShutdownRule(monitor, dto);

        // Trigger shutdown
        monitor.WindowMessageReceived += Raise.With(
            new WindowMessageEventArgs(WindowMessage.QueryEndSession, 0, 0));

        Assert.AreEqual(1, rule.TriggerCount);

        // Confirm shutdown (not cancelled)
        monitor.WindowMessageReceived += Raise.With(
            new WindowMessageEventArgs(WindowMessage.EndSession, 1, 0));

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void IgnoresOtherMessages()
    {
        var monitor = A.Fake<IWindowMessageMonitor>();
        var dto = new ShutdownRuleDto { SchemeGuid = Guid.NewGuid() };
        var rule = new ShutdownRule(monitor, dto);

        monitor.WindowMessageReceived += Raise.With(
            new WindowMessageEventArgs(WindowMessage.Close, 0, 0));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void ExposesSchemeGuidFromDto()
    {
        var guid = Guid.NewGuid();
        var monitor = A.Fake<IWindowMessageMonitor>();
        var dto = new ShutdownRuleDto { SchemeGuid = guid };

        var rule = new ShutdownRule(monitor, dto);

        Assert.AreEqual(guid, rule.SchemeGuid);
    }

    [TestMethod]
    public void GetDescription_ReturnsExpectedText()
    {
        var dto = new ShutdownRuleDto { SchemeGuid = Guid.NewGuid() };
        Assert.AreEqual("Shutdown Rule", dto.GetDescription());
    }

    [TestMethod]
    public void SubscribesToWindowMessageMonitor()
    {
        var monitor = A.Fake<IWindowMessageMonitor>();
        var dto = new ShutdownRuleDto { SchemeGuid = Guid.NewGuid() };

        _ = new ShutdownRule(monitor, dto);

        _ = A.CallTo(monitor)
            .Where(call => call.Method.Name == "add_WindowMessageReceived")
            .MustHaveHappenedOnceExactly();
    }
}
