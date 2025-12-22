namespace RuleManagement;

public class RulesUpdatedEventArgs(string serialized) : EventArgs
{
    public string Serialized { get; set; } = serialized;
}
