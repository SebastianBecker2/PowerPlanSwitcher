namespace RuleManagement.Rules;

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PowerManagement;

public class Rules
{
    public class RuleContainerProbe
    {
        public int? SchemaVersion { get; set; }
    }

    private class RuleContainer : RuleContainerProbe
    {
        public List<IRuleDto> Rules { get; set; } = [];
    }

    private List<IRule> rules = [];
    private readonly RuleFactory ruleFactory;

    public event EventHandler<RulesUpdatedEventArgs>? RulesUpdated;

    public Rules(
        string ruleJson,
        MigrationPolicy migrationPolicy,
        IBatteryMonitor batteryMonitor,
        RuleFactory ruleFactory)
    {
        this.ruleFactory = ruleFactory;

        // Overwrite json with migrated version
        ruleJson = MigratePowerRulesToRules(
            ruleJson,
            migrationPolicy,
            batteryMonitor);

        rules = LoadRules(ruleJson);
    }

    public IEnumerable<IRule> GetRules() => rules ?? [];

    public void SetRules(IEnumerable<IRule> newRules)
    {
        rules = [.. newRules];

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
                    Index = rules.Count,
                    PowerLineStatus = PowerLineStatus.Online,
                    SchemeGuid = migrationPolicy.AcPowerSchemeGuid,
                });
            }

            if (migrationPolicy.BatterPowerSchemeGuid != Guid.Empty)
            {
                rules.Add(new PowerLineRuleDto()
                {
                    Index = rules.Count,
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

    private List<IRule> LoadRules(string json)
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
                new JsonSerializerSettings {
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
