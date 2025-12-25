namespace RuleManagement;

using PowerManagement;
using ProcessManagement;
using RuleManagement.Dto;
using RuleManagement.Rules;
using SystemManagement;

public class RuleFactory(
    IBatteryMonitor batteryMonitor,
    IProcessMonitor processMonitor,
    IIdleMonitor idleMonitor)
{
    private readonly IBatteryMonitor powerManager = batteryMonitor;
    private readonly IProcessMonitor processMonitor = processMonitor;
    private readonly IIdleMonitor idleMonitor = idleMonitor;

    public IRule Create(IRuleDto dto) =>
        dto switch
        {
            ProcessRuleDto processDto => new ProcessRule(processMonitor, processDto),
            PowerLineRuleDto powerLineDto => new PowerLineRule(powerManager, powerLineDto),
            IdleRuleDto idleRuleDto => new IdleRule(idleMonitor, idleRuleDto),
            StartupRuleDto startupRuleDto => new StartupRule(startupRuleDto),
            _ => throw new NotSupportedException($"Unknown rule DTO type: {dto.GetType().Name}")
        };
}
