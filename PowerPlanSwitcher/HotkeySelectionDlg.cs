namespace PowerPlanSwitcher;

using System;
using System.Windows.Forms;
using Hotkeys;

public partial class HotkeySelectionDlg : Form
{
    public Hotkey? Hotkey { get; set; }
    private readonly HotkeyManager hotkeyManager;

    public HotkeySelectionDlg(HotkeyManager hotkeyManager)
    {
        this.hotkeyManager = hotkeyManager;
        InitializeComponent();
        ApplyLocalization();
    }

    private void ApplyLocalization()
    {
        Text = Strings.HotkeySelectionDlg_Title;
        label1.Text = Strings.HotkeySelectionDlg_LblSelectedHotkey;
        BtnOk.Text = Strings.HotkeySelectionDlg_BtnOk;
        BtnCancel.Text = Strings.HotkeySelectionDlg_BtnCancel;
    }

    protected override void OnLoad(EventArgs e)
    {
        hotkeyManager.KeyPressed += HotkeyManager_KeyPressed;
        base.OnLoad(e);
    }

    protected override void OnClosed(EventArgs e)
    {
        hotkeyManager.KeyPressed -= HotkeyManager_KeyPressed;
        base.OnClosed(e);
    }

    private void HotkeyManager_KeyPressed(
        object? sender,
        KeyPressedEventArgs e)
    {
        Hotkey = new Hotkey
        {
            Key = e.PressedKey,
            Modifier = e.ModifierKeys,
        };

        Invoke(new Action(() => LblHotkeyPreview.Text = Hotkey.ToString()));
    }
}
