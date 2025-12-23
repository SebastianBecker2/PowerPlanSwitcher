namespace RuleManagement;

using PowerManagement;
using ProcessManagement;
using RuleManagement.Dto;
using RuleManagement.Rules;

public class RuleFactory(
    IBatteryMonitor batteryMonitor,
    IProcessMonitor processMonitor)
{
    private readonly IBatteryMonitor powerManager = batteryMonitor;
    private readonly IProcessMonitor processMonitor = processMonitor;

    public IRule Create(IRuleDto dto) =>
        dto switch
        {
            ProcessRuleDto processDto => new ProcessRule(processMonitor, processDto),
            PowerLineRuleDto powerLineDto => new PowerLineRule(powerManager, powerLineDto),
            _ => throw new NotSupportedException($"Unknown rule DTO type: {dto.GetType().Name}")
        };
}
