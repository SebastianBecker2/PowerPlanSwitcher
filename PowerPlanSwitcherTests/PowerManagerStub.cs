namespace PowerPlanSwitcherTests
{
    using System;
    using System.Collections.Generic;
    using PowerPlanSwitcher;
    using PowerPlanSwitcher.PowerManagement;

    internal class PowerManagerStub : IPowerManager
    {
        public static Guid CreatePowerSchemeGuid(int i) =>
            new($"00000000-0000-0000-0000-{i:000000000000}");

        private static readonly Dictionary<Guid, string> PowerSchemes = new() {
            { CreatePowerSchemeGuid(1_000), "PowerScheme 1000" },
            { CreatePowerSchemeGuid(1_001), "PowerScheme 1001" },
            { CreatePowerSchemeGuid(1_002), "PowerScheme 1002" },
            { CreatePowerSchemeGuid(1_003), "PowerScheme 1003" },
            { CreatePowerSchemeGuid(1_004), "PowerScheme 1004" },
            { CreatePowerSchemeGuid(1_005), "PowerScheme 1005" },
        };
        private Guid activePowerSchemeGuid = PowerSchemes.First().Key;

        public event EventHandler<ActivePowerSchemeChangedEventArgs>?
            ActivePowerSchemeChanged;
        protected virtual void OnActivePowerSchemeChanged(Guid powerSchemeGuid) =>
            ActivePowerSchemeChanged?.Invoke(
                this,
                new ActivePowerSchemeChangedEventArgs(powerSchemeGuid));

        public Guid GetActivePowerSchemeGuid() => activePowerSchemeGuid;
        public IEnumerable<Guid> GetPowerSchemeGuids() =>
            PowerSchemes.Select(kvp => kvp.Key);
        public string? GetPowerSchemeName(Guid schemeGuid) =>
            PowerSchemes[schemeGuid];
        public IEnumerable<(Guid guid, string? name)> GetPowerSchemes() =>
            PowerSchemes.Select(kvp => (kvp.Key, (string?)kvp.Value));
        public void SetActivePowerScheme(Guid schemeGuid)
        {
            activePowerSchemeGuid = schemeGuid;
            OnActivePowerSchemeChanged(schemeGuid);
        }
    }
}
