namespace RuleManagementTest;

using FakeItEasy;
using PowerManagement;
using RuleManagement;
using RuleManagement.Rules;

[TestClass]
public sealed class RuleManagerTest
{
    private IBatteryMonitor batteryMonitor = null!;
    private RuleFactory ruleFactory = null!;

    [TestInitialize]
    public void Setup()
    {
        batteryMonitor = A.Fake<IBatteryMonitor>();
        ruleFactory = A.Fake<RuleFactory>();
    }

    [TestMethod]
    public void RuleDtoVersion1_IsLoadedProperly()
    {
        var migrationPolicy = new MigrationPolicy(
            MigratedPowerRulesToRules: true,
            AcPowerSchemeGuid: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            BatterPowerSchemeGuid: Guid.Parse("22222222-2222-2222-2222-222222222222")
        );

        var version1Json = /*lang=json,strict*/ @"
        {
            ""SchemaVersion"": 1,
            ""Rules"": [
                {
                    ""$type"": ""RuleManagement.Rules.PowerLineRuleDto, RuleManagement"",
                    ""PowerLineStatus"": 0,
                    ""Index"": 0,
                    ""SchemeGuid"": ""a1841308-3541-4fab-bc81-f71556f20b4a""
                },
                {
                    ""$type"": ""RuleManagement.Rules.ProcessRuleDto, RuleManagement"",
                    ""FilePath"": ""testpath"",
                    ""Type"": 0,
                    ""Index"": 0,
                    ""SchemeGuid"": ""381b4222-f694-41f0-9685-ff5bb260df2e""
                }
            ]
        }";

        var manager = new RuleManager(version1Json, migrationPolicy, batteryMonitor, ruleFactory);
        var rules = manager.GetRules().ToList();

        Assert.AreEqual(2, rules.Count);
        AssertRule(rules[0], new PowerLineRuleDto
        {
            Index = 0,
            PowerLineStatus = PowerLineStatus.Offline,
            SchemeGuid = Guid.Parse("a1841308-3541-4fab-bc81-f71556f20b4a"),
        });
        AssertRule(rules[1], new ProcessRuleDto
        {
            Index = 0,
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
            AcPowerSchemeGuid: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            BatterPowerSchemeGuid: Guid.Parse("22222222-2222-2222-2222-222222222222")
        );

        var legacyJson = /*lang=json,strict*/ @"
        [
          {
            ""Index"": 0,
            ""FilePath"": ""testpath"",
            ""Type"": 2,
            ""SchemeGuid"": ""381b4222-f694-41f0-9685-ff5bb260df2e""
          },
          {
            ""Index"": 1,
            ""FilePath"": ""anotherpathtest"",
            ""Type"": 1,
            ""SchemeGuid"": ""8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c""
          }
        ]";

        var manager = new RuleManager(legacyJson, migrationPolicy, batteryMonitor, ruleFactory);
        var rules = manager.GetRules().ToList();

        Assert.AreEqual(4, rules.Count, "Migration adds 2 PowerLineRules to the initial 2 ProcessRules");
        AssertRule(rules[0], new ProcessRuleDto
        {
            Index = 0,
            FilePath = "testpath",
            Type = ComparisonType.EndsWith,
            SchemeGuid = Guid.Parse("381b4222-f694-41f0-9685-ff5bb260df2e"),
        });
        AssertRule(rules[1], new ProcessRuleDto
        {
            Index = 1,
            FilePath = "anotherpathtest",
            Type = ComparisonType.Exact,
            SchemeGuid = Guid.Parse("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"),
        });
        AssertRule(rules[2], new PowerLineRuleDto
        {
            Index = 2,
            PowerLineStatus = PowerLineStatus.Online,
            SchemeGuid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        });
        AssertRule(rules[3], new PowerLineRuleDto
        {
            Index = 3,
            PowerLineStatus = PowerLineStatus.Offline,
            SchemeGuid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        });
    }

    [TestMethod]
    public void RuleDtoVersion0_IsLoadedProperly()
    {
        var migrationPolicy = new MigrationPolicy(
            MigratedPowerRulesToRules: true,
            AcPowerSchemeGuid: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            BatterPowerSchemeGuid: Guid.Parse("22222222-2222-2222-2222-222222222222")
        );

        var legacyJson = /*lang=json,strict*/ @"
        [
          {
            ""$type"": ""PowerPlanSwitcher.RuleManagement.Rules.PowerLineRule, PowerPlanSwitcher"",
            ""Index"": 0,
            ""SchemeGuid"": ""a1841308-3541-4fab-bc81-f71556f20b4a"",
            ""PowerLineStatus"": 1
          },
          {
            ""$type"": ""PowerPlanSwitcher.RuleManagement.Rules.ProcessRule, PowerPlanSwitcher"",
            ""Index"": 1,
            ""SchemeGuid"": ""8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"",
            ""FilePath"": ""asdf"",
            ""Type"": 0
          }
        ]";

        var manager = new RuleManager(legacyJson, migrationPolicy, batteryMonitor, ruleFactory);
        var rules = manager.GetRules().ToList();

        Assert.AreEqual(2, rules.Count);
        AssertRule(rules[0], new PowerLineRuleDto
        {
            Index = 0,
            PowerLineStatus = PowerLineStatus.Online,
            SchemeGuid = Guid.Parse("a1841308-3541-4fab-bc81-f71556f20b4a"),
        });
        AssertRule(rules[1], new ProcessRuleDto
        {
            Index = 1,
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
            AcPowerSchemeGuid: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            BatterPowerSchemeGuid: Guid.Parse("22222222-2222-2222-2222-222222222222")
        );

        var originalJson = /*lang=json,strict*/ @"
        {
            ""$type"": ""RuleManagement.Rules.RuleManager+RuleContainer, RuleManagement"",
            ""Rules"": [
                {
                    ""$type"": ""RuleManagement.Rules.ProcessRuleDto, RuleManagement"",
                    ""FilePath"": ""testpath"",
                    ""Type"": 0,
                    ""Index"": 0,
                    ""SchemeGuid"": ""381b4222-f694-41f0-9685-ff5bb260df2e""
                },
                {
                    ""$type"": ""RuleManagement.Rules.PowerLineRuleDto, RuleManagement"",
                    ""PowerLineStatus"": 1,
                    ""Index"": 1,
                    ""SchemeGuid"": ""11111111-1111-1111-1111-111111111111""
                }
            ],
            ""SchemaVersion"": 1
        }";

        var manager = new RuleManager(originalJson, migrationPolicy, batteryMonitor, ruleFactory);

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


    private static void AssertRule(IRule rule, PowerLineRuleDto dto)
    {
        Assert.IsInstanceOfType(rule, typeof(PowerLineRule));
        Assert.AreEqual(dto.Index, ((PowerLineRule)rule).Index);
        Assert.AreEqual(dto.SchemeGuid, ((PowerLineRule)rule).SchemeGuid);
        Assert.AreEqual(dto.PowerLineStatus, ((PowerLineRule)rule).PowerLineStatus);
    }

    private static void AssertRule(IRule rule, ProcessRuleDto dto)
    {
        Assert.IsInstanceOfType(rule, typeof(ProcessRule));
        Assert.AreEqual(dto.Index, ((ProcessRule)rule).Index);
        Assert.AreEqual(dto.SchemeGuid, ((ProcessRule)rule).SchemeGuid);
        Assert.AreEqual(dto.FilePath, ((ProcessRule)rule).FilePath);
        Assert.AreEqual(dto.Type, ((ProcessRule)rule).Type);
    }
}
