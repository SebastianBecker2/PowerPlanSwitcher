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

        public Guid GetPowerSchemeGuid(PowerLineStatus powerLineStatus) =>
            powerLineStatus switch
            {
                PowerLineStatus.Online =>
                    PowerManagerStub.CreatePowerSchemeGuid(1_000_000),
                PowerLineStatus.Offline =>
                    PowerManagerStub.CreatePowerSchemeGuid(1_000_001),
                PowerLineStatus.Unknown =>
                    Guid.Empty,
                _ =>
                    Guid.Empty,
            };
    }
}
