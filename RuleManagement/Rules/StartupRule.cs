namespace RuleManagement.Rules;

using System;
using RuleManagement.Dto;

public class StartupRule :
    Rule<StartupRuleDto>,
    IRule<StartupRuleDto>
{
    public Guid SchemeGuid => Dto.SchemeGuid;

    public StartupRule(StartupRuleDto startupRuleDto)
        : base(startupRuleDto) =>
        TriggerCount = 1;
}
