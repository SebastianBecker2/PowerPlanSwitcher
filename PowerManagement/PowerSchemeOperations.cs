namespace PowerManagement;

public sealed record PowerSchemeInfo(
    Guid Id,
    string? Name,
    bool Visible,
    bool IsActive);

public static class PowerSchemeOperations
{
    public static IReadOnlyList<PowerSchemeInfo> ListSchemes(
        PowerSchemeUserPreferences? preferences = null)
    {
        preferences ??= PowerSchemeUserPreferences.Load();
        var active = PowerManager.Api.GetActivePowerSchemeGuid();

        return
        [
            .. PowerManager.Api.GetPowerSchemes()
                .Where(scheme => !string.IsNullOrWhiteSpace(scheme.name)
                    || scheme.guid != Guid.Empty)
                .Select(scheme => new PowerSchemeInfo(
                    scheme.guid,
                    scheme.name,
                    preferences.IsVisible(scheme.guid),
                    scheme.guid == active))
        ];
    }

    public static IReadOnlyList<Guid> GetCycleCandidates(
        bool visibleOnly,
        PowerSchemeUserPreferences? preferences = null)
    {
        preferences ??= PowerSchemeUserPreferences.Load();

        return
        [
            .. PowerManager.Api.GetPowerSchemeGuids()
                .Where(guid => !visibleOnly || preferences.IsVisible(guid))
        ];
    }

    public static bool TryResolveScheme(
        string idOrName,
        out Guid schemeGuid,
        out string? error)
    {
        schemeGuid = Guid.Empty;
        error = null;

        if (string.IsNullOrWhiteSpace(idOrName))
        {
            error = "Power plan id or name is required.";
            return false;
        }

        if (Guid.TryParse(idOrName.Trim(), out var parsedGuid))
        {
            if (!PowerManager.Api.GetPowerSchemeGuids().Contains(parsedGuid))
            {
                error = $"No power plan found with id '{parsedGuid}'.";
                return false;
            }

            schemeGuid = parsedGuid;
            return true;
        }

        var matches = PowerManager.Api.GetPowerSchemes()
            .Where(scheme => string.Equals(
                scheme.name,
                idOrName.Trim(),
                StringComparison.OrdinalIgnoreCase))
            .Select(scheme => scheme.guid)
            .Distinct()
            .ToList();

        if (matches.Count == 0)
        {
            error = $"No power plan found with name '{idOrName}'.";
            return false;
        }

        if (matches.Count > 1)
        {
            error =
                $"Multiple power plans match name '{idOrName}'. " +
                "Use the plan id from 'pps list' instead.";
            return false;
        }

        schemeGuid = matches[0];
        return true;
    }

    public static void Activate(Guid schemeGuid) =>
        PowerManager.Api.SetActivePowerScheme(schemeGuid);

    public static bool TryCycle(
        bool visibleOnly,
        out Guid activatedGuid,
        out string? error,
        PowerSchemeUserPreferences? preferences = null)
    {
        activatedGuid = Guid.Empty;
        preferences ??= PowerSchemeUserPreferences.Load();

        var candidates = GetCycleCandidates(visibleOnly, preferences);
        if (candidates.Count == 0)
        {
            error = visibleOnly
                ? "No visible power plans are available to cycle."
                : "No power plans are available to cycle.";
            return false;
        }

        var active = PowerManager.Api.GetActivePowerSchemeGuid();
        var index = PowerSchemeCycle.GetNextSchemeIndex(candidates, active);
        if (index < 0)
        {
            error = "Could not determine the next power plan.";
            return false;
        }

        activatedGuid = candidates[index];
        Activate(activatedGuid);
        error = null;
        return true;
    }
}
