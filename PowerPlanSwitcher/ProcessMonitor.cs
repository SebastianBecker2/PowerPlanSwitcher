namespace PowerPlanSwitcher
{
    using System.ComponentModel;
    using System.Diagnostics;
    using Properties;
    using Timer = System.Threading.Timer;

    internal class ProcessMonitor : IDisposable
    {
        private const int UpdateTimerInterval = 1000;
        private DateTime lastUpdate;
        private readonly Timer? updateTimer;
        private bool disposedValue;
        private static Guid baselinePowerSchemeGuid;
        private PowerRule? previouslyAppliedPowerRule;

        public ProcessMonitor()
        {
            baselinePowerSchemeGuid = PowerManager.GetActivePowerSchemeGuid();
            if (baselinePowerSchemeGuid == Guid.Empty)
            {
                throw new InvalidOperationException(
                    "Unable to determine active power scheme");
            }

            updateTimer = new Timer(HandleUpdateTimerTick);
            _ = updateTimer.Change(0, Timeout.Infinite);
        }

        private void HandleUpdateTimerTick(object? _)
        {
            try
            {
                var batteryMonitorPowerSchemeGuid =
                    BatteryMonitor.GetPowerSchemeGuid();
                if (batteryMonitorPowerSchemeGuid != Guid.Empty)
                {
                    baselinePowerSchemeGuid = batteryMonitorPowerSchemeGuid;
                }

                if (DateTime.Now - lastUpdate <
                    TimeSpan.FromSeconds(Settings.Default.PowerRuleCheckInterval))
                {
                    return;
                }
                lastUpdate = DateTime.Now;

                var processes = GetOwnedProcesses();

                var applicableRule = PowerRule.GetPowerRules()
                    .FirstOrDefault(r => CheckRule(r, processes));

                // To avoid applying the same rule consecutively
                // or resetting to baseline consecutively
                if (applicableRule == previouslyAppliedPowerRule)
                {
                    return;
                }

                // If no rule was active and the user changed the power
                // scheme, we use the newly set power scheme as the new
                // baseline.
                if (previouslyAppliedPowerRule is null)
                {
                    baselinePowerSchemeGuid =
                        PowerManager.GetActivePowerSchemeGuid();
                }
                else
                {
                    previouslyAppliedPowerRule.Active = false;
                }

                previouslyAppliedPowerRule = applicableRule;

                if (applicableRule is null)
                {
                    PowerManager.SetActivePowerScheme(baselinePowerSchemeGuid);
                    Program.ShowToastNotification(
                        baselinePowerSchemeGuid,
                        "No rule applies");
                    return;
                }


                applicableRule.Active = true;
                PowerManager.SetActivePowerScheme(applicableRule.SchemeGuid);
                Program.ShowToastNotification(
                    applicableRule.SchemeGuid,
                    $"Rule {applicableRule.Index} applies");
            }
            finally
            {
                _ = updateTimer!.Change(
                    UpdateTimerInterval,
                    Timeout.Infinite);
            }
        }

        public static bool CheckRule(
            PowerRule powerRule,
            IEnumerable<CachingProcess>? processes = null)
        {
            if (processes is null)
            {
                processes = GetOwnedProcesses();
                return CheckRule(powerRule, processes);
            }

            return processes.Any(p =>
            {
                try
                {
                    var path = p.FileName;
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        return false;
                    }

                    if (powerRule.Type == RuleType.Exact)
                    {
                        return path == powerRule.FilePath;
                    }

                    if (powerRule.Type == RuleType.StartsWith)
                    {
#pragma warning disable CA1310 // Specify StringComparison for correctness
                        return path.StartsWith(powerRule.FilePath);
                    }

                    if (powerRule.Type == RuleType.EndsWith)
                    {
                        return path.EndsWith(powerRule.FilePath);
#pragma warning restore CA1310 // Specify StringComparison for correctness
                    }

                    throw new InvalidOperationException(
                        $"Unable to apply rule type {powerRule.Type}");
                }
                catch (Win32Exception)
                {
                    //ProcessBlacklist.Add(p.ProcessName);
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            });
        }

        public static IEnumerable<CachingProcess> GetOwnedProcesses() =>
            Process.GetProcesses()
                .Select(CachingProcess.Create)
                .Where(p => p is not null)
                .Cast<CachingProcess>();

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                updateTimer?.Dispose();
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
