namespace RuleManagement.Dto;

public class IdleRuleDto : RuleDto, IRuleDto
{
    public TimeSpan IdleTimeThreshold { get; set; }

    public override string GetDescription() =>
        $"Idle Time -> {IdleTimeThreshold}";
}
