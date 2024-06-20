namespace PowerPlanSwitcher
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Security.Principal;
    using static Vanara.PInvoke.AdvApi32;
    using static Vanara.PInvoke.Kernel32;

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
                    fileName = process.MainModule?.FileName?.ToLowerInvariant()
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
            SafeHTOKEN processHandle;
            try
            {
                processHandle = SafeHTOKEN.FromProcess(
                    process.Handle,
                    TokenAccess.TOKEN_QUERY);
                if (processHandle.IsInvalid)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

            try
            {
                using var wi = new WindowsIdentity(
                    processHandle.DangerousGetHandle());

                return wi.Name;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (processHandle.IsInvalid)
                {
                    _ = CloseHandle(processHandle.DangerousGetHandle());
                }
            }
        }

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
