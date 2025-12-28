namespace RuleManagement.Rules;

using System;
using RuleManagement.Dto;
using SystemManagement;

public class IdleRule :
    Rule<IdleRuleDto>,
    IRule<IdleRuleDto>,
    IDisposable
{
    public Guid SchemeGuid => Dto.SchemeGuid;
    public TimeSpan IdleTimeThreshold => Dto.IdleTimeThreshold;

    private readonly IIdleMonitor idleMonitor;

    public IdleRule(
        IIdleMonitor idleMonitor,
        IdleRuleDto idleRuleDto)
        : base(idleRuleDto)
    {
        this.idleMonitor = idleMonitor;

        idleMonitor.IdleTimeChanged += IdleMonitor_IdleTimeChanged;
    }

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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "IdleRule does not have a finalizer")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Suppression of CA1816 is necessary")]
    public void Dispose() =>
        idleMonitor.IdleTimeChanged -= IdleMonitor_IdleTimeChanged;

}
