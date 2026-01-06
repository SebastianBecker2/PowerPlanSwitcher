namespace SystemManagement;

using System.Runtime.InteropServices;
using Serilog;
using Vanara.PInvoke;

public class IdleMonitor : IDisposable, IIdleMonitor
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
    public static class Static
    {
        public static TimeSpan GetIdleTime()
        {
            var info = new User32.LASTINPUTINFO
            {
                cbSize = (uint)Marshal.SizeOf<User32.LASTINPUTINFO>()
            };

            if (!User32.GetLastInputInfo(ref info))
            {
                return TimeSpan.Zero;
            }

            var idleTicks = (uint)Environment.TickCount - info.dwTime;
            return TimeSpan.FromMilliseconds(idleTicks);
        }
    }

    public event EventHandler<IdleTimeChangedEventArgs>? IdleTimeChanged;
    protected virtual void OnIdleTimeChanged(IdleTimeChangedEventArgs e) =>
        IdleTimeChanged?.Invoke(this, e);
    protected virtual void OnIdleTimeChanged(TimeSpan idleTime) =>
        OnIdleTimeChanged(new IdleTimeChangedEventArgs(idleTime));

    private TimeSpan PollingInterval { get; } = TimeSpan.FromSeconds(1);
    private readonly WindowMessageTimer.Timer pollingTimer;
    private volatile bool monitoring;
    private bool disposedValue;

    public IdleMonitor()
    {
        pollingTimer = new(PollingInterval);
        pollingTimer.Tick += Timer_Tick;
    }

    public void StartMonitoring()
    {
        if (monitoring)
        {
            return;
        }
        monitoring = true;

        pollingTimer.Start();

        Log.Information("Idle time monitoring started");
    }

    public void StopMonitoring()
    {
        pollingTimer.Stop();
        monitoring = false;

        Log.Information("Idle time monitoring stopped");
    }

    public TimeSpan GetIdleTime() => Static.GetIdleTime();

    private void Timer_Tick()
    {
        if (IdleTimeChanged is null)
        {
            return;
        }

        var idleTime = Static.GetIdleTime();
        OnIdleTimeChanged(idleTime);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                pollingTimer.Dispose();
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
