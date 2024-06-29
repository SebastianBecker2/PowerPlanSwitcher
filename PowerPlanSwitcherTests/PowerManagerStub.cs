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
            { CreatePowerSchemeGuid(100), "PowerScheme 100" },
            { CreatePowerSchemeGuid(101), "PowerScheme 101" },
            { CreatePowerSchemeGuid(102), "PowerScheme 102" },
            { CreatePowerSchemeGuid(103), "PowerScheme 103" },
            { CreatePowerSchemeGuid(104), "PowerScheme 104" },
            { CreatePowerSchemeGuid(105), "PowerScheme 105" },
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
