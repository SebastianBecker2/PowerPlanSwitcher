namespace RuleManagement.Rules;

public class ProcessRuleDto : RuleDto, IRuleDto
{
    public string FilePath
    {
        get => filePath;
        set => filePath = value.ToLowerInvariant();
    }
    public ComparisonType Type { get; set; }

    private string filePath = string.Empty;
    private static readonly List<(ComparisonType type, string text)>
        ComparisonTypeText =
        [
            (ComparisonType.Exact, "Match exact Path"),
            (ComparisonType.StartsWith, "Path starts with"),
            (ComparisonType.EndsWith, "Path ends with"),
        ];
    private static readonly Dictionary<string, ComparisonType> TextToTypeMap =
        ComparisonTypeText.ToDictionary(
            entry => entry.text,
            entry => entry.type,
            StringComparer.Ordinal);

    public override string GetDescription() =>
        $"Process -> {ComparisonTypeToText(Type)} -> {FilePath}";

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
