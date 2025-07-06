namespace PowerPlanSwitcher.ProcessManagement
{
    public interface ICachedProcess : IEquatable<ICachedProcess>
    {
        public string ExecutablePath { get; }
        public bool IsOwnProcess { get; }
        public int ProcessId { get; }
        public string ProcessName { get; }
    }
}
