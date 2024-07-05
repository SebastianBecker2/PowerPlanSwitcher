namespace PowerPlanSwitcher.PowerManagement
{
    using System;

    public interface IBatteryMonitor
    {
        event EventHandler<PowerLineStatusChangedEventArgs>?
            PowerLineStatusChanged;

        bool HasSystemBattery { get; }

        PowerLineStatus PowerLineStatus { get; }

        Guid GetPowerSchemeGuid(PowerLineStatus powerLineStatus);
    }
}
