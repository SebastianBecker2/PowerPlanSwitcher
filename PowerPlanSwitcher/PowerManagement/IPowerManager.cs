namespace PowerPlanSwitcher.PowerManagement
{
    using System;
    using System.Collections.Generic;

    public interface IPowerManager
    {
        event EventHandler<ActivePowerSchemeChangedEventArgs>? ActivePowerSchemeChanged;

        Guid GetActivePowerSchemeGuid();
        IEnumerable<Guid> GetPowerSchemeGuids();
        string? GetPowerSchemeName(Guid schemeGuid);
        IEnumerable<(Guid guid, string? name)> GetPowerSchemes();
        void SetActivePowerScheme(Guid schemeGuid);
    }
}
