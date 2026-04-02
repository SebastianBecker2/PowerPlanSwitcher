namespace PowerPlanSwitcher;

using Properties;

internal static class PowerSchemeEntryCache
{
    internal sealed record Entry(
        Guid Guid,
        string? Name,
        Image? Icon,
        bool Visible);

    private static readonly object CacheLock = new();
    private static IReadOnlyList<Entry>? cachedEntries;

    static PowerSchemeEntryCache() =>
        Settings.Default.PropertyChanged += (_, e) =>
        {
            if (string.Equals(
                e.PropertyName,
                nameof(Settings.Default.PowerSchemeSettings),
                StringComparison.Ordinal))
            {
                Invalidate();
            }
        };

    public static IReadOnlyList<Entry> GetEntries()
    {
        if (cachedEntries is not null)
        {
            return cachedEntries;
        }

        lock (CacheLock)
        {
            if (cachedEntries is not null)
            {
                return cachedEntries;
            }

            cachedEntries =
            [
                .. PowerManagement.PowerManager.Api.GetPowerSchemes().Select(static powerScheme =>
                {
                    var setting = PowerSchemeSettings.GetSetting(powerScheme.guid);

                    return new Entry(
                        powerScheme.guid,
                        powerScheme.name,
                        setting?.Icon,
                        setting?.Visible ?? true);
                })
            ];

            return cachedEntries;
        }
    }

    public static void Invalidate()
    {
        lock (CacheLock)
        {
            cachedEntries = null;
        }
    }
}

