namespace RuleManagement.Rules;

using System;
using RuleManagement.Dto;
using SystemManagement;

public class IdleRule :
    Rule<IdleRuleDto>,
    IRule<IdleRuleDto>
{
    public Guid SchemeGuid => Dto.SchemeGuid;
    public TimeSpan IdleTimeThreshold => Dto.IdleTimeThreshold;

    public IdleRule(
        IIdleMonitor idleMonitor,
        IdleRuleDto idleRuleDto)
        : base(idleRuleDto) =>
        idleMonitor.IdleTimeChanged += IdleMonitor_IdleTimeChanged;

    private void IdleMonitor_IdleTimeChanged(
        object? _,
        IdleTimeChangedEventArgs e)
    {
        if (e.IdleTime >= IdleTimeThreshold)
        {
            TriggerCount = Math.Min(TriggerCount + 1, 1);
        }
        else
        {
            TriggerCount = Math.Max(TriggerCount - 1, 0);
        }
    }
}
