namespace PowerPlanSwitcher.ProcessManagement
{
    public interface ICachedProcess : IEquatable<ICachedProcess>
    {
        string ExecutablePath { get; }
        bool IsOwnProcess { get; }
        int ProcessId { get; }
        string ProcessName { get; }
    }
}
