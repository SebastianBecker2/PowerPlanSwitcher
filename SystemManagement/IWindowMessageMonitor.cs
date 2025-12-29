namespace SystemManagement;

using System;

public interface IWindowMessageMonitor
{
    public event EventHandler<WindowMessageEventArgs>? WindowMessageReceived;

    public void StartMonitoring();
    public void StopMonitoring();
}
