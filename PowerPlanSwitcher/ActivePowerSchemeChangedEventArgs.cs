namespace PowerPlanSwitcher
{
    public class ActivePowerSchemeChangedEventArgs : EventArgs
    {
        public Guid ActiveSchemeGuid { get; }

        public ActivePowerSchemeChangedEventArgs(Guid activeSchemeGuid) =>
            ActiveSchemeGuid = activeSchemeGuid;
    }
}
