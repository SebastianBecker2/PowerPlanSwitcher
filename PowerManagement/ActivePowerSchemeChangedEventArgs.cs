namespace PowerManagement;

public class ActivePowerSchemeChangedEventArgs(Guid activeSchemeGuid)
    : EventArgs
{
    public Guid ActiveSchemeGuid { get; } = activeSchemeGuid;
}
