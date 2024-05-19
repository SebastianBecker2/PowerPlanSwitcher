namespace PowerPlanSwitcher
{
    using System;
    using System.Windows.Forms;
    using Hotkeys;

    public partial class HotkeySelectionDlg : Form
    {
        public Keys Key { get; set; } = Keys.None;
        public ModifierKeys Modifier { get; set; } = Hotkeys.ModifierKeys.None;

        public HotkeySelectionDlg() => InitializeComponent();

        protected override void OnLoad(EventArgs e)
        {
            Program.HotkeyManager.KeyPressed += HotkeyManager_KeyPressed;
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            Program.HotkeyManager.KeyPressed -= HotkeyManager_KeyPressed;
            base.OnClosed(e);
        }

        private void HotkeyManager_KeyPressed(
            object? sender,
            KeyPressedEventArgs e)
        {
            Key = e.PressedKey;
            Modifier = e.ModifierKeys;

            Invoke(new Action(() =>
                LblHotkeyPreview.Text = HotkeyManager.ToString(Key, Modifier)));
        }
    }
}
