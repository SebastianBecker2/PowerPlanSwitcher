namespace PowerPlanSwitcher
{
    using Microsoft.Win32;

    public class BatteryMonitor
    {
        public event EventHandler<EventArgs>? PowerLineStatusChanged;
        protected virtual void OnPowerLineStatusChanged(EventArgs e) =>
            PowerLineStatusChanged?.Invoke(this, e);
        protected virtual void OnPowerLineStatusChanged() =>
            OnPowerLineStatusChanged(new EventArgs());

        public BatteryMonitor() =>
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

        public static bool HasSystemBattery =>
            SystemInformation.PowerStatus.BatteryChargeStatus
            != BatteryChargeStatus.NoSystemBattery;

        public static PowerLineStatus PowerLineStatus =>
            SystemInformation.PowerStatus.PowerLineStatus;

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

            OnPowerLineStatusChanged();
        }
    }
}
