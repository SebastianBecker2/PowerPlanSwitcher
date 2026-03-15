namespace ProcessManagement;

using System;
using System.Text;
using Serilog;
using static Vanara.PInvoke.Kernel32;

public class Process : IProcess
{
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

            var executablePath = TryGetExecutablePath(processEntry.th32ProcessID);
            if (executablePath is null)
            {
                return null;
            }

            var p = new Process
            {
                ProcessId = processId,
                ProcessName = processEntry.szExeFile,
                StartTime = startTime.Value,
                ExecutablePath = executablePath.ToLowerInvariant(),
                MainWindowTitle = string.Empty,
            };

            return p;
        }
        catch (Exception ex)
        {
            Log.Debug(ex, "Failed to create process snapshot entry from PROCESSENTRY32.");
            return null;
        }
    }

    public static Process? CreateFromProcess(
        System.Diagnostics.Process process)
    {
        try
        {
            var p = new Process
            {
                ProcessId = process.Id,
                ProcessName = process.ProcessName,
                StartTime = process.StartTime,
                ExecutablePath =
                        process.MainModule!.FileName.ToLowerInvariant(),
                MainWindowTitle = process.MainWindowTitle ?? "",
            };

            return p;
        }
        catch (Exception ex)
        {
            Log.Debug(ex, "Failed to create process snapshot entry from System.Diagnostics.Process.");
            return null;
        }
    }

    public override bool Equals(object? obj) =>
        Equals(obj as IProcess);

    public bool Equals(IProcess? other) =>
        other is not null
        && ProcessId == other.ProcessId
        && StartTime == other.StartTime;

    public override int GetHashCode() =>
        HashCode.Combine(
            ProcessId,
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
        catch (Exception ex)
        {
            Log.Debug(ex, "Failed to convert process creation time from FILETIME.");
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
        catch (Exception ex)
        {
            Log.Debug(ex, "Failed to read main window title for process {ProcessId}", pid);
            return null;
        }
    }

}
