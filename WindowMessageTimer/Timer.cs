namespace WindowMessageTimer;

using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using Vanara.PInvoke;

public sealed class Timer(TimeSpan interval) : IDisposable
{
    private static readonly ConcurrentDictionary<HWND, Timer> Timers = new();

    private static readonly string ClassName = "WindowMessageTimerClass";
    private static readonly string ThreadName = "WindowMessageTimerThread";

    private static readonly object ClassRegistrationLock = new();
    private static bool classRegistered;
    private static User32.WindowProc? staticWndProcDelegate;

    private TimeSpan interval = interval;
    private readonly ManualResetEventSlim ready = new();

    private Thread? thread;
    private HWND hwnd;
    private int inTick;

    private const uint TimerId = 1;
    private const User32.WindowMessage WmChangeInterval =
        User32.WindowMessage.WM_APP + 1;

    public Timer(int milliseconds)
        : this(TimeSpan.FromMilliseconds(milliseconds))
    {
    }

    public event Action? Tick;

    public void Start()
    {
        if (thread != null)
        {
            return;
        }

        thread = new Thread(MessagePumpThread)
        {
            IsBackground = true,
            Name = ThreadName,
        };

        thread.Start();
        ready.Wait();
    }

    public void ChangeInterval(TimeSpan newInterval)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(
            newInterval,
            TimeSpan.Zero);

        interval = newInterval;

        if (hwnd != HWND.NULL)
        {
            _ = User32.PostMessage(
                hwnd,
                WmChangeInterval,
                IntPtr.Zero,
                IntPtr.Zero);
        }
    }

    private static void EnsureWindowClassRegistered()
    {
        if (classRegistered)
        {
            return;
        }

        lock (ClassRegistrationLock)
        {
            if (classRegistered)
            {
                return;
            }

            staticWndProcDelegate = StaticWndProc;

            var wc = new User32.WNDCLASSEX
            {
                cbSize = (uint)Marshal.SizeOf<User32.WNDCLASSEX>(),
                lpfnWndProc = staticWndProcDelegate,
                hInstance = Kernel32.GetModuleHandle(),
                lpszClassName = ClassName,
            };

            _ = User32.RegisterClassEx(in wc);
            classRegistered = true;
        }
    }

    private void MessagePumpThread()
    {
        EnsureWindowClassRegistered();

        hwnd = User32.CreateWindowEx(
            0,
            ClassName,
            null,
            0,
            0, 0, 0, 0,
            HWND.HWND_MESSAGE,
            IntPtr.Zero,
            Kernel32.GetModuleHandle(),
            IntPtr.Zero
        );

        if (hwnd == HWND.NULL)
        {
            throw new InvalidOperationException(
                "Failed to create message window.");
        }

        if (!Timers.TryAdd(hwnd, this))
        {
            throw new InvalidOperationException(
                "Failed to register timer instance for window.");
        }

        ready.Set();

        ApplyInterval();

        while (User32.GetMessage(out var msg, HWND.NULL, 0, 0) > 0)
        {
            _ = User32.TranslateMessage(in msg);
            _ = User32.DispatchMessage(in msg);
        }

        _ = Timers.TryRemove(hwnd, out _);
    }

    private void ApplyInterval()
    {
        var intervalMs = (uint)interval.TotalMilliseconds;

        if (User32.SetTimer(hwnd, TimerId, intervalMs, null) == 0)
        {
            throw new InvalidOperationException("SetTimer failed.");
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "Missing cases are ignored")]
    private static IntPtr StaticWndProc(
        HWND hwnd,
        uint msg,
        IntPtr wParam,
        IntPtr lParam)
    {
        if (!Timers.TryGetValue(hwnd, out var timer))
        {
            return User32.DefWindowProc(hwnd, msg, wParam, lParam);
        }

        switch (msg)
        {
            case TimerId:
                break;
        }

        switch ((User32.WindowMessage)msg)
        {
            case User32.WindowMessage.WM_TIMER:
            {
                if (wParam == (IntPtr)TimerId)
                {
                    if (Interlocked.Exchange(ref timer.inTick, 1) == 1)
                    {
                        return IntPtr.Zero;
                    }

                    try
                    {
                        timer.Tick?.Invoke();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        timer.inTick = 0;
                    }
                }

                break;
            }

            case WmChangeInterval:
            {
                timer.ApplyInterval();
                break;
            }

            case User32.WindowMessage.WM_CLOSE:
            {
                _ = User32.DestroyWindow(hwnd);
                return IntPtr.Zero;
            }

            case User32.WindowMessage.WM_DESTROY:
            {
                User32.PostQuitMessage(0);
                return IntPtr.Zero;
            }
        }

        return User32.DefWindowProc(hwnd, msg, wParam, lParam);
    }

    public void Stop()
    {
        if (thread == null)
        {
            return;
        }

        if (hwnd != HWND.NULL)
        {
            _ = User32.PostMessage(
                hwnd,
                (uint)User32.WindowMessage.WM_CLOSE,
                IntPtr.Zero,
                IntPtr.Zero);
        }

        thread.Join();
        thread = null;
        hwnd = HWND.NULL;
    }

    public void Dispose() => Stop();
}
