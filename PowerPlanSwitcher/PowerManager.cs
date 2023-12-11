namespace PowerPlanSwitcher
{
    using Vanara.Extensions;
    using Vanara.PInvoke;
    using static Vanara.PInvoke.PowrProf;

    internal class PowerManager : IDisposable
    {
        private bool disposedValue;
        private readonly SafeHPOWERNOTIFY powerSettingsChangedCallbackHandler;

        // This variable must not be a local but instead have a lifetime
        // exceeding the registration of the callback initialized with
        // PowerSettingRegisterNotification and removed with
        // PowerSettingUnregisterNotification.
        // In other words, the lifetime must extend until the PowerManager is
        // disposed properly.
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DEVICE_NOTIFY_SUBSCRIBE_PARAMETERS
            powerSettingsChangedCallback;

        public event EventHandler<ActivePowerSchemeChangedEventArgs>?
            ActivePowerSchemeChanged;
        protected virtual void OnActivePowerSchemeChanged(Guid activeSchemeGuid) =>
            ActivePowerSchemeChanged?.Invoke(
                this,
                new ActivePowerSchemeChangedEventArgs(activeSchemeGuid));

        public static string? GetPowerSchemeName(Guid schemeGuid)
        {
            var friendlyName = PowerReadFriendlyName(schemeGuid, null, null);
            if (string.IsNullOrWhiteSpace(friendlyName))
            {
                return null;
            }
            return friendlyName;
        }

        public static IEnumerable<Guid> GetPowerSchemeGuids() =>
            PowerEnumerate<Guid>(null, null, POWER_DATA_ACCESSOR.ACCESS_SCHEME);

        public static IEnumerable<(Guid guid, string? name)> GetPowerSchemes()
        {
            foreach (var guid in GetPowerSchemeGuids())
            {
                yield return (guid, GetPowerSchemeName(guid));
            }
        }

        public static Guid GetActivePowerSchemeGuid()
        {
            if (PowerGetActiveScheme(out var activeScheme).Failed)
            {
                return Guid.Empty;
            }
            return activeScheme;
        }

        public static void SetActivePowerScheme(Guid schemeGuid)
        {
            if (PowerSetActiveScheme(HKEY.NULL, schemeGuid).Failed)
            {
                throw new InvalidOperationException(
                    $"Unable to set the active power scheme to {schemeGuid}");
            }
        }

        public PowerManager()
        {
            powerSettingsChangedCallback = new DEVICE_NOTIFY_SUBSCRIBE_PARAMETERS
            {
                Callback = HandlePowerSettingsChanged,
                Context = IntPtr.Zero
            };

            if (PowerSettingRegisterNotification(
                GUID_ACTIVE_POWERSCHEME,
                DEVICE_NOTIFY.DEVICE_NOTIFY_CALLBACK,
                powerSettingsChangedCallback,
                out powerSettingsChangedCallbackHandler).Failed)
            {
                throw new InvalidOperationException(
                    "Unable to register callback for Power Setting " +
                    "Notifications.");
            }
        }

        private Win32Error HandlePowerSettingsChanged(
            IntPtr context,
            uint eventType,
            IntPtr settingPtr)
        {
            if (eventType != (uint)User32.PowerBroadcastType.PBT_POWERSETTINGCHANGE || settingPtr == IntPtr.Zero)
            {
                return Win32Error.NO_ERROR;
            }

            var setting = settingPtr.ToStructure<User32.POWERBROADCAST_SETTING>();
            if (setting.PowerSetting != GUID_ACTIVE_POWERSCHEME)
            {
                return Win32Error.NO_ERROR;
            }

            try
            {
                OnActivePowerSchemeChanged(new Guid(setting.Data));
            }
            catch (ArgumentException) { }
            return Win32Error.NO_ERROR;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                if (powerSettingsChangedCallbackHandler != IntPtr.Zero)
                {
                    _ = PowerSettingUnregisterNotification(
                        powerSettingsChangedCallbackHandler);
                }
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
