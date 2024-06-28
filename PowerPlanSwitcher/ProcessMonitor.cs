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

        public ProcessMonitor() =>
            updateTimer = new Timer(HandleUpdateTimerTick);

        public void StartMonitoring()
        {
            baselinePowerSchemeGuid = PowerManager.GetActivePowerSchemeGuid();
            if (baselinePowerSchemeGuid == Guid.Empty)
            {
                throw new InvalidOperationException(
                    "Unable to determine active power scheme");
            }

            _ = updateTimer!.Change(0, Timeout.Infinite);
        }

        private void HandleUpdateTimerTick(object? _)
        {
            try
            {
                if (DateTime.Now - lastUpdate <
                    TimeSpan.FromSeconds(Settings.Default.PowerRuleCheckInterval))
                {
                    return;
                }
                lastUpdate = DateTime.Now;

                var batteryMonitorPowerSchemeGuid =
                    BatteryMonitor.GetPowerSchemeGuid();
                if (batteryMonitorPowerSchemeGuid != Guid.Empty)
                {
                    baselinePowerSchemeGuid = batteryMonitorPowerSchemeGuid;
                }

                var processes = GetUsersProcesses();

                var applicableRule = PowerRule.GetPowerRules()
                    .FirstOrDefault(r => CheckRule(r, processes));

                // To avoid applying the same rule consecutively
                // or resetting to baseline consecutively
                if (applicableRule == previouslyAppliedPowerRule)
                {
                    return;
                }

                var activePowerSchemeGuid =
                    PowerManager.GetActivePowerSchemeGuid();

                // If no rule was active and the user changed the power
                // scheme, we use the newly set power scheme as the new
                // baseline.
                if (previouslyAppliedPowerRule is null)
                {
                    if (activePowerSchemeGuid != Guid.Empty)
                    {
                        baselinePowerSchemeGuid = activePowerSchemeGuid;
                    }
                }
                else
                {
                    previouslyAppliedPowerRule.Active = false;
                }

                previouslyAppliedPowerRule = applicableRule;

                if (applicableRule is null)
                {
                    if (baselinePowerSchemeGuid == activePowerSchemeGuid)
                    {
                        return;
                    }

                    PowerManager.SetActivePowerScheme(baselinePowerSchemeGuid);
                    ToastDlg.ShowToastNotification(
                        baselinePowerSchemeGuid,
                        "No rule applies");
                    return;
                }

                // We need to make sure the rule has it's flag active.
                // Even if we don't set an active power scheme or show a toast
                // notification because the power scheme is already active.
                applicableRule.Active = true;

                if (applicableRule.SchemeGuid == activePowerSchemeGuid)
                {
                    return;
                }

                PowerManager.SetActivePowerScheme(applicableRule.SchemeGuid);
                ToastDlg.ShowToastNotification(
                    applicableRule.SchemeGuid,
                    $"Rule {applicableRule.Index + 1} applies");
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
                processes = GetUsersProcesses();
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
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            });
        }

        public static IEnumerable<CachingProcess> GetUsersProcesses() =>
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
