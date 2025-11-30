using PowerPlanSwitcher.RuleManagement.Rules;

namespace RuleManagement.Rules;

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PowerPlanSwitcher.PowerManagement;
using PowerPlanSwitcher.Properties;

internal class Rules
{
    private static List<IRule>? rules;
    private static readonly object Lock = new();

    public static IEnumerable<IRule> GetRules()
    {
        lock (Lock)
        {
            rules ??= LoadRules();
            return rules ?? [];
        }
    }

    private static void MigratePowerRulesToRules()
    {
        if (Settings.Default.MigratedPowerRulesToRules)
        {
            return;
        }

        if (!BatteryMonitor.Static.HasSystemBattery)
        {
            Settings.Default.MigratedPowerRulesToRules = true;
            Settings.Default.Save();
            return;
        }

        var rules = JsonConvert.DeserializeObject<List<ProcessRule>>(
            Settings.Default.PowerRules)?.Cast<IRule>()?.ToList() ?? [];

        if (Settings.Default.AcPowerSchemeGuid != Guid.Empty)
        {
            rules.Add(new PowerLineRule()
            {
                Index = rules.Count,
                PowerLineStatus = PowerLineStatus.Online,
                SchemeGuid = Settings.Default.AcPowerSchemeGuid,
            });
        }

        if (Settings.Default.BatterPowerSchemeGuid != Guid.Empty)
        {
            rules.Add(new PowerLineRule()
            {
                Index = rules.Count,
                PowerLineStatus = PowerLineStatus.Offline,
                SchemeGuid = Settings.Default.BatterPowerSchemeGuid,
            });
        }

        SaveRules(rules);

        Settings.Default.MigratedPowerRulesToRules = true;
        Settings.Default.Save();
    }

    private static List<IRule>? LoadRules()
    {
        MigratePowerRulesToRules();

        return JsonConvert.DeserializeObject<List<IRule>>(
            Settings.Default.Rules,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
    }

    public static void SetRules(IEnumerable<IRule> newRules)
    {
        rules = [.. newRules];
        SaveRules(rules);
    }

    private static void SaveRules(IEnumerable<IRule> rules)
    {
        Settings.Default.Rules =
            JsonConvert.SerializeObject(rules,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
        Settings.Default.Save();
    }
}
