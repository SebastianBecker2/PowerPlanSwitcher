using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace RuleManagement.Localization;

public sealed class JsonResourceManager : ResourceManager
{
    private readonly IReadOnlyDictionary<string, string> _external;

    public JsonResourceManager(string baseName, Assembly assembly,
        IReadOnlyDictionary<string, string> external)
        : base(baseName, assembly) => _external = external;

    public override string? GetString(string name) =>
        _external.TryGetValue(name, out var v) ? v : base.GetString(name);

    public override string? GetString(string name, CultureInfo? culture) =>
        _external.TryGetValue(name, out var v) ? v : base.GetString(name, culture);
}
