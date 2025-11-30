namespace RuleManagement;

using System;

public class RuleApplicationChangedEventArgs(
    Guid powerSchemeGuid,
    string? reason,
    IRule? rule)
    : EventArgs
{
    public Guid PowerSchemeGuid { get; set; } = powerSchemeGuid;
    public string? Reason { get; set; } = reason;
    public IRule? Rule { get; set; } = rule;
}
