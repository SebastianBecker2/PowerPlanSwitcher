namespace RuleManagementTest;

using System.ComponentModel;
using System.Reflection;
using FakeItEasy;
using ProcessManagement;
using RuleManagement;
using RuleManagement.Rules;

[TestClass]
public sealed class ProcessRuleTest
{
    private IProcessMonitor processMonitor = null!;

    [TestInitialize]
    public void Setup() => processMonitor = A.Fake<IProcessMonitor>();

    [TestMethod]
    public void ProcessCreated_PathMatchesExact_IncrementsTriggerCount()
    {
        // Arrange
        var dto = new ProcessRuleDto { FilePath = "test.exe", Type = ComparisonType.Exact, SchemeGuid = Guid.NewGuid() };
        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        A.CallTo(() => process.ExecutablePath).Returns("test.exe");

        // Act
        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        // Assert
        Assert.AreEqual(1, rule.TriggerCount, "TriggerCount should increment when path matches exactly");
    }

    [TestMethod]
    public void ProcessCreated_PathDoesNotMatch_DoesNotIncrementTriggerCount()
    {
        var dto = new ProcessRuleDto { FilePath = "test.exe", Type = ComparisonType.Exact, SchemeGuid = Guid.NewGuid() };
        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        A.CallTo(() => process.ExecutablePath).Returns("other.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(0, rule.TriggerCount, "TriggerCount should remain zero when path does not match");
    }

    [TestMethod]
    public void ProcessTerminated_PathMatches_DecrementsTriggerCount()
    {
        var dto = new ProcessRuleDto { FilePath = "test.exe", Type = ComparisonType.Exact, SchemeGuid = Guid.NewGuid() };
        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        A.CallTo(() => process.ExecutablePath).Returns("test.exe");

        // Increment first
        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));
        Assert.AreEqual(1, rule.TriggerCount);

        // Act: terminate
        processMonitor.ProcessTerminated += Raise.With(new ProcessEventArgs(process));

        // Assert
        Assert.AreEqual(0, rule.TriggerCount, "TriggerCount should decrement when matching process terminates");
    }

    [TestMethod]
    public void ProcessTerminated_PathMatches_TriggerCountNeverNegative()
    {
        var dto = new ProcessRuleDto { FilePath = "test.exe", Type = ComparisonType.Exact, SchemeGuid = Guid.NewGuid() };
        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        A.CallTo(() => process.ExecutablePath).Returns("test.exe");

        // Act: terminate without create
        processMonitor.ProcessTerminated += Raise.With(new ProcessEventArgs(process));

        // Assert
        Assert.AreEqual(0, rule.TriggerCount, "TriggerCount should not go below zero");
    }

    [TestMethod]
    public void GetDescription_ReturnsExpectedText()
    {
        var dto = new ProcessRuleDto { FilePath = "test.exe", Type = ComparisonType.StartsWith, SchemeGuid = Guid.NewGuid() };

        var description = dto.GetDescription();

        Assert.AreEqual("Process -> Path starts with -> test.exe", description);
    }

    [TestMethod]
    public void ComparisonTypeToText_UnknownType_ReturnsEmptyString()
    {
        var method = typeof(ProcessRuleDto)
            .GetMethod("ComparisonTypeToText", BindingFlags.Public | BindingFlags.Static);
        Assert.IsNotNull(method);

        var result = method.Invoke(null, [(ComparisonType)999]);

        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void TextToComparisonType_InvalidText_Throws()
    {
        var method = typeof(ProcessRuleDto)
            .GetMethod("TextToComparisonType", BindingFlags.Public | BindingFlags.Static);
        Assert.IsNotNull(method);

        try
        {
            _ = method.Invoke(null, ["Not a valid type"]);
            Assert.Fail("Expected InvalidOperationException was not thrown.");
        }
        catch (TargetInvocationException tie)
        {
            Assert.IsInstanceOfType(tie.InnerException, typeof(InvalidOperationException));
        }
    }

    [TestMethod]
    public void CheckRule_Win32Exception_ReturnsFalse()
    {
        var processMonitor = A.Fake<IProcessMonitor>();
        var dto = new ProcessRuleDto { FilePath = "test.exe", Type = ComparisonType.Exact, SchemeGuid = Guid.NewGuid() };
        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        A.CallTo(() => process.ExecutablePath).Throws(new Win32Exception());

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(0, rule.TriggerCount, "TriggerCount should remain zero when Win32Exception is thrown");
    }
}
