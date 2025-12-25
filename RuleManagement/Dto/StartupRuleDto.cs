namespace RuleManagement.Dto;

public class StartupRuleDto : RuleDto, IRuleDto
{
    public override string GetDescription() => $"Startup Rule";
}
