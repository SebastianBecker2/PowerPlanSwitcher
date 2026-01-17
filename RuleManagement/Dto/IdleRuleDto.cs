namespace RuleManagement.Dto;

public class IdleRuleDto : RuleDto, IRuleDto
{
    public TimeSpan IdleTimeThreshold { get; set; }
    public bool CheckExecutionState { get; set; } = true;
    public bool CheckFullscreenApps { get; set; } = true;

    public override string GetDescription() =>
        $"Idle Time -> {IdleTimeThreshold}";
}
