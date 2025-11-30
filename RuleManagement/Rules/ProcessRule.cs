namespace RuleManagement.Rules;

using System.ComponentModel;
using Newtonsoft.Json;
using ProcessManagement;

public class ProcessRule : IRule
{
    [JsonIgnore]
    public int ActivationCount { get; set; }
    public int Index { get; set; }
    public Guid SchemeGuid { get; set; }
    public string FilePath
    {
        get => filePath;
        set => filePath = value.ToLowerInvariant();
    }
    public ComparisonType Type { get; set; }

    private static readonly List<(ComparisonType type, string text)>
        ComparisonTypeText =
        [
            (ComparisonType.Exact, "Match exact Path"),
            (ComparisonType.StartsWith, "Path starts with"),
            (ComparisonType.EndsWith, "Path ends with"),
        ];

    private string filePath = string.Empty;

    public string GetDescription() =>
        $"Process -> {ComparisonTypeToText(Type)} -> {FilePath}";

    public bool CheckRule(IProcess process)
    {
        try
        {
            var path = process.ExecutablePath;
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            return Type switch
            {
                ComparisonType.Exact => path == FilePath,
                ComparisonType.StartsWith => path.StartsWith(
                    FilePath,
                    StringComparison.InvariantCulture),
                ComparisonType.EndsWith => path.EndsWith(
                    FilePath,
                    StringComparison.InvariantCulture),
                _ => throw new InvalidOperationException(
                    $"Unable to apply rule type {Type}"),
            };
        }
        catch (Win32Exception)
        {
            return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public static string ComparisonTypeToText(ComparisonType ruleType)
    {
        (ComparisonType type, string text)? entry = ComparisonTypeText
            .FirstOrDefault(rtt => rtt.type == ruleType);
        return entry?.text ?? string.Empty;
    }

    public static ComparisonType TextToComparisonType(string text)
    {
        (ComparisonType type, string text)? entry = ComparisonTypeText
            .FirstOrDefault(rtt => rtt.text == text);
        return entry?.type ?? throw new InvalidOperationException(
            "No RuleType matches the provided text. " +
            "Unable to convert to RuleType!");
    }
}
