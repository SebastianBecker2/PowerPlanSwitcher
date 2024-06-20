namespace PowerPlanSwitcher
{
    using Newtonsoft.Json;
    using Properties;

    public class PowerRule
    {
        public int Index { get; set; }
        public string FilePath
        {
            get => filePath;
            set => filePath = value.ToLowerInvariant();
        }
        public RuleType Type { get; set; }
        public Guid SchemeGuid { get; set; }
        [JsonIgnore]
        public bool Active { get; set; }

        private static readonly List<(RuleType type, string text)> RuleTypeTexts =
            [
                (RuleType.Exact, "Match exact Path"),
                (RuleType.StartsWith, "Path starts with"),
                (RuleType.EndsWith, "Path ends with"),
            ];

        private static List<PowerRule>? powerRules;
        private string filePath = string.Empty;
        private static readonly object PowerRulesLock = new();

        public static IEnumerable<PowerRule> GetPowerRules()
        {
            LoadPowerRules();
            return powerRules ?? [];
        }

        public static void SetPowerRules(IEnumerable<PowerRule> newPowerRules) =>
            powerRules = newPowerRules.ToList();

        public static void SavePowerRules()
        {
            Settings.Default.PowerRules =
                JsonConvert.SerializeObject(powerRules);
            Settings.Default.Save();
        }

        private static void LoadPowerRules()
        {
            if (powerRules is not null)
            {
                return;
            }

            lock (PowerRulesLock)
            {
                if (powerRules is not null)
                {
                    return;
                }

                powerRules = JsonConvert.DeserializeObject<List<PowerRule>>(
                    Settings.Default.PowerRules) ?? [];
            }
        }

        public static string RuleTypeToText(RuleType ruleType)
        {
            (RuleType type, string text)? type = RuleTypeTexts
                .FirstOrDefault(rtt => rtt.type == ruleType);
            return type?.text ?? string.Empty;
        }

        public static RuleType TextToRuleType(string text)
        {
            (RuleType type, string text)? type = RuleTypeTexts
                .FirstOrDefault(rtt => rtt.text == text);
            return type?.type ?? throw new InvalidOperationException(
                "No RuleType matches the provided text. " +
                "Unable to convert to RuleType!");
        }
    }
}
