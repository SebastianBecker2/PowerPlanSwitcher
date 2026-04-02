namespace ProcessManagement;

using System.Runtime.InteropServices;
using Serilog;
using static Vanara.PInvoke.Kernel32;

public class ProcessMonitor : IDisposable, IProcessMonitor
{
    private readonly record struct ProcessIdentity(int ProcessId, DateTime StartTime);

    public static class Api
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

    private readonly WindowMessageTimer.Timer timer;
    private Dictionary<ProcessIdentity, IProcess> previousProcesses = [];
    private readonly TimeSpan summaryInterval = TimeSpan.FromSeconds(30);
    private DateTime lastSummaryTimestampUtc = DateTime.UtcNow;
    private int createdSinceSummary;
    private int terminatedSinceSummary;
    private bool disposedValue;

    public ProcessMonitor(TimeSpan updateInterval)
    {
        timer = new(updateInterval);
        timer.Tick += Timer_Tick;
    }

    private void Timer_Tick()
    {
        if (ProcessCreated is null && ProcessTerminated is null)
        {
            return;
        }

        var tickStart = DateTime.UtcNow;
        var currentProcesses = CreateProcessMap(GetUsersProcesses());
        var logProcessEvents = Log.IsEnabled(Serilog.Events.LogEventLevel.Debug);

        foreach (var (identity, addedProcess) in currentProcesses)
        {
            if (previousProcesses.ContainsKey(identity))
            {
                continue;
            }

            if (logProcessEvents)
            {
                Log.ForContext("EventType", "Process.Created")
                    .Debug(
                    "Process created: {ProcessId} " +
                    "{ProcessName} {ExecutablePath}",
                    addedProcess.ProcessId,
                    addedProcess.ProcessName,
                    addedProcess.ExecutablePath);
            }
            createdSinceSummary++;
            OnProcessCreated(addedProcess);
        }

        foreach (var (identity, removedProcess) in previousProcesses)
        {
            if (currentProcesses.ContainsKey(identity))
            {
                continue;
            }

            if (logProcessEvents)
            {
                Log.ForContext("EventType", "Process.Terminated")
                    .Debug(
                    "Process terminated: {ProcessId} " +
                    "{ProcessName} {ExecutablePath}",
                    removedProcess.ProcessId,
                    removedProcess.ProcessName,
                    removedProcess.ExecutablePath);
            }
            terminatedSinceSummary++;
            OnProcessTerminated(removedProcess);
        }

        previousProcesses = currentProcesses;

        var now = DateTime.UtcNow;
        if (now - lastSummaryTimestampUtc < summaryInterval)
        {
            return;
        }

        var elapsedMs = (now - tickStart).TotalMilliseconds;
        if (createdSinceSummary > 0
            || terminatedSinceSummary > 0
            || Log.IsEnabled(Serilog.Events.LogEventLevel.Debug))
        {
            Log.ForContext("EventType", "Process.Summary")
                .Information(
                    "Process monitor summary: Created={CreatedCount} Terminated={TerminatedCount} Tracked={TrackedCount} TickDurationMs={TickDurationMs}",
                    createdSinceSummary,
                    terminatedSinceSummary,
                    previousProcesses.Count,
                    elapsedMs);
        }

        createdSinceSummary = 0;
        terminatedSinceSummary = 0;
        lastSummaryTimestampUtc = now;
    }

    public void StartMonitoring()
    {
        previousProcesses = CreateProcessMap(GetUsersProcesses());
        timer.Start();
        Log.ForContext("EventType", "Process.MonitoringStarted")
            .Information("Process monitoring started");
    }

    public void StopMonitoring()
    {
        timer.Stop();
        Log.ForContext("EventType", "Process.MonitoringStopped")
            .Information("Process monitoring stopped");
    }

    public IEnumerable<IProcess> GetUsersProcesses() =>
        Api.GetUsersProcesses();

    private static ProcessIdentity GetProcessIdentity(IProcess process) =>
        new(process.ProcessId, process.StartTime);

    private static Dictionary<ProcessIdentity, IProcess> CreateProcessMap(
        IEnumerable<IProcess> processes)
    {
        var map = new Dictionary<ProcessIdentity, IProcess>();
        foreach (var process in processes)
        {
            map[GetProcessIdentity(process)] = process;
        }

        return map;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposedValue)
        {
            return;
        }

        if (disposing)
        {
            StopMonitoring();
            timer.Dispose();
        }
        disposedValue = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

