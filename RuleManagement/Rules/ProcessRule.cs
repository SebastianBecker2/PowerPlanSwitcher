namespace RuleManagement.Rules;

using System.ComponentModel;
using DotNet.Globbing;
using ProcessManagement;
using RuleManagement.Dto;

public class ProcessRule
    : Rule<ProcessRuleDto>,
    IRule<ProcessRuleDto>,
    IDisposable
{
    private readonly Glob? glob;

    public Guid SchemeGuid => Dto.SchemeGuid;
    public string Pattern => Dto.Pattern;
    public ComparisonType Type => Dto.Type;

    private readonly IProcessMonitor processMonitor;

    public ProcessRule(
        IProcessMonitor processMonitor,
        ProcessRuleDto processRuleDto)
        : base(processRuleDto)
    {
        this.processMonitor = processMonitor;

        if (processRuleDto.Type == ComparisonType.Wildcard)
        {
            glob = Glob.Parse(
                processRuleDto.Pattern,
                new GlobOptions
                {
                    Evaluation = { CaseInsensitive = false }
                });
        }
    }

    public override void StartRuling()
    {
        processMonitor.ProcessCreated += ProcessMonitor_ProcessCreated;
        processMonitor.ProcessTerminated += ProcessMonitor_ProcessTerminated;

        TriggerCount = 0;
        foreach (var process in processMonitor.GetUsersProcesses())
        {
            if (CheckRule(process))
            {
                TriggerCount++;
            }
        }
    }

    public override void StopRuling()
    {
        processMonitor.ProcessCreated -= ProcessMonitor_ProcessCreated;
        processMonitor.ProcessTerminated -= ProcessMonitor_ProcessTerminated;
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
                ComparisonType.Exact => path == Pattern,
                ComparisonType.StartsWith => path.StartsWith(
                    Pattern,
                    StringComparison.InvariantCulture),
                ComparisonType.EndsWith => path.EndsWith(
                    Pattern,
                    StringComparison.InvariantCulture),
                ComparisonType.Wildcard => glob?.IsMatch(path) ?? false,
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Suppression of CA1816 is necessary")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "ProcessRule does not have a finalizer")]
    public void Dispose() => StopRuling();
}
