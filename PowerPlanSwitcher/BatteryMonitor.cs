namespace PowerPlanSwitcher
{
    using PowerPlanSwitcher.Properties;
    using static Vanara.PInvoke.Kernel32;

    public class BatteryMonitor
    {
        private static Guid AcPowerSchemeGuid =>
            Settings.Default.AcPowerSchemeGuid;
        private static Guid BatterPowerSchemeGuid =>
            Settings.Default.BatterPowerSchemeGuid;

        private static bool toggleMark = true;

        public static bool HasSystemBattery() =>
            SystemInformation.PowerStatus.BatteryChargeStatus
            != BatteryChargeStatus.NoSystemBattery;

        public static void Initialize()
        {
            if (!GetSystemPowerStatus(out var powerStatus))
            {
                return;
            }

            if (!toggleMark
                && BatterPowerSchemeGuid != Guid.Empty
                && AcPowerSchemeGuid == Guid.Empty
                && powerStatus.ACLineStatus == AC_STATUS.AC_ONLINE)
            {
                toggleMark = true;
            }

            if (toggleMark
                && AcPowerSchemeGuid != Guid.Empty
                && BatterPowerSchemeGuid == Guid.Empty
                && powerStatus.ACLineStatus == AC_STATUS.AC_OFFLINE)
            {
                toggleMark = false;
            }
        }

        public static Guid GetPowerSchemeGuid()
        {
            if (!HasSystemBattery())
            {
                return Guid.Empty;
            }

            if (!GetSystemPowerStatus(out var powerStatus))
            {
                return Guid.Empty;
            }

            var activePowerSchemeGuid = PowerManager.GetActivePowerSchemeGuid();

            if (!toggleMark
                && AcPowerSchemeGuid != Guid.Empty
                && powerStatus.ACLineStatus == AC_STATUS.AC_ONLINE
                && activePowerSchemeGuid != AcPowerSchemeGuid)
            {
                toggleMark = true;
                return AcPowerSchemeGuid;
            }

            if (toggleMark
                && BatterPowerSchemeGuid != Guid.Empty
                && powerStatus.ACLineStatus == AC_STATUS.AC_OFFLINE
                && activePowerSchemeGuid != BatterPowerSchemeGuid)
            {
                toggleMark = false;
                return BatterPowerSchemeGuid;
            }

            Initialize();

            return Guid.Empty;
        }
    }
}
