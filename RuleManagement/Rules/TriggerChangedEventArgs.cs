namespace RuleManagement.Rules;

public class TriggerChangedEventArgs(IRule rule) : EventArgs
{
    public IRule Rule { get; set; } = rule;
}
