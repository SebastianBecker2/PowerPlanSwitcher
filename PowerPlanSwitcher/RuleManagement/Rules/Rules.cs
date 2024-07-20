namespace PowerPlanSwitcher.RuleManagement.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using PowerPlanSwitcher.Properties;

    internal class Rules
    {
        private static List<IRule>? rules;
        private static readonly object Lock = new();

        public static IEnumerable<IRule> GetRules()
        {
            lock (Lock)
            {
                rules ??= LoadRules();
                return rules ?? [];
            }
        }

        private static List<IRule>? LoadRules() =>
            JsonConvert.DeserializeObject<List<IRule>>(
                Settings.Default.Rules,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

        public static void SetRules(IEnumerable<IRule> newRules)
        {
            rules = newRules.ToList();
            SaveRules(rules);
        }

        private static void SaveRules(IEnumerable<IRule> rules)
        {
            Settings.Default.Rules =
                JsonConvert.SerializeObject(rules,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            Settings.Default.Save();
        }
    }
}
