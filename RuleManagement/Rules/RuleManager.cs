namespace RuleManagement.Rules;

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PowerManagement;

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

    private List<IRule> rules = [];
    public IRule? AppliedRule { get; private set; }

    public event EventHandler<RulesUpdatedEventArgs>? RulesUpdated;
    public event EventHandler<RuleApplicationChangedEventArgs>? RuleApplicationChanged;

    public RuleManager() { }

    public RuleManager(
        string ruleJson,
        MigrationPolicy migrationPolicy,
        IBatteryMonitor batteryMonitor,
        RuleFactory ruleFactory)
    {
        // Overwrite json with migrated version
        ruleJson = MigratePowerRulesToRules(
            ruleJson,
            migrationPolicy,
            batteryMonitor);

        rules = LoadRules(ruleJson, ruleFactory);
        Subscribe(rules);
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
        }
    }

    private void Unsubscribe(IEnumerable<IRule> rules)
    {
        foreach (var rule in rules)
        {
            rule.TriggerChanged -= Rule_TriggerChanged;
        }
    }

    public void SetRules(IEnumerable<IRule> newRules)
    {
        Unsubscribe(rules);
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

        var rules = JsonConvert.DeserializeObject<List<ProcessRuleDto>>(
            ruleJson)?.Cast<IRuleDto>()?.ToList() ?? [];

        if (batteryMonitor.HasSystemBattery)
        {
            if (migrationPolicy.AcPowerSchemeGuid != Guid.Empty)
            {

                rules.Add(new PowerLineRuleDto()
                {
                    PowerLineStatus = PowerLineStatus.Online,
                    SchemeGuid = migrationPolicy.AcPowerSchemeGuid,
                });
            }

            if (migrationPolicy.BatterPowerSchemeGuid != Guid.Empty)
            {
                rules.Add(new PowerLineRuleDto()
                {
                    PowerLineStatus = PowerLineStatus.Offline,
                    SchemeGuid = migrationPolicy.BatterPowerSchemeGuid,
                });
            }
        }

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
        var version = DetectSchemaVersion(json);

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
                return [];
            }

            return [.. container.Rules.Select(ruleFactory.Create)];
        }
        else
        {
            throw new NotSupportedException(
                $"Unsupported schema version {version}");
        }
    }
}
