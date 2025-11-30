namespace RuleManagement.Rules;

using System;
using PowerManagement;

public class PowerLineRule(
    IBatteryMonitor batteryMonitor,
    PowerLineRuleDto dto)
    : Rule,
    IRule
{
    public IRuleDto Dto => dto;
    public int Index => dto.Index;
    public Guid SchemeGuid => dto.SchemeGuid;
    public PowerLineStatus PowerLineStatus => dto.PowerLineStatus;


    private static readonly List<(PowerLineStatus status, string text)>
        PowerLineStatusText =
        [
            (PowerLineStatus.Online, "Plugged in"),
            (PowerLineStatus.Offline, "On battery"),
            (PowerLineStatus.Unknown, "Unkown status"),
        ];

    public string GetDescription() =>
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
        (PowerLineStatus status, string text)? entry = PowerLineStatusText
            .FirstOrDefault(rtt => rtt.text == text);
        return entry?.status ?? throw new InvalidOperationException(
            "No PowerLineStatus matches the provided text. " +
            "Unable to convert to PowerLineStatus!");
    }
}
