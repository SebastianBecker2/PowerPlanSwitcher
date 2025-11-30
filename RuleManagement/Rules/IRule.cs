namespace RuleManagement.Rules;

using System;

public interface IRule
{
    public int ActivationCount { get; set; }
    public int Index { get; set; }
    public Guid SchemeGuid { get; set; }

    public string GetDescription();
}
