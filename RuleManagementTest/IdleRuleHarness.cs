namespace RuleManagementTest;

using FakeItEasy;
using PowerManagement;
using RuleManagement.Dto;
using RuleManagement.Rules;
using SystemManagement;

internal class IdleRuleHarness
{
    public IdleRuleDto IdleRuleDto { get; }

    public IIdleMonitor IdleMonitor { get; }
    public IPowerManager PowerManager { get; }
    public ISystemManager SystemManager { get; }

    public IdleRule IdleRule { get; }

    public IdleRuleHarness(IdleRuleDto dto)
    {
        IdleRuleDto = dto;
        IdleMonitor = A.Fake<IIdleMonitor>();
        PowerManager = A.Fake<IPowerManager>();
        SystemManager = A.Fake<ISystemManager>();
        IdleRule = new IdleRule(
            IdleMonitor,
            PowerManager,
            SystemManager,
            IdleRuleDto);
    }
}
