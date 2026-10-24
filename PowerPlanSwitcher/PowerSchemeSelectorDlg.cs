namespace PowerPlanSwitcher;

using PowerManagement;
using Properties;
using Serilog;

public partial class PowerSchemeSelectorDlg : Form
{
    private static Color ButtonBackgroundColor =>
        ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
        ? Color.WhiteSmoke
        : Color.FromArgb(0x15, 0x15, 0x15);
    private static Color SelectedButtonBackgroundColor =>
        ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
        ? Color.FromArgb(0xEb, 0xEb, 0xEb)
        : Color.FromArgb(0x20, 0x20, 0x20);
    private static Color FAMOBColor =>
        ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
        ? Color.White
        : Color.Black;
    private static Color TlpPowerSchemesBackColor =>
        ColorThemeHelper.GetActiveColorTheme() == ColorTheme.Light
        ? Color.Silver
        : Color.DimGray;

    private Font? menuFont;
    private Color? customTextColor;
    private bool shownTriggered;
    private bool previewMode;
    private int previewWidth;
    private int previewItemHeight;

    internal void ApplyPreview(
        Font font,
        Color? textColor,
        int width,
        int itemHeight)
    {
        previewMode = true;
        previewWidth = SelectorMenuAppearance.ClampWidth(width);
        previewItemHeight = SelectorMenuAppearance.ClampItemHeight(itemHeight);

        var colorChanged = !SameColor(customTextColor, textColor);
        customTextColor = textColor;

        var fontChanged = !SameFont(menuFont, font);
        if (fontChanged)
        {
            var next = new Font(font, font.Style);
            var previous = menuFont;
            menuFont = next;
            if (IsHandleCreated)
            {
                RebuildButtons();
            }

            previous?.Dispose();
        }
        else if (colorChanged && IsHandleCreated)
        {
            ApplyTextColor();
        }

        if (!IsHandleCreated)
        {
            return;
        }

        UpdateLayout();
        if (Visible)
        {
            BringToFront();
        }
    }

    private Color MenuTextColor =>
        customTextColor ?? SelectorMenuAppearance.ThemeTextColor();

    private static bool SameColor(Color? current, Color? next)
    {
        if (current is null || next is null)
        {
            return current is null && next is null;
        }

        return current.Value.ToArgb() == next.Value.ToArgb();
    }

    private void ApplyTextColor()
    {
        var color = MenuTextColor;
        foreach (Control control in TlpPowerSchemes.Controls)
        {
            control.ForeColor = color;
        }
    }

    private static bool SameFont(Font? current, Font next) =>
        current is not null
        && current.FontFamily.Name == next.FontFamily.Name
        && current.Style == next.Style
        && Math.Abs(current.SizeInPoints - next.SizeInPoints) < 0.1f;

    private static string FormatPlanText(string? name, Guid guid, bool active)
    {
        name ??= guid.ToString();
        return active ? Strings.Menu_ActivePrefix + name : " " + name;
    }

    private static SelectorButton CreateMenuButton(
        string text,
        Image? icon,
        bool active,
        Font font,
        Color textColor)
    {
        var button = new SelectorButton
        {
            FlatStyle = FlatStyle.Flat,
            Image = icon,
            ImageAlign = ContentAlignment.MiddleLeft,
            TextImageRelation = TextImageRelation.ImageBeforeText,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(20, 0, 0, 0),
            ForeColor = textColor,
            BackColor = active
                ? SelectedButtonBackgroundColor
                : ButtonBackgroundColor,
            Margin = Padding.Empty,
            Text = text,
            Font = font,
            Dock = DockStyle.Fill,
        };
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = FAMOBColor;
        return button;
    }

    public PowerSchemeSelectorDlg()
    {
        InitializeComponent();
        Text = Strings.PowerSchemeSelectorDlg_Title;
        customTextColor = SelectorMenuAppearance.ParseTextColor(
            Settings.Default.SelectorMenuFontColor);
        _ = new DpiImageScaler(this);
    }

    protected override bool ShowWithoutActivation => previewMode;

    protected override CreateParams CreateParams
    {
        get
        {
            SetStyle(ControlStyles.ApplyThemingImplicitly, false);
            return base.CreateParams;
        }
    }

    private sealed class SelectorLayoutPanel : TableLayoutPanel
    {
        protected override CreateParams CreateParams
        {
            get
            {
                SetStyle(ControlStyles.ApplyThemingImplicitly, false);
                return base.CreateParams;
            }
        }
    }

    private sealed class SelectorButton : Button
    {
        private bool hot;

        public SelectorButton()
        {
            SetStyle(
                ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw,
                true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            TabStop = false;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                SetStyle(ControlStyles.ApplyThemingImplicitly, false);
                return base.CreateParams;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            hot = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hot = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var graphics = pevent.Graphics;
            var back = hot ? FlatAppearance.MouseOverBackColor : BackColor;
            using (var brush = new SolidBrush(back))
            {
                graphics.FillRectangle(brush, ClientRectangle);
            }

            var x = Padding.Left;
            if (Image is not null)
            {
                var imageY = Math.Max(0, (Height - Image.Height) / 2);
                graphics.DrawImage(Image, x, imageY, Image.Width, Image.Height);
                x += Image.Width + LogicalToDeviceUnits(8);
            }

            var textRect = new Rectangle(
                x,
                0,
                Math.Max(0, Width - x - LogicalToDeviceUnits(8)),
                Height);
            TextRenderer.DrawText(
                graphics,
                Text,
                Font,
                textRect,
                ForeColor,
                TextFormatFlags.Left
                | TextFormatFlags.VerticalCenter
                | TextFormatFlags.EndEllipsis
                | TextFormatFlags.NoPadding);
        }
    }

