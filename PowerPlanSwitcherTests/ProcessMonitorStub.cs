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
            new() { ExecutablePath = $"{i}", ProcessId = i };

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

        public void StartSimulation(
            IEnumerable<(Action Action, ICachedProcess Process)> processActions)
        {
            void Create(ICachedProcess p)
            {
                if (initialProcesses.Contains(p))
                {
                    return;
                }
                initialProcesses = initialProcesses.Append(p);
                OnProcessCreated(p);
            }

            void Terminate(ICachedProcess p)
            {
                if (!initialProcesses.Contains(p))
                {
                    return;
                }
                initialProcesses = initialProcesses.Where(ip => !ip.Equals(p));
                OnProcessTerminated(p);
            }

            Task.Factory.StartNew(() =>
            {
                foreach (var action in processActions)
                {
                    switch (action.Action)
                    {
                        case Action.Create:
                            Create(action.Process);
                            break;
                        case Action.Terminate:
                            Terminate(action.Process);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }).Wait();
        }

        public void StopMonitoring()
        { }
    }
}
