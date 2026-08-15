namespace PowerPlanSwitcher.Cli;

using System.CommandLine;
using System.Text.Json;
using PowerManagement;

internal static class Program
{
    private static readonly JsonSerializerOptions JsonWriteOptions = new()
    {
        WriteIndented = true,
    };

    private static async Task<int> Main(string[] args)
    {
        var root = new RootCommand(
            "Command-line interface for PowerPlanSwitcher. " +
            "Talks to Windows power APIs and reads Visible settings from the tray app's user config.");

        root.Subcommands.Add(CreateListCommand());
        root.Subcommands.Add(CreateActivateCommand());
        root.Subcommands.Add(CreateCycleCommand());
        root.Subcommands.Add(CreateGetActiveCommand());

        return await root.Parse(args).InvokeAsync();
    }

    private static Command CreateListCommand()
    {
        var visibleOnlyOption = new Option<bool>("--visible")
        {
            Description = "Only list plans marked visible in PowerPlanSwitcher settings.",
        };
        var jsonOption = new Option<bool>("--json")
        {
            Description = "Emit machine-readable JSON.",
        };

        var command = new Command("list", "List Windows power plans.")
        {
            visibleOnlyOption,
            jsonOption,
        };

        command.SetAction(parseResult =>
        {
            var visibleOnly = parseResult.GetValue(visibleOnlyOption);
            var json = parseResult.GetValue(jsonOption);
            var preferences = PowerSchemeUserPreferences.Load();
            var schemes = PowerSchemeOperations.ListSchemes(preferences)
                .Where(scheme => !visibleOnly || scheme.Visible)
                .ToList();

            if (json)
            {
                Console.WriteLine(JsonSerializer.Serialize(schemes, JsonWriteOptions));
                return 0;
            }

            if (schemes.Count == 0)
            {
                Console.Error.WriteLine(visibleOnly
                    ? "No visible power plans found."
                    : "No power plans found.");
                return 1;
            }

            foreach (var scheme in schemes)
            {
                var markers = new List<string>();
                if (scheme.IsActive)
                {
                    markers.Add("active");
                }
                if (scheme.Visible)
                {
                    markers.Add("visible");
                }

                var markerText = markers.Count > 0
                    ? $" [{string.Join(", ", markers)}]"
                    : string.Empty;

                Console.WriteLine(
                    $"{scheme.Id}  {scheme.Name ?? "<No Name>"}{markerText}");
            }

            return 0;
        });

        return command;
    }

    private static Command CreateActivateCommand()
    {
        var idArgument = new Argument<string>("id")
        {
            Description = "Power plan GUID or exact name (from 'pps list').",
        };

        var command = new Command("activate", "Activate a power plan by GUID or name.")
        {
            idArgument,
        };

        command.SetAction(parseResult =>
        {
            var id = parseResult.GetValue(idArgument);
            if (!PowerSchemeOperations.TryResolveScheme(id!, out var schemeGuid, out var error))
            {
                Console.Error.WriteLine(error);
                return 2;
            }

            try
            {
                PowerSchemeOperations.Activate(schemeGuid);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to activate power plan: {ex.Message}");
                return 1;
            }

            var name = PowerManager.Api.GetPowerSchemeName(schemeGuid) ?? "<No Name>";
            Console.WriteLine($"Activated: {schemeGuid}  {name}");
            return 0;
        });

        return command;
    }

    private static Command CreateCycleCommand()
    {
        var allOption = new Option<bool>("--all")
        {
            Description = "Cycle through all power plans (ignore Visible flags).",
        };
        var visibleOption = new Option<bool>("--visible")
        {
            Description = "Cycle through visible power plans only.",
        };

        var command = new Command(
            "cycle",
            "Activate the next power plan. " +
            "Default follows the tray app's CycleOnlyVisible setting.")
        {
            allOption,
            visibleOption,
        };

        command.SetAction(parseResult =>
        {
            var all = parseResult.GetValue(allOption);
            var visible = parseResult.GetValue(visibleOption);
            if (all && visible)
            {
                Console.Error.WriteLine("Specify either --all or --visible, not both.");
                return 1;
            }

            var preferences = PowerSchemeUserPreferences.Load();
            var visibleOnly = visible || (!all && preferences.CycleOnlyVisible);

            if (!PowerSchemeOperations.TryCycle(
                    visibleOnly,
                    out var activatedGuid,
                    out var error,
                    preferences))
            {
                Console.Error.WriteLine(error);
                return 1;
            }

            var name = PowerManager.Api.GetPowerSchemeName(activatedGuid) ?? "<No Name>";
            var scope = visibleOnly ? "visible" : "all";
            Console.WriteLine($"Cycled ({scope}): {activatedGuid}  {name}");
            return 0;
        });

        return command;
    }

    private static Command CreateGetActiveCommand()
    {
        var jsonOption = new Option<bool>("--json")
        {
            Description = "Emit machine-readable JSON.",
        };

        var command = new Command("get-active", "Show the currently active power plan.")
        {
            jsonOption,
        };

        command.SetAction(parseResult =>
        {
            var json = parseResult.GetValue(jsonOption);
            var preferences = PowerSchemeUserPreferences.Load();
            var active = PowerSchemeOperations.ListSchemes(preferences)
                .FirstOrDefault(scheme => scheme.IsActive);

            if (active is null)
            {
                var guid = PowerManager.Api.GetActivePowerSchemeGuid();
                if (guid == Guid.Empty)
                {
                    Console.Error.WriteLine("No active power plan could be determined.");
                    return 1;
                }

                active = new PowerSchemeInfo(
                    guid,
                    PowerManager.Api.GetPowerSchemeName(guid),
                    preferences.IsVisible(guid),
                    IsActive: true);
            }

            if (json)
            {
                Console.WriteLine(JsonSerializer.Serialize(active, JsonWriteOptions));
                return 0;
            }

            Console.WriteLine($"{active.Id}  {active.Name ?? "<No Name>"}");
            return 0;
        });

        return command;
    }
}
