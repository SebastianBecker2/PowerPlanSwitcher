namespace PowerPlanSwitcher
{
    using PowerPlanSwitcher.PowerManagement;

    internal class ContextMenu : ContextMenuStrip
    {

        public event EventHandler<SettingsChangedEventArgs>? SettingsChanged;
        protected virtual void OnSettingsChanged() =>
            SettingsChanged?.Invoke(this, new SettingsChangedEventArgs());

        public ContextMenu()
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
            AddAboutButton();
            _ = Items.Add(new ToolStripSeparator());
            AddCloseButton();

            //#if DEBUG
            //            using var dlg = new SettingsDlg();
            //            if (dlg.ShowDialog() != DialogResult.OK)
            //            {
            //                return;
            //            }
            //            OnSettingsChanged();
            //#endif
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
                    PowerManager.Static.SetActivePowerScheme(guid);

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
                Program.HotkeyManager.RemoveAllHotkeys();
                try
                {
                    using var dlg = new SettingsDlg();
                    if (dlg.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    OnSettingsChanged();
                }
                finally
                {
                    Program.RegisterHotkeys();
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
}
