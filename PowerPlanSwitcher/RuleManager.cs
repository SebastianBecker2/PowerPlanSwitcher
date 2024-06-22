namespace PowerPlanSwitcher
{
    using System.ComponentModel;
    using System.Globalization;
    using PowerPlanSwitcher.Properties;

    internal class RuleManager : IDisposable
    {
        private readonly object syncObj = new();
        private readonly BatteryMonitor batteryMonitor = new();
        private readonly ProcessMonitor processMonitor = new();
        private readonly IEnumerable<PowerRule> rules = PowerRule.GetPowerRules();
        private static Guid baselinePowerSchemeGuid;
        private bool disposedValue;

        public void StartEngine()
        {
            lock (syncObj)
            {
                InitializeBatterManager();
                InitializeProcessMonitor();
            }
        }

        private void InitializeBatterManager()
        {
            if (!BatteryMonitor.HasSystemBattery)
            {
                return;
            }

            batteryMonitor.PowerLineStatusChanged +=
                BatteryMonitor_PowerLineStatusChanged;

            baselinePowerSchemeGuid = GetPowerSchemeForPowerLineStatus();
            PowerManager.SetActivePowerScheme(baselinePowerSchemeGuid);
        }

        private void InitializeProcessMonitor()
        {
            if (!rules.Any())
            {
                return;
            }

            processMonitor.ProcessCreated += ProcessMonitor_ProcessCreated;
            processMonitor.ProcessTerminated += ProcessMonitor_ProcessTerminated;
            processMonitor.StartMonitoring();

            var processes = ProcessMonitor.GetUsersProcesses();
            var ruleApplied = false;

            foreach (var rule in rules)
            {
                if (!CheckRule(rule, processes))
                {
                    continue;
                }
                rule.ActivationCount++;

                if (ruleApplied)
                {
                    continue;
                }
                ruleApplied = true;

                _ = ActivateRule(rule);
            }
        }

        private void ProcessMonitor_ProcessCreated(
            object? sender,
            ProcessEventArgs e)
        {
            lock (syncObj)
            {
                var higherRuleActive = false;
                foreach (var rule in rules)
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

                    _ = ActivateRule(rule);
                }
            }
        }

        private void ProcessMonitor_ProcessTerminated(
            object? sender,
            ProcessEventArgs e)
        {
            lock (syncObj)
            {
                var higherRuleActive = false;
                foreach (var rule in rules)
                {
                    if (CheckRule(rule, e.Process))
                    {
                        rule.ActivationCount--;
                    }

                    if (higherRuleActive)
                    {
                        continue;
                    }

                    if (rule.ActivationCount == 0)
                    {
                        continue;
                    }
                    higherRuleActive = true;

                    _ = ActivateRule(rule);
                }

                if (!higherRuleActive)
                {
                    PowerManager.SetActivePowerScheme(baselinePowerSchemeGuid);
                    ToastDlg.ShowToastNotification(
                        baselinePowerSchemeGuid,
                        "No rule applies");
                }
            }
        }

        private void BatteryMonitor_PowerLineStatusChanged(
            object? sender,
            EventArgs e)
        {
            lock (syncObj)
            {
                baselinePowerSchemeGuid = GetPowerSchemeForPowerLineStatus();

                if (HasActiveRule())
                {
                    return;
                }

                PowerManager.SetActivePowerScheme(baselinePowerSchemeGuid);
            }
        }

        private static bool ActivateRule(PowerRule rule)
        {
            if (rule.SchemeGuid == PowerManager.GetActivePowerSchemeGuid())
            {
                return false;
            }

            PowerManager.SetActivePowerScheme(rule.SchemeGuid);
            ToastDlg.ShowToastNotification(
                rule.SchemeGuid,
                $"Rule {rule.Index + 1} applies");
            return true;
        }

        private static Guid GetPowerSchemeForPowerLineStatus() =>
            BatteryMonitor.PowerLineStatus switch
            {
                PowerLineStatus.Online => Settings.Default.AcPowerSchemeGuid,
                PowerLineStatus.Offline => Settings.Default.BatterPowerSchemeGuid,
                PowerLineStatus.Unknown => baselinePowerSchemeGuid,
                _ => baselinePowerSchemeGuid,
            };

        private PowerRule? GetActiveRule() =>
            rules.FirstOrDefault(r => r.ActivationCount > 0);

        private bool HasActiveRule() => GetActiveRule() is not null;

        private static bool CheckRule(PowerRule powerRule, CachedProcess process)
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
                        true,
                        CultureInfo.InvariantCulture),
                    RuleType.EndsWith => path.EndsWith(
                        powerRule.FilePath,
                        true,
                        CultureInfo.InvariantCulture),
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

        private static bool CheckRule(
            PowerRule powerRule,
            IEnumerable<CachedProcess> processes) =>
            processes.Any(p => CheckRule(powerRule, p));

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }
            if (disposing)
            {
                processMonitor?.Dispose();
            }

            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
