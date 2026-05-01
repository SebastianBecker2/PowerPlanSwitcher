namespace RuleManagement.Dto;

using System;

public class StartupRuleDto : RuleDto, IRuleDto
{
    /// <summary>
    /// Gets or sets the delay before the Startup Rule becomes triggered after monitoring starts.
    /// If null, the rule is triggered immediately (subject to duration). If set, triggering is postponed
    /// until the delay has elapsed.
    /// Valid range when enabled: 1 second to 24 hours (same as duration).
    /// </summary>
    public TimeSpan? Delay { get; set; }

    /// <summary>
    /// Gets or sets the duration for which the Startup Rule will remain active after it becomes triggered.
    /// If null, the rule remains active indefinitely once triggered. If set, the rule will be automatically untriggered
    /// after the specified duration has elapsed (counted from the moment the rule actually triggers, including after any delay).
    /// Valid range: 1 second to 24 hours.
    /// </summary>
    public TimeSpan? Duration { get; set; }

    public override string GetDescription()
    {
        var delayPart = Delay is null ? null : $"delay {FormatSpanForDescription(Delay.Value)}";
        var durationPart = Duration is null ? null : $"duration {FormatSpanForDescription(Duration.Value)}";

        if (delayPart is null && durationPart is null)
        {
            return "Startup Rule";
        }

        if (delayPart is not null && durationPart is null)
        {
            return $"Startup Rule ({delayPart})";
        }

        if (delayPart is null && durationPart is not null)
        {
            return $"Startup Rule ({durationPart})";
        }

        return $"Startup Rule ({delayPart}, {durationPart})";
    }

    private static string FormatSpanForDescription(TimeSpan span)
    {
        var totalSeconds = (long)span.TotalSeconds;
        if (totalSeconds < 60)
        {
            return $"{totalSeconds} second{(totalSeconds != 1 ? "s" : "")}";
        }

        var totalMinutes = totalSeconds / 60;
        if (totalMinutes < 60)
        {
            return $"{totalMinutes} minute{(totalMinutes != 1 ? "s" : "")}";
        }

        var totalHours = totalMinutes / 60;
        return $"{totalHours} hour{(totalHours != 1 ? "s" : "")}";
    }
}
