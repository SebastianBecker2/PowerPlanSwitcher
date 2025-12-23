namespace RuleManagement.Rules;

using System;
using RuleManagement.Dto;
using RuleManagement.Events;

public interface IRule
{
    public int TriggerCount { get; }
    public IRuleDto Dto { get; }

    public event EventHandler<TriggerChangedEventArgs>? TriggerChanged;
}

public interface IRule<TDto> : IRule where TDto : IRuleDto
{
    public new TDto Dto { get; }
}
