namespace PowerPlanSwitcher.RuleControl;

using RuleManagement.Dto;

public partial class StartupRuleControl : UserControl
{
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
            dto.Duration = GetSelectedDuration();
            return dto;
        }

        set
        {
            dto = value;
            SetSelectedDuration(dto.Duration);
        }
    }
    private StartupRuleDto dto = new();

    public StartupRuleControl()
    {
        InitializeComponent();
        CmbUnit.SelectedIndex = 0;

        var durationHint = "Enable this option to automatically untrigger this Startup Rule after the specified duration." +
            $"{Environment.NewLine}If disabled, the Startup Rule remains triggered indefinitely.";
        TipHints.SetToolTip(PibDurationHint, durationHint);
    }

    private void PibDurationHint_Click(object sender, EventArgs e) =>
        TipHints.Show(TipHints.GetToolTip(PibDurationHint),
            PibDurationHint,
            0,
            PibDurationHint.Height,
            3000);

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
