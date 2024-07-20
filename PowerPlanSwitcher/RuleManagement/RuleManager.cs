namespace PowerPlanSwitcher.RuleManagement
{
    using System.ComponentModel;
    using PowerPlanSwitcher.PowerManagement;
    using PowerPlanSwitcher.ProcessManagement;

    public class RuleManager
    {
        public event EventHandler<RuleApplicationChangedEventArgs>?
            RuleApplicationChanged;
        protected virtual void OnRuleApplicationChanged(
            RuleApplicationChangedEventArgs args) =>
            RuleApplicationChanged?.Invoke(this, args);
        protected virtual void OnRuleApplicationChanged(
            Guid powerSchemeGuid,
            string? reason,
            PowerRule? rule) =>
            OnRuleApplicationChanged(
                new RuleApplicationChangedEventArgs(
                    powerSchemeGuid,
                    reason,
                    rule));

        public IBatteryMonitor? BatteryMonitor { get; set; }
        public IProcessMonitor? ProcessMonitor { get; set; }
        private readonly IPowerManager powerManager;
        private IEnumerable<PowerRule>? rules;
        private readonly object syncObj = new();
        private Guid baselinePowerSchemeGuid;

        public RuleManager(IPowerManager powerManager)
        {
            this.powerManager = powerManager;

            if (baselinePowerSchemeGuid == Guid.Empty)
            {
                baselinePowerSchemeGuid =
                    this.powerManager.GetActivePowerSchemeGuid();
                if (baselinePowerSchemeGuid == Guid.Empty)
                {
                    throw new InvalidOperationException(
                        "Unable to determine active power scheme");
                }
            }

            this.powerManager.ActivePowerSchemeChanged +=
                PowerManager_ActivePowerSchemeChanged;
        }

        public void StartEngine(IEnumerable<PowerRule> rules)
        {
            lock (syncObj)
            {
                StopEngine();

                // BatteryMonitor isn't reliant on rules,
                // so we start it even when we don't have any rules.
                StartBatteryMonitor();

                this.rules = rules;
                if (!rules.Any())
                {
                    return;
                }

                foreach (var rule in rules)
                {
                    rule.ActivationCount = 0;
                }

                StartProcessMonitor();
            }
        }

        private void StartBatteryMonitor()
        {
            if (BatteryMonitor is null)
            {
                return;
            }

            if (!BatteryMonitor.HasSystemBattery)
            {
                return;
            }

            BatteryMonitor.PowerLineStatusChanged +=
                BatteryMonitor_PowerLineStatusChanged;

            BatteryMonitor_PowerLineStatusChanged(
                BatteryMonitor,
                new PowerLineStatusChangedEventArgs(
                    BatteryMonitor.PowerLineStatus));
        }

        private void StartProcessMonitor()
        {
            ProcessMonitor!.ProcessCreated += ProcessMonitor_ProcessCreated;
            ProcessMonitor!.ProcessTerminated += ProcessMonitor_ProcessTerminated;
            ProcessMonitor!.StartMonitoring();

            HandleProcessesCreated(ProcessMonitor.GetUsersProcesses());
        }

        public void StopEngine()
        {
            lock (syncObj)
            {
                StopBatteryMonitor();

                StopProcessMonitor();
            }
        }

        private void StopBatteryMonitor()
        {
            if (BatteryMonitor is null)
            {
                return;
            }

            BatteryMonitor.PowerLineStatusChanged -=
                BatteryMonitor_PowerLineStatusChanged;
        }

        private void StopProcessMonitor()
        {
            if (ProcessMonitor is null)
            {
                return;
            }

            if (HasActiveRule())
            {
                ApplyBaselinePowerScheme();
                foreach (var rule in rules ?? [])
                {
                    rule.ActivationCount = 0;
                }
            }

            ProcessMonitor.ProcessCreated -= ProcessMonitor_ProcessCreated;
            ProcessMonitor.ProcessTerminated -= ProcessMonitor_ProcessTerminated;
        }

        private void PowerManager_ActivePowerSchemeChanged(
            object? sender,
            ActivePowerSchemeChangedEventArgs e)
        {
            lock (syncObj)
            {
                if (HasActiveRule())
                {
                    return;
                }

                if (e.ActiveSchemeGuid == Guid.Empty)
                {
                    return;
                }

                baselinePowerSchemeGuid = e.ActiveSchemeGuid;
            }
        }

        private void BatteryMonitor_PowerLineStatusChanged(
            object? sender,
            PowerLineStatusChangedEventArgs e)
        {
            lock (syncObj)
            {
                if (BatteryMonitor!.GetPowerSchemeGuid(e.PowerLineStatus)
                    == Guid.Empty)
                {
                    return;
                }

                baselinePowerSchemeGuid =
                    BatteryMonitor!.GetPowerSchemeGuid(e.PowerLineStatus);

                if (HasActiveRule())
                {
                    return;
                }

                OnRuleApplicationChanged(baselinePowerSchemeGuid, "Battery Management", null);
            }
        }

        private void ProcessMonitor_ProcessCreated(
            object? sender,
            ProcessEventArgs e)
        {
            lock (syncObj)
            {
                var higherRuleActive = false;
                foreach (var rule in rules ?? [])
                {
                    if (rule.ActivationCount > 0)
                    {
                        higherRuleActive = true;
                    }

                    if (!CheckRule(rule, e.Process))
                    {
                        continue;
                    }
                    rule.ActivationCount++;

                    if (higherRuleActive)
                    {
                        continue;
                    }
                    higherRuleActive = true;

                    ApplyRule(rule);
                }
            }
        }

        private void ProcessMonitor_ProcessTerminated(
            object? sender,
            ProcessEventArgs e)
        {
            lock (syncObj)
            {
                bool? needToSwitch = null;
                PowerRule? ruleToApply = null;

                foreach (var rule in rules ?? [])
                {
                    if (CheckRule(rule, e.Process))
                    {
                        rule.ActivationCount =
                            Math.Max(rule.ActivationCount - 1, 0);

                        needToSwitch ??= rule.ActivationCount == 0;
                        if (rule.ActivationCount > 0)
                        {
                            ruleToApply ??= needToSwitch == true ? rule : null;
                        }

                        continue;
                    }

                    if (rule.ActivationCount > 0)
                    {
                        needToSwitch ??= false;
                        ruleToApply ??= needToSwitch == true ? rule : null;
                    }
                }

                if (needToSwitch == true)
                {
                    if (ruleToApply is null)
                    {
                        ApplyBaselinePowerScheme();
                        return;
                    }
                    ApplyRule(ruleToApply);
                }
            }
        }

        private void HandleProcessesCreated(
            IEnumerable<ICachedProcess> processes)
        {
            lock (syncObj)
            {
                PowerRule? ruleToApply = null;

                foreach (var rule in rules ?? [])
                {
                    rule.ActivationCount += CheckRule(rule, processes);

                    if (rule.ActivationCount > 0)
                    {
                        ruleToApply ??= rule;
                    }
                }

                if (ruleToApply is not null)
                {
                    ApplyRule(ruleToApply);
                }
            }
        }

        private void ApplyRule(PowerRule rule) =>
            OnRuleApplicationChanged(
                rule.SchemeGuid,
                $"Rule {rule.Index + 1} applies",
                rule);

        private void ApplyBaselinePowerScheme() =>
            OnRuleApplicationChanged(
                baselinePowerSchemeGuid,
                "No rule applies",
                null);

        private PowerRule? GetActiveRule() =>
            rules?.FirstOrDefault(r => r.ActivationCount > 0);

        private bool HasActiveRule() => GetActiveRule() is not null;

        private static bool CheckRule(
            PowerRule powerRule,
            ICachedProcess process)
        {
            try
            {
                var path = process.ExecutablePath;
                if (string.IsNullOrWhiteSpace(path))
                {
                    return false;
                }

                return powerRule.Type switch
                {
                    RuleType.Exact => path == powerRule.FilePath,
                    RuleType.StartsWith => path.StartsWith(
                        powerRule.FilePath,
                        StringComparison.InvariantCulture),
                    RuleType.EndsWith => path.EndsWith(
                        powerRule.FilePath,
                        StringComparison.InvariantCulture),
                    _ => throw new InvalidOperationException(
                        $"Unable to apply rule type {powerRule.Type}"),
                };
            }
            catch (Win32Exception)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        private static int CheckRule(
            PowerRule powerRule,
            IEnumerable<ICachedProcess> processes) =>
            processes.Count(p => CheckRule(powerRule, p));
    }
}
