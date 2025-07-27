namespace PowerPlanSwitcher.ProcessManagement
{
    using System;

    public interface IProcessMonitor
    {
        public event EventHandler<ProcessEventArgs>? ProcessCreated;
        public event EventHandler<ProcessEventArgs>? ProcessTerminated;

        public void StartMonitoring();
        public void StopMonitoring();

        public IEnumerable<IProcess> GetUsersProcesses();
    }
}
