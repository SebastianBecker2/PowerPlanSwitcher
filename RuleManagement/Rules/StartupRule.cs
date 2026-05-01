namespace RuleManagement.Rules;

using System;
using RuleManagement.Dto;
using Serilog;
using WmTimer = WindowMessageTimer.Timer;

public sealed class StartupRule :
    Rule<StartupRuleDto>,
    IRule<StartupRuleDto>,
    IDisposable
{
    public Guid SchemeGuid => Dto.SchemeGuid;

    private readonly object syncRoot = new();
    private WmTimer? delayTimer;
    private DateTime? delayStartTime;
    private WmTimer? durationTimer;
    private DateTime? triggerInstantiationTime;
    private bool durationElapsed;

    public StartupRule(StartupRuleDto startupRuleDto)
        : base(startupRuleDto) =>
        TriggerCount = Dto.Delay is null ? 1 : 0;

    public override void StartRuling()
    {
        if (Dto.Delay is not null)
        {
            Log.Debug("StartupRule started with delay {Delay}", Dto.Delay);

            lock (syncRoot)
            {
                delayStartTime = DateTime.UtcNow;
                delayTimer = new WmTimer(100); // 100ms polling interval
                delayTimer.Tick += DelayTimer_Tick;
                delayTimer.Start();
            }

            return;
        }

        lock (syncRoot)
        {
            StartDurationPollingLocked();
        }
    }

    private void DelayTimer_Tick()
    {
        WmTimer? timerToStop = null;

        lock (syncRoot)
        {
            if (delayStartTime is null || Dto.Delay is null)
            {
                return;
            }

            var elapsed = DateTime.UtcNow - delayStartTime.Value;
            if (elapsed < Dto.Delay.Value)
            {
                return;
            }

            Log.Debug("StartupRule delay elapsed; triggering");

            timerToStop = delayTimer;
            if (timerToStop is not null)
            {
                timerToStop.Tick -= DelayTimer_Tick;
            }

            delayTimer = null;
            delayStartTime = null;

            TriggerCount = 1;
            StartDurationPollingLocked();
        }

        if (timerToStop is not null)
        {
            _ = ThreadPool.QueueUserWorkItem(_ => StopAndDisposeTimer(timerToStop));
        }
    }

    private void StartDurationPollingLocked()
    {
        if (Dto.Duration is null)
        {
            Log.Debug("StartupRule started without duration; will remain triggered indefinitely");
            return;
        }

        Log.Debug("StartupRule started with duration {Duration}", Dto.Duration);

        durationElapsed = false;
        triggerInstantiationTime = DateTime.UtcNow;
        durationTimer = new WmTimer(100); // 100ms polling interval
        durationTimer.Tick += DurationTimer_Tick;
        durationTimer.Start();
    }

    private void DurationTimer_Tick()
    {
        WmTimer? timerToStop = null;

        lock (syncRoot)
        {
            if (triggerInstantiationTime is null || Dto.Duration is null)
            {
                return;
            }

            if (durationElapsed)
            {
                return;
            }

            var elapsed = DateTime.UtcNow - triggerInstantiationTime.Value;
            if (elapsed >= Dto.Duration.Value)
            {
                Log.Debug("StartupRule duration elapsed; untriggering");
                durationElapsed = true;
                TriggerCount = 0;

                timerToStop = durationTimer;
                if (timerToStop is not null)
                {
                    timerToStop.Tick -= DurationTimer_Tick;
                }

                durationTimer = null;
                triggerInstantiationTime = null;
            }
        }

        if (timerToStop is not null)
        {
            // Stop and dispose outside the timer callback thread to avoid self-join deadlocks.
            _ = ThreadPool.QueueUserWorkItem(_ => StopAndDisposeTimer(timerToStop));
        }
    }

    public override void StopRuling()
    {
        WmTimer? durationTimerToStop;
        WmTimer? delayTimerToStop;

        lock (syncRoot)
        {
            durationTimerToStop = durationTimer;
            delayTimerToStop = delayTimer;

            if (durationTimerToStop is not null)
            {
                durationTimerToStop.Tick -= DurationTimer_Tick;
                durationTimer = null;
            }

            if (delayTimerToStop is not null)
            {
                delayTimerToStop.Tick -= DelayTimer_Tick;
                delayTimer = null;
            }

            triggerInstantiationTime = null;
            delayStartTime = null;
            durationElapsed = false;
        }

        StopAndDisposeTimer(durationTimerToStop);
        StopAndDisposeTimer(delayTimerToStop);
    }

    private static void StopAndDisposeTimer(WmTimer? timer)
    {
        if (timer is null)
        {
            return;
        }

        timer.Stop();
        timer.Dispose();
    }

    public void Dispose() => StopRuling();
}
