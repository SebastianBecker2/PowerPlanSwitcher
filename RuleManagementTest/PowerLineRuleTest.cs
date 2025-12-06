namespace RuleManagementTest;

using System.Reflection;
using FakeItEasy;
using PowerManagement;
using RuleManagement.Rules;

[TestClass]
public sealed class PowerLineRuleTest
{
    private IBatteryMonitor batteryMonitor = null!;

    [TestInitialize]
    public void Setup() => batteryMonitor = A.Fake<IBatteryMonitor>();

    [TestMethod]
    public void BatteryMonitorEvent_StatusMatches_IncrementsTriggerCount()
    {
        // Arrange
        var dto = new PowerLineRuleDto { SchemeGuid = Guid.NewGuid(), PowerLineStatus = PowerLineStatus.Online };
        var rule = new PowerLineRule(batteryMonitor, dto);

        // Act
        batteryMonitor.PowerLineStatusChanged += Raise.With(new PowerLineStatusChangedEventArgs(PowerLineStatus.Online));

        // Assert
        Assert.AreEqual(1, rule.TriggerCount, "TriggerCount should increment when status matches");
    }

    [TestMethod]
    public void BatteryMonitorEvent_StatusDoesNotMatch_DecrementsTriggerCount()
    {
        // Arrange
        var dto = new PowerLineRuleDto { SchemeGuid = Guid.NewGuid(), PowerLineStatus = PowerLineStatus.Online };
        var rule = new PowerLineRule(batteryMonitor, dto);

        // First increment
        batteryMonitor.PowerLineStatusChanged += Raise.With(new PowerLineStatusChangedEventArgs(PowerLineStatus.Online));
        Assert.AreEqual(1, rule.TriggerCount);

        // Act: send non-matching status
        batteryMonitor.PowerLineStatusChanged += Raise.With(new PowerLineStatusChangedEventArgs(PowerLineStatus.Offline));

        // Assert
        Assert.AreEqual(0, rule.TriggerCount, "TriggerCount should decrement when status does not match");
    }

    [TestMethod]
    public void BatteryMonitorEvent_StatusDoesNotMatch_TriggerCountNeverNegative()
    {
        // Arrange
        var dto = new PowerLineRuleDto { SchemeGuid = Guid.NewGuid(), PowerLineStatus = PowerLineStatus.Online };
        var rule = new PowerLineRule(batteryMonitor, dto);

        // Act: send non-matching status multiple times
        batteryMonitor.PowerLineStatusChanged += Raise.With(new PowerLineStatusChangedEventArgs(PowerLineStatus.Offline));
        batteryMonitor.PowerLineStatusChanged += Raise.With(new PowerLineStatusChangedEventArgs(PowerLineStatus.Offline));

        // Assert
        Assert.AreEqual(0, rule.TriggerCount, "TriggerCount should not go below zero");
    }

    [TestMethod]
    public void GetDescription_ReturnsExpectedText()
    {
        // Arrange
        var dto = new PowerLineRuleDto { SchemeGuid = Guid.NewGuid(), PowerLineStatus = PowerLineStatus.Offline };
        var rule = new PowerLineRule(batteryMonitor, dto);

        // Act
        var description = rule.GetDescription();

        // Assert
        Assert.AreEqual("Power Line -> On battery", description);
    }

    [TestMethod]
    public void TextToPowerLineStatus_InvalidText_Throws()
    {
        // Arrange
        var method = typeof(PowerLineRule)
            .GetMethod("TextToPowerLineStatus", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.IsNotNull(method);

        // Act & Assert
        _ = Assert.ThrowsException<TargetInvocationException>(() =>
            method.Invoke(null, ["Not a valid status"]));
    }

    [TestMethod]
    public void PowerLineStatusToText_UnknownStatus_ReturnsEmptyString()
    {
        // Arrange
        var method = typeof(PowerLineRule)
            .GetMethod("PowerLineStatusToText", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.IsNotNull(method);

        // Act
        var result = method.Invoke(null, [(PowerLineStatus)999]);

        // Assert
        Assert.AreEqual(string.Empty, result);
    }
}
