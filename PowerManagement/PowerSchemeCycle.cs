namespace PowerManagement;

public static class PowerSchemeCycle
{
    public static int GetNextSchemeIndex(
        IReadOnlyList<Guid> schemes,
        Guid activeSchemeGuid)
    {
        if (schemes.Count == 0)
        {
            return -1;
        }

        var index = -1;
        if (activeSchemeGuid != Guid.Empty)
        {
            for (var i = 0; i < schemes.Count; i++)
            {
                if (schemes[i] == activeSchemeGuid)
                {
                    index = i;
                    break;
                }
            }
        }
        else
        {
            index = 0;
        }

        // Index -1 when the active scheme is missing from the list;
        // (-1 + 1) % n == 0 selects the first scheme.
        return (index + 1) % schemes.Count;
    }
}
