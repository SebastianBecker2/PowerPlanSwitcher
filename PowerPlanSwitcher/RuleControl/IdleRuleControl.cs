namespace PowerPlanSwitcher.RuleControl;

using RuleManagement.Dto;

public partial class IdleRuleControl : UserControl
{
    private TimeSpan GetSelectedThreshold() =>
        CmbUnit.SelectedIndex switch
        {
            0 => TimeSpan.FromSeconds((double)NudIdleTimeThreshold.Value),
            1 => TimeSpan.FromMinutes((double)NudIdleTimeThreshold.Value),
            2 => TimeSpan.FromHours((double)NudIdleTimeThreshold.Value),
            _ => TimeSpan.FromSeconds((double)NudIdleTimeThreshold.Value),
        };

    private void SetSelectedThreshold(TimeSpan threshold)
    {
        if (threshold.TotalHours >= 1)
        {
            CmbUnit.SelectedIndex = 2;
            NudIdleTimeThreshold.Value = (decimal)threshold.TotalHours;
        }
        else if (threshold.TotalMinutes >= 1)
        {
            CmbUnit.SelectedIndex = 1;
            NudIdleTimeThreshold.Value = (decimal)threshold.TotalMinutes;
        }
        else
        {
            CmbUnit.SelectedIndex = 0;
            NudIdleTimeThreshold.Value = (decimal)threshold.TotalSeconds;
        }
    }

    public IdleRuleDto Dto
    {
        get
        {
            dto.IdleTimeThreshold = GetSelectedThreshold();
            return dto;
        }

        set
        {
            dto = value;
            SetSelectedThreshold(dto.IdleTimeThreshold);
        }
    }
    private IdleRuleDto dto = new();

    public IdleRuleControl()
    {
        InitializeComponent();
        CmbUnit.SelectedIndex = 0;
    }
}
