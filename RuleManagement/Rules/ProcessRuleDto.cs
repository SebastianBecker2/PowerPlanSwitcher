namespace RuleManagement.Rules;

public class ProcessRuleDto : RuleDto, IRuleDto
{
    public string FilePath
    {
        get => filePath;
        set => filePath = value.ToLowerInvariant();
    }
    public ComparisonType Type { get; set; }

    private string filePath = string.Empty;
}
