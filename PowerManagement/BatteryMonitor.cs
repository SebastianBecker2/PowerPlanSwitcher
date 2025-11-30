namespace PowerManagement;

using Microsoft.Win32;

public class BatteryMonitor : IBatteryMonitor
{
#pragma warning disable CA1716 // Identifiers should not match keywords
    public static class Static
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        public static bool HasSystemBattery =>
            SystemInformation.PowerStatus.BatteryChargeStatus
            != BatteryChargeStatus.NoSystemBattery;

        public static PowerLineStatus PowerLineStatus =>
            SystemInformation.PowerStatus.PowerLineStatus;
    }

    public event EventHandler<PowerLineStatusChangedEventArgs>?
        PowerLineStatusChanged;
    protected virtual void OnPowerLineStatusChanged(
        PowerLineStatusChangedEventArgs e) =>
        PowerLineStatusChanged?.Invoke(this, e);
    protected virtual void OnPowerLineStatusChanged(
        PowerLineStatus powerLineStatus) =>
        OnPowerLineStatusChanged(
            new PowerLineStatusChangedEventArgs(powerLineStatus));

    public BatteryMonitor() =>
        SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

    public bool HasSystemBattery =>
        Static.HasSystemBattery;

    public PowerLineStatus PowerLineStatus =>
        Static.PowerLineStatus;

    private void SystemEvents_PowerModeChanged(
        object sender,
        PowerModeChangedEventArgs e)
    {
        if (e.Mode != PowerModes.StatusChange)
        {
            return;
        }

        if (PowerLineStatus == PowerLineStatus.Unknown)
        {
            return;
        }

        OnPowerLineStatusChanged(PowerLineStatus);
    }
}
