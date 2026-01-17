namespace SystemManagement;

using System.Runtime.InteropServices;
using Vanara.PInvoke;

public class SystemManager : ISystemManager
{
#pragma warning disable CA1716 // Identifiers should not match keywords
    public static class Static
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        public static bool IsFullscreenAppActive()
        {
            var hwnd = User32.GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
            {
                return false;
            }

            // Get window rect
            _ = User32.GetWindowRect(hwnd, out var rect);

            // Get the monitor the window is on
            var hMonitor = User32.MonitorFromWindow(
                hwnd,
                User32.MonitorFlags.MONITOR_DEFAULTTONEAREST);
            if (hMonitor == IntPtr.Zero)
            {
                return false;
            }

            // Query monitor info
            var mi = new User32.MONITORINFO
            {
                cbSize = (uint)Marshal.SizeOf<User32.MONITORINFO>()
            };

            if (!User32.GetMonitorInfo(hMonitor, ref mi))
            {
                return false;
            }

            var bounds = mi.rcMonitor;

            const int tolerance = 2;

            var coversScreen =
                Math.Abs(rect.left - bounds.left) <= tolerance &&
                Math.Abs(rect.top - bounds.top) <= tolerance &&
                Math.Abs(rect.right - bounds.right) <= tolerance &&
                Math.Abs(rect.bottom - bounds.bottom) <= tolerance;

            return coversScreen;
        }
    }

    public bool IsFullscreenAppActive() => Static.IsFullscreenAppActive();
}
