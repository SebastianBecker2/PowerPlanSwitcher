namespace RuleManagement.Rules;

public abstract class RuleDto
{
    public Guid SchemeGuid { get; set; }

    public abstract string GetDescription();
}
