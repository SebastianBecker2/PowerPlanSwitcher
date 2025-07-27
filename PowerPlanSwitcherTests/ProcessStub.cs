namespace PowerPlanSwitcherTests
{
    using PowerPlanSwitcher.ProcessManagement;

    internal class ProcessStub : IProcess
    {
        public string ExecutablePath { get; set; } = "";
        public bool IsOwnProcess { get; set; }
        public int ProcessId { get; set; }
        public string ProcessName { get; set; } = "";
        public string MainWindowTitle { get; set; } = "";
        public DateTime StartTime { get; set; }


        public override bool Equals(object? obj) =>
            Equals(obj as IProcess);

        public bool Equals(IProcess? other) =>
            other is not null
            && ProcessId == other.ProcessId;

        public override int GetHashCode() =>
            HashCode.Combine(ProcessId);
    }
}
