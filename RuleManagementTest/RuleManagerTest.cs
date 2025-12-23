namespace RuleManagementTest;

using System.Reflection;
using FakeItEasy;
using PowerManagement;
using ProcessManagement;
using RuleManagement;
using RuleManagement.Dto;
using RuleManagement.Events;
using RuleManagement.Rules;

[TestClass]
public sealed class RuleManagerTest
{
    private IBatteryMonitor batteryMonitor = null!;
    private IProcessMonitor processMonitor = null!;
    private RuleFactory ruleFactory = null!;

    [TestInitialize]
    public void Setup()
    {
        batteryMonitor = A.Fake<IBatteryMonitor>();
        processMonitor = A.Fake<IProcessMonitor>();

        ruleFactory = new RuleFactory(batteryMonitor, processMonitor);
    }

    [TestMethod]
    public void RuleDtoVersion1_IsLoadedProperly()
    {
        var migrationPolicy = new MigrationPolicy(
            MigratedPowerRulesToRules: true,
            AcPowerSchemeGuid: CreateGuid('1'),
            BatterPowerSchemeGuid: CreateGuid('2')
        );

        var version1Json = /*lang=json,strict*/ @"
        {
            ""SchemaVersion"": 1,
            ""Rules"": [
                {
                    ""$type"": ""RuleManagement.Dto.PowerLineRuleDto, RuleManagement"",
                    ""PowerLineStatus"": 0,
                    ""SchemeGuid"": ""a1841308-3541-4fab-bc81-f71556f20b4a""
                },
                {
                    ""$type"": ""RuleManagement.Dto.ProcessRuleDto, RuleManagement"",
                    ""FilePath"": ""testpath"",
                    ""Type"": 0,
                    ""SchemeGuid"": ""381b4222-f694-41f0-9685-ff5bb260df2e""
                }
            ]
        }";

        var manager = new RuleManager(
            ruleFactory,
            version1Json,
            migrationPolicy,
            batteryMonitor);
        var rules = manager.GetRules().ToList();

        Assert.AreEqual(2, rules.Count);
        AssertRule(rules[0], new PowerLineRuleDto
        {
            PowerLineStatus = PowerLineStatus.Offline,
            SchemeGuid = Guid.Parse("a1841308-3541-4fab-bc81-f71556f20b4a"),
        });
        AssertRule(rules[1], new ProcessRuleDto
        {
            FilePath = "testpath",
            Type = ComparisonType.StartsWith,
            SchemeGuid = Guid.Parse("381b4222-f694-41f0-9685-ff5bb260df2e"),
        });
    }

    [TestMethod]
    public void LegacyJson_GetsMigrated()
    {
        _ = A.CallTo(() => batteryMonitor.HasSystemBattery).Returns(true);

        var migrationPolicy = new MigrationPolicy(
            MigratedPowerRulesToRules: false,
            AcPowerSchemeGuid: CreateGuid('1'),
            BatterPowerSchemeGuid: CreateGuid('2')
        );

        var legacyJson = /*lang=json,strict*/ @"
        [
          {
            ""FilePath"": ""testpath"",
            ""Type"": 2,
            ""SchemeGuid"": ""33333333-3333-3333-3333-333333333333""
          },
          {
            ""FilePath"": ""anotherpathtest"",
            ""Type"": 1,
            ""SchemeGuid"": ""44444444-4444-4444-4444-444444444444""
          }
        ]";

        var manager = new RuleManager(
            ruleFactory,
            legacyJson,
            migrationPolicy,
            batteryMonitor);
        var rules = manager.GetRules().ToList();

        Assert.AreEqual(4, rules.Count, "Migration adds 2 PowerLineRules to the initial 2 ProcessRules");
        AssertRule(rules[0], new ProcessRuleDto
        {
            FilePath = "testpath",
            Type = ComparisonType.EndsWith,
            SchemeGuid = CreateGuid('3'),
        });
        AssertRule(rules[1], new ProcessRuleDto
        {
            FilePath = "anotherpathtest",
            Type = ComparisonType.Exact,
            SchemeGuid = CreateGuid('4'),
        });
        AssertRule(rules[2], new PowerLineRuleDto
        {
            PowerLineStatus = PowerLineStatus.Online,
            SchemeGuid = CreateGuid('1'),
        });
        AssertRule(rules[3], new PowerLineRuleDto
        {
            PowerLineStatus = PowerLineStatus.Offline,
            SchemeGuid = CreateGuid('2'),
        });
    }

