namespace ProcessManagement;

using System.Runtime.InteropServices;
using Serilog;
using static Vanara.PInvoke.Kernel32;

public class ProcessMonitor : IDisposable, IProcessMonitor
{
#pragma warning disable CA1716 // Identifiers should not match keywords
    public static class Static
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        private static IEnumerable<PROCESSENTRY32> EnumerateProcesses()
        {
            using var snapshot = CreateToolhelp32Snapshot(TH32CS.TH32CS_SNAPPROCESS, 0);

            var entry = new PROCESSENTRY32
            {
                dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32))
            };

            if (!Process32First(snapshot, ref entry))
            {
                yield break;
            }

            do
            {
                yield return entry;
            }
            while (Process32Next(snapshot, ref entry));
        }

        public static IEnumerable<IProcess> GetUsersProcesses() =>
            EnumerateProcesses()
                .Select(Process.CreateFromProcess)
                .Where(p => p is not null && !p.IsOwnProcess)
                .Cast<Process>();
    }

    public event EventHandler<ProcessEventArgs>? ProcessCreated;
    protected virtual void OnProcessCreated(ProcessEventArgs e) =>
        ProcessCreated?.Invoke(this, e);
    protected virtual void OnProcessCreated(IProcess process) =>
        OnProcessCreated(new ProcessEventArgs(process));

    public event EventHandler<ProcessEventArgs>? ProcessTerminated;
    protected virtual void OnProcessTerminated(ProcessEventArgs e) =>
        ProcessTerminated?.Invoke(this, e);
    protected virtual void OnProcessTerminated(IProcess process) =>
        OnProcessTerminated(new ProcessEventArgs(process));

    private readonly TimeSpan updateTimerInterval = TimeSpan.FromMilliseconds(2000);
    private IEnumerable<IProcess> previousProcesses = [];
    private readonly Timer updateTimer;
    private volatile bool monitoring;
    private bool disposedValue;

    public ProcessMonitor() =>
        updateTimer = new Timer(
            HandleUpdateTimerTick,
            null,
            Timeout.InfiniteTimeSpan,
            Timeout.InfiniteTimeSpan);

    public void StartMonitoring()
    {
        if (monitoring)
        {
            return;
        }
        monitoring = true;

        _ = updateTimer.Change(TimeSpan.Zero, Timeout.InfiniteTimeSpan);
    }

    public void StopMonitoring()
    {
        _ = updateTimer.Change(
            Timeout.InfiniteTimeSpan,
            Timeout.InfiniteTimeSpan);
        monitoring = false;
    }

    private void HandleUpdateTimerTick(object? _)
    {
        try
        {
            var currentProcesses = GetUsersProcesses().ToList();

            var addedProcesses = currentProcesses
                .Except(previousProcesses)
                .ToList();
            var removedProcesses = previousProcesses
                .Except(currentProcesses)
                .ToList();

            foreach (var addedProcess in addedProcesses)
            {
                Log.Information(
                    "Process created: {ProcessId} " +
                    "{ProcessName} {ExecutablePath}",
                    addedProcess.ProcessId,
                    addedProcess.ProcessName,
                    addedProcess.ExecutablePath);
                OnProcessCreated(addedProcess);
            }

            foreach (var removedProcess in removedProcesses)
            {
                Log.Information(
                    "Process terminated: {ProcessId} " +
                    "{ProcessName} {ExecutablePath}",
                    removedProcess.ProcessId,
                    removedProcess.ProcessName,
                    removedProcess.ExecutablePath);
                OnProcessTerminated(removedProcess);
            }

            previousProcesses = currentProcesses;
        }
        finally
        {
            if (monitoring)
            {
                _ = updateTimer.Change(
                    updateTimerInterval,
                    Timeout.InfiniteTimeSpan);
            }
        }
    }

    public IEnumerable<IProcess> GetUsersProcesses() =>
        Static.GetUsersProcesses();

    protected virtual void Dispose(bool disposing)
    {
        if (disposedValue)
        {
            return;
        }

        if (disposing)
        {
            updateTimer.Dispose();
        }
        disposedValue = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
