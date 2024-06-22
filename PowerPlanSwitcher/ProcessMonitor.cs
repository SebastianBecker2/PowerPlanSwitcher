namespace PowerPlanSwitcher
{
    using System.Diagnostics;
    using System.Management;

    internal class ProcessEventArgs(CachedProcess process) : EventArgs
    {
        public CachedProcess Process { get; set; } = process;
    }

    internal class ProcessMonitor : IDisposable
    {
        public event EventHandler<ProcessEventArgs>? ProcessCreated;
        protected virtual void OnProcessCreated(ProcessEventArgs e) =>
            ProcessCreated?.Invoke(this, e);
        protected virtual void OnProcessCreated(CachedProcess process) =>
            OnProcessCreated(new ProcessEventArgs(process));

        public event EventHandler<ProcessEventArgs>? ProcessTerminated;
        protected virtual void OnProcessTerminated(ProcessEventArgs e) =>
            ProcessTerminated?.Invoke(this, e);
        protected virtual void OnProcessTerminated(CachedProcess process) =>
            OnProcessTerminated(new ProcessEventArgs(process));

        private bool disposedValue;
        private readonly ManagementEventWatcher processCreationWatcher = new();
        private readonly ManagementEventWatcher processTerminationWatcher = new();
        private bool monitoring;

        public void StartMonitoring()
        {
            if (monitoring)
            {
                return;
            }
            monitoring = true;

            processCreationWatcher.Query = new WqlEventQuery(
                "__InstanceCreationEvent",
                new TimeSpan(0, 0, 5),
                "TargetInstance isa \"Win32_Process\"");
            processCreationWatcher.EventArrived
                += ProcessStartWatcher_EventArrived;
            processCreationWatcher.Start();

            processTerminationWatcher.Query = new WqlEventQuery(
                "__InstanceDeletionEvent",
                new TimeSpan(0, 0, 5),
                "TargetInstance isa \"Win32_Process\"");
            processTerminationWatcher.EventArrived
                += ProcessEndWatcher_EventArrived;
            processTerminationWatcher.Start();
        }

        public void StopMonitoring()
        {
            processCreationWatcher.Stop();
            processTerminationWatcher.Stop();
            monitoring = false;
        }

        private void ProcessStartWatcher_EventArrived(
            object sender,
            EventArrivedEventArgs e)
        {
            var process = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            var cachedProcess = CachedProcess.CreateFromWin32Process(process);
            if (cachedProcess is null)
            {
                return;
            }
            OnProcessCreated(cachedProcess);
        }

        private void ProcessEndWatcher_EventArrived(
            object sender,
            EventArrivedEventArgs e)
        {
            var process = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            var cachedProcess = CachedProcess.CreateFromWin32Process(process);
            if (cachedProcess is null)
            {
                return;
            }
            OnProcessTerminated(cachedProcess);
        }

        public static IEnumerable<CachedProcess> GetUsersProcesses() =>
            Process.GetProcesses()
                .Select(CachedProcess.CreateFromProcess)
                .Where(p => p is not null && !p.IsOwnProcess)
                .Cast<CachedProcess>();

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                processCreationWatcher?.Dispose();
                processTerminationWatcher?.Dispose();
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
