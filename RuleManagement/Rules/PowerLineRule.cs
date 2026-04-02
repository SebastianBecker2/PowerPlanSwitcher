namespace RuleManagement.Rules;

using System;
using PowerManagement;
using RuleManagement.Dto;

public sealed class PowerLineRule(
    IBatteryMonitor batteryMonitor,
    PowerLineRuleDto powerLineRuleDto)
        : Rule<PowerLineRuleDto>(powerLineRuleDto),
    IRule<PowerLineRuleDto>,
    IDisposable
{
    public Guid SchemeGuid => Dto.SchemeGuid;
    public PowerLineStatus PowerLineStatus => Dto.PowerLineStatus;

    private readonly object syncRoot = new();

    public override void StartRuling()
    {
        batteryMonitor.PowerLineStatusChanged += BatteryMonitor_PowerLineStatusChanged;

        lock (syncRoot)
        {
            if (CheckRule(batteryMonitor.PowerLineStatus))
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
        batteryMonitor.PowerLineStatusChanged -= BatteryMonitor_PowerLineStatusChanged;

    private void BatteryMonitor_PowerLineStatusChanged(
        object? sender,
        PowerLineStatusChangedEventArgs e)
    {
        lock (syncRoot)
        {
            TriggerCount = CheckRule(e.PowerLineStatus)
                ? 1
                : 0;
        }
    }

    private bool CheckRule(PowerLineStatus powerLineStatus) =>
        PowerLineStatus == powerLineStatus;

    public void Dispose() => StopRuling();

}
