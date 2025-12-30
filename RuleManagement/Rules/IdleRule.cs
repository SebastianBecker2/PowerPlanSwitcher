namespace RuleManagement.Rules;

using System;
using RuleManagement.Dto;
using SystemManagement;

public class IdleRule(
    IIdleMonitor idleMonitor,
    IdleRuleDto idleRuleDto) :
    Rule<IdleRuleDto>(idleRuleDto),
    IRule<IdleRuleDto>,
    IDisposable
{
    public Guid SchemeGuid => Dto.SchemeGuid;
    public TimeSpan IdleTimeThreshold => Dto.IdleTimeThreshold;

    public override void StartRuling()
    {
        idleMonitor.IdleTimeChanged += IdleMonitor_IdleTimeChanged;

        if (idleMonitor.GetIdleTime() >= IdleTimeThreshold)
        {
            TriggerCount = 1;
        }
        else
        {
            TriggerCount = 0;
        }
    }

    public override void StopRuling() =>
        idleMonitor.IdleTimeChanged -= IdleMonitor_IdleTimeChanged;

    private void IdleMonitor_IdleTimeChanged(
        object? _,
        IdleTimeChangedEventArgs e)
    {
        if (e.IdleTime >= IdleTimeThreshold)
        {
            TriggerCount = 1;
        }
        else
        {
            TriggerCount = 0;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "IdleRule does not have a finalizer")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Suppression of CA1816 is necessary")]
    public void Dispose() => StopRuling();

}
