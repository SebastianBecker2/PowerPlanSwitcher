namespace RuleManagement.Rules;

using System;
using PowerManagement;

public class PowerLineRule : Rule<PowerLineRuleDto>,
    IRule<PowerLineRuleDto>
{
    public Guid SchemeGuid => Dto.SchemeGuid;
    public PowerLineStatus PowerLineStatus => Dto.PowerLineStatus;

    private static readonly List<(PowerLineStatus status, string text)>
        PowerLineStatusText =
        [
            (PowerLineStatus.Online, "Plugged in"),
            (PowerLineStatus.Offline, "On battery"),
            (PowerLineStatus.Unknown, "Unkown status"),
        ];
    private static readonly Dictionary<string, PowerLineStatus> TextToStatusMap =
        PowerLineStatusText.ToDictionary(entry => entry.text, entry => entry.status, StringComparer.Ordinal);

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

    public override string GetDescription() =>
        $"Power Line -> {PowerLineStatusToText(PowerLineStatus)}";

    private bool CheckRule(PowerLineStatus powerLineStatus) =>
        PowerLineStatus == powerLineStatus;

    private static string PowerLineStatusToText(
        PowerLineStatus powerLineStatus)
    {
        (PowerLineStatus status, string text)? entry = PowerLineStatusText
            .FirstOrDefault(rtt => rtt.status == powerLineStatus);
        return entry?.text ?? string.Empty;
    }

    private static PowerLineStatus TextToPowerLineStatus(string text)
    {
        if (!TextToStatusMap.TryGetValue(text, out var status))
        {
            throw new InvalidOperationException(
                "No PowerLineStatus matches the provided text. " +
                "Unable to convert to PowerLineStatus!");
        }
        return status;
    }
}
