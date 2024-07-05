namespace PowerPlanSwitcher.PowerManagement
{
    using Microsoft.Win32;
    using PowerPlanSwitcher.Properties;

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

            public static Guid GetPowerSchemeGuid(
                PowerLineStatus powerLineStatus) =>
                powerLineStatus switch
                {
                    PowerLineStatus.Online =>
                        Settings.Default.AcPowerSchemeGuid,
                    PowerLineStatus.Offline =>
                        Settings.Default.BatterPowerSchemeGuid,
                    PowerLineStatus.Unknown =>
                        Guid.Empty,
                    _ =>
                        Guid.Empty,
                };
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

        public Guid GetPowerSchemeGuid(PowerLineStatus powerLineStatus) =>
            Static.GetPowerSchemeGuid(powerLineStatus);

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
}
