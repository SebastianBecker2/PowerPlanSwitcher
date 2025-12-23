namespace RuleManagement.Events;

public class RulesUpdatedEventArgs(string serialized) : EventArgs
{
    public string Serialized { get; set; } = serialized;
}