    [TestMethod]
    public void RuleDtoVersion0_IsLoadedProperly()
    {
        var migrationPolicy = new MigrationPolicy(
            MigratedPowerRulesToRules: true,
            AcPowerSchemeGuid: CreateGuid('1'),
            BatterPowerSchemeGuid: CreateGuid('2')
        );

        var legacyJson = /*lang=json,strict*/ @"
        [
          {
            ""$type"": ""PowerPlanSwitcher.RuleManagement.Rules.PowerLineRule, PowerPlanSwitcher"",
            ""SchemeGuid"": ""a1841308-3541-4fab-bc81-f71556f20b4a"",
            ""PowerLineStatus"": 1
          },
          {
            ""$type"": ""PowerPlanSwitcher.RuleManagement.Rules.ProcessRule, PowerPlanSwitcher"",
            ""SchemeGuid"": ""8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"",
            ""FilePath"": ""asdf"",
            ""Type"": 0
          }
        ]";

        var manager = new RuleManager(
            ruleFactory,
            legacyJson,
            migrationPolicy,
            batteryMonitor);
        var rules = manager.GetRules().ToList();

        Assert.AreEqual(2, rules.Count);
        AssertRule(rules[0], new PowerLineRuleDto
        {
            PowerLineStatus = PowerLineStatus.Online,
            SchemeGuid = Guid.Parse("a1841308-3541-4fab-bc81-f71556f20b4a"),
        });
        AssertRule(rules[1], new ProcessRuleDto
        {
            FilePath = "asdf",
            Type = ComparisonType.StartsWith,
            SchemeGuid = Guid.Parse("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"),
        });
    }

    [TestMethod]
    public void RoundTripSerialization_PreservesJson()
    {
        // Arrange
        var migrationPolicy = new MigrationPolicy(
            MigratedPowerRulesToRules: true,
            AcPowerSchemeGuid: CreateGuid('1'),
            BatterPowerSchemeGuid: CreateGuid('2')
        );

        var originalJson = /*lang=json,strict*/ @"
        {
            ""$type"": ""RuleManagement.RuleManager+RuleContainer, RuleManagement"",
            ""Rules"": [
                {
                    ""$type"": ""RuleManagement.Dto.ProcessRuleDto, RuleManagement"",
                    ""FilePath"": ""testpath"",
                    ""Type"": 0,
                    ""SchemeGuid"": ""381b4222-f694-41f0-9685-ff5bb260df2e""
                },
                {
                    ""$type"": ""RuleManagement.Dto.PowerLineRuleDto, RuleManagement"",
                    ""PowerLineStatus"": 1,
                    ""SchemeGuid"": ""11111111-1111-1111-1111-111111111111""
                }
            ],
            ""SchemaVersion"": 1
        }";

        var manager = new RuleManager(
            ruleFactory,
            originalJson,
            migrationPolicy,
            batteryMonitor);

        string? serializedFromEvent = null;
        manager.RulesUpdated += (s, e) => serializedFromEvent = e.Serialized;

        // Act
        var rules = manager.GetRules();
        manager.SetRules(rules); // triggers RulesUpdated

        // Assert
        Assert.IsNotNull(serializedFromEvent, "RulesUpdated event should fire");
        Assert.AreEqual(
            originalJson.Replace(" ", "").Replace("\n", "").Replace("\r", ""),
            serializedFromEvent.Replace(" ", "").Replace("\n", "").Replace("\r", ""),
            "Round-trip serialization should preserve JSON structure"
        );
    }

