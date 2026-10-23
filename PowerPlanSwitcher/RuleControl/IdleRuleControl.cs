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
        ApplyLocalization();
        CmbUnit.SelectedIndex = 0;

        var executionStateText = Strings.IdleRuleControl_ExecutionStateHint;
        TipHints.SetToolTip(PibCheckExecutionState, executionStateText);

        var fullscreenAppText = Strings.IdleRuleControl_FullscreenAppHint;
        TipHints.SetToolTip(PibCheckFullscreenApp, fullscreenAppText);
    }

    private void ApplyLocalization()
    {
        label1.Text = Strings.IdleRuleControl_LblIdleTimeThreshold;
        label2.Text = Strings.IdleRuleControl_LblCheckExecutionState;
        label3.Text = Strings.IdleRuleControl_LblCheckFullscreenApp;

        var unitIndex = CmbUnit.SelectedIndex;
        CmbUnit.Items.Clear();
        CmbUnit.Items.AddRange(new object[]
        {
            Strings.IdleRuleControl_UnitSeconds,
            Strings.IdleRuleControl_UnitMinutes,
            Strings.IdleRuleControl_UnitHours,
        });
        CmbUnit.SelectedIndex = unitIndex >= 0 ? unitIndex : 0;
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
