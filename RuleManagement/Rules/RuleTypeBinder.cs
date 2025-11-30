namespace RuleManagement.Rules;

using Newtonsoft.Json.Serialization;

internal class RuleTypeBinder : ISerializationBinder
{
    private readonly Dictionary<string, Type> map = new()
    {
        // Map legacy type names to new DTOs
        { "PowerPlanSwitcher.RuleManagement.Rules.ProcessRule, PowerPlanSwitcher", typeof(ProcessRuleDto) },
        { "PowerPlanSwitcher.RuleManagement.Rules.PowerLineRule, PowerPlanSwitcher", typeof(PowerLineRuleDto) }
    };

    public Type BindToType(string? assemblyName, string typeName)
    {
        var fullName = $"{typeName}, {assemblyName}";
        if (map.TryGetValue(fullName, out var mapped))
        {
            return mapped;
        }

        // fallback: try normal resolution
        return Type.GetType(fullName, throwOnError: true)!;
    }

    public void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
    {
        // When serializing, always use current type names
        assemblyName = serializedType.Assembly.FullName;
        typeName = serializedType.FullName;
    }
}
