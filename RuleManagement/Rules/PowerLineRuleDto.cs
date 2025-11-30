namespace RuleManagement.Rules;

using PowerManagement;

public class PowerLineRuleDto : RuleDto, IRuleDto
{
    public PowerLineStatus PowerLineStatus { get; set; }
}
