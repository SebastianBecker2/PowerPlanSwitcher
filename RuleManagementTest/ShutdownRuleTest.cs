namespace RuleManagementTest;

using FakeItEasy;
using RuleManagement.Dto;
using RuleManagement.Rules;
using SystemManagement;

[TestClass]
public sealed class ShutdownRuleTest
{
    private IWindowMessageMonitor monitor = null!;

    [TestInitialize]
    public void Setup() => monitor = A.Fake<IWindowMessageMonitor>();

    [TestMethod]
    public void InitialTriggerCount_IsZero()
    {
        var dto = new ShutdownRuleDto
        {
            SchemeGuid = Guid.NewGuid()
        };
        var rule = new ShutdownRule(monitor, dto);
        rule.StartRuling();

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void QueryEndSession_SetsTriggerCountToOne()
    {
        var dto = new ShutdownRuleDto
        {
            SchemeGuid = Guid.NewGuid()
        };
        var rule = new ShutdownRule(monitor, dto);
        rule.StartRuling();

        var args = new WindowMessageEventArgs(WindowMessage.QueryEndSession, 0, 0);

        monitor.WindowMessageReceived += Raise.With(args);

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void EndSessionWithZero_SetsTriggerCountToZero()
    {
        var dto = new ShutdownRuleDto
        {
            SchemeGuid = Guid.NewGuid()
        };
        var rule = new ShutdownRule(monitor, dto);
        rule.StartRuling();

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
        var dto = new ShutdownRuleDto
        {
            SchemeGuid = Guid.NewGuid()
        };
        var rule = new ShutdownRule(monitor, dto);
        rule.StartRuling();

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
        var dto = new ShutdownRuleDto
        {
            SchemeGuid = Guid.NewGuid()
        };
        var rule = new ShutdownRule(monitor, dto);
        rule.StartRuling();

        monitor.WindowMessageReceived += Raise.With(
            new WindowMessageEventArgs(WindowMessage.Close, 0, 0));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void ExposesSchemeGuidFromDto()
    {
        var guid = Guid.NewGuid();
        var dto = new ShutdownRuleDto
        {
            SchemeGuid = guid
        };
        var rule = new ShutdownRule(monitor, dto);
        rule.StartRuling();

        Assert.AreEqual(guid, rule.SchemeGuid);
    }

    [TestMethod]
    public void GetDescription_ReturnsExpectedText()
    {
        var dto = new ShutdownRuleDto
        {
            SchemeGuid = Guid.NewGuid()
        };
        Assert.AreEqual("Shutdown Rule", dto.GetDescription());
    }

    [TestMethod]
    public void SubscribesToWindowMessageMonitor()
    {
        var dto = new ShutdownRuleDto
        {
            SchemeGuid = Guid.NewGuid()
        };
        var rule = new ShutdownRule(monitor, dto);
        rule.StartRuling();

        _ = A.CallTo(monitor)
            .Where(call => call.Method.Name == "add_WindowMessageReceived")
            .MustHaveHappenedOnceExactly();
    }
}
