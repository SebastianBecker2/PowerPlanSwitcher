namespace PowerPlanSwitcherTests
{
    using PowerPlanSwitcher.ProcessManagement;

    internal class CachedProcessStub : ICachedProcess
    {
        public string ExecutablePath { get; set; } = "";

        public bool IsOwnProcess { get; set; }

        public int ProcessId { get; set; }

        public string ProcessName { get; set; } = "";

        public override bool Equals(object? obj) =>
            Equals(obj as ICachedProcess);

        public bool Equals(ICachedProcess? other) =>
            other is not null
            && ProcessId == other.ProcessId;

        public override int GetHashCode() =>
            HashCode.Combine(ProcessId);
    }
}
