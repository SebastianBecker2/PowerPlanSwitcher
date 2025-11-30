namespace PowerPlanSwitcher.PowerManagement
{
    using System;
    using System.Collections.Generic;

    public interface IPowerManager
    {
        public event EventHandler<ActivePowerSchemeChangedEventArgs>? ActivePowerSchemeChanged;

        public Guid GetActivePowerSchemeGuid();
        public IEnumerable<Guid> GetPowerSchemeGuids();
        public string? GetPowerSchemeName(Guid schemeGuid);
        public IEnumerable<(Guid guid, string? name)> GetPowerSchemes();
        public void SetActivePowerScheme(Guid schemeGuid);
    }
}