    private SelectorButton CreateButton(
        Guid guid,
        string? name,
        Image? icon,
        bool active)
    {
        name ??= guid.ToString();
        Font font = menuFont ?? SelectorMenuAppearance.CreateFont(
            Settings.Default.SelectorMenuFont,
            Font);
        menuFont = font;
        var button = CreateMenuButton(
            FormatPlanText(name, guid, active),
            icon,
            active,
            font,
            MenuTextColor);
        button.Tag = guid;

        if (!previewMode)
        {
            button.Click += (_, _) =>
            {
                Log.ForContext("EventType", "PowerScheme.ActivationRequested")
                    .Information(
                    "Activating power scheme: {PowerSchemeName} " +
                    "{PowerSchemeGuid} Reason: User selection",
                    name,
                    guid);
                _ = PowerManager.Api.SetActivePowerSchemeAsync((Guid)button.Tag);
                Close();
            };
        }

        return button;
    }

    protected override void OnLoad(EventArgs e)
    {
        RebuildButtons();
        UpdateLayout();

        base.OnLoad(e);
    }

    private void RebuildButtons()
    {
        TlpPowerSchemes.SuspendLayout();
        while (TlpPowerSchemes.Controls.Count > 0)
        {
            TlpPowerSchemes.Controls[0].Dispose();
        }

        TlpPowerSchemes.RowStyles.Clear();
        TlpPowerSchemes.RowCount = 0;
        TlpPowerSchemes.BackColor = TlpPowerSchemesBackColor;

        var activeSchemeGuid = PowerManager.Api.GetActivePowerSchemeGuid();
        var row = 0;
        foreach (var entry in PowerSchemeEntryCache.GetEntries())
        {
            if (!entry.Visible)
            {
                continue;
            }

            TlpPowerSchemes.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TlpPowerSchemes.Controls.Add(
                CreateButton(
                    entry.Guid,
                    entry.Name,
                    entry.Icon,
                    activeSchemeGuid == entry.Guid),
                0,
                row);
            row++;
        }

        TlpPowerSchemes.RowCount = row;
        TlpPowerSchemes.ResumeLayout();
    }

    protected override void OnDpiChangedAfterParent(EventArgs e)
    {
        base.OnDpiChangedAfterParent(e);
        UpdateLayout();
    }

    private void UpdateLayout()
    {
        var itemHeight = previewMode
            ? previewItemHeight
            : SelectorMenuAppearance.ClampItemHeight(
                Settings.Default.SelectorMenuItemHeight);
        var width = previewMode
            ? previewWidth
            : SelectorMenuAppearance.ClampWidth(Settings.Default.SelectorMenuWidth);
        var content = new Size(
            LogicalToDeviceUnits(width),
            LogicalToDeviceUnits(TlpPowerSchemes.Controls.Count * itemHeight));
        var available = Screen.FromRectangle(Taskbar.CurrentBounds).WorkingArea.Size;
        var window = SelectorMenuAppearance.FitMenuWindow(
            content,
            available,
            SystemInformation.VerticalScrollBarWidth,
            SystemInformation.HorizontalScrollBarHeight);

        TlpPowerSchemes.Dock = DockStyle.None;
        TlpPowerSchemes.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        TlpPowerSchemes.Location = Point.Empty;
        TlpPowerSchemes.MinimumSize = content;
        TlpPowerSchemes.MaximumSize = content;
        TlpPowerSchemes.Size = content;
        AutoScroll = window.Width != content.Width || window.Height != content.Height;
        Size = window;
        Location = GetPositionOnTaskbar(Size);
    }

    private void DisposeMenuFont()
    {
        menuFont?.Dispose();
        menuFont = null;
    }

    protected override void OnShown(EventArgs e)
    {
        if (previewMode)
        {
            BringToFront();
            base.OnShown(e);
            return;
        }

        // Brute force the dialog to frontmostestest topmostest
        WindowState = FormWindowState.Minimized;
        Show();
        WindowState = FormWindowState.Normal;
        BringToFront();
        Activate();
        _ = Focus();

        shownTriggered = true;

        base.OnShown(e);
    }

    protected override void OnDeactivate(EventArgs e)
    {
        // For some reason, with version .Net 8.0, the OnDeactivate
        // event is called when the dialog is shown for the first time.
        // It triggeres OnActivate, OnDeactivate and then OnActivate again.
        // Couldn't figure out why. Didn't seem to happen in .Net 5.0.
        // So we check if the Shown event was triggered before. Because
        // it cames last when showing the dialog.
        if (!previewMode && shownTriggered)
        {
            DialogResult = DialogResult.Cancel;
        }
        base.OnDeactivate(e);
    }

    private static Point GetPositionOnTaskbar(Size windowSize)
    {
        var bounds = Taskbar.CurrentBounds;
        switch (Taskbar.Position)
        {
            case TaskbarPosition.Left:
                bounds.Location += bounds.Size;
                return new Point(bounds.X, bounds.Y - windowSize.Height);

            case TaskbarPosition.Top:
                bounds.Location += bounds.Size;
                return new Point(bounds.X - windowSize.Width, bounds.Y);

            case TaskbarPosition.Right:
                bounds.Location -= windowSize;
                return new Point(bounds.X, bounds.Y + bounds.Height);

            case TaskbarPosition.Bottom:
                bounds.Location -= windowSize;
                return new Point(bounds.X + bounds.Width, bounds.Y);

            case TaskbarPosition.Unknown:
            default:
                return new Point(0, 0);
        }
    }
}

