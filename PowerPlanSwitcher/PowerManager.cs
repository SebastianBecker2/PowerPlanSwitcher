namespace PowerPlanSwitcher
{
    using System.Runtime.InteropServices;

    internal class PowerManager : IDisposable
    {
        private const uint DefaultBufferSize = 64;

        private bool disposedValue;
        private readonly IntPtr powerSettingsChangedCallbackHandler;

        public event EventHandler<ActivePowerSchemeChangedEventArgs>?
            ActivePowerSchemeChanged;
        protected virtual void OnActivePowerSchemeChanged(Guid activeSchemeGuid) =>
            ActivePowerSchemeChanged?.Invoke(
                this,
                new ActivePowerSchemeChangedEventArgs(activeSchemeGuid));

        // WinAPI compliant identifier naming
        // ReSharper disable InconsistentNaming
        // ReSharper disable IdentifierTypo
#pragma warning disable IDE1006 // Naming Styles
        private const uint ERROR_SUCCESS = 0;
        private const uint ERROR_MORE_DATA = 234;

        private const int DEVICE_NOTIFY_CALLBACK = 0x2;
        private const int PBT_POWERSETTINGCHANGE = 0x8013;

        private static Guid GUID_POWERSCHEME_PERSONALITY =
            Guid.Parse("245d8541-3943-4422-b025-13A784F679B7");

        [DllImport("PowrProf.dll")]
        private static extern uint PowerEnumerate(
            IntPtr RootPowerKey,
            IntPtr SchemeGuid,
            IntPtr SubGroupOfPowerSettingsGuid,
            uint AccessFlags,
            uint Index,
            ref Guid Buffer,
            ref uint BufferSize);

        [DllImport("PowrProf.dll")]
        private static extern uint PowerReadFriendlyName(
            IntPtr RootPowerKey,
            ref Guid SchemeGuid,
            IntPtr SubGroupOfPowerSettingsGuid,
            IntPtr PowerSettingGuid,
            IntPtr Buffer,
            ref uint BufferSize);

        [DllImport("powrprof.dll")]
        private static extern uint PowerGetActiveScheme(
            IntPtr UserRootPowerKey,
            ref IntPtr ActivePolicyGuid);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LocalFree(
            IntPtr hMem);

        [DllImport("powrprof.dll")]
        private static extern uint PowerSetActiveScheme(
            IntPtr UserRootPowerKey,
            ref Guid SchemeGuid);

        [DllImport("powrprof.dll")]
        private static extern uint PowerSettingRegisterNotification(
            ref Guid SettingGuid,
            uint Flags,
            ref DEVICE_NOTIFY_SUBSCRIBE_PARAMETERS Recipient,
            ref IntPtr RegistrationHandle);

        [DllImport("powrprof.dll")]
        private static extern uint PowerSettingUnregisterNotification(
            IntPtr RegistrationHandle);

        private delegate int DeviceNotifyCallback(
            IntPtr context,
            int type,
            IntPtr setting);

        [StructLayout(LayoutKind.Sequential)]
        private struct DEVICE_NOTIFY_SUBSCRIBE_PARAMETERS
        {
            public DeviceNotifyCallback Callback;
            public IntPtr Context;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POWERBROADCAST_SETTING
        {
            public readonly Guid PowerSetting;
            public readonly uint DataLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public readonly byte[] Data;

            public void Deconstruct(
                out Guid powerSettings,
                out uint dataLength,
                out byte[] data)
            {
                powerSettings = PowerSetting;
                dataLength = DataLength;
                data = Data;
            }
        }

        private enum AccessFlags : uint
        {
            ACCESS_SCHEME = 16,
            ACCESS_SUBGROUP = 17,
            ACCESS_INDIVIDUAL_SETTING = 18
        }
#pragma warning restore IDE1006 // Naming Styles
        // ReSharper restore IdentifierTypo
        // ReSharper restore InconsistentNaming

        public static string? GetPowerSchemeName(Guid schemeGuid) =>
            ReadFriendlyName(schemeGuid, DefaultBufferSize);

        private static string? ReadFriendlyName(
            Guid schemeGuid,
            uint bufferSize)
        {
            var pSizeName = Marshal.AllocHGlobal((int)bufferSize);
            try
            {
                var res = PowerReadFriendlyName(
                    IntPtr.Zero,
                    ref schemeGuid,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    pSizeName,
                    ref bufferSize);

                if (ERROR_MORE_DATA == res)
                {
                    return ReadFriendlyName(schemeGuid, bufferSize);
                }

                if (ERROR_SUCCESS == res)
                {
                    return Marshal.PtrToStringUni(pSizeName);
                }

                return null;
            }
            finally
            {
                Marshal.FreeHGlobal(pSizeName);
            }
        }

        public static IEnumerable<Guid> GetPowerSchemeGuids()
        {
            var schemeGuid = Guid.Empty;
            var sizeSchemeGuid = (uint)Marshal.SizeOf(typeof(Guid));

            for (uint schemeIndex = 0;
                 PowerEnumerate(
                     IntPtr.Zero,
                     IntPtr.Zero,
                     IntPtr.Zero,
                     (uint)AccessFlags.ACCESS_SCHEME,
                     schemeIndex,
                     ref schemeGuid,
                     ref sizeSchemeGuid) == ERROR_SUCCESS;
                 schemeIndex++)
            {
                yield return schemeGuid;
            }
        }

        public static IEnumerable<KeyValuePair<Guid, string?>> GetPowerSchemes()
        {
            foreach (var guid in GetPowerSchemeGuids())
            {
                yield return KeyValuePair.Create(guid, GetPowerSchemeName(guid));
            }
        }

        public static Guid GetActivePowerSchemeGuid()
        {
            var activeSchemeGuidPtr = IntPtr.Zero;
            try
            {

                var res = PowerGetActiveScheme(
                    IntPtr.Zero,
                    ref activeSchemeGuidPtr);
                if (res != ERROR_SUCCESS)
                {
                    return Guid.Empty;
                }

                return Marshal.PtrToStructure<Guid>(activeSchemeGuidPtr);
            }
            finally
            {
                _ = LocalFree(activeSchemeGuidPtr);
            }
        }

        public static void SetActivePowerScheme(Guid schemeGuid)
        {
            if (PowerSetActiveScheme(IntPtr.Zero, ref schemeGuid)
                != ERROR_SUCCESS)
            {
                throw new InvalidOperationException(
                    $"Unable to set the active power scheme to {schemeGuid}");
            }
        }

        public PowerManager()
        {
#if DEBUG
            return;
#endif
            var callback =
                new DEVICE_NOTIFY_SUBSCRIBE_PARAMETERS
                {
                    Callback = HandlePowerSettingsChanged,
                    Context = IntPtr.Zero
                };

            var result = PowerSettingRegisterNotification(
                ref GUID_POWERSCHEME_PERSONALITY,
                DEVICE_NOTIFY_CALLBACK,
                ref callback,
                ref powerSettingsChangedCallbackHandler);

            if (result != ERROR_SUCCESS)
            {
                throw new InvalidOperationException(
                    "Unable to register callback for Power Setting " +
                    "Notifications.");
            }
        }

        private int HandlePowerSettingsChanged(
            IntPtr context,
            int eventType,
            IntPtr setting)
        {
            if (eventType != PBT_POWERSETTINGCHANGE || setting == IntPtr.Zero)
            {
                return 0;
            }

            var (powerSetting, _, schemeGuid) =
                Marshal.PtrToStructure<POWERBROADCAST_SETTING>(setting);
            if (powerSetting != GUID_POWERSCHEME_PERSONALITY)
            {
                return 0;
            }

            OnActivePowerSchemeChanged(new Guid(schemeGuid));
            return 0;
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
