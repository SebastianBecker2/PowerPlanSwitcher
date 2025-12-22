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
}
