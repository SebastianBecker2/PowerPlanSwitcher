namespace PowerPlanSwitcherTests
{
    using System;
    using System.Collections.Generic;
    using PowerPlanSwitcher.ProcessManagement;

    internal class ProcessMonitorStub(
        IEnumerable<ICachedProcess> initialProcesses,
        IEnumerable<ProcessMonitorStub.Action> processActions)
        : IProcessMonitor
    {
        public enum ActionType
        {
            Create,
            Terminate,
        }

        public class Action(ActionType type, ICachedProcess process)
        {
            public ActionType ActionType { get; set; } = type;
            public ICachedProcess Process { get; set; } = process;
        }

        public event EventHandler<ProcessEventArgs>? ProcessCreated;
        protected virtual void OnProcessCreated(ICachedProcess process) =>
            ProcessCreated?.Invoke(this, new ProcessEventArgs(process));
        public event EventHandler<ProcessEventArgs>? ProcessTerminated;
        protected virtual void OnProcessTerminated(ICachedProcess process) =>
            ProcessTerminated?.Invoke(this, new ProcessEventArgs(process));

        private Task? actionTask;

        public IEnumerable<ICachedProcess> GetUsersProcesses() =>
            initialProcesses;

        public void StartMonitoring() =>
            actionTask = Task.Factory.StartNew(() =>
            {
                foreach (var action in processActions)
                {
                    switch (action.ActionType)
                    {
                        case ActionType.Create:
                            OnProcessCreated(action.Process);
                            break;
                        case ActionType.Terminate:
                            OnProcessTerminated(action.Process);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            });

        public Task? GetActionTask() => actionTask;

        public bool IsRunning() => actionTask is not null;

        public void StopMonitoring()
        { }
    }
}
