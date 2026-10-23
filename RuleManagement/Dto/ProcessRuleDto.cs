namespace RuleManagement.Dto;
using Newtonsoft.Json;

public class ProcessRuleDto : RuleDto, IRuleDto
{
    [JsonProperty("FilePath")]
    public string Pattern { get; set; } = "";
    public ComparisonType Type { get; set; }

    // 英文原文保留，只用于反查（兼容旧数据 / 持久化字段），
    // 显示时改用 Strings.Rule_Process_CmpXXX。
    // English text kept only for reverse lookup (legacy data /
    // persisted fields); display uses Strings.Rule_Process_CmpXXX.
    private static readonly List<(ComparisonType type, string text)>
        ComparisonTypeText =
        [
            (ComparisonType.Exact, "Match exact Path"),
            (ComparisonType.StartsWith, "Path starts with"),
            (ComparisonType.EndsWith, "Path ends with"),
            (ComparisonType.Wildcard, "Wildcard match"),
        ];
    private static readonly Dictionary<string, ComparisonType> TextToTypeMap =
        ComparisonTypeText.ToDictionary(
            entry => entry.text,
            entry => entry.type,
            StringComparer.Ordinal);

    public override string GetDescription() =>
        string.Format(
            Strings.Rule_Process_Desc,
            ComparisonTypeToText(Type),
            Pattern);

    // 返回本地化文本，供 UI 显示。
    // Returns the localized text for UI display.
    public static string ComparisonTypeToText(ComparisonType ruleType) =>
        ruleType switch
        {
            ComparisonType.Exact => Strings.Rule_Process_CmpExact,
            ComparisonType.StartsWith => Strings.Rule_Process_CmpStartsWith,
            ComparisonType.EndsWith => Strings.Rule_Process_CmpEndsWith,
            ComparisonType.Wildcard => Strings.Rule_Process_CmpWildcard,
            _ => string.Empty,
        };

    public static ComparisonType TextToComparisonType(string text)
    {
        if (!TextToTypeMap.TryGetValue(text, out var type))
        {
            throw new InvalidOperationException(
                "No RuleType matches the provided text. " +
                "Unable to convert to RuleType!");
        }
        return type;
    }

}