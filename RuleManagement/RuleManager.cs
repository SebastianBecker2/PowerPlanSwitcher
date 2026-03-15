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
            Log.Information("Skipped migration of PowerRules to Rules because migration flag is already set");
            return ruleJson;
        }

        Log.Information(
            "Started migration of PowerRules to Rules with input length {InputLength}",
            ruleJson.Length);

        try
        {
            // This migration stage predates schema versioning and expects the pre-migration
            // format: a raw JSON array of ProcessRuleDto objects (no container wrapper).
            // If the input is a JSON object rather than an array (e.g. an unknown schema
            // version was somehow passed through), Newtonsoft.Json will throw a
            // JsonSerializationException. In that case the exception propagates so
            // the caller is aware that migration could not proceed safely.
            var rules = JsonConvert.DeserializeObject<List<ProcessRuleDto>>(
                ruleJson)?.Cast<IRuleDto>()?.ToList() ?? [];

            Log.Information("Recognized {PowerRuleCount} PowerRules to migrate", rules.Count);
            Log.Information(
                "PowerRules migration input rule type summary: {RuleTypeSummary}",
                BuildRuleTypeSummary(rules));

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
                else
                {
                    Log.Warning("Skipped adding online PowerLineRule because AC scheme guid is empty");
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
                else
                {
                    Log.Warning("Skipped adding offline PowerLineRule because battery scheme guid is empty");
                }
            }
            else
            {
                Log.Warning("Skipped adding PowerLineRules because no system battery was detected");
            }

            Log.Information("Finished migration of PowerRules to Rules with {RuleCount}", rules.Count);
            Log.Information(
                "PowerRules migration output rule type summary: {RuleTypeSummary}",
                BuildRuleTypeSummary(rules));

            RuleContainer ruleContainer = new()
            {
                SchemaVersion = 1,
                Rules = rules,
            };

            var migratedRuleJson = JsonConvert.SerializeObject(ruleContainer,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
            Log.Information(
                "Serialized migrated PowerRules payload with output length {OutputLength}",
                migratedRuleJson.Length);

            return migratedRuleJson;
        }
        catch (Exception ex)
        {
            Log.Error(
                ex,
                "Failed migration of PowerRules to Rules during phase {Phase}",
                "deserialize/migrate/serialize");
            throw;
        }
    }

    private static string MigrateStartupRule(
        string ruleJson,
        MigrationPolicy migrationPolicy)
    {
        if (migrationPolicy.MigratedStartupRule)
        {
            Log.Information("Skipped migration of StartupRule because migration flag is already set");
            return ruleJson;
        }

        Log.Information(
            "Started migration of StartupRule with input length {InputLength}",
            ruleJson.Length);

        try
        {
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
                Log.Information(
                    "StartupRule migration input rule type summary: {RuleTypeSummary}",
                    BuildRuleTypeSummary(dtos));

                if (migrationPolicy.ActivateInitialPowerScheme)
                {
                    if (migrationPolicy.InitialPowerSchemeGuid == Guid.Empty)
                    {
                        Log.Warning("Migration is adding StartupRuleDto with an empty initial power scheme guid");
                    }

                    dtos.Add(new StartupRuleDto()
                    {
                        SchemeGuid = migrationPolicy.InitialPowerSchemeGuid,
                    });
                    Log.Information("Migration added StartupRuleDto");
                }
                else
                {
                    Log.Information("Skipped adding StartupRuleDto because ActivateInitialPowerScheme is false");
                }

                RuleContainer ruleContainer = new()
                {
                    SchemaVersion = 1,
                    Rules = dtos,
                };

                Log.Information("Finished migration of StartupRule with {RuleCount}", dtos.Count);
                Log.Information(
                    "StartupRule migration output rule type summary: {RuleTypeSummary}",
                    BuildRuleTypeSummary(dtos));

                var migratedRuleJson = JsonConvert.SerializeObject(ruleContainer,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                Log.Information(
                    "Serialized migrated StartupRule payload with output length {OutputLength}",
                    migratedRuleJson.Length);

                return migratedRuleJson;
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
                Log.Information(
                    "StartupRule migration input rule type summary: {RuleTypeSummary}",
                    BuildRuleTypeSummary(container.Rules));

                if (migrationPolicy.ActivateInitialPowerScheme)
                 {
                    if (migrationPolicy.InitialPowerSchemeGuid == Guid.Empty)
                    {
                        Log.Warning("Migration is adding StartupRuleDto with an empty initial power scheme guid");
                    }

                    container.Rules.Add(new StartupRuleDto()
                    {
                        SchemeGuid = migrationPolicy.InitialPowerSchemeGuid,
                    });
                    Log.Information("Migration added StartupRuleDto");
                }
                else
                {
                    Log.Information("Skipped adding StartupRuleDto because ActivateInitialPowerScheme is false");
                }

                RuleContainer ruleContainer = new()
                {
                    SchemaVersion = 1,
                    Rules = container.Rules,
                };

                Log.Information("Finished migration of StartupRule with {RuleCount}", ruleContainer.Rules.Count);
                Log.Information(
                    "StartupRule migration output rule type summary: {RuleTypeSummary}",
                    BuildRuleTypeSummary(ruleContainer.Rules));

                var migratedRuleJson = JsonConvert.SerializeObject(ruleContainer,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                Log.Information(
                    "Serialized migrated StartupRule payload with output length {OutputLength}",
                    migratedRuleJson.Length);

                return migratedRuleJson;
            }
            else
            {
                Log.Warning("Unknown schema version {SchemaVersion}. Skipped migration.", version);
                return ruleJson;
            }
        }
        catch (Exception ex)
        {
            Log.Error(
                ex,
                "Failed migration of StartupRule for schema version {SchemaVersion}",
                DetectSchemaVersion(ruleJson));
            throw;
        }
    }

    private static string BuildRuleTypeSummary(IEnumerable<IRuleDto> ruleDtos)
    {
        var summary = ruleDtos
            .GroupBy(dto => dto.GetType().Name)
            .Select(group => $"{group.Key}:{group.Count()}")
            .ToArray();

        return summary.Length == 0
            ? "None"
            : string.Join(", ", summary);
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