    [TestMethod]
    public void SetRules_FiresRulesUpdatedEvent()
    {
        // Arrange
        List<IRule> rules = [
            new PowerLineRule(
                batteryMonitor,
                new PowerLineRuleDto {
                    PowerLineStatus = PowerLineStatus.Online,
                    SchemeGuid = CreateGuid('1')
                }),
            new PowerLineRule(
                batteryMonitor,
                new PowerLineRuleDto {
                    PowerLineStatus = PowerLineStatus.Online,
                    SchemeGuid = CreateGuid('2')
                }),
            new ProcessRule(
                processMonitor,
                new ProcessRuleDto {
                    FilePath = "testPath",
                    Type = ComparisonType.StartsWith,
                    SchemeGuid = CreateGuid('3')
                }),
            new ProcessRule(
                processMonitor,
                new ProcessRuleDto {
                    FilePath = "anotherTestPath",
                    Type = ComparisonType.Exact,
                    SchemeGuid = CreateGuid('4')
                }),
            ];
        var manager = new RuleManager(ruleFactory);
        string? serializedFromEvent = null;
        manager.RulesUpdated += (s, e) => serializedFromEvent = e.Serialized;
        var migrationPolicy = new MigrationPolicy(
            MigratedPowerRulesToRules: true,
            AcPowerSchemeGuid: CreateGuid('5'),
            BatterPowerSchemeGuid: CreateGuid('6')
        );

        // Act
        manager.SetRules(rules);

        // Assert
        Assert.IsNotNull(serializedFromEvent, "RulesUpdated event should fire");

        manager = new RuleManager(
            ruleFactory,
            serializedFromEvent,
            migrationPolicy,
            batteryMonitor);
        var deserializedRules = manager.GetRules();

        Assert.AreEqual(rules.Count, deserializedRules.Count(), "Rule count should match after round-trip");

        for (var i = 0; i < rules.Count; i++)
        {
            var originalRule = rules[i];
            var deserializedRule = deserializedRules.Skip(i).First();

            Assert.AreEqual(originalRule.Dto.SchemeGuid, deserializedRule.Dto.SchemeGuid, $"Rule {i} SchemeGuid mismatch");
            switch (deserializedRule)
            {
                case PowerLineRule powerLineRule when originalRule.Dto is PowerLineRuleDto deserializedDto:
                    AssertRule(powerLineRule, deserializedDto);
                    break;

                case ProcessRule processRule when originalRule.Dto is ProcessRuleDto deserializedDto:
                    AssertRule(processRule, deserializedDto);
                    break;

                default:
                    Assert.Fail($"Unexpected DTO type at index {i}");
                    break;
            }
        }
    }

    [TestMethod]
    public void TriggeringRule_AppliesRuleAndFiresEvent()
    {
        // Arrange
        List<TestRule> rules = [
            CreateTestRule('1'),
            ];

        var manager = new RuleManager(ruleFactory);
        manager.SetRules(rules);

        IRule? appliedRuleFromEvent = null;
        manager.RuleApplicationChanged += (s, e) => appliedRuleFromEvent = e.Rule;

        // Act
        rules[0].TriggerCount = 1;

        // Assert
        Assert.IsNotNull(appliedRuleFromEvent, "RulesApplicationChanged should fire when rule is triggered");
        Assert.AreEqual(rules[0], appliedRuleFromEvent, "Applied rule should be the triggered PowerLineRule");
        Assert.AreEqual(rules[0], manager.AppliedRule, "RuleManager.AppliedRule should be updated");
    }

