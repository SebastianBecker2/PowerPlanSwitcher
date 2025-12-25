namespace SystemManagement;

using System;

public interface IWindowMessageMonitor
{
    event EventHandler<WindowMessageEventArgs>? WindowMessageReceived;

    void StartMonitoring();
    void StopMonitoring();
}