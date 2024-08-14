namespace PowerPlanSwitcher.ProcessManagement
{
    using System.Diagnostics;
    using Timer = System.Threading.Timer;

    public class ProcessMonitor : IDisposable, IProcessMonitor
    {
#pragma warning disable CA1716 // Identifiers should not match keywords
        public static class Static
#pragma warning restore CA1716 // Identifiers should not match keywords
        {
            public static IEnumerable<ICachedProcess> GetUsersProcesses() =>
            Process.GetProcesses()
                .Select(CachedProcess.CreateFromProcess)
                .Where(p => p is not null && !p.IsOwnProcess)
                .Cast<CachedProcess>();
        }

        public event EventHandler<ProcessEventArgs>? ProcessCreated;
        protected virtual void OnProcessCreated(ProcessEventArgs e) =>
            ProcessCreated?.Invoke(this, e);
        protected virtual void OnProcessCreated(ICachedProcess process) =>
            OnProcessCreated(new ProcessEventArgs(process));

        public event EventHandler<ProcessEventArgs>? ProcessTerminated;
        protected virtual void OnProcessTerminated(ProcessEventArgs e) =>
            ProcessTerminated?.Invoke(this, e);
        protected virtual void OnProcessTerminated(ICachedProcess process) =>
            OnProcessTerminated(new ProcessEventArgs(process));

        private const int UpdateTimerInterval = 2000;
        private IEnumerable<ICachedProcess> previousProcesses = [];
        private readonly Timer updateTimer;
        private bool disposedValue;
        private bool monitoring;

        public ProcessMonitor() =>
            updateTimer = new Timer(HandleUpdateTimerTick);

        public void StartMonitoring()
        {
            if (monitoring)
            {
                return;
            }
            monitoring = true;

            _ = updateTimer.Change(0, Timeout.Infinite);
        }

        public void StopMonitoring()
        {
            _ = updateTimer.Change(Timeout.Infinite, Timeout.Infinite);
            monitoring = false;
        }

        private void HandleUpdateTimerTick(object? _)
        {
            try
            {
                var currentProcesses = GetUsersProcesses().ToList();

                var addedProcesses = currentProcesses.Except(previousProcesses).ToList();
                var removedProcesses = previousProcesses.Except(currentProcesses).ToList();

                foreach (var addedProcess in addedProcesses)
                {
                    OnProcessCreated(addedProcess);
                }

                foreach (var removedProcess in removedProcesses)
                {
                    OnProcessTerminated(removedProcess);
                }

                previousProcesses = currentProcesses;
            }
            finally
            {
                _ = updateTimer.Change(
                    UpdateTimerInterval,
                    Timeout.Infinite);
            }
        }

        public IEnumerable<ICachedProcess> GetUsersProcesses() =>
            Static.GetUsersProcesses();

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                updateTimer.Dispose();
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
