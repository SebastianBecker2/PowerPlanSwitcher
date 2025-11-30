namespace DebugCmd;

using PowerManagement;

internal class BatteryMonitorTest : IBatteryMonitor
{
    public bool HasSystemBattery => true;

    public PowerLineStatus PowerLineStatus => PowerLineStatus.Online;

    public event EventHandler<PowerLineStatusChangedEventArgs>? PowerLineStatusChanged;
}
