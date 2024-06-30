namespace PowerPlanSwitcherTests
{
    using System;
    using System.Collections.Generic;
    using PowerPlanSwitcher.ProcessManagement;

    public enum Action
    {
        Create,
        Terminate,
    }

    internal partial class ProcessMonitorStub(
        IEnumerable<ICachedProcess> initialProcesses)
        : IProcessMonitor
    {
        public static (Action Action, ICachedProcess Process) CreateAction(
            Action action,
            int i) =>
            (action, CreateProcess(i));

        public static CachedProcessStub CreateProcess(int i) =>
            new() { ExecutablePath = $"{i}" };

        public static List<CachedProcessStub> CreateProcesses(
            int start,
            int count) =>
            [.. Enumerable
                .Range(start, count)
                .Select(CreateProcess)];

        public event EventHandler<ProcessEventArgs>? ProcessCreated;
        protected virtual void OnProcessCreated(ICachedProcess process) =>
            ProcessCreated?.Invoke(this, new ProcessEventArgs(process));
        public event EventHandler<ProcessEventArgs>? ProcessTerminated;
        protected virtual void OnProcessTerminated(ICachedProcess process) =>
            ProcessTerminated?.Invoke(this, new ProcessEventArgs(process));

        public IEnumerable<ICachedProcess> GetUsersProcesses() =>
            initialProcesses;

        public void StartMonitoring() { }

        public Task StartSimulation(
            IEnumerable<(Action Action, ICachedProcess Process)> processActions) =>
            Task.Factory.StartNew(() =>
            {
                foreach (var action in processActions)
                {
                    switch (action.Action)
                    {
                        case Action.Create:
                            OnProcessCreated(action.Process);
                            break;
                        case Action.Terminate:
                            OnProcessTerminated(action.Process);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            });

        public void StopMonitoring()
        { }
    }
}
