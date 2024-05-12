namespace PowerPlanSwitcher
{
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
            if ((key?.GetValue("AppsUseLightTheme") as int?
                ?? key?.GetValue("SystemUsesLightTheme") as int?
                ?? 1)
                == 1)
            {
                return ColorTheme.Light;
            }
            return ColorTheme.Dark;
        }
    }
}
