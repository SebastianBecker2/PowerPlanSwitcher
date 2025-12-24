namespace PowerPlanSwitcher.RuleControl;

using RuleManagement.Dto;

public partial class IdleRuleControl : UserControl
{
    public IdleRuleDto Dto
    {
        get
        {
            dto.IdleTimeThreshold =
                TimeSpan.FromSeconds((double)NudIdleTimeThreshold.Value);
            return dto;
        }

        set
        {
            dto = value;
            NudIdleTimeThreshold.Value =
                (decimal)dto.IdleTimeThreshold.TotalSeconds;
        }
    }
    private IdleRuleDto dto = new();

    public IdleRuleControl() => InitializeComponent();
}
