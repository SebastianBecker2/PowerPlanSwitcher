namespace RuleManagement.Rules;

using System;
using PowerManagement;
using RuleManagement.Dto;
using SystemManagement;

public sealed class IdleRule(
    IIdleMonitor idleMonitor,
    IPowerManager powerManager,
    ISystemManager systemManager,
    IdleRuleDto idleRuleDto) :
    Rule<IdleRuleDto>(idleRuleDto),
    IRule<IdleRuleDto>,
    IDisposable
{
    public Guid SchemeGuid => Dto.SchemeGuid;
    public TimeSpan IdleTimeThreshold => Dto.IdleTimeThreshold;
    public bool CheckExecutionState => Dto.CheckExecutionState;
    public bool CheckFullscreenApps => Dto.CheckFullscreenApps;

    private readonly object syncRoot = new();

    public override void StartRuling()
    {
        idleMonitor.IdleTimeChanged += IdleMonitor_IdleTimeChanged;

        lock (syncRoot)
        {
            if (CheckRule(idleMonitor.GetIdleTime()))
            {
                TriggerCount = 1;
            }
            else
            {
                TriggerCount = 0;
            }
        }
    }

    public override void StopRuling() =>
        idleMonitor.IdleTimeChanged -= IdleMonitor_IdleTimeChanged;

    private void IdleMonitor_IdleTimeChanged(
        object? _,
        IdleTimeChangedEventArgs e)
    {
        if (CheckRule(e.IdleTime))
        {
            TriggerCount = 1;
        }
        else
        {
            TriggerCount = 0;
        }
    }

    public bool CheckRule(TimeSpan idleTime)
    {
        lock (syncRoot)
        {
            if (idleTime < IdleTimeThreshold)
            {
                return false;
            }

            if (CheckExecutionState && powerManager.IsExecutionStateBlockingIdle())
            {
                return false;
            }

            if (CheckFullscreenApps && systemManager.IsFullscreenAppActive())
            {
                return false;
            }

            return true;
        }
    }

    public void Dispose() => StopRuling();

}
