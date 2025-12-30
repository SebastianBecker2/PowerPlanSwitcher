namespace RuleManagement;

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PowerManagement;
using RuleManagement.Dto;
using RuleManagement.Events;
using RuleManagement.Rules;
using Serilog;

public class RuleManager
{
    private class RuleContainerProbe
    {
        public int? SchemaVersion { get; set; }
    }

    private class RuleContainer : RuleContainerProbe
    {
        public List<IRuleDto> Rules { get; set; } = [];
    }

    private readonly RuleFactory ruleFactory;

    private List<IRule> rules = [];

    public IRule? AppliedRule { get; private set; }

    public event EventHandler<RulesUpdatedEventArgs>? RulesUpdated;
    public event EventHandler<RuleApplicationChangedEventArgs>? RuleApplicationChanged;

    public RuleManager(
        RuleFactory ruleFactory,
        string? ruleJson = null,
        MigrationPolicy? migrationPolicy = null,
        IBatteryMonitor? batteryMonitor = null)
    {
        this.ruleFactory = ruleFactory;

        if (string.IsNullOrWhiteSpace(ruleJson)
            || migrationPolicy is null
            || batteryMonitor is null)
        {
            return;
        }

        // Overwrite json with migrated version
        ruleJson = MigratePowerRulesToRules(
            ruleJson,
            migrationPolicy,
            batteryMonitor);
        ruleJson = MigrateStartupRule(
            ruleJson,
            migrationPolicy);

        rules = LoadRules(ruleJson, ruleFactory);
        Subscribe(rules);
    }

    public void StartMonitoring()
    {
        Log.Information("Rule monitoring started");

        Subscribe(rules);
    }

    public void StopMonitoring()
    {
        Unsubscribe(rules);

        Log.Information("Rule monitoring stopped");
    }

    private void Rule_TriggerChanged(object? sender, TriggerChangedEventArgs e)
    {
        if (sender is not IRule rule)
        {
            throw new InvalidCastException("Sender was not an IRule.");
        }

        var index = rules.IndexOf(rule);

        // If rule is not triggered, apply the next triggered rule
        // or no rule (nextRule == null) if no other rule is triggered
        if (rule.TriggerCount == 0)
        {
            var nextRule = rules.FirstOrDefault(r => r.TriggerCount > 0);
            // Next triggered rule is already applied
            if (nextRule == AppliedRule)
            {
                return;
            }
            AppliedRule = nextRule;
            RuleApplicationChanged?.Invoke(this, new RuleApplicationChangedEventArgs(nextRule));
            return;
        }

        var firstTriggeredRuleIndex = rules.FindIndex(r => r.TriggerCount > 0);

        // The rule is the highest with a TriggerCount > 1
        // so if the TriggerCount is exactly 1, we still need to apply it
        // otherwise it was already applied.
        if (index == firstTriggeredRuleIndex && rule.TriggerCount == 1)
        {
            AppliedRule = rule;
            RuleApplicationChanged?.Invoke(this, new RuleApplicationChangedEventArgs(rule));
            return;
        }

        // The triggered rule has to be applied
        // if no other rule with higher prio is triggered
        if (firstTriggeredRuleIndex == -1 || firstTriggeredRuleIndex > index)
        {
            AppliedRule = rule;
            RuleApplicationChanged?.Invoke(this, new RuleApplicationChangedEventArgs(rule));
            return;
        }
    }

    public IEnumerable<IRule> GetRules() => rules ?? [];

    private void Subscribe(IEnumerable<IRule> rules)
    {
        foreach (var rule in rules)
        {
            rule.TriggerChanged += Rule_TriggerChanged;
            rule.StartRuling();
        }

        var firstTriggeredRule = rules.FirstOrDefault(r => r.TriggerCount > 0);
        if (firstTriggeredRule is null)
        {
            return;
        }
        Rule_TriggerChanged(
            firstTriggeredRule,
            new TriggerChangedEventArgs(firstTriggeredRule));
    }

    private void Unsubscribe(IEnumerable<IRule> rules)
    {
        foreach (var rule in rules)
        {
            rule.TriggerChanged -= Rule_TriggerChanged;
            rule.StopRuling();
        }
    }

    public void SetRules(IEnumerable<IRuleDto> newRuleDtos) =>
        SetRules(newRuleDtos.Select(ruleFactory.Create));