    [TestMethod]
    public void TriggeringHigherPriorityRule_ReplacesAppliedRule()
    {
        // Arrange
        List<TestRule> rules = [
            CreateTestRule('1'),
            CreateTestRule('2'),
            ];

        var manager = new RuleManager(ruleFactory);
        manager.SetRules(rules);

        IRule? appliedRuleFromEvent = null;
        manager.RuleApplicationChanged += (s, e) => appliedRuleFromEvent = e.Rule;

        // Act
        rules[1].TriggerCount = 1;
        rules[0].TriggerCount = 1;

        // Assert
        Assert.IsNotNull(appliedRuleFromEvent, "RulesApplicationChanged should fire when rule is triggered");
        Assert.AreEqual(rules[0], appliedRuleFromEvent, "Applied rule should be the triggered PowerLineRule");
        Assert.AreEqual(rules[0], manager.AppliedRule, "RuleManager.AppliedRule should be updated");
    }

    [TestMethod]
    public void ReTriggeringAppliedRule_DoesNotFireEvent()
    {
        // Arrange
        List<TestRule> rules = [
            CreateTestRule('1'),
            ];

        var manager = new RuleManager(ruleFactory);
        manager.SetRules(rules);

        var eventFireCount = 0;
        IRule? appliedRuleFromEvent = null;
        manager.RuleApplicationChanged += (s, e) =>
        {
            eventFireCount++;
            appliedRuleFromEvent = e.Rule;
        };

        // Act
        rules[0].TriggerCount = 1; // first trigger
        rules[0].TriggerCount = 2; // re-trigger same rule

        // Assert
        Assert.IsNotNull(appliedRuleFromEvent, "RulesApplicationChanged should fire when rule is triggered");
        Assert.AreEqual(1, eventFireCount, "RuleApplicationChanged should fire only once for the same rule");
        Assert.AreEqual(rules[0], appliedRuleFromEvent, "Applied rule should be the triggered rule");
        Assert.AreEqual(rules[0], manager.AppliedRule, "RuleManager.AppliedRule should be updated");
    }

    [TestMethod]
    public void UntriggeringAppliedRule_FallsBackToNextTriggeredRule()
    {
        // Arrange
        List<TestRule> rules = [
            CreateTestRule('1'),
            CreateTestRule('2'),
            ];

        var manager = new RuleManager(ruleFactory);
        manager.SetRules(rules);

        List<IRule?> appliedRules = [];
        manager.RuleApplicationChanged += (s, e) => appliedRules.Add(e.Rule);

        // Act
        rules[1].TriggerCount = 1; // apply rule[1]
        rules[0].TriggerCount = 1; // apply rule[0] (higher priority)
        rules[0].TriggerCount = 0; // untrigger rule[0], should fall back to rule[1]

        // Assert
        Assert.AreEqual(3, appliedRules.Count);
        Assert.AreEqual(rules[1], appliedRules[0], "First applied rule should be rules[1]");
        Assert.AreEqual(rules[0], appliedRules[1], "Second applied rule should be rules[0]");
        Assert.AreEqual(rules[1], appliedRules[2], "Third applied rule should fall back to rules[1]");
        Assert.AreEqual(rules[1], manager.AppliedRule, "Final applied rule should be rules[1]");
    }

    [TestMethod]
    public void NoRulesTriggered_AppliedRuleIsNull()
    {
        // Arrange
        var rules = new List<TestRule>
        {
            CreateTestRule('1'),
            CreateTestRule('2'),
        };

        var manager = new RuleManager(ruleFactory);
        manager.SetRules(rules);

        var eventFireCount = 0;
        manager.RuleApplicationChanged += (s, e) => eventFireCount++;

        // Act
        // Do not trigger any rules (TriggerCount stays at 0 for all)

        // Assert
        Assert.IsNull(manager.AppliedRule, "AppliedRule should remain null when no rules are triggered");
        Assert.AreEqual(0, eventFireCount, "RuleApplicationChanged should not fire when no rules are triggered");
    }

    [TestMethod]
    public void EmptyRuleSet_ReturnsEmptyRules()
    {
        // Arrange
        var manager = new RuleManager(ruleFactory);
        manager.SetRules(Array.Empty<IRule>());

        // Act
        var rules = manager.GetRules();

        // Assert
        Assert.IsNotNull(rules, "GetRules should not return null");
        Assert.AreEqual(0, rules.Count(), "GetRules should return an empty collection when no rules are set");
        Assert.IsNull(manager.AppliedRule, "AppliedRule should remain null when no rules exist");
    }

