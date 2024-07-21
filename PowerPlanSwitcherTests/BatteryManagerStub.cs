namespace PowerPlanSwitcherTests
{
    using System;
    using System.Windows.Forms;
    using PowerPlanSwitcher.PowerManagement;

    internal class BatteryManagerStub(bool hasSystemBattery = true)
        : IBatteryMonitor
    {
        public event EventHandler<PowerLineStatusChangedEventArgs>? PowerLineStatusChanged;
        protected virtual void OnPowerLineStatusChanged() =>
            PowerLineStatusChanged?.Invoke(
                this,
                new PowerLineStatusChangedEventArgs(powerLineStatus));

        public bool HasSystemBattery { get; set; } = hasSystemBattery;

        private PowerLineStatus powerLineStatus = PowerLineStatus.Online;
        public PowerLineStatus PowerLineStatus
        {
            get => powerLineStatus; set
            {
                powerLineStatus = value;
                OnPowerLineStatusChanged();
            }
        }
    }
}
