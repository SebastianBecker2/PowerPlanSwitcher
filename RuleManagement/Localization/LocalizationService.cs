using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text.Json;

namespace RuleManagement.Localization;

public static class LocalizationService
{
    private static IReadOnlyDictionary<string, string> _strings =
        new Dictionary<string, string>();

    public static void Initialize(string? cultureName = null)
    {
        cultureName ??= CultureInfo.CurrentUICulture.Name;
        _strings = LoadJson(cultureName);
    }

    private static IReadOnlyDictionary<string, string> LoadJson(string cultureName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Lang", $"{cultureName}.json");
        if (!File.Exists(path))
            return new Dictionary<string, string>();
        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(
                File.ReadAllText(path)) ?? new Dictionary<string, string>();
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

    public static ResourceManager CreateResourceManager(string baseName, Assembly assembly)
        => new JsonResourceManager(baseName, assembly, _strings);
}
