namespace PowerPlanSwitcher.RuleControl;

using RuleManagement.Dto;

public partial class StartupRuleControl : UserControl
{
    private TimeSpan? GetSelectedDelay()
    {
        if (!ChbEnableDelay.Checked)
        {
            return null;
        }

        return CmbDelayUnit.SelectedIndex switch
        {
            0 => TimeSpan.FromSeconds((double)NudDelay.Value),
            1 => TimeSpan.FromMinutes((double)NudDelay.Value),
            2 => TimeSpan.FromHours((double)NudDelay.Value),
            _ => TimeSpan.FromSeconds((double)NudDelay.Value),
        };
    }

    private void SetSelectedDelay(TimeSpan? delay)
    {
        if (delay is null)
        {
            ChbEnableDelay.Checked = false;
            CmbDelayUnit.SelectedIndex = 0;
            NudDelay.Value = 0;
            return;
        }

        ChbEnableDelay.Checked = true;

        var totalSeconds = delay.Value.TotalSeconds;
        if (totalSeconds > 86400) // More than 24 hours
        {
            totalSeconds = 86400;
        }

        if (totalSeconds <= 60)
        {
            CmbDelayUnit.SelectedIndex = 0;
            NudDelay.Value = (decimal)totalSeconds;
        }
        else if (totalSeconds <= 3600)
        {
            CmbDelayUnit.SelectedIndex = 1;
            NudDelay.Value = (decimal)(totalSeconds / 60);
        }
        else
        {
            CmbDelayUnit.SelectedIndex = 2;
            NudDelay.Value = (decimal)(totalSeconds / 3600);
        }
    }

    private TimeSpan? GetSelectedDuration()
    {
        if (!ChbEnableDuration.Checked)
        {
            return null;
        }

        return CmbUnit.SelectedIndex switch
        {
            0 => TimeSpan.FromSeconds((double)NudDuration.Value),
            1 => TimeSpan.FromMinutes((double)NudDuration.Value),
            2 => TimeSpan.FromHours((double)NudDuration.Value),
            _ => TimeSpan.FromSeconds((double)NudDuration.Value),
        };
    }

    private void SetSelectedDuration(TimeSpan? duration)
    {
        if (duration is null)
        {
            ChbEnableDuration.Checked = false;
            CmbUnit.SelectedIndex = 0;
            NudDuration.Value = NudDuration.Minimum;
            return;
        }

        ChbEnableDuration.Checked = true;

        var totalSeconds = duration.Value.TotalSeconds;
        if (totalSeconds > 86400) // More than 24 hours
        {
            totalSeconds = 86400;
        }

        if (totalSeconds <= 60)
        {
            CmbUnit.SelectedIndex = 0;
            NudDuration.Value = (decimal)totalSeconds;
        }
        else if (totalSeconds <= 3600)
        {
            CmbUnit.SelectedIndex = 1;
            NudDuration.Value = (decimal)(totalSeconds / 60);
        }
        else
        {
            CmbUnit.SelectedIndex = 2;
            NudDuration.Value = (decimal)(totalSeconds / 3600);
        }
    }

    public StartupRuleDto Dto
    {
        get
        {
            dto.Delay = GetSelectedDelay();
            dto.Duration = GetSelectedDuration();
            return dto;
        }

        set
        {
            dto = value;
            SetSelectedDelay(dto.Delay);
            SetSelectedDuration(dto.Duration);
        }
    }
    private StartupRuleDto dto = new();

    public StartupRuleControl()
    {
        InitializeComponent();
        CmbDelayUnit.SelectedIndex = 0;
        CmbUnit.SelectedIndex = 0;

        var delayHint = "When enabled, triggering of this Startup Rule is postponed by the configured delay after monitoring starts." +
            $"{Environment.NewLine}If you also enable a duration, the countdown for that duration begins only when the rule actually becomes triggered (after the delay).";
        TipHints.SetToolTip(PibDelayHint, delayHint);

        var durationHint = "Enable this option to automatically untrigger this Startup Rule after the specified duration." +
            $"{Environment.NewLine}If disabled, the Startup Rule remains triggered indefinitely once it has triggered.";
        TipHints.SetToolTip(PibDurationHint, durationHint);
    }

    private void PibDelayHint_Click(object sender, EventArgs e) =>
        TipHints.Show(TipHints.GetToolTip(PibDelayHint),
            PibDelayHint,
            0,
            PibDelayHint.Height,
            3000);

    private void PibDurationHint_Click(object sender, EventArgs e) =>
        TipHints.Show(TipHints.GetToolTip(PibDurationHint),
            PibDurationHint,
            0,
            PibDurationHint.Height,
            3000);

    private void ChbEnableDelay_CheckedChanged(object sender, EventArgs e)
    {
        NudDelay.Enabled = ChbEnableDelay.Checked;
        CmbDelayUnit.Enabled = ChbEnableDelay.Checked;
        if (ChbEnableDelay.Checked && NudDelay.Value == 0)
        {
            NudDelay.Value = 1; // Default to 1 unit when enabling
        }
    }

    private void ChbEnableDuration_CheckedChanged(object sender, EventArgs e)
    {
        NudDuration.Enabled = ChbEnableDuration.Checked;
        CmbUnit.Enabled = ChbEnableDuration.Checked;
        if (ChbEnableDuration.Checked && NudDuration.Value == 0)
        {
            NudDuration.Value = 1; // Default to 1 unit when enabling
        }
    }
}
