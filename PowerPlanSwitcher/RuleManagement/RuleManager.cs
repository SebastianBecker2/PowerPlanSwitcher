namespace PowerPlanSwitcher.RuleManagement
{
    using PowerPlanSwitcher.PowerManagement;
    using PowerPlanSwitcher.ProcessManagement;
    using PowerPlanSwitcher.RuleManagement.Rules;

    public class RuleManager(IPowerManager powerManager)
    {
        public event EventHandler<RuleApplicationChangedEventArgs>?
            RuleApplicationChanged;
        protected virtual void OnRuleApplicationChanged(
            RuleApplicationChangedEventArgs args) =>
            RuleApplicationChanged?.Invoke(this, args);
        protected virtual void OnRuleApplicationChanged(
            Guid powerSchemeGuid,
            string? reason,
            IRule? rule) =>
            OnRuleApplicationChanged(
                new RuleApplicationChangedEventArgs(
                    powerSchemeGuid,
                    reason,
                    rule));

        public IBatteryMonitor? BatteryMonitor { get; set; }
        public IProcessMonitor? ProcessMonitor { get; set; }

        private IEnumerable<IRule>? rules;
        private readonly object syncObj = new();
        private Guid baselinePowerSchemeGuid;

        public void StartEngine(IEnumerable<IRule> rules)
        {
            lock (syncObj)
            {
                StopEngine();

                this.rules = rules;
                if (rules.Any())
                {
                    foreach (var rule in rules)
                    {
                        rule.ActivationCount = 0;
                    }

                    StartBatteryMonitor();

                    StartProcessMonitor();
                }

                baselinePowerSchemeGuid =
                    powerManager.GetActivePowerSchemeGuid();
                if (baselinePowerSchemeGuid == Guid.Empty)
                {
                    throw new InvalidOperationException(
                        "Unable to determine active power scheme");
                }

                powerManager.ActivePowerSchemeChanged +=
                    PowerManager_ActivePowerSchemeChanged;
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
                if (HasActiveRule())
                {
                    ApplyBaselinePowerScheme();
                    foreach (var rule in rules ?? [])
                    {
                        rule.ActivationCount = 0;
                    }
                }

                StopBatteryMonitor();

                StopProcessMonitor();

                baselinePowerSchemeGuid = Guid.Empty;
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
                bool? needToSwitch = null;
                IRule? ruleToApply = null;

                foreach (var rule in rules ?? [])
                {
                    if (rule is not PowerLineRule powerLineRule)
                    {
                        if (rule.ActivationCount == 0)
                        {
                            continue;
                        }

                        needToSwitch ??= false;
                        ruleToApply ??= needToSwitch == true ? rule : null;
                        continue;
                    }

                    if (powerLineRule.CheckRule(e.PowerLineStatus))
                    {
                        rule.ActivationCount = 1;
                        needToSwitch ??= true;
                        ruleToApply ??= needToSwitch == true ? rule : null;
                        continue;
                    }

                    rule.ActivationCount = 0;
                    needToSwitch ??= true;
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
                IRule? ruleToApply = null;

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
                bool? needToSwitch = null;
                IRule? ruleToApply = null;

                foreach (var rule in rules ?? [])
                {
                    if (rule is not ProcessRule processRule)
                    {
                        if (rule.ActivationCount == 0)
                        {
                            continue;
                        }

                        needToSwitch ??= false;
                        ruleToApply ??= needToSwitch == true ? rule : null;
                        continue;
                    }

                    var activationCount = CheckRule(rule, processes);
                    if (activationCount > 0)
                    {
                        needToSwitch ??= rule.ActivationCount == 0;
                        rule.ActivationCount += activationCount;
                        ruleToApply ??= needToSwitch == true ? rule : null;
                        continue;
                    }
                }

                if (needToSwitch == true && ruleToApply is not null)
                {
                    ApplyRule(ruleToApply);
                }
            }
        }

        private void ApplyRule(IRule rule) =>
            OnRuleApplicationChanged(
                rule.SchemeGuid,
                $"Rule {rule.Index + 1} applies",
                rule);

        private void ApplyBaselinePowerScheme()
        {
            if (baselinePowerSchemeGuid == Guid.Empty)
            {
                return;
            }
            OnRuleApplicationChanged(
                baselinePowerSchemeGuid,
                "No rule applies",
                null);
        }

        private IRule? GetActiveRule() =>
            rules?.FirstOrDefault(r => r.ActivationCount > 0);

        private bool HasActiveRule() => GetActiveRule() is not null;

        private static bool CheckRule(IRule rule, ICachedProcess process)
        {
            if (rule is ProcessRule powerRule)
            {
                return powerRule.CheckRule(process);
            }
            return false;
        }

        private static int CheckRule(
            IRule rule,
            IEnumerable<ICachedProcess> processes) =>
            processes.Count(p => CheckRule(rule, p));
    }
}
