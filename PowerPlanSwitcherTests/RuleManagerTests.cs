namespace PowerPlanSwitcherTests
{
    using System;
    using PowerPlanSwitcher.PowerManagement;
    using PowerPlanSwitcher.RuleManagement;

    [TestClass]
    public class RuleManagerTests
    {
        private static PowerRule CreatePowerRule(int i) =>
            new()
            {
                FilePath = $"{i}",
                Index = i,
                Type = RuleType.Exact,
                SchemeGuid = PowerManagerStub.CreatePowerSchemeGuid(i),
            };

        private static CachedProcessStub CreateProcess(int i) =>
            new() { ExecutablePath = $"{i}" };

        private static List<CachedProcessStub> CreateProcesses(
            int start,
            int count) =>
            [.. Enumerable
                .Range(start, count)
                .Select(CreateProcess)];

        private static ProcessMonitorStub.Action CreateProcessCreatedAction(
            int i) =>
            new(ProcessMonitorStub.ActionType.Create, CreateProcess(i));

        private static ProcessMonitorStub.Action CreateProcessTerminatedAction(
            int i) =>
            new(ProcessMonitorStub.ActionType.Terminate, CreateProcess(i));

        private static void AssertRuleApplication(
            RuleApplicationChangedEventArgs e,
            int i)
        {
            if (i < 0)
            {
                Assert.IsNull(e.Rule);
                StringAssert.Contains(e.Reason!, "No rule applies");
                Assert.AreEqual(
                    PowerManagerStub.CreatePowerSchemeGuid(Math.Abs(i)),
                    e.PowerSchemeGuid);
                return;
            }

            Assert.AreEqual(i, e.Rule!.Index);
            StringAssert.Contains(e.Reason!, $"Rule {i + 1} applies");
            Assert.AreEqual(
                PowerManagerStub.CreatePowerSchemeGuid(i),
                e.PowerSchemeGuid);
        }

        private static List<PowerRule> CreateRules(int start, int count) =>
            Enumerable.Range(start, count).Select(CreatePowerRule).ToList();

        [TestMethod]
        public void NoRules()
        {
            // Data
            var initialProcesses = CreateProcesses(4, 6);
            initialProcesses.AddRange(CreateProcesses(0, 2));
            List<ProcessMonitorStub.Action> processActions =
            [
                CreateProcessCreatedAction(2),
                CreateProcessTerminatedAction(1),
                CreateProcessTerminatedAction(2),
                CreateProcessCreatedAction(1),
                CreateProcessTerminatedAction(1),
            ];
            List<int> expectedRuleApplications = [];

            // Arrange
            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses, []);
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                // Assert
                AssertRuleApplication(
                    e,
                    expectedRuleApplications[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            // Check
            ruleManager.StartEngine([]);
            // Wait for all simulated actions to finish
            processMonitor.GetActionTask()?.Wait();

            // Assert
            Assert.IsFalse(processMonitor.IsRunning());
            Assert.AreEqual(
                expectedRuleApplications.Count,
                ruleApplicationCount);
        }

        [TestMethod]
        public void InitialProcesses()
        {
            // Data
            var powerRules = CreateRules(1, 4);
            var initialProcesses = CreateProcesses(4, 6);
            initialProcesses.AddRange(CreateProcesses(0, 2));
            List<int> expectedRuleApplications = [1];

            // Arrange
            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses, []);
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                // Assert
                AssertRuleApplication(
                    e,
                    expectedRuleApplications[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            // Check
            ruleManager.StartEngine(powerRules);

            // Assert
            Assert.IsTrue(processMonitor.IsRunning());
            Assert.AreEqual(
                expectedRuleApplications.Count,
                ruleApplicationCount);
        }

        [TestMethod]
        public void FallbackToBaseline()
        {
            // Data
            var powerRules = CreateRules(1, 4);
            var initialProcesses = CreateProcesses(3, 7);
            List<ProcessMonitorStub.Action> processActions =
            [
                CreateProcessTerminatedAction(3),
                CreateProcessTerminatedAction(4),
            ];
            List<int> expectedRuleApplications = [3, 4, -100];

            // Arrange
            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                initialProcesses,
                processActions);
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                // Assert
                AssertRuleApplication(
                    e,
                    expectedRuleApplications[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            // Check
            ruleManager.StartEngine(powerRules);
            // Wait for all simulated actions to finish
            processMonitor.GetActionTask()?.Wait();

            // Assert
            Assert.IsTrue(processMonitor.IsRunning());
            Assert.AreEqual(
                expectedRuleApplications.Count,
                ruleApplicationCount);
        }

        [TestMethod]
        public void MissingPowerManager()
        {
            // Data
            var powerRules = CreateRules(1, 4);

            // Arrange
            var ruleManager = new RuleManager();

            // Check
            try
            {
                ruleManager.StartEngine(powerRules);
            }
            catch (InvalidOperationException exc)
            {
                StringAssert.Contains(
                    exc.Message,
                    $"{nameof(PowerManager)} is null.");
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void ProcessTermination()
        {
            // Data
            var powerRules = CreateRules(1, 4);
            var initialProcesses = CreateProcesses(3, 7);
            initialProcesses.AddRange(CreateProcesses(0, 2));
            List<ProcessMonitorStub.Action> processActions =
            [
                CreateProcessTerminatedAction(0),
                CreateProcessTerminatedAction(5),
                CreateProcessTerminatedAction(7),
                CreateProcessTerminatedAction(3),
                CreateProcessTerminatedAction(1),
                CreateProcessTerminatedAction(6),
            ];
            List<int> expectedRuleApplications = [1, 4];

            // Arrange
            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                initialProcesses,
                processActions);
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                // Assert
                AssertRuleApplication(
                    e,
                    expectedRuleApplications[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            // Check
            ruleManager.StartEngine(powerRules);
            // Wait for all simulated actions to finish
            processMonitor.GetActionTask()?.Wait();

            // Assert
            Assert.IsTrue(processMonitor.IsRunning());
            Assert.AreEqual(
                expectedRuleApplications.Count,
                ruleApplicationCount);
        }

        // Rules           [  1,2,3,4          ]
        // Init Processes  [0,1     4,5,6,7,8,9]
        // Create Event    [    2              ]
        // Terminate Event [  1                ]
        // Terminate Event [    2              ]
        // Create Event    [  1                ]
        // Terminate Event [  1                ]
        [TestMethod]
        public void ManyEvents()
        {
            // Data
            var powerRules = CreateRules(1, 4);
            var initialProcesses = CreateProcesses(4, 6);
            initialProcesses.AddRange(CreateProcesses(0, 2));
            List<ProcessMonitorStub.Action> processActions =
            [
                CreateProcessCreatedAction(2),
                CreateProcessTerminatedAction(1),
                CreateProcessTerminatedAction(2),
                CreateProcessCreatedAction(1),
                CreateProcessTerminatedAction(1),
            ];
            List<int> expectedRuleApplications = [1, 2, 4, 1, 4];

            // Arrange
            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                initialProcesses,
                processActions);
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                // Assert
                AssertRuleApplication(
                    e,
                    expectedRuleApplications[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            // Check
            ruleManager.StartEngine(powerRules);
            // Wait for all simulated actions to finish
            processMonitor.GetActionTask()?.Wait();

            // Assert
            Assert.IsTrue(processMonitor.IsRunning());
            Assert.AreEqual(
                expectedRuleApplications.Count,
                ruleApplicationCount);
        }
    }
}
