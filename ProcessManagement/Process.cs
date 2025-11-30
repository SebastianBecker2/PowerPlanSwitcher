namespace ProcessManagement;

using System;

public class Process : IProcess
{
    private static readonly Process OwnProcess =
        CreateFromProcess(System.Diagnostics.Process.GetCurrentProcess())
        ?? throw new InvalidOperationException(
            "Unable to determine own process!");

    public static Process? CreateFromProcess(
        System.Diagnostics.Process process)
    {
        try
        {
            var d = process.StartTime;
            return new Process
            {
                ProcessId = process.Id,
                ProcessName = process.ProcessName,
                ExecutablePath =
                        process.MainModule!.FileName.ToLowerInvariant(),
                MainWindowTitle = process.MainWindowTitle ?? "",
            };
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
}
