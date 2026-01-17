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
            dto.CheckExecutionState = ChbCheckExecutionState.Checked;
            dto.CheckFullscreenApps = ChbCheckFullscreenApp.Checked;
            return dto;
        }

        set
        {
            dto = value;
            SetSelectedThreshold(dto.IdleTimeThreshold);
            ChbCheckExecutionState.Checked = dto.CheckExecutionState;
            ChbCheckFullscreenApp.Checked = dto.CheckFullscreenApps;
        }
    }
    private IdleRuleDto dto = new();

    public IdleRuleControl()
    {
        InitializeComponent();
        CmbUnit.SelectedIndex = 0;

        var executionStateText = $"Prevent idle when another process set any of these Execution States:" +
            $"{Environment.NewLine}- ES_AWAYMODE_REQUIRED Forces the system to continue running critical background processes.{Environment.NewLine}" +
            $"- ES_DISPLAY_REQUIRED Forces the display to be on by resetting the display idle timer.{Environment.NewLine}" +
            $"- ES_SYSTEM_REQUIRED Forces the system to be in the working state by resetting the system idle timer.";
        TipHints.SetToolTip(PibCheckExecutionState, executionStateText);

        var fullscreenAppText = $"Prevent idle when the active process is running in fullscreen mode.";
        TipHints.SetToolTip(PibCheckFullscreenApp, fullscreenAppText);
    }

    private void PibCheckExecutionState_Click(object sender, EventArgs e) =>
        TipHints.Show(TipHints.GetToolTip(PibCheckExecutionState),
            PibCheckExecutionState,
            0,
            PibCheckExecutionState.Height,
            3000);

    private void PibCheckFullscreenApp_Click(object sender, EventArgs e) =>
        TipHints.Show(TipHints.GetToolTip(PibCheckFullscreenApp),
            PibCheckFullscreenApp,
            0,
            PibCheckFullscreenApp.Height,
            3000);
}
