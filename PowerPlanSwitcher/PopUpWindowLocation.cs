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
    // 枚举 → 本地化显示名
    // Enum -> localized display name
    public static string GetDisplayName(PopUpWindowLocation location) => location switch
    {
        PopUpWindowLocation.BottomRight => Strings.PopUpLocation_BottomRight,
        PopUpWindowLocation.System => Strings.PopUpLocation_System,
        PopUpWindowLocation.Center => Strings.PopUpLocation_Center,
        PopUpWindowLocation.Off => Strings.PopUpLocation_Off,
        _ => location.ToString(),
    };

    // 显示名列表（顺序与 GetPopUpWindowLocations 一致）
    // Display name list (order matches GetPopUpWindowLocations)
    public static IEnumerable<string> GetDisplayNames() =>
        GetPopUpWindowLocations().Select(GetDisplayName);

    // 枚举列表，顺序固定
    // Enum list, order is fixed
    public static IList<PopUpWindowLocation> GetPopUpWindowLocations() =>
        Enum.GetValues<PopUpWindowLocation>();

    // 读取已保存的枚举名；兼容旧版英文显示名
    // Read saved enum name; compatible with legacy English display names
    public static PopUpWindowLocation GetSelectedPopUpWindowLocation(
        string settingsValue)
    {
        if (string.IsNullOrEmpty(settingsValue))
        {
            return PopUpWindowLocation.Off;
        }

        if (Enum.TryParse<PopUpWindowLocation>(settingsValue, out var location))
        {
            return location;
        }

        // 兼容旧格式：英文显示名
        // Legacy format: English display name
        return settingsValue switch
        {
            "Bottom Right" => PopUpWindowLocation.BottomRight,
            "Use System Setting" => PopUpWindowLocation.System,
            "Center" => PopUpWindowLocation.Center,
            "Off" => PopUpWindowLocation.Off,
            _ => PopUpWindowLocation.Off,
        };
    }

    // 保存为枚举名（语言无关）
    // Save as enum name (language independent)
    public static void SetSelectedPopUpWindowLocation(
        PopUpWindowLocation location,
        bool batteryManagement)
    {
        if (batteryManagement)
        {
            Settings.Default.PopUpWindowLocationBM = location.ToString();
        }
        else
        {
            Settings.Default.PopUpWindowLocationGlobal = location.ToString();
        }
    }

    public static bool ShouldShowToast(string reason)
    {
        // "Battery Management" 是内部保留参数，不属于 UI 文本，保持英文
        // "Battery Management" is an internal reserved argument, not UI text; keep English
        var popUpWindowSetting = reason == "Battery Management"
            ? Settings.Default.PopUpWindowLocationBM
            : Settings.Default.PopUpWindowLocationGlobal;

        var location = GetSelectedPopUpWindowLocation(popUpWindowSetting);

        return location != PopUpWindowLocation.Off;
    }

    public static Point GetPositionOnTaskbar(
        Size windowSize,
        string activationReason)
    {
        // "Battery Management" 是内部保留参数，不属于 UI 文本，保持英文
        // "Battery Management" is an internal reserved argument, not UI text; keep English
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
