namespace RuleManagement.Rules;

using System;
using RuleManagement.Dto;
using SystemManagement;

public sealed class ShutdownRule(
    IWindowMessageMonitor windowMessageMonitor,
    ShutdownRuleDto shutdownRuleDto) :
    Rule<ShutdownRuleDto>(shutdownRuleDto),
    IRule<ShutdownRuleDto>,
    IDisposable
{
    public Guid SchemeGuid => Dto.SchemeGuid;

    public override void StartRuling() =>
        windowMessageMonitor.WindowMessageReceived +=
            WindowMessageMonitor_WindowMessageReceived;

    public override void StopRuling() =>
        windowMessageMonitor.WindowMessageReceived -=
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

    public void Dispose() => StopRuling();
}
