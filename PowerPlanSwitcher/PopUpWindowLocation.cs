namespace PowerPlanSwitcher
{
    using PowerPlanSwitcher.Properties;

    internal enum PopUpWindowLocation
    {
        BottomRight,
        System ,
        Center,
    }

    internal static class PopUpWindowLocationHelper
    {
        private static readonly List<(string name, PopUpWindowLocation theme)> PopUpWindowLocations =
        [
            ( "Bottom Right", PopUpWindowLocation.BottomRight ),
            ( "Use System Setting", PopUpWindowLocation.System ),
            ( "Center", PopUpWindowLocation.Center ),
        ];

        public static IEnumerable<string> GetDisplayNames() =>
            PopUpWindowLocations.Select(ct => ct.name);

        public static PopUpWindowLocation GetSelectedPopUpWindowLocation() =>
            PopUpWindowLocations.FirstOrDefault(
                ct => ct.name == Settings.Default.PopUpWindowLocation,
                new("", PopUpWindowLocation.BottomRight))
            .theme;

        public static Point GetPositionOnTaskbar(Size windowSize)
        {
            var popUpWindowLocation = GetSelectedPopUpWindowLocation();
            if (popUpWindowLocation == PopUpWindowLocation.BottomRight)
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
            
            Rectangle workArea = Screen.PrimaryScreen.WorkingArea;
            int y = workArea.Top + (workArea.Height - windowSize.Height) / 2;
            if (popUpWindowLocation == PopUpWindowLocation.System)
            {
                return new Point(workArea.Left + (workArea.Width - windowSize.Width), y);
            }
            
            int x = workArea.Left + (workArea.Width - windowSize.Width) / 2;
            return new Point(x, y);
        }
    }
}
