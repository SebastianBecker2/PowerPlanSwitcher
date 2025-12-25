namespace RuleManagement.Rules;

using System;
using RuleManagement.Dto;
using SystemManagement;

public class ShutdownRule :
    Rule<ShutdownRuleDto>,
    IRule<ShutdownRuleDto>
{
    public Guid SchemeGuid => Dto.SchemeGuid;

    public ShutdownRule(
        IWindowMessageMonitor windowMessageMonitor,
        ShutdownRuleDto shutdownRuleDto)
        : base(shutdownRuleDto) =>
        windowMessageMonitor.WindowMessageReceived +=
        WindowMessageMonitor_WindowMessageReceived;

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
}
