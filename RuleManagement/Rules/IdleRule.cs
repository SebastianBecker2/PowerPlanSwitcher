namespace RuleManagement.Rules;

using System;
using System.Text;
using PowerManagement;
using RuleManagement.Dto;
using Serilog;
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
    private string? lastIdleBlockReason;

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
                lastIdleBlockReason = null;
                return false;
            }

            var blockReasons = new StringBuilder();

            if (CheckExecutionState
                && powerManager.TryGetExecutionState(out var executionState)
                && PowerManager.Api.IsExecutionStateBlockingIdle(executionState))
            {
                AppendBlockReason(
                    blockReasons,
                    $"execution state 0x{executionState:X} " +
                    $"({PowerManager.Api.GetExecutionStateDescription(executionState)})");
            }

            if (CheckFullscreenApps && systemManager.IsFullscreenAppActive())
            {
                AppendBlockReason(blockReasons, "fullscreen app active");
            }

            if (blockReasons.Length > 0)
            {
                LogIdleBlockedIfChanged(idleTime, blockReasons.ToString());
                return false;
            }

            lastIdleBlockReason = null;
            return true;
        }
    }

    private void LogIdleBlockedIfChanged(TimeSpan idleTime, string blockReasons)
    {
        if (blockReasons == lastIdleBlockReason)
        {
            return;
        }

        lastIdleBlockReason = blockReasons;

        Log.Information(
            "Idle rule blocked: idle time {IdleTime} meets threshold {IdleTimeThreshold}, but {BlockReasons}",
            idleTime,
            IdleTimeThreshold,
            blockReasons);
    }

    private static void AppendBlockReason(StringBuilder blockReasons, string reason)
    {
        if (blockReasons.Length > 0)
        {
            _ = blockReasons.Append("; ");
        }

        _ = blockReasons.Append(reason);
    }

    public void Dispose() => StopRuling();

}
