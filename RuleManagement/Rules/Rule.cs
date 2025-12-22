namespace RuleManagement.Rules;

using Newtonsoft.Json;

public abstract class Rule<TDto>(TDto dto) : IRule<TDto> where TDto : IRuleDto
{
    [JsonIgnore]
    public int TriggerCount
    {
        get => triggerCount;
        protected set
        {
            triggerCount = value;
            OnTriggerChanged(this);
        }
    }
    private int triggerCount;

    // Strongly typed DTO
    public TDto Dto { get; } = dto;

    // Explicit implementation for the non-generic interface
    IRuleDto IRule.Dto => Dto;

    public event EventHandler<TriggerChangedEventArgs>? TriggerChanged;
    protected void OnTriggerChanged(IRule<TDto> rule) =>
        TriggerChanged?.Invoke(this, new TriggerChangedEventArgs(rule));
}