    [TestMethod]
    public void UnsupportedSchemaVersion_ThrowsException()
    {
        // Arrange: JSON with schema version 99 (unsupported)
        var invalidJson = /*lang=json,strict*/ @"
        {
            ""SchemaVersion"": 99,
            ""Rules"": []
        }";

        var migrationPolicy = new MigrationPolicy(
            MigratedPowerRulesToRules: true,
            AcPowerSchemeGuid: Guid.Empty,
            BatterPowerSchemeGuid: Guid.Empty);

        // Act & Assert
        _ = Assert.ThrowsException<NotSupportedException>(() =>
            new RuleManager(ruleFactory, invalidJson, migrationPolicy, batteryMonitor));
    }

    [TestMethod]
    public void UntriggeringLastAppliedRule_RaisesEventWithNull()
    {
        // Arrange
        var rules = new List<TestRule> { CreateTestRule('1') };

        var manager = new RuleManager(ruleFactory);
        manager.SetRules(rules);

        List<IRule?> appliedRules = [];
        manager.RuleApplicationChanged += (s, e) => appliedRules.Add(e.Rule);

        // Act
        rules[0].TriggerCount = 1; // apply the rule
        rules[0].TriggerCount = 0; // untrigger it, no other rules active

        // Assert
        Assert.AreEqual(2, appliedRules.Count, "Event should fire twice: once for applying, once for clearing");
        Assert.AreEqual(rules[0], appliedRules[0], "First event should apply the rule");
        Assert.IsNull(appliedRules[1], "Second event should clear the applied rule (null)");
        Assert.IsNull(manager.AppliedRule, "Final AppliedRule should be null");
    }

    [TestMethod]
    public void TriggeringLowerPriorityRule_WhenNoHigherPriorityTriggered_AppliesRule()
    {
        // Arrange
        var rules = new List<TestRule>
        {
            CreateTestRule('1'), // higher priority (index 0)
            CreateTestRule('2'), // lower priority (index 1)
        };

        var manager = new RuleManager(ruleFactory);
        manager.SetRules(rules);

        List<IRule?> appliedRules = [];
        manager.RuleApplicationChanged += (s, e) => appliedRules.Add(e.Rule);

        // Act
        rules[1].TriggerCount = 1; // trigger only the lower-priority rule

        // Assert
        Assert.AreEqual(1, appliedRules.Count, "Event should fire once");
        Assert.AreEqual(rules[1], appliedRules[0], "Applied rule should be the lower-priority rule");
        Assert.AreEqual(rules[1], manager.AppliedRule, "Manager.AppliedRule should be updated to the lower-priority rule");
    }

    [TestMethod]
    public void TriggeringLowerPriorityRule_WhenHigherPriorityAlreadyApplied_DoesNotFireEvent()
    {
        // Arrange
        var rules = new List<TestRule>
        {
            CreateTestRule('1'), // higher priority (index 0)
            CreateTestRule('2'), // lower priority (index 1)
        };

        var manager = new RuleManager(ruleFactory);
        manager.SetRules(rules);

        var eventFireCount = 0;
        List<IRule?> appliedRules = [];
        manager.RuleApplicationChanged += (s, e) =>
        {
            eventFireCount++;
            appliedRules.Add(e.Rule);
        };

        // Act
        rules[0].TriggerCount = 1; // trigger higher-priority rule
        rules[1].TriggerCount = 1; // trigger lower-priority rule afterwards

        // Assert
        Assert.AreEqual(1, eventFireCount, "Event should fire only once for the higher-priority rule");
        Assert.AreEqual(rules[0], appliedRules[0], "Applied rule should remain the higher-priority rule");
        Assert.AreEqual(rules[0], manager.AppliedRule, "Manager.AppliedRule should still be the higher-priority rule");
    }