    public void SetRules(IEnumerable<IRule> newRules)
    {
        Unsubscribe(rules);
        foreach (var rule in rules)
        {
            (rule as IDisposable)?.Dispose();
        }
        rules = [.. newRules];
        Subscribe(rules);

        var ruleContainer = new RuleContainer
        {
            SchemaVersion = 1,
            Rules = [.. rules.Select(r => r.Dto)],
        };
        var json = JsonConvert.SerializeObject(
            ruleContainer,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

        RulesUpdated?.Invoke(this, new RulesUpdatedEventArgs(json));
    }

    private static string MigratePowerRulesToRules(
        string ruleJson,
        MigrationPolicy migrationPolicy,
        IBatteryMonitor batteryMonitor)
    {
        if (migrationPolicy.MigratedPowerRulesToRules)
        {
            return ruleJson;
        }

        Log.Information("Started migration of PowerRules to Rules");

        var rules = JsonConvert.DeserializeObject<List<ProcessRuleDto>>(
            ruleJson)?.Cast<IRuleDto>()?.ToList() ?? [];

        Log.Information("Recognized {PowerRuleCount} PowerRules to migrate", rules.Count);

        if (batteryMonitor.HasSystemBattery)
        {
            if (migrationPolicy.AcPowerSchemeGuid != Guid.Empty)
            {
                rules.Add(new PowerLineRuleDto()
                {
                    PowerLineStatus = PowerLineStatus.Online,
                    SchemeGuid = migrationPolicy.AcPowerSchemeGuid,
                });
                Log.Information("Migration added PowerLineRule for PowerLineStatus Online");
            }

            if (migrationPolicy.BatterPowerSchemeGuid != Guid.Empty)
            {
                rules.Add(new PowerLineRuleDto()
                {
                    PowerLineStatus = PowerLineStatus.Offline,
                    SchemeGuid = migrationPolicy.BatterPowerSchemeGuid,
                });
                Log.Information("Migration added PowerLineRule for PowerLineStatus Offline");
            }
        }

        Log.Information("Finished migration of PowerRules to Rules with {RuleCount}", rules.Count);

        RuleContainer ruleContainer = new()
        {
            SchemaVersion = 1,
            Rules = rules,
        };

        return JsonConvert.SerializeObject(ruleContainer,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
    }

    private static string MigrateStartupRule(
        string ruleJson,
        MigrationPolicy migrationPolicy)
    {
        if (migrationPolicy.MigratedStartupRule)
        {
            return ruleJson;
        }

        Log.Information("Started migration of StartupRule");

        var version = DetectSchemaVersion(ruleJson);
        Log.Information("Detected schema version {SchemaVersion}", version);

        if (version == 0)
        {
            var dtos = JsonConvert.DeserializeObject<List<IRuleDto>>(
                ruleJson,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    SerializationBinder = new RuleTypeBinder()
                })
                ?? [];

            Log.Information("Recognized {DtoCount} IRuleDto to migrate", dtos.Count);

            if (migrationPolicy.ActivateInitialPowerScheme)
            {
                dtos.Add(new StartupRuleDto()
                {
                    SchemeGuid = migrationPolicy.InitialPowerSchemeGuid,
                });
                Log.Information("Migration added StartupRuleDto");
            }

            RuleContainer ruleContainer = new()
            {
                SchemaVersion = 1,
                Rules = dtos,
            };

            Log.Information("Finished migration of StartupRule with {RuleCount}", dtos.Count);

            return JsonConvert.SerializeObject(ruleContainer,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
        }
        else if (version == 1)
        {
            var container = JsonConvert.DeserializeObject<RuleContainer>(
                ruleJson,
                 new JsonSerializerSettings
                 {
                     TypeNameHandling = TypeNameHandling.Objects
                 })
                ?? new RuleContainer();

            Log.Information("Recognized {DtoCount} IRuleDto to migrate", container.Rules.Count);

            if (migrationPolicy.ActivateInitialPowerScheme)
            {
                container.Rules.Add(new StartupRuleDto()
                {
                    SchemeGuid = migrationPolicy.InitialPowerSchemeGuid,
                });
                Log.Information("Migration added StartupRuleDto");
            }

            RuleContainer ruleContainer = new()
            {
                SchemaVersion = 1,
                Rules = container.Rules,
            };

            Log.Information("Finished migration of StartupRule with {RuleCount}", ruleContainer.Rules.Count);

            return JsonConvert.SerializeObject(ruleContainer,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
        }
        else
        {
            Log.Information("Unkonwn schema version {SchemaVersion}. Skipped migration.", version);
            return ruleJson;
        }
    }

    private static int DetectSchemaVersion(string json)
    {
        try
        {
            var probe = JsonConvert.DeserializeObject<RuleContainerProbe>(json);
            // 0 = Post-Migration but pre-schema versioning
            return probe?.SchemaVersion ?? 0;
        }
        catch
        {
            // If it's just a raw array, parsing into RuleContainerProbe will fail
            return 0;
        }
    }

    private static List<IRule> LoadRules(string json, RuleFactory ruleFactory)
    {
        Log.Information("Started loading of rules");

        var version = DetectSchemaVersion(json);
        Log.Information("Detected schema version {SchemaVersion}", version);

        if (version == 0)
        {
            // Schema v0: container with rules still using old types
            var dtos = JsonConvert.DeserializeObject<List<IRuleDto>>(
                json,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    SerializationBinder = new RuleTypeBinder()
                })
                ?? [];

            Log.Information("Loaded {DtoCount} IRuleDto", dtos.Count);

            return [.. dtos.Select(ruleFactory.Create)];
        }
        else if (version == 1)
        {
            // Schema v1: container with DTOs
            var container = JsonConvert.DeserializeObject<RuleContainer>(json,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });

            if (container is null)
            {
                Log.Information("Unable to deserialize json");
                return [];
            }

            Log.Information("Loaded {DtoCount} IRuleDto", container.Rules.Count);
            return [.. container.Rules.Select(ruleFactory.Create)];
        }
        else
        {
            Log.Information("Unkonwn schema version {SchemaVersion}. Unable to load rules", version);
            return [];
        }
    }
}
