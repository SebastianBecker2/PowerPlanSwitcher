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

        private static readonly List<Tuple<RuleType, string>> RuleTypeTexts = new()
        {
            Tuple.Create(RuleType.Exact, "Match exact Path"),
            Tuple.Create(RuleType.StartsWith, "Path starts with"),
            Tuple.Create(RuleType.EndsWith, "Path ends with"),
        };

        private static List<PowerRule>? powerRules;
        private string filePath = string.Empty;
        private static readonly object PowerRulesLock = new();

        public static IEnumerable<PowerRule> GetPowerRules()
        {
            LoadPowerRules();
            return powerRules ?? new List<PowerRule>();
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
                    Settings.Default.PowerRules) ?? new List<PowerRule>();
            }
        }

        public static string RuleTypeToText(RuleType ruleType)
        {
            var text = RuleTypeTexts
                .FirstOrDefault(rtt => rtt.Item1 == ruleType)?.Item2;
            return text ?? string.Empty;
        }

        public static RuleType TextToRuleType(string text)
        {
            var type = RuleTypeTexts
                .FirstOrDefault(rtt => rtt.Item2 == text)?.Item1;
            if (type != null)
            {
                return (RuleType)type;
            }
            throw new InvalidOperationException(
                "No RuleType matches the provided text. " +
                "Unable to convert to RuleType!");
        }
    }
}
