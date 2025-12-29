namespace PowerManagement;

using Serilog;
using Vanara.Extensions;
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

//using static Vanara.PInvoke.User32;

public class BatteryMonitor : IBatteryMonitor, IDisposable
{
#pragma warning disable CA1716 // Identifiers should not match keywords
    public static class Static
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        private static Kernel32.SYSTEM_POWER_STATUS GetSystemPowerStatus() =>
            Kernel32.GetSystemPowerStatus(out var status)
                ? status
                : throw new InvalidOperationException(
                    "Unable to get system power status.");

        public static bool HasSystemBattery =>
            !GetSystemPowerStatus().BatteryFlag.HasFlag(
                Kernel32.BATTERY_STATUS.BATTERY_NONE);

        public static PowerLineStatus PowerLineStatus =>
            GetSystemPowerStatus().ACLineStatus switch
            {
                Kernel32.AC_STATUS.AC_OFFLINE => PowerLineStatus.Offline,
                Kernel32.AC_STATUS.AC_ONLINE => PowerLineStatus.Online,
                Kernel32.AC_STATUS.AC_LINE_BACKUP_POWER => PowerLineStatus.Unknown,
                Kernel32.AC_STATUS.AC_UNKNOWN => PowerLineStatus.Unknown,
                _ => PowerLineStatus.Unknown,
            };
    }

    private readonly HWND hwnd;
    private readonly SafeHPOWERSETTINGNOTIFY hNotifyPowerSource;
    private bool disposedValue;

    public event EventHandler<PowerLineStatusChangedEventArgs>?
        PowerLineStatusChanged;
    protected virtual void OnPowerLineStatusChanged(
        PowerLineStatusChangedEventArgs e) =>
        PowerLineStatusChanged?.Invoke(this, e);
    protected virtual void OnPowerLineStatusChanged(
        PowerLineStatus powerLineStatus) =>
        OnPowerLineStatusChanged(
            new PowerLineStatusChangedEventArgs(powerLineStatus));

    public BatteryMonitor()
    {
        hwnd = CreateMessageWindow();

        // Register for AC/DC power source notifications
        hNotifyPowerSource = RegisterPowerSettingNotification(
            hwnd.DangerousGetHandle(),
            PowrProf.GUID_ACDC_POWER_SOURCE,
            DEVICE_NOTIFY.DEVICE_NOTIFY_WINDOW_HANDLE);
    }

    private HWND CreateMessageWindow()
    {
        var wndClass = new WNDCLASS
        {
            lpszClassName = "PowerMonitorWnd",
            lpfnWndProc = WndProc
        };
        RegisterClass(wndClass);

        return CreateWindowEx(0, wndClass.lpszClassName, "",
            0, 0, 0, 0, 0, HWND.HWND_MESSAGE, default, default, IntPtr.Zero);
    }

    private IntPtr WndProc(HWND hwnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg == (uint)WindowMessage.WM_POWERBROADCAST &&
            wParam.ToInt32() == (int)PowerBroadcastType.PBT_POWERSETTINGCHANGE)
        {
            var data = lParam.ToStructure<POWERBROADCAST_SETTING>();
            if (data.PowerSetting == PowrProf.GUID_ACDC_POWER_SOURCE)
            {
                Log.Information("Power line status changed: {Status}", PowerLineStatus);
                OnPowerLineStatusChanged(PowerLineStatus);
            }
        }
        return DefWindowProc(hwnd, msg, wParam, lParam);
    }

    public bool HasSystemBattery =>
        Static.HasSystemBattery;

    public PowerLineStatus PowerLineStatus =>
        Static.PowerLineStatus;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                if (hNotifyPowerSource != IntPtr.Zero)
                {
                    UnregisterPowerSettingNotification(hNotifyPowerSource);
                }

                DestroyWindow(hwnd);
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
