namespace RuleManagement.Rules;

using System;
using PowerManagement;
using RuleManagement.Dto;

public class PowerLineRule : Rule<PowerLineRuleDto>,
    IRule<PowerLineRuleDto>
{
    public Guid SchemeGuid => Dto.SchemeGuid;
    public PowerLineStatus PowerLineStatus => Dto.PowerLineStatus;

    public PowerLineRule(
        IBatteryMonitor batteryMonitor,
        PowerLineRuleDto powerLineRuleDto)
        : base(powerLineRuleDto) =>
        batteryMonitor.PowerLineStatusChanged += BatteryMonitor_PowerLineStatusChanged;

    private void BatteryMonitor_PowerLineStatusChanged(
        object? sender,
        PowerLineStatusChangedEventArgs e)
    {
        if (CheckRule(e.PowerLineStatus))
        {
            TriggerCount++;
        }
        else
        {
            TriggerCount = Math.Max(TriggerCount - 1, 0);
        }
    }

    private bool CheckRule(PowerLineStatus powerLineStatus) =>
        PowerLineStatus == powerLineStatus;
}
