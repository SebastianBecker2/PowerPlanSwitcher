namespace PowerPlanSwitcher.RuleControl;

using RuleManagement.Dto;
using PowerLineStatus = PowerManagement.PowerLineStatus;

public partial class PowerLineRuleControl : UserControl
{
    private static readonly List<PowerLineStatus> PowerLineStatuses =
        [.. Enum.GetValues(typeof(PowerLineStatus)).Cast<PowerLineStatus>()];

    public PowerLineRuleDto Dto
    {
        get
        {
            dto.PowerLineStatus =
                PowerLineStatuses[CmbPowerLineStatus.SelectedIndex];
            return dto;
        }

        set
        {
            dto = value;
            CmbPowerLineStatus.SelectedIndex =
                PowerLineStatuses.IndexOf(dto.PowerLineStatus);
        }
    }
    private PowerLineRuleDto dto = new();

    public PowerLineRuleControl()
    {
        InitializeComponent();

        CmbPowerLineStatus.Items.AddRange([.. PowerLineStatuses
            .Select(PowerLineRuleDto.PowerLineStatusToText)
            .Cast<object>()]);
        CmbPowerLineStatus.SelectedIndex = 0;
    }
}
