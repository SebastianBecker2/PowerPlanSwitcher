namespace RuleManagementTest;

using System.ComponentModel;
using System.Reflection;
using FakeItEasy;
using ProcessManagement;
using RuleManagement;
using RuleManagement.Dto;
using RuleManagement.Rules;

[TestClass]
public sealed class ProcessRuleTest
{
    private IProcessMonitor processMonitor = null!;

    [TestInitialize]
    public void Setup() => processMonitor = A.Fake<IProcessMonitor>();

    [TestMethod]
    public void InitialTriggerCount_IsZero()
    {
        var monitor = A.Fake<IProcessMonitor>();
        var dto = new ProcessRuleDto { SchemeGuid = Guid.NewGuid() };

        var rule = new ProcessRule(monitor, dto);

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void ProcessCreated_PathMatchesExact_IncrementsTriggerCount()
    {
        // Arrange
        var dto = new ProcessRuleDto { Pattern = "test.exe", Type = ComparisonType.Exact, SchemeGuid = Guid.NewGuid() };
        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns("test.exe");

        // Act
        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        // Assert
        Assert.AreEqual(1, rule.TriggerCount, "TriggerCount should increment when path matches exactly");
    }

    [TestMethod]
    public void ProcessCreated_PathDoesNotMatch_DoesNotIncrementTriggerCount()
    {
        var dto = new ProcessRuleDto { Pattern = "test.exe", Type = ComparisonType.Exact, SchemeGuid = Guid.NewGuid() };
        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns("other.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(0, rule.TriggerCount, "TriggerCount should remain zero when path does not match");
    }

    [TestMethod]
    public void ProcessTerminated_PathMatches_DecrementsTriggerCount()
    {
        var dto = new ProcessRuleDto { Pattern = "test.exe", Type = ComparisonType.Exact, SchemeGuid = Guid.NewGuid() };
        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns("test.exe");

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
        var dto = new ProcessRuleDto { Pattern = "test.exe", Type = ComparisonType.Exact, SchemeGuid = Guid.NewGuid() };
        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns("test.exe");

        // Act: terminate without create
        processMonitor.ProcessTerminated += Raise.With(new ProcessEventArgs(process));

        // Assert
        Assert.AreEqual(0, rule.TriggerCount, "TriggerCount should not go below zero");
    }

    [TestMethod]
    public void ProcessCreated_WildcardMatch_IncrementsTriggerCount()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = "C:\\Games\\*.exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns("C:\\Games\\MyGame.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void ProcessCreated_WildcardDoesNotMatch_DoesNotIncrement()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = "C:\\Games\\*.exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns("C:\\Other\\App.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void ProcessTerminated_WildcardMatch_DecrementsTriggerCount()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = "C:\\Games\\*.exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns("C:\\Games\\MyGame.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));
        Assert.AreEqual(1, rule.TriggerCount);

        processMonitor.ProcessTerminated += Raise.With(new ProcessEventArgs(process));
        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void ProcessTerminated_WildcardMatch_TriggerCountNeverNegative()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = "*.exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns("test.exe");

        processMonitor.ProcessTerminated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void Wildcard_Matches_Globstar()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = @"C:\GAMES\**\*.EXE",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath)
            .Returns(@"C:\GAMES\FIRSTGAME\BINARIES\MYGAME.EXE");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void Wildcard_SingleStar_DoesNotCrossDirectories()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = @"C:\Games\*.exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath)
            .Returns(@"C:\Games\Sub\Game.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void Wildcard_Globstar_MatchesZeroSegments()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = @"C:\Games\**\*.exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath)
            .Returns(@"C:\Games\Game.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void Wildcard_Globstar_MatchesDeepDirectories()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = @"C:\Games\**\*.exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath)
            .Returns(@"C:\Games\A\B\C\Game.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void Wildcard_MixedLiteralAndGlobstar_Matches()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = @"C:\Games\**\*.exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath)
            .Returns(@"C:\Games\FirstGame\Binaries\MyGame.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void Wildcard_MixedSingleCharacterWildcard_Matches()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = @"C:\Games\My?ame.exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns(@"C:\Games\MyGame.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void Wildcard_CharacterClass_Matches()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = @"C:\Games\Game[0-9].exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath)
            .Returns(@"C:\Games\Game5.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void CheckRule_ExecutablePathEmpty_ReturnsFalse()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = "*.exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns("");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void CheckRule_ExecutablePathWhitespace_ReturnsFalse()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = "*.exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns("   ");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void MultipleMatchingProcesses_TriggerCountAccumulates()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = "*.exe",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var p1 = A.Fake<IProcess>();
        var p2 = A.Fake<IProcess>();

        _ = A.CallTo(() => p1.ExecutablePath).Returns("a.exe");
        _ = A.CallTo(() => p2.ExecutablePath).Returns("b.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(p1));
        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(p2));

        Assert.AreEqual(2, rule.TriggerCount);

        processMonitor.ProcessTerminated += Raise.With(new ProcessEventArgs(p1));

        Assert.AreEqual(1, rule.TriggerCount);
    }

    [TestMethod]
    public void Wildcard_InvalidPattern_DoesNotThrowAndNeverMatches()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = @"C:\Games\**[",
            Type = ComparisonType.Wildcard,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns(@"C:\Games\Game.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void StartsWith_PatternLongerThanPath_DoesNotMatch()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = @"C:\Games\Longer",
            Type = ComparisonType.StartsWith,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns(@"C:\Games");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void EndsWith_PatternLongerThanPath_DoesNotMatch()
    {
        var dto = new ProcessRuleDto
        {
            Pattern = @"Longer.exe",
            Type = ComparisonType.EndsWith,
            SchemeGuid = Guid.NewGuid()
        };

        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Returns(@"Game.exe");

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(0, rule.TriggerCount);
    }

    [TestMethod]
    public void GetDescription_ReturnsExpectedText()
    {
        var dto = new ProcessRuleDto { Pattern = "test.exe", Type = ComparisonType.StartsWith, SchemeGuid = Guid.NewGuid() };

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
        var dto = new ProcessRuleDto { Pattern = "test.exe", Type = ComparisonType.Exact, SchemeGuid = Guid.NewGuid() };
        var rule = new ProcessRule(processMonitor, dto);

        var process = A.Fake<IProcess>();
        _ = A.CallTo(() => process.ExecutablePath).Throws(new Win32Exception());

        processMonitor.ProcessCreated += Raise.With(new ProcessEventArgs(process));

        Assert.AreEqual(0, rule.TriggerCount, "TriggerCount should remain zero when Win32Exception is thrown");
    }
}
