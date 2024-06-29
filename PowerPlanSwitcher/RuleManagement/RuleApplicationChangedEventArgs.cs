namespace PowerPlanSwitcher.RuleManagement
{
    using System;

    public class RuleApplicationChangedEventArgs(
        Guid powerSchemeGuid,
        string? reason,
        PowerRule? rule)
        : EventArgs
    {
        public Guid PowerSchemeGuid { get; set; } = powerSchemeGuid;
        public string? Reason { get; set; } = reason;
        public PowerRule? Rule { get; set; } = rule;
    }
}
