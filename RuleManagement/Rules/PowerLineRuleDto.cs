namespace RuleManagement.Rules;

using PowerManagement;

public class PowerLineRuleDto : RuleDto, IRuleDto
{
    public PowerLineStatus PowerLineStatus { get; set; }

    private static readonly List<(PowerLineStatus status, string text)>
        PowerLineStatusText =
        [
            (PowerLineStatus.Online, "Plugged in"),
            (PowerLineStatus.Offline, "On battery"),
            (PowerLineStatus.Unknown, "Unkown status"),
        ];
    private static readonly Dictionary<string, PowerLineStatus> TextToStatusMap =
        PowerLineStatusText.ToDictionary(
            entry => entry.text,
            entry => entry.status,
            StringComparer.Ordinal);

    public override string GetDescription() =>
        $"Power Line -> {PowerLineStatusToText(PowerLineStatus)}";

    public static string PowerLineStatusToText(
        PowerLineStatus powerLineStatus)
    {
        (PowerLineStatus status, string text)? entry = PowerLineStatusText
            .FirstOrDefault(rtt => rtt.status == powerLineStatus);
        return entry?.text ?? string.Empty;
    }

    public static PowerLineStatus TextToPowerLineStatus(string text)
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
