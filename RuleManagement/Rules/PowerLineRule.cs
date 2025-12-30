namespace RuleManagement.Rules;

using System;
using PowerManagement;
using RuleManagement.Dto;

public class PowerLineRule(
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
            if (CheckRule(e.PowerLineStatus))
            {
                TriggerCount++;
            }
            else
            {
                TriggerCount = Math.Max(TriggerCount - 1, 0);
            }
        }
    }

    private bool CheckRule(PowerLineStatus powerLineStatus) =>
        PowerLineStatus == powerLineStatus;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Suppression of CA1816 is necessary")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "PowerLineRule does not have a finalizer")]
    public void Dispose() => StopRuling();

}
