namespace PowerPlanSwitcherTests
{
    using System;
    using System.Windows.Forms;
    using PowerPlanSwitcher.PowerManagement;

    internal class BatteryMonitorStub(
        PowerLineStatus powerLineStatus = PowerLineStatus.Online,
        bool hasSystemBattery = true)
        : IBatteryMonitor
    {
        public event EventHandler<PowerLineStatusChangedEventArgs>? PowerLineStatusChanged;
        protected virtual void OnPowerLineStatusChanged() =>
            PowerLineStatusChanged?.Invoke(
                this,
                new PowerLineStatusChangedEventArgs(powerLineStatus));

        public bool HasSystemBattery { get; set; } = hasSystemBattery;

        private PowerLineStatus powerLineStatus = powerLineStatus;
        public PowerLineStatus PowerLineStatus
        {
            get => powerLineStatus;
            set
            {
                powerLineStatus = value;
                OnPowerLineStatusChanged();
            }
        }
    }
}
