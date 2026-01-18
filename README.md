# PowerPlanSwitcher

[![Build status](https://ci.appveyor.com/api/projects/status/k4umrwnp4grsp164/branch/main?svg=true)](https://ci.appveyor.com/project/SebastianBecker2/powerplanswitcher/branch/main)

Configure visibility, icon and global hotkey for your Windows power plans in the settings dialog.

<img width="707" height="353" alt="image" src="https://github.com/user-attachments/assets/33c0498f-782a-468c-9fd5-cecdb458a9a7" />

Switch your power plan easily and quickly by clicking the tray-icon.

<img width="383" height="229" alt="image" src="https://github.com/user-attachments/assets/393e5673-2456-4cfd-98d5-8e77233deebd" />

Or let PowerPlanSwitcher automatically switch between power plans depending on what you do.

## Using rules to automatically switch power plans

Create a list of rules that determine which power plan should be active. Rules will be triggered by various events but only the first rule in the list with a trigger of 1 or higher, determines the power plan.

<img width="707" height="353" alt="image" src="https://github.com/user-attachments/assets/f432d005-7de7-4b2b-bdb0-502b5bb9c41a" />

Create rules that switch Windows power plans based on:

- ProcessRule: Processes being created or terminated

<img width="671" height="241" alt="image" src="https://github.com/user-attachments/assets/04d67658-fa1d-4d1d-8fd5-f6d180db1308" />

- PowerLineRule: Plugging or unplugging your laptop from power

<img width="671" height="186" alt="image" src="https://github.com/user-attachments/assets/b4966c3f-d179-4c8f-bef0-b64f4a39f7e2" />

- IdleRule: No user input happening for a configurable amount of time

<img width="671" height="249" alt="image" src="https://github.com/user-attachments/assets/326a2f9e-d63b-414c-824f-201489c86a67" />

- StartupRule: PowerPlanSwitcher being started

<img width="671" height="153" alt="image" src="https://github.com/user-attachments/assets/479e1770-6074-4b43-addd-c7fa49eb63b5" />

- ShutdownRule: Windows initiating a shutdown

<img width="671" height="153" alt="image" src="https://github.com/user-attachments/assets/a08f3430-6901-42ea-ba80-62ffef428bb9" />

Furher settings allow you to:
- Configure a global hotkey to cycle through your power plans.
- Set your prefered color theme.
- Specify the display of notifications when a power plan is switched by a rule or a hotkey press.
- Activate extended logging.

<img width="707" height="351" alt="image" src="https://github.com/user-attachments/assets/884ee63a-1c6e-402c-9a1a-e112f8785050" />

PowerPlanSwitcher is using the following tools, resources and libraries:

- [.NET 8](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [Newtonsoft.Json](https://www.newtonsoft.com/json)
- The amazing [FatCow IconPack](https://www.fatcow.com/free-icons)
- [WindowsAPICodePack](https://github.com/contre/Windows-API-Code-Pack-1.1)
- [Vanara](https://github.com/dahall/Vanara)
- [Serilog](https://serilog.net/)
- [SkiaSharp](https://github.com/mono/SkiaSharp)
- [SevenZipSharp](https://github.com/squid-box/SevenZipSharp)
- [WiX Toolset 6](https://wixtoolset.org/)
- [HotkeyManager](https://github.com/SebastianBecker2/HotkeyManager)
- [DotNet.Glob](https://github.com/dazinator/DotNet.Glob)
- [AutoFac](https://autofac.org/)
