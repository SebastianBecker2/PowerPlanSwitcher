namespace PowerPlanSwitcher;

using System.Runtime.InteropServices;

internal sealed class ThemedTabControl : TabControl
{
    private const int TcmAdjustRect = 0x1328;
    private const int WmPaint = 0x000F;
    private const uint RedrawInvalidate = 0x0001;
    private const uint RedrawAllChildren = 0x0080;

    private readonly HashSet<Control> watchedControls = [];
    private bool comboThemeRestoreQueued;

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ComboBoxInfo
    {
        public int cbSize;
        public Rect rcItem;
        public Rect rcButton;
        public int stateButton;
        public IntPtr hwndCombo;
        public IntPtr hwndEdit;
        public IntPtr hwndList;
    }

    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
    private static extern int SetWindowTheme(
        IntPtr hwnd,
        string? subAppName,
        string? subIdList);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref ComboBoxInfo info);

    [DllImport("user32.dll")]
    private static extern bool RedrawWindow(
        IntPtr hwnd,
        IntPtr updateRect,
        IntPtr updateRegion,
        uint flags);

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        ApplyPageColors();
        Watch(this);
        ScheduleComboThemeRestore();
    }

    protected override void OnControlAdded(ControlEventArgs e)
    {
        base.OnControlAdded(e);
        if (e.Control is TabPage page)
        {
            ApplyPageColor(page);
        }

        if (e.Control is not null)
        {
            Watch(e.Control);
        }
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == TcmAdjustRect && !DesignMode && Application.IsDarkModeEnabled)
        {
            base.WndProc(ref m);
            var rect = Marshal.PtrToStructure<Rect>(m.LParam);
            var inflateX = LogicalToDeviceUnits(4);
            var inflateY = LogicalToDeviceUnits(4);
            rect.Left -= inflateX;
            rect.Right += inflateX;
            rect.Top -= LogicalToDeviceUnits(1);
            rect.Bottom += inflateY;
            Marshal.StructureToPtr(rect, m.LParam, true);
            return;
        }

        base.WndProc(ref m);

        if (m.Msg == WmPaint && Application.IsDarkModeEnabled)
        {
            PaintOverPageBorder();
        }
    }

    private void PaintOverPageBorder()
    {
        var page = DisplayRectangle;
        var client = ClientRectangle;
        if (client.Width <= 0 || client.Height <= 0)
        {
            return;
        }

        using var g = Graphics.FromHwnd(Handle);
        using var brush = new SolidBrush(SystemColors.Control);
        var border = LogicalToDeviceUnits(4);

        g.FillRectangle(brush, client.Left, page.Top, border, client.Bottom - page.Top);
        g.FillRectangle(
            brush,
            client.Right - border,
            page.Top,
            border,
            client.Bottom - page.Top);
        g.FillRectangle(
            brush,
            client.Left,
            client.Bottom - border,
            client.Width,
            border);
        g.FillRectangle(brush, page.Left, page.Top, page.Width, LogicalToDeviceUnits(2));
    }

    private void ApplyPageColors()
    {
        foreach (TabPage page in TabPages)
        {
            ApplyPageColor(page);
        }
    }

    private static void ApplyPageColor(TabPage page)
    {
        if (!Application.IsDarkModeEnabled)
        {
            return;
        }

        page.UseVisualStyleBackColor = false;
        page.BackColor = SystemColors.Control;
    }

    private void Watch(Control control)
    {
        if (!watchedControls.Add(control))
        {
            return;
        }

        control.ControlAdded += WatchedControlAdded;
        control.HandleCreated += WatchedHandleCreated;
        foreach (Control child in control.Controls)
        {
            Watch(child);
        }
    }

    private void WatchedControlAdded(object? sender, ControlEventArgs e)
    {
        if (e.Control is not null)
        {
            Watch(e.Control);
        }
    }

    private void WatchedHandleCreated(object? sender, EventArgs e) =>
        ScheduleComboThemeRestore();

    private void ScheduleComboThemeRestore()
    {
        if (comboThemeRestoreQueued
            || IsDisposed
            || !IsHandleCreated
            || !Application.IsDarkModeEnabled)
        {
            return;
        }

        comboThemeRestoreQueued = true;
        BeginInvoke(RestoreQueuedComboThemes);
    }

    private void RestoreQueuedComboThemes()
    {
        comboThemeRestoreQueued = false;
        if (IsDisposed || !Application.IsDarkModeEnabled)
        {
            return;
        }

        // The tab control paints every descendant with DarkMode_Explorer.
        // That replaces the combo box theme and leaves the closed control white.
        RestoreComboThemes(this);
    }

    private static void RestoreComboThemes(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            if (child is ComboBox { IsHandleCreated: true } combo)
            {
                var comboThemed = SetWindowTheme(combo.Handle, "DarkMode_CFD", null) >= 0;
                var info = new ComboBoxInfo
                {
                    cbSize = Marshal.SizeOf<ComboBoxInfo>(),
                };
                var listThemed = false;
                if (GetComboBoxInfo(combo.Handle, ref info) && info.hwndList != IntPtr.Zero)
                {
                    listThemed = SetWindowTheme(info.hwndList, "DarkMode_Explorer", null) >= 0;
                }

                if (comboThemed || listThemed)
                {
                    _ = RedrawWindow(
                        combo.Handle,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        RedrawInvalidate | RedrawAllChildren);
                }
            }

            RestoreComboThemes(child);
        }
    }
}
