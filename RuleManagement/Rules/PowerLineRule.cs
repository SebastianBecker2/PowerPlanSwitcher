namespace RuleManagement.Rules;

using System;
using PowerManagement;
using RuleManagement.Dto;

public class PowerLineRule
    : Rule<PowerLineRuleDto>,
    IRule<PowerLineRuleDto>,
    IDisposable
{
    public Guid SchemeGuid => Dto.SchemeGuid;
    public PowerLineStatus PowerLineStatus => Dto.PowerLineStatus;

    private readonly IBatteryMonitor batteryMonitor;

    public PowerLineRule(
        IBatteryMonitor batteryMonitor,
        PowerLineRuleDto powerLineRuleDto)
        : base(powerLineRuleDto)
    {
        this.batteryMonitor = batteryMonitor;

        batteryMonitor.PowerLineStatusChanged += BatteryMonitor_PowerLineStatusChanged;
    }

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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Suppression of CA1816 is necessary")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "PowerLineRule does not have a finalizer")]
    public void Dispose() =>
        batteryMonitor.PowerLineStatusChanged -= BatteryMonitor_PowerLineStatusChanged;

}
