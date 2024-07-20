namespace PowerPlanSwitcher.RuleManagement.Rules
{
    using System.ComponentModel;
    using Newtonsoft.Json;
    using PowerPlanSwitcher.ProcessManagement;
    using PowerPlanSwitcher.RuleManagement;

    public class ProcessRule : IRule
    {
        [JsonIgnore]
        public int ActivationCount { get; set; }
        public int Index { get; set; }
        public Guid SchemeGuid { get; set; }
        public string FilePath
        {
            get => filePath;
            set => filePath = value.ToLowerInvariant();
        }
        public PathCheckType Type { get; set; }

        private static readonly List<(PathCheckType type, string text)>
            PathCheckTypeText =
            [
                (PathCheckType.Exact, "Match exact Path"),
                (PathCheckType.StartsWith, "Path starts with"),
                (PathCheckType.EndsWith, "Path ends with"),
            ];

        private string filePath = string.Empty;

        public string GetDescription() =>
            $"Process -> {RuleTypeToText(Type)} -> {FilePath}";

        public bool CheckRule(ICachedProcess process)
        {
            try
            {
                var path = process.ExecutablePath;
                if (string.IsNullOrWhiteSpace(path))
                {
                    return false;
                }

                return Type switch
                {
                    PathCheckType.Exact => path == FilePath,
                    PathCheckType.StartsWith => path.StartsWith(
                        FilePath,
                        StringComparison.InvariantCulture),
                    PathCheckType.EndsWith => path.EndsWith(
                        FilePath,
                        StringComparison.InvariantCulture),
                    _ => throw new InvalidOperationException(
                        $"Unable to apply rule type {Type}"),
                };
            }
            catch (Win32Exception)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public static string RuleTypeToText(PathCheckType ruleType)
        {
            (PathCheckType type, string text)? entry = PathCheckTypeText
                .FirstOrDefault(rtt => rtt.type == ruleType);
            return entry?.text ?? string.Empty;
        }

        public static PathCheckType TextToRuleType(string text)
        {
            (PathCheckType type, string text)? entry = PathCheckTypeText
                .FirstOrDefault(rtt => rtt.text == text);
            return entry?.type ?? throw new InvalidOperationException(
                "No RuleType matches the provided text. " +
                "Unable to convert to RuleType!");
        }
    }
}
