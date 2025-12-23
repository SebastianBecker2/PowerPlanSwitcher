namespace RuleManagement.Events;

using System;
using RuleManagement.Rules;

public class RuleApplicationChangedEventArgs(IRule? rule)
    : EventArgs
{
    public IRule? Rule { get; set; } = rule;
}
