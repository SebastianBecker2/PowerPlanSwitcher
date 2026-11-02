using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

var repoRoot = FindRepoRoot();
var resxPaths = new[]
{
    Path.Combine(repoRoot, "PowerPlanSwitcher", "Translations", "Strings.resx"),
    Path.Combine(repoRoot, "RuleManagement", "Translations", "Strings.resx"),
};
var langDir = Path.Combine(repoRoot, "PowerPlanSwitcher", "Lang");

var check = false;
var create = false;
string? onlyCulture = null;

for (var i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "--check":
            check = true;
            break;
        case "--create":
            create = true;
            break;
        case "--culture" when i + 1 < args.Length:
            onlyCulture = args[++i];
            break;
    }
}

var resx = new Dictionary<string, string>(StringComparer.Ordinal);
foreach (var path in resxPaths)
{
    if (!File.Exists(path))
    {
        Console.Error.WriteLine($"missing resx: {path}");
        return 1;
    }
    var doc = XDocument.Load(path);
    foreach (var data in doc.Root!.Elements("data"))
    {
        var name = data.Attribute("name")?.Value;
        if (string.IsNullOrEmpty(name)) continue;
        var value = (data.Element("value")?.Value ?? "").Replace("\r\n", "\n");
        if (!resx.TryAdd(name, value))
            Console.Error.WriteLine($"WARNING: duplicate key: {name}");
    }
}

Directory.CreateDirectory(langDir);

string[] jsonPaths;
if (onlyCulture is not null)
{
    var target = Path.Combine(langDir, onlyCulture + ".json");
    if (!File.Exists(target))
    {
        if (!create)
        {
            Console.Error.WriteLine(
                $"{Path.GetFileName(target)} does not exist. Use --create to generate it from Strings.resx.");
            return 1;
        }
        File.WriteAllText(target, "{}\n", new UTF8Encoding(false));
        Console.WriteLine($"created {target}");
    }
    jsonPaths = new[] { target };
}
else
{
    jsonPaths = Directory.GetFiles(langDir, "*.json");
    if (jsonPaths.Length == 0)
    {
        Console.Error.WriteLine($"no JSON files in {langDir}");
        return 1;
    }
}

var anyMissing = false;
var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
};

foreach (var jsonPath in jsonPaths.OrderBy(p => p, StringComparer.Ordinal))
{
    var fileName = Path.GetFileName(jsonPath);
    JsonObject obj;
    try
    {
        obj = JsonNode.Parse(File.ReadAllText(jsonPath))?.AsObject() ?? new JsonObject();
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"{fileName}: failed to parse: {ex.Message}");
        return 1;
    }

    var existing = new HashSet<string>(obj.Select(kv => kv.Key), StringComparer.Ordinal);
    var missing = resx.Keys.Where(k => !existing.Contains(k)).ToList();
    var extra = existing.Where(k => !resx.ContainsKey(k)).ToList();

    if (extra.Count > 0)
        Console.Error.WriteLine(
            $"{fileName}: {extra.Count} key(s) not in resx (left untouched): {string.Join(", ", extra)}");

    if (missing.Count == 0)
    {
        Console.WriteLine($"{fileName}: up to date ({existing.Count} keys)");
        continue;
    }

    anyMissing = true;
    Console.WriteLine($"{fileName}: missing {missing.Count} key(s)");
    foreach (var k in missing) Console.WriteLine($"  + {k}");

    if (check) continue;

    foreach (var k in missing) obj[k] = resx[k];
    File.WriteAllText(jsonPath, obj.ToJsonString(jsonOptions) + "\n", new UTF8Encoding(false));
    Console.WriteLine($"  -> updated {jsonPath}");
}

return check && anyMissing ? 2 : 0;

static string FindRepoRoot()
{
    var dir = new DirectoryInfo(AppContext.BaseDirectory);
    while (dir is not null)
    {
        if (File.Exists(Path.Combine(dir.FullName, "PowerPlanSwitcher.sln")))
            return dir.FullName;
        dir = dir.Parent;
    }
    throw new InvalidOperationException("Could not find repo root (PowerPlanSwitcher.sln)");
}