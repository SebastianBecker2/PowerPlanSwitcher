namespace PowerPlanSwitcher.PowerManagement
{
    using System;

    public interface IBatteryMonitor
    {
        public event EventHandler<PowerLineStatusChangedEventArgs>?
            PowerLineStatusChanged;

        public bool HasSystemBattery { get; }

        public PowerLineStatus PowerLineStatus { get; }
    }
}
