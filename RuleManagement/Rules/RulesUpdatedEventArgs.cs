namespace RuleManagement.Rules;

public class RulesUpdatedEventArgs(string serialized)
{
    public string Serialized { get; set; } = serialized;
}
