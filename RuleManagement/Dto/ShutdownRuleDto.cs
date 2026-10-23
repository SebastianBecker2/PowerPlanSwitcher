namespace RuleManagement.Dto;

public class ShutdownRuleDto : RuleDto, IRuleDto
{
    public override string GetDescription() => Strings.Rule_Shutdown_Desc;
}