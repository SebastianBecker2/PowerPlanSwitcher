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
        List<IProcess> initialProcesses)
        : IProcessMonitor
    {
        public static (Action Action, IProcess Process) CreateAction(
            Action action,
            int i) =>
            (action, CreateProcess(i));

        public static ProcessStub CreateProcess(int i) =>
            new() { ExecutablePath = $"{i}", ProcessId = i };

        public event EventHandler<ProcessEventArgs>? ProcessCreated;
        protected virtual void OnProcessCreated(IProcess process) =>
            ProcessCreated?.Invoke(this, new ProcessEventArgs(process));
        public event EventHandler<ProcessEventArgs>? ProcessTerminated;
        protected virtual void OnProcessTerminated(IProcess process) =>
            ProcessTerminated?.Invoke(this, new ProcessEventArgs(process));

        public IEnumerable<IProcess> GetUsersProcesses() =>
            initialProcesses;

        public void StartMonitoring()
        {
            foreach (var p in initialProcesses)
            {
                OnProcessCreated(p);
            }
        }

        public void StartSimulation(
            IEnumerable<(Action Action, IProcess Process)> processActions)
        {
            void Create(IProcess p)
            {
                initialProcesses.Add(p);
                OnProcessCreated(p);
            }

            void Terminate(IProcess p)
            {
                if (!initialProcesses.Remove(p))
                {
                    return;
                }
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
