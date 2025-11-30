namespace RuleManagement.Rules;

public class TriggeredEventArgs(IRule rule)
{
    public IRule Rule { get; set; } = rule;
}
