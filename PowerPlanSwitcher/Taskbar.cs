namespace PowerPlanSwitcher;

using System;
using System.Runtime.InteropServices;

public enum TaskbarPosition
{
    Unknown = -1,
    Left,
    Top,
    Right,
    Bottom,
}

public static class Taskbar
{
    [Flags]
    private enum ABS
    {
        AutoHide = 0x01,
        AlwaysOnTop = 0x02,
    }

    ////private enum ABE : uint
    private enum AppBarEdge : uint
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3
    }

    ////private enum ABM : uint
    private enum AppBarMessage : uint
    {
        New = 0x00000000,
        Remove = 0x00000001,
        QueryPos = 0x00000002,
        SetPos = 0x00000003,
        GetState = 0x00000004,
        GetTaskbarPos = 0x00000005,
        Activate = 0x00000006,
        GetAutoHideBar = 0x00000007,
        SetAutoHideBar = 0x00000008,
        WindowPosChanged = 0x00000009,
        SetState = 0x0000000A,
    }

    private const string ClassName = "Shell_TrayWnd";
    private static AppBarData appBarData;

    /// <summary>Static initializer of the <see cref="Taskbar" /> class.</summary>
    static Taskbar() => appBarData = new AppBarData
    {
        Size = (uint)Marshal.SizeOf(typeof(AppBarData)),
        WindowHandle = FindWindow(ClassName, null)
    };

    /// <summary>
    ///   Gets a value indicating whether the taskbar is always on top of other windows.
    /// </summary>
    /// <value><c>true</c> if the taskbar is always on top of other windows; otherwise, <c>false</c>.</value>
    /// <remarks>This property always returns <c>false</c> on Windows 7 and newer.</remarks>
    public static bool AlwaysOnTop
    {
        get
        {
            var state = SHAppBarMessage(AppBarMessage.GetState, ref appBarData).ToInt32();
            return ((ABS)state).HasFlag(ABS.AlwaysOnTop);
        }
    }

    /// <summary>
    ///   Gets a value indicating whether the taskbar is automatically hidden when inactive.
    /// </summary>
    /// <value><c>true</c> if the taskbar is set to auto-hide is enabled; otherwise, <c>false</c>.</value>
    public static bool AutoHide
    {
        get
        {
            var state = SHAppBarMessage(AppBarMessage.GetState, ref appBarData).ToInt32();
            return ((ABS)state).HasFlag(ABS.AutoHide);
        }
    }

    /// <summary>Gets the current display bounds of the taskbar.</summary>
    public static System.Drawing.Rectangle CurrentBounds
    {
        get
        {
            var rect = new Rectangle();
            if (GetWindowRect(Handle, ref rect))
            {
                return System.Drawing.Rectangle.FromLTRB(
                    rect.Left,
                    rect.Top,
                    rect.Right,
                    rect.Bottom);
            }

            return System.Drawing.Rectangle.Empty;
        }
    }

    /// <summary>Gets the display bounds when the taskbar is fully visible.</summary>
    public static System.Drawing.Rectangle DisplayBounds
    {
        get
        {
            if (RefreshBoundsAndPosition())
            {
                return System.Drawing.Rectangle.FromLTRB(
                    appBarData.Rectangle.Left,
                    appBarData.Rectangle.Top,
                    appBarData.Rectangle.Right,
                    appBarData.Rectangle.Bottom);
            }

            return CurrentBounds;
        }
    }

    /// <summary>Gets the taskbar's window handle.</summary>
    public static IntPtr Handle => appBarData.WindowHandle;

    /// <summary>Gets the taskbar's position on the screen.</summary>
    public static TaskbarPosition Position
    {
        get
        {
            if (RefreshBoundsAndPosition())
            {
                return (TaskbarPosition)appBarData.Edge;
            }

            return TaskbarPosition.Unknown;
        }
    }

    /// <summary>Hides the taskbar.</summary>
    public static void Hide()
    {
        const int sW_HIDE = 0;
        _ = ShowWindow(Handle, sW_HIDE);
    }

    /// <summary>Shows the taskbar.</summary>
    public static void Show()
    {
        const int sW_SHOW = 1;
        _ = ShowWindow(Handle, sW_SHOW);
    }

    private static bool RefreshBoundsAndPosition() =>
        //! SHAppBarMessage returns IntPtr.Zero **if it fails**
        SHAppBarMessage(AppBarMessage.GetTaskbarPos, ref appBarData) != IntPtr.Zero;

    #region DllImports

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindow(string lpClassName, string? lpWindowName);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

    [DllImport("shell32.dll", SetLastError = true)]
    private static extern IntPtr SHAppBarMessage(AppBarMessage dwMessage, [In] ref AppBarData pData);

    [DllImport("user32.dll")]
    private static extern int ShowWindow(IntPtr hwnd, int command);

    #endregion DllImports

    [StructLayout(LayoutKind.Sequential)]
    private struct AppBarData
    {
        public uint Size { get; set; }
        public IntPtr WindowHandle { get; set; }
        public uint CallbackMessage { get; set; }
        public AppBarEdge Edge { get; set; }
        public Rectangle Rectangle { get; set; }
        public IntPtr State { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Rectangle
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
    }
}
