namespace SystemManagement;

public sealed class WindowMessageEventArgs(
    WindowMessage message,
    IntPtr wParam,
    IntPtr lParam)
    : EventArgs
{
    public WindowMessage Message { get; } = message;
    public IntPtr WParam { get; } = wParam;
    public IntPtr LParam { get; } = lParam;
}
