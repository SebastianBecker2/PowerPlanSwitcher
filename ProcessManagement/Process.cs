namespace ProcessManagement;

using System;
using System.Text;
using static Vanara.PInvoke.Kernel32;

public class Process : IProcess
{
    private readonly record struct ProcessKey(int Pid, DateTime StartTime);
    private static readonly Dictionary<ProcessKey, Process> Cache = [];
    private static readonly object CacheLock = new();

    private static readonly Process OwnProcess =
        CreateFromProcess(System.Diagnostics.Process.GetCurrentProcess())
        ?? throw new InvalidOperationException(
            "Unable to determine own process!");

    public static Process? CreateFromProcess(PROCESSENTRY32 processEntry)
    {
        try
        {
            var processId = (int)processEntry.th32ProcessID;
            var startTime = TryGetCreationTime(processEntry.th32ProcessID);
            if (startTime is null)
            {
                return null;
            }

            var key = new ProcessKey(processId, startTime.Value);
            lock (CacheLock)
            {
                if (Cache.TryGetValue(key, out var cachedProcess))
                {
                    return cachedProcess;
                }
            }

            var executablePath = TryGetExecutablePath(processEntry.th32ProcessID);
            if (executablePath is null)
            {
                return null;
            }

            var mainWindowTitle = TryGetMainWindowTitle(processEntry.th32ProcessID);
            if (mainWindowTitle is null)
            {
                return null;
            }

            var p = new Process
            {
                ProcessId = processId,
                ProcessName = processEntry.szExeFile,
                StartTime = startTime.Value,
            };

            lock (CacheLock)
            {
                Cache[key] = p;
            }
            return p;
        }
        catch
        {
            return null;
        }
    }

    public static Process? CreateFromProcess(
        System.Diagnostics.Process process)
    {
        try
        {
            var key = new ProcessKey(process.Id, process.StartTime);
            lock (CacheLock)
            {
                if (Cache.TryGetValue(key, out var cachedProcess))
                {
                    return cachedProcess;
                }
            }

            var p = new Process
            {
                ProcessId = process.Id,
                ProcessName = process.ProcessName,
                StartTime = process.StartTime,
                ExecutablePath =
                        process.MainModule!.FileName.ToLowerInvariant(),
                MainWindowTitle = process.MainWindowTitle ?? "",
            };

            lock (CacheLock)
            {
                Cache[key] = p;
            }
            return p;
        }
        catch
        {
            return null;
        }
    }

    public override bool Equals(object? obj) =>
        Equals(obj as IProcess);

    public bool Equals(IProcess? other) =>
        other is not null
        && ProcessId == other.ProcessId
        && ProcessName == other.ProcessName
        && ExecutablePath == other.ExecutablePath
        && MainWindowTitle == other.MainWindowTitle
        && StartTime == other.StartTime;

    public override int GetHashCode() =>
        HashCode.Combine(
            ProcessId,
            ProcessName,
            ExecutablePath,
            MainWindowTitle,
            StartTime);

    public int ProcessId { get; private set; }
    public string ProcessName { get; private set; } = "";
    public string ExecutablePath { get; private set; } = "";
    public string MainWindowTitle { get; private set; } = "";
    public DateTime StartTime { get; private set; }
    public bool IsOwnProcess => ProcessId == OwnProcess.ProcessId;

    protected Process() { }

    private static DateTime? TryGetCreationTime(uint pid)
    {
        var access = (uint)ProcessAccess.PROCESS_QUERY_LIMITED_INFORMATION;

        using var hProc = OpenProcess(access, false, pid);
        if (hProc.IsInvalid)
        {
            return null;
        }

        if (!GetProcessTimes(hProc, out var ct, out _, out _, out _))
        {
            return null;
        }

        // Combine into unsigned 64-bit
        var fileTime = ((ulong)ct.dwHighDateTime << 32) | (uint)ct.dwLowDateTime;

        // Validate
        if (fileTime == 0)
        {
            return null;
        }

        try
        {
            return DateTime.FromFileTimeUtc((long)fileTime);
        }
        catch
        {
            return null;
        }
    }

    private static string? TryGetExecutablePath(uint pid)
    {
        var access = (uint)ProcessAccess.PROCESS_QUERY_LIMITED_INFORMATION;

        using var hProc = OpenProcess(access, false, pid);
        if (hProc.IsInvalid)
        {
            return null;
        }

        var sb = new StringBuilder(1024);
        var size = (uint)sb.Capacity;

        if (QueryFullProcessImageName(hProc, 0, sb, ref size))
        {
            return sb.ToString();
        }

        return null;
    }

    private static string? TryGetMainWindowTitle(uint pid)
    {
        try
        {
            var proc = System.Diagnostics.Process.GetProcessById((int)pid);

            // Accessing MainWindowTitle does NOT throw for protected processes
            return proc.MainWindowTitle;
        }
        catch
        {
            return null;
        }
    }

}
