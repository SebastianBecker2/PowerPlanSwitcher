namespace RuleManagement.Rules;

using System;
using RuleManagement.Dto;
using SystemManagement;

public class ShutdownRule :
    Rule<ShutdownRuleDto>,
    IRule<ShutdownRuleDto>,
    IDisposable
{
    public Guid SchemeGuid => Dto.SchemeGuid;

    private readonly IWindowMessageMonitor windowMessageMonitor;

    public ShutdownRule(
        IWindowMessageMonitor windowMessageMonitor,
        ShutdownRuleDto shutdownRuleDto)
        : base(shutdownRuleDto)
    {
        this.windowMessageMonitor = windowMessageMonitor;

        windowMessageMonitor.WindowMessageReceived +=
            WindowMessageMonitor_WindowMessageReceived;
    }

    private void WindowMessageMonitor_WindowMessageReceived(
        object? sender,
        WindowMessageEventArgs e)
    {
        if (e.Message == WindowMessage.QueryEndSession)
        {
            TriggerCount = 1;
        }
        // Shutdown was cancelled
        else if (e.Message == WindowMessage.EndSession
            && e.WParam == 0)
        {
            TriggerCount = 0;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Suppression of CA1816 is necessary")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "ShutdownRule does not have a finalizer")]
    public void Dispose() =>
        windowMessageMonitor.WindowMessageReceived -=
            WindowMessageMonitor_WindowMessageReceived;
}
