namespace PowerPlanSwitcher
{
    internal class ContextMenu : ContextMenuStrip
    {
        public ContextMenu() : base()
        {
            BuildContextMenu();

            Opening += (s, e) => BuildContextMenu();
        }

        private void BuildContextMenu()
        {
            Items.Clear();
            AddPowerSchemes();
            _ = Items.Add(new ToolStripSeparator());
            AddSettingsButton();
            _ = Items.Add(new ToolStripSeparator());
            AddCloseButton();
        }

        private void AddPowerSchemes()
        {
            var activeSchemeGuid = PowerManager.GetActivePowerSchemeGuid();
            foreach (var (guid, name) in PowerManager.GetPowerSchemes())
            {
                var setting = PowerSchemeSettings.GetSetting(guid);
                if (setting is not null && !setting.Visible)
                {
                    continue;
                }

                var schemeButton = new ToolStripMenuItem
                {
                    Image = setting?.Icon,
                    Text = (activeSchemeGuid == guid ? "(Active) " : "")
                        + (name ?? guid.ToString()),
                };

                schemeButton.Click += (s, e) =>
                    PowerManager.SetActivePowerScheme(guid);

                _ = Items.Add(schemeButton);
            }
        }

        private void AddSettingsButton()
        {
            var settingsButton = new ToolStripMenuItem
            {
                Text = "Open settings..."
            };

            settingsButton.Click += (s, e) =>
            {
                using var dlg = new SettingsDlg();
                _ = dlg.ShowDialog();
            };

            _ = Items.Add(settingsButton);
        }

        private void AddCloseButton()
        {
            var closeButton = new ToolStripMenuItem
            {
                Text = "Close PowerPlanSwitcher"
            };

            closeButton.Click += (s, e) => Application.Exit();

            _ = Items.Add(closeButton);
        }
    }
}
