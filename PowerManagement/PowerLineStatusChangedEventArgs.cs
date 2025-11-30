namespace PowerManagement;

using System;

public class PowerLineStatusChangedEventArgs(
    PowerLineStatus powerLineStatus)
    : EventArgs
{
    public PowerLineStatus PowerLineStatus { get; set; } = powerLineStatus;
}
