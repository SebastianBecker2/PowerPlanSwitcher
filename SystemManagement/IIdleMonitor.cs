namespace SystemManagement;

using System;

public interface IIdleMonitor
{
    public event EventHandler<IdleTimeChangedEventArgs>? IdleTimeChanged;

    public void StartMonitoring();

    public void StopMonitoring();

    public TimeSpan GetIdleTime();
}
