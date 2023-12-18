namespace PowerPlanSwitcher
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Security.Principal;

    public class CachingProcess : IDisposable
    {
        private sealed class CacheKey(Process process)
        {
            private string ProcessName { get; } = process.ProcessName;
            private int ProcessId { get; } = process.Id;

            public override bool Equals(object? obj) => Equals(obj as CacheKey);

            private bool Equals(CacheKey? other) =>
                other is not null
                && ProcessName == other.ProcessName
                && ProcessId == other.ProcessId;

            public override int GetHashCode() =>
                HashCode.Combine(ProcessName, ProcessId);
        }

        private static readonly ConcurrentDictionary<CacheKey, CachingProcess>
            Cache = new();
        private static readonly CachingProcess CurrentProcess =
            new(Process.GetCurrentProcess());

        public static CachingProcess? Create(Process process)
        {
            var key = new CacheKey(process);
            if (!Cache.TryGetValue(key, out var cachedProcess))
            {
                cachedProcess = new CachingProcess(process);
                _ = Cache.TryAdd(key, cachedProcess);
            }

            if (cachedProcess.SessionId != CurrentProcess.SessionId
                || cachedProcess.Owner != CurrentProcess.Owner)
            {
                return null;
            }

            return cachedProcess;
        }

        private readonly Process process;
        private bool disposedValue;
        private string? fileName;
        private string? owner;

        public int Id => process.Id;

        public string ProcessName => process.ProcessName;

        public int SessionId => process.SessionId;

        public DateTime StartTime => process.StartTime;

        public string FileName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = process.MainModule!.FileName?.ToLowerInvariant()
                        ?? "";
                }

                return fileName;
            }
        }

        public string? Owner
        {
            get
            {
                if (string.IsNullOrWhiteSpace(owner))
                {
                    owner = GetProcessOwner(process) ?? "SYSTEM";
                }

                return owner;
            }
        }

        private CachingProcess(Process process) => this.process = process;

        private static string? GetProcessOwner(Process process)
        {
            var processHandle = IntPtr.Zero;
            try
            {
                if (!OpenProcessToken(process.Handle, TOKEN_QUERY, out processHandle))
                {
                    return null;
                }

                using var wi = new WindowsIdentity(processHandle);
                return wi.Name;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                {
                    _ = CloseHandle(processHandle);
                }
            }
        }

        // WinAPI compliant identifier naming
        // ReSharper disable InconsistentNaming
        // ReSharper disable IdentifierTypo
#pragma warning disable IDE1006 // Naming Styles
        private const int TOKEN_QUERY = 8;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(
            IntPtr ProcessHandle,
            uint DesiredAccess,
            out IntPtr TokenHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);
#pragma warning restore IDE1006 // Naming Styles
        // ReSharper restore IdentifierTypo
        // ReSharper restore InconsistentNaming

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                process.Dispose();
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
