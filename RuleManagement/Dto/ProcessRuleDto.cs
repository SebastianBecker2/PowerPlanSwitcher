namespace RuleManagement.Dto;
using Newtonsoft.Json;

public class ProcessRuleDto : RuleDto, IRuleDto
{
    [JsonProperty("FilePath")]
    public string Pattern { get; set; } = "";
    public ComparisonType Type { get; set; }

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
        $"Process -> {ComparisonTypeToText(Type)} -> {Pattern}";

    public static string ComparisonTypeToText(ComparisonType ruleType)
    {
        (ComparisonType type, string text)? entry = ComparisonTypeText
            .FirstOrDefault(rtt => rtt.type == ruleType);
        return entry?.text ?? string.Empty;
    }

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
