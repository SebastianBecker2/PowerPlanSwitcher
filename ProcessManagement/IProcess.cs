namespace ProcessManagement;

public interface IProcess : IEquatable<IProcess>
{
    public string ExecutablePath { get; }
    public bool IsOwnProcess { get; }
    public int ProcessId { get; }
    public string ProcessName { get; }
    public string MainWindowTitle { get; }
    public DateTime StartTime { get; }
}
