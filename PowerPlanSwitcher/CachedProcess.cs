namespace PowerPlanSwitcher
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Globalization;
    using System.Management;

    public class CachedProcess
    {
        private sealed class CacheKey(int processId, string processName)
        {
            private int ProcessId { get; } = processId;
            private string ProcessName { get; } = processName;

            public override bool Equals(object? obj) => Equals(obj as CacheKey);

            private bool Equals(CacheKey? other) =>
                other is not null
                && ProcessId == other.ProcessId
                && ProcessName == other.ProcessName;

            public override int GetHashCode() =>
                HashCode.Combine(ProcessId, ProcessName);
        }

        private static readonly ConcurrentDictionary<CacheKey, CachedProcess>
            Cache = new();
        private static readonly CachedProcess OwnProcess =
            CreateFromProcess(Process.GetCurrentProcess())
            ?? throw new InvalidOperationException(
                "Unable to determine own process!");

        private static CachedProcess? Create(
            int processId,
            string processName,
            Func<CachedProcess> creator)
        {
            var key = new CacheKey(processId, processName);
            if (Cache.TryGetValue(key, out var cp))
            {
                return cp;
            }

            try
            {
                cp = creator();
            }
            catch (Exception)
            {
                return null;
            }

            _ = Cache.TryAdd(key, cp);
            return cp;
        }

        public static CachedProcess? CreateFromProcess(Process process) =>
            Create(
                process.Id,
                process.ProcessName,
                () => new CachedProcess
                {
                    ProcessId = process.Id,
                    ProcessName = process.ProcessName,
                    //SessionId = process.SessionId,
                    //StartTime = process.StartTime,
                    ExecutablePath = process.MainModule!.FileName,
                });

        public static CachedProcess? CreateFromWin32Process(
            ManagementBaseObject obj)
        {
#pragma warning disable CA1507 // Use nameof to express symbol names
            try
            {
                var processId = Convert.ToInt32(
                    obj["ProcessId"],
                    CultureInfo.InvariantCulture);
                var processName = obj["Name"]!.ToString()!.ToLowerInvariant();

                return Create(
                    processId,
                    processName,
                    () => new CachedProcess
                    {
                        ProcessId = processId,
                        ProcessName = processName,
                        //SessionId = Convert.ToInt32(
                        //    obj["SessionId"],
                        //    CultureInfo.InvariantCulture),
                        //StartTime = DateTime.ParseExact(
                        //    obj["CreationDate"]!.ToString()![..21],
                        //    "yyyyMMddHHmmss.ffffff",
                        //    null),
                        ExecutablePath = obj["ExecutablePath"]
                            !.ToString()
                            !.ToLowerInvariant(),
                    });
            }
            catch (Exception)
            {
                return null;
            }
#pragma warning restore CA1507 // Use nameof to express symbol names
        }

        public int ProcessId { get; private set; }
        public string ProcessName { get; private set; } = "";
        //public int SessionId { get; private set; }
        //public DateTime StartTime { get; private set; }
        public string ExecutablePath { get; private set; } = "";
        public bool IsOwnProcess => ProcessId == OwnProcess.ProcessId;

        private CachedProcess() { }
    }
}