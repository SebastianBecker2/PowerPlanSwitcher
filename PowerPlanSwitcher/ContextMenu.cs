namespace PowerPlanSwitcher;

using Autofac;
using Hotkeys;
using PowerManagement;
using Serilog;

internal class ContextMenu : ContextMenuStrip
{
    private readonly HotkeyManager hotkeyManager;
    private readonly ILifetimeScope scope;

    public ContextMenu(ILifetimeScope scope)
    {
        this.scope = scope;
        hotkeyManager = scope.Resolve<HotkeyManager>();

        BuildContextMenu();

        Opening += (s, e) => BuildContextMenu();
    }

    private void BuildContextMenu()
    {
        Items.Clear();
        AddPowerSchemes();
        _ = Items.Add(new ToolStripSeparator());
        AddSettingsButton();
        AddAboutButton();
        _ = Items.Add(new ToolStripSeparator());
        AddCloseButton();
    }

    private void AddPowerSchemes()
    {
        var activeSchemeGuid = PowerManager.Static.GetActivePowerSchemeGuid();
        foreach (var (guid, name) in PowerManager.Static.GetPowerSchemes())
        {
            var setting = PowerSchemeSettings.GetSetting(guid);
            if (setting is not null && !setting.Visible)
            {
                continue;
            }

            var button = new ToolStripMenuItem
            {
                Image = setting?.Icon,
                Text = (activeSchemeGuid == guid ? "(Active) " : string.Empty)
                    + (name ?? guid.ToString()),
            };

            button.Click += (_, _) =>
            {
                Log.Information(
                    "Activating power scheme: {PowerSchemeName} " +
                    "{PowerSchemeGuid} Reason: User selection",
                    name,
                    guid);
                PowerManager.Static.SetActivePowerScheme(guid);
            };

            _ = Items.Add(button);
        }
    }

    private void AddSettingsButton()
    {
        var button = new ToolStripMenuItem
        {
            Text = "Open settings..."
        };

        button.Click += (_, _) =>
        {
            hotkeyManager.RemoveAllHotkeys();
            try
            {
                using var dlg = scope.Resolve<SettingsDlg>();
                _ = dlg.ShowDialog();
            }
            finally
            {
                Program.RegisterHotkeys(hotkeyManager);
            }
        };

        _ = Items.Add(button);
    }

    private void AddAboutButton()
    {
        var button = new ToolStripMenuItem
        {
            Text = "About..."
        };

        button.Click += (_, _) =>
        {
            using var dlg = new AboutBox();
            _ = dlg.ShowDialog();
        };

        _ = Items.Add(button);
    }

    private void AddCloseButton()
    {
        var button = new ToolStripMenuItem
        {
            Text = "Close PowerPlanSwitcher"
        };

        button.Click += (_, _) => Application.Exit();

        _ = Items.Add(button);
    }
}
