namespace PowerPlanSwitcher;

using System.Runtime.InteropServices;

internal sealed class ThemedTabControl : TabControl
{
    private const int TcmAdjustRect = 0x1328;
    private const int WmPaint = 0x000F;

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        ApplyPageColors();
    }

    protected override void OnControlAdded(ControlEventArgs e)
    {
        base.OnControlAdded(e);
        if (e.Control is TabPage page)
        {
            ApplyPageColor(page);
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
}
