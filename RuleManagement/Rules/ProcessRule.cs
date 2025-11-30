namespace RuleManagement.Rules;

using System.ComponentModel;
using ProcessManagement;

public class ProcessRule(
    IProcessMonitor processMonitor,
    ProcessRuleDto dto)
    : Rule,
    IRule
{
    public IRuleDto Dto => dto;
    public int Index => dto.Index;
    public Guid SchemeGuid => dto.SchemeGuid;
    public string FilePath => dto.FilePath;
    public ComparisonType Type => dto.Type;

    private static readonly List<(ComparisonType type, string text)>
        ComparisonTypeText =
        [
            (ComparisonType.Exact, "Match exact Path"),
            (ComparisonType.StartsWith, "Path starts with"),
            (ComparisonType.EndsWith, "Path ends with"),
        ];

    public string GetDescription() =>
        $"Process -> {ComparisonTypeToText(Type)} -> {FilePath}";

    private bool CheckRule(IProcess process)
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

    private static string ComparisonTypeToText(ComparisonType ruleType)
    {
        (ComparisonType type, string text)? entry = ComparisonTypeText
            .FirstOrDefault(rtt => rtt.type == ruleType);
        return entry?.text ?? string.Empty;
    }

    private static ComparisonType TextToComparisonType(string text)
    {
        (ComparisonType type, string text)? entry = ComparisonTypeText
            .FirstOrDefault(rtt => rtt.text == text);
        return entry?.type ?? throw new InvalidOperationException(
            "No RuleType matches the provided text. " +
            "Unable to convert to RuleType!");
    }
}
