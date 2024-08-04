namespace PowerPlanSwitcher.RuleManagement.Rules
{
    using System;

    public interface IRule
    {
        int ActivationCount { get; set; }
        int Index { get; set; }
        Guid SchemeGuid { get; set; }

        string GetDescription();
    }
}
