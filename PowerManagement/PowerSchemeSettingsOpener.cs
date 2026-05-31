namespace PowerManagement;

using System.Diagnostics;
using Serilog;

public static class PowerSchemeSettingsOpener
{
    public static bool IsKnownPowerScheme(Guid schemeGuid)
    {
        if (schemeGuid == Guid.Empty)
        {
            return false;
        }

        return !string.IsNullOrWhiteSpace(PowerManager.Api.GetPowerSchemeName(schemeGuid));
    }

    public static bool TryOpenPowerOptions()
    {
        if (TryStartProcess("control.exe", "powercfg.cpl,,0"))
        {
            return true;
        }

        if (TryStartProcess("ms-settings:powersleep", null))
        {
            return true;
        }

        return TryStartProcess("powercfg.cpl", null);
    }

    private static bool TryStartProcess(string fileName, string? arguments)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                UseShellExecute = true,
            };

            if (!string.IsNullOrEmpty(arguments))
            {
                startInfo.Arguments = arguments;
            }

            _ = Process.Start(startInfo);
            return true;
        }
        catch (Exception ex)
        {
            Log.Warning(
                ex,
                "Failed to open power options via {FileName} {Arguments}",
                fileName,
                arguments ?? string.Empty);
            return false;
        }
    }
}
