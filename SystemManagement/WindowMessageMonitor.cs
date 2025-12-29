namespace SystemManagement;

using System;
using System.Collections.Generic;
using Serilog;
using Vanara.PInvoke;

public sealed class WindowMessageMonitor(
    IEnumerable<WindowMessage> messages)
    : IWindowMessageMonitor
    , IDisposable
{
    private readonly HashSet<WindowMessage> messages = [.. messages];
    private WindowClass? wndClass;
    private bool monitoring;
    private bool disposed;

    public event EventHandler<WindowMessageEventArgs>? WindowMessageReceived;

    public void StartMonitoring()
    {
        if (monitoring)
        {
            return;
        }
        monitoring = true;

        Log.Information("Window message monitoring started");

        wndClass = new WindowClass(this);
        _ = wndClass.CreateMessageWindow();
    }

    public void StopMonitoring()
    {
        if (!monitoring)
        {
            return;
        }
        monitoring = false;

        Log.Information("Window message monitoring stopped");

        wndClass?.Destroy();
        wndClass = null;
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        wndClass?.Destroy();
    }

    private void HandleMessage(uint msg, IntPtr wParam, IntPtr lParam)
    {
        var msgEnum = (WindowMessage)msg;
        if (messages.Contains(msgEnum))
        {
            Log.Information("Window message received: {Message}", msgEnum);

            WindowMessageReceived?.Invoke(
                this,
                new WindowMessageEventArgs(msgEnum, wParam, lParam)
            );
        }
    }

    private sealed class WindowClass
    {
        private readonly WindowMessageMonitor owner;
        private readonly User32.WNDCLASSEX wc;
        private readonly string className;
        private HWND hwnd;

        public WindowClass(WindowMessageMonitor owner)
        {
            this.owner = owner;
            className = "WindowMessageMonitor_" + Guid.NewGuid();

            wc = new User32.WNDCLASSEX
            {
                cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf<User32.WNDCLASSEX>(),
                lpfnWndProc = WndProc,
                lpszClassName = className
            };

            _ = User32.RegisterClassEx(wc);
        }

        public HWND CreateMessageWindow()
        {
            hwnd = User32.CreateWindowEx(
                0,
                className,
                string.Empty,
                0,
                0, 0, 0, 0,
                HWND.NULL,
                default,
                default,
                IntPtr.Zero
            );
            _ = User32.ShowWindow(hwnd, ShowWindowCommand.SW_HIDE);
            _ = User32.ShutdownBlockReasonCreate(hwnd, "Monitoring shutdown");
            return hwnd;
        }

        public void Destroy()
        {
            if (hwnd != HWND.NULL)
            {
                _ = User32.DestroyWindow(hwnd);
                hwnd = HWND.NULL;
            }
        }

        private IntPtr WndProc(HWND hwnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            owner.HandleMessage(msg, wParam, lParam);
            return User32.DefWindowProc(hwnd, msg, wParam, lParam);
        }
    }
}
