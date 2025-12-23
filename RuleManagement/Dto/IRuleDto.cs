namespace RuleManagement.Dto;

public interface IRuleDto
{
    public Guid SchemeGuid { get; set; }

    public string GetDescription();
}
