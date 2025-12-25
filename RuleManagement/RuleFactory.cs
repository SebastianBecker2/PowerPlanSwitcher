namespace RuleManagement;

using PowerManagement;
using ProcessManagement;
using RuleManagement.Dto;
using RuleManagement.Rules;
using SystemManagement;

public class RuleFactory(
    IBatteryMonitor batteryMonitor,
    IProcessMonitor processMonitor,
    IIdleMonitor idleMonitor,
    IWindowMessageMonitor windowMessageMonitor)
{
    private readonly IBatteryMonitor powerManager = batteryMonitor;
    private readonly IProcessMonitor processMonitor = processMonitor;
    private readonly IIdleMonitor idleMonitor = idleMonitor;
    private readonly IWindowMessageMonitor windowMessageMonitor = windowMessageMonitor;

    public IRule Create(IRuleDto dto) =>
        dto switch
        {
            ProcessRuleDto processDto => new ProcessRule(processMonitor, processDto),
            PowerLineRuleDto powerLineDto => new PowerLineRule(powerManager, powerLineDto),
            IdleRuleDto idleRuleDto => new IdleRule(idleMonitor, idleRuleDto),
            StartupRuleDto startupRuleDto => new StartupRule(startupRuleDto),
            ShutdownRuleDto shutdownRuleDto => new ShutdownRule(windowMessageMonitor, shutdownRuleDto),
            _ => throw new NotSupportedException($"Unknown rule DTO type: {dto.GetType().Name}")
        };
}