    [TestMethod]
    public void TriggerChanged_WithNonRuleSender_Throws()
    {
        // Arrange
        var manager = new RuleManager(ruleFactory);
        var method = typeof(RuleManager)
            .GetMethod("Rule_TriggerChanged", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(method, "Rule_TriggerChanged should exist");

        // Use a real IRule for event args, but a non-IRule sender
        var rule = CreateTestRule('1');
        var args = new TriggerChangedEventArgs(rule);

        // Act
        try
        {
            _ = method.Invoke(manager, [new object(), args]);
            Assert.Fail("Expected InvalidCastException was not thrown.");
        }
        catch (TargetInvocationException tie)
        {
            // Assert
            Assert.IsInstanceOfType(tie.InnerException, typeof(InvalidCastException),
                "InnerException should be InvalidCastException when sender is not IRule.");
        }
    }

    [TestMethod]
    public void CallingSetRules_BehavesConsistent_ForAllOverloads()
    {
        // Arrange
        List<IRule> rules = [
            new PowerLineRule(
                batteryMonitor,
                new PowerLineRuleDto {
                    PowerLineStatus = PowerLineStatus.Online,
                    SchemeGuid = CreateGuid('1')
                }),
            new PowerLineRule(
                batteryMonitor,
                new PowerLineRuleDto {
                    PowerLineStatus = PowerLineStatus.Online,
                    SchemeGuid = CreateGuid('2')
                }),
            new ProcessRule(
                processMonitor,
                new ProcessRuleDto {
                    FilePath = "testPath",
                    Type = ComparisonType.StartsWith,
                    SchemeGuid = CreateGuid('3')
                }),
            new ProcessRule(
                processMonitor,
                new ProcessRuleDto {
                    FilePath = "anotherTestPath",
                    Type = ComparisonType.Exact,
                    SchemeGuid = CreateGuid('4')
                }),
            ];
        var manager = new RuleManager(ruleFactory);
        List<string> serializedFromEvent = [];
        manager.RulesUpdated += (s, e) => serializedFromEvent.Add(e.Serialized);
        var migrationPolicy = new MigrationPolicy(
            MigratedPowerRulesToRules: true,
            AcPowerSchemeGuid: CreateGuid('5'),
            BatterPowerSchemeGuid: CreateGuid('6')
        );

        // Act
        manager.SetRules(rules);
        manager.SetRules(rules.Select(r => r.Dto));

        // Assert
        Assert.AreEqual(2, serializedFromEvent.Count);
        Assert.IsFalse(string.IsNullOrEmpty(serializedFromEvent[0]));
        Assert.AreEqual(serializedFromEvent[0], serializedFromEvent[1]);
    }

    private static void AssertRule(IRule rule, PowerLineRuleDto dto)
    {
        Assert.IsInstanceOfType(rule, typeof(PowerLineRule));
        Assert.AreEqual(dto.SchemeGuid, ((PowerLineRule)rule).SchemeGuid);
        Assert.AreEqual(dto.PowerLineStatus, ((PowerLineRule)rule).PowerLineStatus);
    }

    private static void AssertRule(IRule rule, ProcessRuleDto dto)
    {
        Assert.IsInstanceOfType(rule, typeof(ProcessRule));
        Assert.AreEqual(dto.SchemeGuid, ((ProcessRule)rule).SchemeGuid);
        Assert.AreEqual(dto.FilePath, ((ProcessRule)rule).FilePath);
        Assert.AreEqual(dto.Type, ((ProcessRule)rule).Type);
    }

    private static Guid CreateGuid(char c)
    {
        // Build a string of the form "cccccccc-cccc-cccc-cccc-cccccccccccc"
        var hex = new string(c, 8) + "-" +
                     new string(c, 4) + "-" +
                     new string(c, 4) + "-" +
                     new string(c, 4) + "-" +
                     new string(c, 12);

        return Guid.Parse(hex);
    }

    private static TestRule CreateTestRule(char c) =>
        new(new TestRuleDto { SchemeGuid = CreateGuid(c) });
}
