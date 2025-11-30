namespace RuleManagement.Rules;

using Newtonsoft.Json;

public abstract class Rule
{
    [JsonIgnore]
    public int TriggerCount { get; set; }

    public event EventHandler<TriggeredEventArgs>? Triggered;
    protected void OnTriggered(IRule rule) =>
        Triggered?.Invoke(this, new TriggeredEventArgs(rule));
}
