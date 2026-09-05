namespace PowerPlanSwitcher;

using Microsoft.Win32;
using PowerPlanSwitcher.Properties;

internal enum ColorTheme
{
    System,
    Light,
    Dark,
}

internal static class ColorThemeHelper
{
    private static readonly string WindowsColorThemeKey =
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";

    private static readonly List<(string name, ColorTheme theme)> ColorThemes =
    [
        ( "Use System Setting", ColorTheme.System ),
        ( "Light Mode", ColorTheme.Light ),
        ( "Dark Mode", ColorTheme.Dark ),
    ];

    private static SynchronizationContext? uiContext;
    private static int systemThemeApplyGeneration;

    public static event EventHandler? ApplicationColorModeChanged;

    public static IEnumerable<string> GetDisplayNames() =>
        ColorThemes.Select(ct => ct.name);

    public static ColorTheme GetSelectedColorTheme() =>
        ColorThemes.FirstOrDefault(
            ct => ct.name == Settings.Default.ColorTheme,
            new("", ColorTheme.System))
        .theme;

    public static ColorTheme GetActiveColorTheme()
    {
        var colorTheme = GetSelectedColorTheme();
        if (colorTheme != ColorTheme.System)
        {
            return colorTheme;
        }

        using var key = Registry.CurrentUser.OpenSubKey(WindowsColorThemeKey);
        if ((key?.GetValue("SystemUsesLightTheme") as int?
            ?? 1)
            == 1)
        {
            return ColorTheme.Light;
        }
        return ColorTheme.Dark;
    }

    public static void Initialize()
    {
        uiContext = SynchronizationContext.Current;
        SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
    }

    public static void ApplyToApplication()
    {
        var colorMode = GetActiveColorTheme() == ColorTheme.Dark
            ? SystemColorMode.Dark
            : SystemColorMode.Classic;

        if (Application.ColorMode == colorMode)
        {
            return;
        }

        Application.SetColorMode(colorMode);
        ApplicationColorModeChanged?.Invoke(null, EventArgs.Empty);
    }

    public static void ApplyToDataGridView(DataGridView grid)
    {
        if (!Application.IsDarkModeEnabled)
        {
            return;
        }

        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
        grid.RowHeadersDefaultCellStyle.BackColor = SystemColors.Control;
        grid.RowHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
        grid.BackgroundColor = SystemColors.Window;
        grid.GridColor = SystemColors.ControlDark;
        grid.DefaultCellStyle.BackColor = SystemColors.Window;
        grid.DefaultCellStyle.ForeColor = SystemColors.WindowText;
        grid.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
        grid.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
    }

    private static void OnUserPreferenceChanged(
        object sender,
        UserPreferenceChangedEventArgs e)
    {
        if (GetSelectedColorTheme() != ColorTheme.System)
        {
            return;
        }

        if (e.Category is not (UserPreferenceCategory.General
            or UserPreferenceCategory.Color))
        {
            return;
        }

        var generation = Interlocked.Increment(ref systemThemeApplyGeneration);
        _ = Task.Delay(150).ContinueWith(_ =>
        {
            if (generation != Volatile.Read(ref systemThemeApplyGeneration))
            {
                return;
            }

            void apply() => ApplyToApplication();
            if (uiContext is not null)
            {
                uiContext.Post(_ => apply(), null);
                return;
            }

            apply();
        });
    }
}
