namespace PowerPlanSwitcher
{
    using Properties;

    internal class TrayIcon : IDisposable
    {
        private static readonly Icon DefaultIcon =
            IconFromImage(Resources.power_surge);

        private bool disposedValue;
        private readonly NotifyIcon notifyIcon = new()
        {
            Icon = DefaultIcon,
            Text = "PowerPlanSwitcher",
            Visible = true,
            ContextMenuStrip = new ContextMenu()
        };
        private readonly PowerManager powerManager = new();

        public TrayIcon() =>
            powerManager.ActivePowerSchemeChanged += (_, e) =>
               {
                   var setting = PowerSchemeSettings.GetSetting(e.ActiveSchemeGuid);
                   if (setting?.Icon is null)
                   {
                       notifyIcon.Icon = DefaultIcon;
                       return;
                   }
                   notifyIcon.Icon = IconFromImage(setting.Icon);
               };

        private static Icon IconFromImage(Image img)
        {
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);

            // Header
            bw.Write((short)0);   // 0 : reserved
            bw.Write((short)1);   // 2 : 1=ico, 2=cur
            bw.Write((short)1);   // 4 : number of images
                                  // Image directory
            var w = img.Width;
            if (w >= 256)
            {
                w = 0;
            }

            bw.Write((byte)w);    // 0 : width of image
            var h = img.Height;
            if (h >= 256)
            {
                h = 0;
            }

            bw.Write((byte)h);    // 1 : height of image
            bw.Write((byte)0);    // 2 : number of colors in palette
            bw.Write((byte)0);    // 3 : reserved
            bw.Write((short)0);   // 4 : number of color planes
            bw.Write((short)0);   // 6 : bits per pixel
            var sizeHere = ms.Position;
            bw.Write(0);     // 8 : image size
            var start = (int)ms.Position + 4;
            bw.Write(start);      // 12: offset of image data
                                  // Image data
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var imageSize = (int)ms.Position - start;
            _ = ms.Seek(sizeHere, SeekOrigin.Begin);
            bw.Write(imageSize);
            _ = ms.Seek(0, SeekOrigin.Begin);

            // And load it
            return new Icon(ms);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                notifyIcon.Dispose();
                powerManager.Dispose();
            }
            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
