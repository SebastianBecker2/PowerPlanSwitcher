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

    public override void StartRuling()
    {
        // No additional action needed; TriggerCount is always 1;
    }

    public override void StopRuling()
    {
        // No additional action needed; TriggerCount is always 1;
    }
}
