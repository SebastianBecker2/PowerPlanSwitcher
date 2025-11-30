using Autofac;
using DebugCmd;
using PowerManagement;
using ProcessManagement;
using RuleManagement.Rules;






var preMigrationRuleJson = /*lang=json,strict*/ @"
[
  {
    ""Index"": 0,
    ""FilePath"": ""testpath"",
    ""Type"": 0,
    ""SchemeGuid"": ""381b4222-f694-41f0-9685-ff5bb260df2e""
  },
  {
    ""Index"": 1,
    ""FilePath"": ""anotherpathtest"",
    ""Type"": 1,
    ""SchemeGuid"": ""8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c""
  }
]";

var version0RuleJson = /*lang=json,strict*/ @"
[
  {
    ""$type"": ""PowerPlanSwitcher.RuleManagement.Rules.PowerLineRule, PowerPlanSwitcher"",
    ""Index"": 0,
    ""SchemeGuid"": ""a1841308-3541-4fab-bc81-f71556f20b4a"",
    ""PowerLineStatus"": 0
  },
  {
    ""$type"": ""PowerPlanSwitcher.RuleManagement.Rules.ProcessRule, PowerPlanSwitcher"",
    ""Index"": 1,
    ""SchemeGuid"": ""8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"",
    ""FilePath"": ""asdf"",
    ""Type"": 0
  }
]
";

var version1RuleJson = /*lang=json,strict*/ @"
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
            ""$type"": ""RuleManagement.Rules.ProcessRuleDto, RuleManagement"",
            ""FilePath"": ""anotherpathtest"",
            ""Type"": 1,
            ""Index"": 1,
            ""SchemeGuid"": ""8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c""
        },
        {
            ""$type"": ""RuleManagement.Rules.PowerLineRuleDto, RuleManagement"",
            ""PowerLineStatus"": 1,
            ""Index"": 2,
            ""SchemeGuid"": ""11111111-1111-1111-1111-111111111111""
        },
        {
            ""$type"": ""RuleManagement.Rules.PowerLineRuleDto, RuleManagement"",
            ""PowerLineStatus"": 0,
            ""Index"": 3,
            ""SchemeGuid"": ""22222222-2222-2222-2222-222222222222""
        }
    ],
    ""SchemaVersion"": 1
}
";






var builder = new ContainerBuilder();

builder.RegisterType<BatteryMonitorTest>()
    .As<IBatteryMonitor>()
    .SingleInstance();

builder.RegisterType<ProcessMonitor>()
    .As<IProcessMonitor>()
    .SingleInstance();

builder.RegisterType<RuleFactory>()
    .AsSelf()
    .SingleInstance();

builder.RegisterInstance(new MigrationPolicy(
        MigratedPowerRulesToRules: true,
        AcPowerSchemeGuid: Guid.Parse("11111111-1111-1111-1111-111111111111"),
        BatterPowerSchemeGuid: Guid.Parse("22222222-2222-2222-2222-222222222222")))
    .AsSelf()
    .SingleInstance();

builder.RegisterType<RuleManager>()
    .AsSelf()
    .WithParameter("ruleJson", version0RuleJson) // supply the string
    .SingleInstance();

var container = builder.Build();

var ruleManager = container.Resolve<RuleManager>();

//var ruleManager = new RuleManager(
//    version1RuleJson,
//    new MigrationPolicy(
//        MigratedPowerRulesToRules: true,
//        AcPowerSchemeGuid: Guid.Parse("11111111-1111-1111-1111-111111111111"),
//        BatterPowerSchemeGuid: Guid.Parse("22222222-2222-2222-2222-222222222222")
//        ),
//    container.Resolve<IBatteryMonitor>(),
//    container.Resolve<RuleFactory>());

ruleManager.RulesUpdated += (s, e) =>
{
    Console.WriteLine("Rules updated:");
    Console.WriteLine(e.Serialized);
};

var rules = ruleManager.GetRules();
ruleManager.SetRules(rules);


//using var monitor = new BatteryMonitor();
//monitor.PowerLineStatusChanged += (s, e) =>
//    Console.WriteLine($"Power line status changed to: {e.PowerLineStatus}");

//Console.WriteLine("Monitoring power line status changes. Press Enter to exit.");
Console.ReadLine();
