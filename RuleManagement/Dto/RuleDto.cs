namespace RuleManagement.Dto;

public abstract class RuleDto
{
    public Guid SchemeGuid { get; set; }

    public abstract string GetDescription();
}
