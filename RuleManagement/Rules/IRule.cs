namespace RuleManagement.Rules;

using System;

public interface IRule
{
    public int TriggerCount { get; set; }
    public IRuleDto Dto { get; }

    public string GetDescription();

    public event EventHandler<TriggeredEventArgs>? Triggered;
}
