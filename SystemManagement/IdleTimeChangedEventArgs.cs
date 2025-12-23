namespace SystemManagement;

public class IdleTimeChangedEventArgs(TimeSpan idleTime) : EventArgs
{
    public TimeSpan IdleTime { get; set; } = idleTime;
}
