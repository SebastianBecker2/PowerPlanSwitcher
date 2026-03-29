namespace RuleManagement.Dto;

using System;

public class StartupRuleDto : RuleDto, IRuleDto
{
    /// <summary>
    /// Gets or sets the duration for which the Startup Rule will remain active after instantiation.
    /// If null, the rule remains active indefinitely. If set, the rule will be automatically untriggered
    /// after the specified duration has elapsed.
    /// Valid range: 1 second to 24 hours.
    /// </summary>
    public TimeSpan? Duration { get; set; }

    public override string GetDescription()
    {
        if (Duration is null)
        {
            return "Startup Rule";
        }

        var totalSeconds = (long)Duration.Value.TotalSeconds;
        if (totalSeconds < 60)
        {
            return $"Startup Rule ({totalSeconds} second{(totalSeconds != 1 ? "s" : "")})";
        }

        var totalMinutes = totalSeconds / 60;
        if (totalMinutes < 60)
        {
            return $"Startup Rule ({totalMinutes} minute{(totalMinutes != 1 ? "s" : "")})";
        }

        var totalHours = totalMinutes / 60;
        return $"Startup Rule ({totalHours} hour{(totalHours != 1 ? "s" : "")})";
    }
}
