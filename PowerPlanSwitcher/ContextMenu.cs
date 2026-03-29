namespace PowerPlanSwitcher;

using Hotkeys;
using PowerManagement;
using PowerPlanSwitcher.Properties;
using Serilog;

internal class ContextMenu : ContextMenuStrip
{
    private HotkeyManager HotkeyManager { get; init; }
    private Func<SettingsDlg> SettingsDlgFactory { get; init; }
    private IPowerManager PowerManager { get; init; }
    private Guid ActiveSchemeGuid { get; set; }
    private bool IsMenuDirty { get; set; }
    private readonly List<(Guid guid, ToolStripMenuItem menuItem, string label)> schemeButtons = [];

    public ContextMenu(
        HotkeyManager hotkeyManager,
        IPowerManager powerManager,
        Func<SettingsDlg> settingsDlgFactory)
    {
        HotkeyManager = hotkeyManager;
        PowerManager = powerManager;
        SettingsDlgFactory = settingsDlgFactory;
        ActiveSchemeGuid = powerManager.GetActivePowerSchemeGuid();
        IsMenuDirty = false;

        powerManager.ActivePowerSchemeChanged += (_, e) =>
            ActiveSchemeGuid = e.ActiveSchemeGuid;
        Settings.Default.PropertyChanged += (_, e) =>
        {
            if (string.Equals(
                e.PropertyName,
                nameof(Settings.Default.PowerSchemeSettings),
                StringComparison.Ordinal))
            {
                IsMenuDirty = true;
            }
        };

        BuildContextMenu();

        Opening += (s, e) =>
        {
            if (IsMenuDirty)
            {
                BuildContextMenu();
            }
            else
            {
                RefreshPowerSchemeButtons();
            }
        };
    }

    private void BuildContextMenu()
    {
        var activeSchemeGuid = ActiveSchemeGuid;

        Items.Clear();
        schemeButtons.Clear();

        AddPowerSchemes(activeSchemeGuid);
        _ = Items.Add(new ToolStripSeparator());
        AddSettingsButton();
        AddAboutButton();
        _ = Items.Add(new ToolStripSeparator());
        AddCloseButton();

        IsMenuDirty = false;
    }

    private void RefreshPowerSchemeButtons()
    {
        var activeSchemeGuid = ActiveSchemeGuid;
        foreach (var (guid, menuItem, label) in schemeButtons)
        {
            menuItem.Text = (guid == activeSchemeGuid ? "(Active) " : string.Empty) + label;
        }
    }

    private void AddPowerSchemes(Guid activeSchemeGuid)
    {
        foreach (var scheme in PowerSchemeEntryCache.GetEntries())
        {
            if (!scheme.Visible)
            {
                continue;
            }

            var button = new ToolStripMenuItem
            {
                Image = scheme.Icon,
                Text = (activeSchemeGuid == scheme.Guid ? "(Active) " : string.Empty)
                    + (scheme.Name ?? scheme.Guid.ToString()),
            };

            var label = scheme.Name ?? scheme.Guid.ToString();
            schemeButtons.Add((scheme.Guid, button, label));

            button.Click += (_, _) =>
            {
                Log.ForContext("EventType", "PowerScheme.ActivationRequested")
                    .Information(
                    "Activating power scheme: {PowerSchemeName} " +
                    "{PowerSchemeGuid} Reason: User selection",
                    scheme.Name,
                    scheme.Guid);
                _ = PowerManager.SetActivePowerSchemeAsync(scheme.Guid);
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
            HotkeyManager.RemoveAllHotkeys();
            try
            {
                using var dlg = SettingsDlgFactory();
                _ = dlg.ShowDialog();
            }
            finally
            {
                Program.RegisterHotkeys(HotkeyManager);
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
