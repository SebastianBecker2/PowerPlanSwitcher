namespace RuleManagement.Dto;

public class ShutdownRuleDto : RuleDto, IRuleDto
{
    public override string GetDescription() => $"Shutdown Rule";
}
