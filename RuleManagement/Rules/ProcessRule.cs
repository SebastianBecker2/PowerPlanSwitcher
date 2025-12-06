namespace RuleManagement.Rules;

using System.ComponentModel;
using ProcessManagement;

public class ProcessRule
    : Rule<ProcessRuleDto>,
    IRule<ProcessRuleDto>
{
    public Guid SchemeGuid => Dto.SchemeGuid;
    public string FilePath => Dto.FilePath;
    public ComparisonType Type => Dto.Type;

    private static readonly List<(ComparisonType type, string text)>
        ComparisonTypeText =
        [
            (ComparisonType.Exact, "Match exact Path"),
            (ComparisonType.StartsWith, "Path starts with"),
            (ComparisonType.EndsWith, "Path ends with"),
        ];
    private static readonly IReadOnlyDictionary<string, ComparisonType> TextToTypeMap =
        ComparisonTypeText.ToDictionary(entry => entry.text, entry => entry.type, StringComparer.Ordinal);

    public ProcessRule(
        IProcessMonitor processMonitor,
        ProcessRuleDto processRuleDto)
        : base(processRuleDto)
    {
        processMonitor.ProcessCreated += ProcessMonitor_ProcessCreated;
        processMonitor.ProcessTerminated += ProcessMonitor_ProcessTerminated;
    }

    private void ProcessMonitor_ProcessCreated(object? sender, ProcessEventArgs e)
    {

        if (CheckRule(e.Process))
        {
            TriggerCount++;
        }
    }

    private void ProcessMonitor_ProcessTerminated(object? sender, ProcessEventArgs e)
    {
        // If rule applies
        if (CheckRule(e.Process))
        {
            TriggerCount = Math.Max(TriggerCount - 1, 0);
        }
    }

    public override string GetDescription() =>
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
        if (!TextToTypeMap.TryGetValue(text, out var type))
        {
            throw new InvalidOperationException(
                "No RuleType matches the provided text. " +
                "Unable to convert to RuleType!");
        }
        return type;
    }
}
