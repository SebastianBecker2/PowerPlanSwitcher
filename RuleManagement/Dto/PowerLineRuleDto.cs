namespace RuleManagement.Dto;

using PowerManagement;

public class PowerLineRuleDto : RuleDto, IRuleDto
{
    public PowerLineStatus PowerLineStatus { get; set; }

    // 英文原文保留，只用于反查。
    // English text kept only for reverse lookup.
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
        string.Format(
            Strings.Rule_PowerLine_Desc,
            PowerLineStatusToText(PowerLineStatus));

    // 返回本地化文本，供 UI 显示。
    // Returns the localized text for UI display.
    public static string PowerLineStatusToText(
        PowerLineStatus powerLineStatus) =>
        powerLineStatus switch
        {
            PowerLineStatus.Online => Strings.Rule_PowerLine_Online,
            PowerLineStatus.Offline => Strings.Rule_PowerLine_Offline,
            PowerLineStatus.Unknown => Strings.Rule_PowerLine_Unknown,
            _ => string.Empty,
        };

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