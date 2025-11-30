namespace RuleManagement.Rules;

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PowerManagement;

internal class Rules
{
    private List<IRule> rules = [];

    public event EventHandler<RulesUpdatedEventArgs>? RulesUpdated;

    public Rules(
        string ruleJson,
        bool migratedPowerRulesToRules,
        Guid acPowerSchemeGuid,
        Guid batterPowerSchemeGuid)
    {
        if (!migratedPowerRulesToRules)
        {
            // Overwrite json with migrated version
            ruleJson = MigratePowerRulesToRules(ruleJson, acPowerSchemeGuid, batterPowerSchemeGuid);
        }

        rules = LoadRules(ruleJson);
    }

    public IEnumerable<IRule> GetRules() => rules ?? [];

    private static string MigratePowerRulesToRules(
        string ruleJson,
        Guid acPowerSchemeGuid,
        Guid batterPowerSchemeGuid)
    {
        if (!BatteryMonitor.Static.HasSystemBattery)
        {
            return ruleJson;
        }

        var rules = JsonConvert.DeserializeObject<List<ProcessRule>>(
            ruleJson)?.Cast<IRule>()?.ToList() ?? [];

        if (acPowerSchemeGuid != Guid.Empty)
        {
            rules.Add(new PowerLineRule()
            {
                Index = rules.Count,
                PowerLineStatus = PowerLineStatus.Online,
                SchemeGuid = acPowerSchemeGuid,
            });
        }

        if (batterPowerSchemeGuid != Guid.Empty)
        {
            rules.Add(new PowerLineRule()
            {
                Index = rules.Count,
                PowerLineStatus = PowerLineStatus.Offline,
                SchemeGuid = batterPowerSchemeGuid,
            });
        }

        return JsonConvert.SerializeObject(rules,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
    }

    private static List<IRule> LoadRules(string ruleJson) =>
        JsonConvert.DeserializeObject<List<IRule>>(
            ruleJson,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            })
            ?? [];

    public void SetRules(IEnumerable<IRule> newRules)
    {
        rules = [.. newRules];

        var json = JsonConvert.SerializeObject(rules,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

        RulesUpdated?.Invoke(this, new RulesUpdatedEventArgs(json));
    }
}
