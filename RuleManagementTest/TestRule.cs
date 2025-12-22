namespace RuleManagementTest;

using System;
using RuleManagement.Rules;

internal class TestRuleDto : IRuleDto
{
    public Guid SchemeGuid { get; set; }

    public string GetDescription() => "";
}

internal class TestRule(TestRuleDto dto) : IRule<TestRuleDto>
{
    public int TriggerCount
    {
        get => triggerCount;
        set
        {
            triggerCount = value;
            TriggerChanged?.Invoke(this, new TriggerChangedEventArgs(this));
        }
    }
    private int triggerCount;

    // Strongly typed DTO
    public TestRuleDto Dto { get; } = dto;

    // Explicit implementation for the non-generic interface
    IRuleDto IRule.Dto => Dto;

    public event EventHandler<TriggerChangedEventArgs>? TriggerChanged;

    public static string GetDescription() => "";
}
