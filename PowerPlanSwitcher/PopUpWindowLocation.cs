namespace PowerPlanSwitcher;

using PowerPlanSwitcher.Properties;

internal enum PopUpWindowLocation
{
    BottomRight,
    System,
    Center,
    Off,
}

internal static class PopUpWindowLocationHelper
{
    private static readonly List<(string name, PopUpWindowLocation theme)>
        PopUpWindowLocations =
    [
        ( "Bottom Right", PopUpWindowLocation.BottomRight ),
        ( "Use System Setting", PopUpWindowLocation.System ),
        ( "Center", PopUpWindowLocation.Center ),
        ( "Off", PopUpWindowLocation.Off ),
    ];

    public static bool ShouldShowToast(string reason)
    {
        var popUpWindowSetting = reason == "Battery Management"
            ? Settings.Default.PopUpWindowLocationBM
            : Settings.Default.PopUpWindowLocationGlobal;

        return
            !string.IsNullOrEmpty(popUpWindowSetting)
            && popUpWindowSetting != "Off";
    }

    public static IEnumerable<string> GetDisplayNames() =>
        PopUpWindowLocations.Select(ct => ct.name);

    public static PopUpWindowLocation GetSelectedPopUpWindowLocation(
        string settingsValue) =>
        PopUpWindowLocations
            .FirstOrDefault(
                ct => ct.name == settingsValue,
                new("", PopUpWindowLocation.Off))
        .theme;

    public static Point GetPositionOnTaskbar(
        Size windowSize,
        string activationReason)
    {
        PopUpWindowLocation popUpWindowLocation;
        if (activationReason == "Battery Management")
        {
            popUpWindowLocation = GetSelectedPopUpWindowLocation(
                Settings.Default.PopUpWindowLocationBM);
        }
        else
        {
            popUpWindowLocation = GetSelectedPopUpWindowLocation(
                Settings.Default.PopUpWindowLocationGlobal);
        }

        return GetPositionOnTaskbar(
            windowSize,
            popUpWindowLocation);
    }

    public static Point GetPositionOnTaskbar(
        Size windowSize,
        PopUpWindowLocation location)
    {
        if (location == PopUpWindowLocation.BottomRight)
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

        var primaryScreen = Screen.PrimaryScreen;
        if (primaryScreen is null)
        {
            return GetPositionOnTaskbar(
                windowSize,
                PopUpWindowLocation.BottomRight);
        }

        var workArea = primaryScreen.WorkingArea;
        var y = workArea.Top + ((workArea.Height - windowSize.Height) / 2);
        if (location == PopUpWindowLocation.System)
        {
            return new Point(
                workArea.Left + (workArea.Width - windowSize.Width),
                y);
        }

        var x = workArea.Left + ((workArea.Width - windowSize.Width) / 2);
        return new Point(x, y);
    }
}
