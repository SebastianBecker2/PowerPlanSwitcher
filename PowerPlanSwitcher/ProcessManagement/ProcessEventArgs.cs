namespace PowerPlanSwitcher.ProcessManagement
{
    using System;

    public class ProcessEventArgs(ICachedProcess process) : EventArgs
    {
        public ICachedProcess Process { get; set; } = process;
    }
}
