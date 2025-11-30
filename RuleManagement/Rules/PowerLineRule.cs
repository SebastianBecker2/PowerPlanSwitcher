namespace PowerPlanSwitcher.RuleManagement.Rules
{
    using System;
    using Newtonsoft.Json;

    public class PowerLineRule : IRule
    {
        [JsonIgnore]
        public int ActivationCount { get; set; }
        public int Index { get; set; }
        public Guid SchemeGuid { get; set; }
        public PowerLineStatus PowerLineStatus { get; set; }

        private static readonly List<(PowerLineStatus status, string text)>
            PowerLineStatusText =
            [
                (PowerLineStatus.Online, "Plugged in"),
                (PowerLineStatus.Offline, "On battery"),
                (PowerLineStatus.Unknown, "Unkown status"),
            ];

        public string GetDescription() =>
            $"Power Line -> {PowerLineStatusToText(PowerLineStatus)}";

        public bool CheckRule(PowerLineStatus powerLineStatus) =>
            PowerLineStatus == powerLineStatus;

        public static string PowerLineStatusToText(
            PowerLineStatus powerLineStatus)
        {
            (PowerLineStatus status, string text)? entry = PowerLineStatusText
                .FirstOrDefault(rtt => rtt.status == powerLineStatus);
            return entry?.text ?? string.Empty;
        }

        public static PowerLineStatus TextToPowerLineStatus(string text)
        {
            (PowerLineStatus status, string text)? entry = PowerLineStatusText
                .FirstOrDefault(rtt => rtt.text == text);
            return entry?.status ?? throw new InvalidOperationException(
                "No PowerLineStatus matches the provided text. " +
                "Unable to convert to PowerLineStatus!");
        }
    }
}
