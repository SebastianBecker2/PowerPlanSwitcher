namespace PowerPlanSwitcher.ProcessManagement
{
    using System;

    public interface IProcessMonitor
    {
        event EventHandler<ProcessEventArgs>? ProcessCreated;
        event EventHandler<ProcessEventArgs>? ProcessTerminated;

        void StartMonitoring();
        void StopMonitoring();

        IEnumerable<ICachedProcess> GetUsersProcesses();
    }
}
