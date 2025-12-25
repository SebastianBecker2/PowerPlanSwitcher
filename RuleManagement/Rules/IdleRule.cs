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
        object? sender,
        IdleTimeChangedEventArgs e)
    {
        if (CheckRule(e.IdleTime))
        {
            TriggerCount = Math.Min(TriggerCount + 1, 1);
        }
        else
        {
            TriggerCount = Math.Max(TriggerCount - 1, 0);
        }
    }

    private bool CheckRule(TimeSpan idleTime) =>
        idleTime >= IdleTimeThreshold;
}
