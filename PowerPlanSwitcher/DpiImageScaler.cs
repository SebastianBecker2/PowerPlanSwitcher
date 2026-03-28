namespace PowerPlanSwitcher;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

internal sealed class DpiImageScaler : IDisposable
{
    private const int DefaultDpi = 96;

    private readonly Control root;
    private readonly Dictionary<Control, Registration> registrations = [];
    private readonly HashSet<Control> observedControls = [];

    private bool disposed;

    public DpiImageScaler(Control root)
    {
        this.root = root;

        ObserveControlTree(root);
        RegisterControlTree(root);

        if (root is Form form)
        {
            form.DpiChanged += HandleDpiChanged;
        }

        root.HandleCreated += HandleRootHandleCreated;
        root.Disposed += HandleRootDisposed;

        if (root.IsHandleCreated)
        {
            ApplyCurrentScale();
        }
    }

    public void OverrideSource(ButtonBase button, Image? sourceImage, Size? logicalSize = null) =>
        OverrideSourceCore(button, sourceImage, logicalSize);

    public void OverrideSource(PictureBox pictureBox, Image? sourceImage, Size? logicalSize = null) =>
        OverrideSourceCore(pictureBox, sourceImage, logicalSize);

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;

        if (root is Form form)
        {
            form.DpiChanged -= HandleDpiChanged;
        }

        root.HandleCreated -= HandleRootHandleCreated;
        root.Disposed -= HandleRootDisposed;

        foreach (var control in observedControls)
        {
            control.ControlAdded -= HandleControlAdded;
            control.Disposed -= HandleObservedControlDisposed;
        }

        foreach (var registration in registrations.Values)
        {
            registration.Dispose();
        }

        registrations.Clear();
        observedControls.Clear();
    }

    private void ObserveControlTree(Control control)
    {
        if (!observedControls.Add(control))
        {
            return;
        }

        control.ControlAdded += HandleControlAdded;
        control.Disposed += HandleObservedControlDisposed;

        foreach (Control child in control.Controls)
        {
            ObserveControlTree(child);
        }
    }

    private void RegisterControlTree(Control control)
    {
        TryRegister(control);

        foreach (Control child in control.Controls)
        {
            RegisterControlTree(child);
        }
    }

    private void TryRegister(Control control)
    {
        switch (control)
        {
            case ButtonBase button when button.Image is not null:
                OverrideSource(button, button.Image, button.Image.Size);
                break;
            case PictureBox pictureBox when pictureBox.Image is not null:
                OverrideSource(pictureBox, pictureBox.Image, pictureBox.Image.Size);
                break;
        }
    }

    private void OverrideSourceCore(Control control, Image? sourceImage, Size? logicalSize)
    {
        RemoveRegistration(control);

        if (sourceImage is null)
        {
            return;
        }

        var registration = new Registration(control, sourceImage, logicalSize ?? sourceImage.Size);
        registrations[control] = registration;
        ApplyScale(registration, GetCurrentDpi());
    }

    private void RemoveRegistration(Control control)
    {
        if (!registrations.Remove(control, out var registration))
        {
            return;
        }

        registration.Dispose();
    }

    private void HandleControlAdded(object? sender, ControlEventArgs e)
    {
        if (e.Control is null)
        {
            return;
        }

        ObserveControlTree(e.Control);
        RegisterControlTree(e.Control);
        ApplyCurrentScale();
    }

    private void HandleObservedControlDisposed(object? sender, EventArgs e)
    {
        if (sender is not Control control)
        {
            return;
        }

        RemoveRegistration(control);
        control.ControlAdded -= HandleControlAdded;
        control.Disposed -= HandleObservedControlDisposed;
        _ = observedControls.Remove(control);
    }

    private void HandleDpiChanged(object? sender, EventArgs e)
    {
        if (disposed || root.IsDisposed)
        {
            return;
        }

        if (root.IsHandleCreated)
        {
            root.BeginInvoke(new MethodInvoker(ApplyCurrentScale));
            return;
        }

        ApplyCurrentScale();
    }

    private void HandleRootHandleCreated(object? sender, EventArgs e) => ApplyCurrentScale();

    private void HandleRootDisposed(object? sender, EventArgs e) => Dispose();

    private void ApplyCurrentScale()
    {
        if (disposed)
        {
            return;
        }

        var dpi = GetCurrentDpi();
        foreach (var registration in registrations.Values)
        {
            ApplyScale(registration, dpi);
        }
    }

    private static void ApplyScale(Registration registration, int dpi)
    {
        if (registration.Control.IsDisposed)
        {
            return;
        }

        var targetSize = ScaleSize(registration.LogicalSize, dpi);
        if (targetSize.Width <= 0 || targetSize.Height <= 0)
        {
            return;
        }

        var scaledImage = ScaleImage(registration.SourceImage, targetSize);
        registration.SetScaledImage(scaledImage);
    }

    private int GetCurrentDpi()
    {
        if (root.IsDisposed)
        {
            return DefaultDpi;
        }

        var form = root as Form ?? root.FindForm();
        if (form?.IsHandleCreated == true && form.DeviceDpi > 0)
        {
            return form.DeviceDpi;
        }

        return root.DeviceDpi > 0 ? root.DeviceDpi : DefaultDpi;
    }

    private static Size ScaleSize(Size logicalSize, int dpi)
    {
        var width = Math.Max(1, (int)Math.Round(logicalSize.Width * dpi / (double)DefaultDpi));
        var height = Math.Max(1, (int)Math.Round(logicalSize.Height * dpi / (double)DefaultDpi));
        return new Size(width, height);
    }

    private static Bitmap ScaleImage(Image sourceImage, Size targetSize)
    {
        var scaledImage = new Bitmap(targetSize.Width, targetSize.Height);
        scaledImage.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);

        using var graphics = Graphics.FromImage(scaledImage);
        graphics.CompositingMode = CompositingMode.SourceOver;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.Clear(Color.Transparent);
        graphics.DrawImage(sourceImage, new Rectangle(Point.Empty, targetSize));

        return scaledImage;
    }

    private sealed class Registration(Control control, Image sourceImage, Size logicalSize) : IDisposable
    {
        public Control Control { get; } = control;
        public Bitmap SourceImage { get; } = new(sourceImage);
        public Size LogicalSize { get; } = logicalSize;

        private Image? scaledImage;

        public void Dispose()
        {
            scaledImage?.Dispose();
            SourceImage.Dispose();
        }

        public void SetScaledImage(Image image)
        {
            var previousImage = scaledImage;
            scaledImage = image;

            switch (Control)
            {
                case ButtonBase button:
                    button.Image = image;
                    break;
                case PictureBox pictureBox:
                    pictureBox.Image = image;
                    break;
            }

            previousImage?.Dispose();
        }
    }
}