namespace PowerPlanSwitcherTests
{
    using PowerPlanSwitcher.ProcessManagement;

    internal class CachedProcessStub : ICachedProcess
    {
        public string ExecutablePath { get; set; } = "";

        public bool IsOwnProcess { get; set; }

        public int ProcessId { get; set; }

        public string ProcessName { get; set; } = "";
    }
}
