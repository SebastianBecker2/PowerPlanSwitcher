namespace PowerPlanSwitcher.ProcessManagement
{
    using System;

    public class ProcessEventArgs(IProcess process) : EventArgs
    {
        public IProcess Process { get; set; } = process;
    }
}
