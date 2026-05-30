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
    private WmTimer? durationTimer;
    private DateTime? triggerInstantiationTime;
    private bool durationElapsed;

    public StartupRule(StartupRuleDto startupRuleDto)
        : base(startupRuleDto) =>
        TriggerCount = 1;

    public override void StartRuling()
    {
        if (Dto.Duration is null)
        {
            Log.Debug("StartupRule started without duration; will remain triggered indefinitely");
            return;
        }

        Log.Debug("StartupRule started with duration {Duration}", Dto.Duration);

        lock (syncRoot)
        {
            durationElapsed = false;
            triggerInstantiationTime = DateTime.UtcNow;
            durationTimer = new WmTimer(100); // 100ms polling interval
            durationTimer.Tick += DurationTimer_Tick;
            durationTimer.Start();
        }
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
        WmTimer? timerToStop;

        lock (syncRoot)
        {
            timerToStop = durationTimer;

            if (timerToStop is not null)
            {
                timerToStop.Tick -= DurationTimer_Tick;
                durationTimer = null;
            }

            triggerInstantiationTime = null;
            durationElapsed = false;
        }

        StopAndDisposeTimer(timerToStop);
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
