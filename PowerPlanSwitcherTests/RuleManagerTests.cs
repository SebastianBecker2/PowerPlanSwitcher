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
            var initialProcesses = ProcessMonitorStub.CreateProcesses(4, 6);
            initialProcesses.AddRange(ProcessMonitorStub.CreateProcesses(0, 2));
            List<int> expectedRuleApplications = [];

            // Arrange
            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses);
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
            };

            // Check
            ruleManager.StartEngine([]);

            // Assert
            Assert.AreEqual(
                expectedRuleApplications.Count,
                ruleApplicationCount);
        }

        [TestMethod]
        public void InitialProcesses()
        {
            // Data
            var powerRules = CreateRules(1, 4);
            var initialProcesses = ProcessMonitorStub.CreateProcesses(4, 6);
            initialProcesses.AddRange(ProcessMonitorStub.CreateProcesses(0, 2));
            List<int> expectedRuleApplications = [1];

            // Arrange
            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses);
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
            Assert.AreEqual(
                expectedRuleApplications.Count,
                ruleApplicationCount);
        }

        [TestMethod]
        public void FallbackToBaseline()
        {
            // Data
            var powerRules = CreateRules(1, 4);
            var initialProcesses = ProcessMonitorStub.CreateProcesses(3, 7);
            List<int> expectedRuleApplications = [3, 4, -100];

            // Arrange
            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses);
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
            // Simulate process changes
            processMonitor.StartSimulation(
            [
                ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                ProcessMonitorStub.CreateAction(Action.Terminate, 4),
            ])?.Wait();

            // Assert
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
            var initialProcesses = ProcessMonitorStub.CreateProcesses(3, 7);
            initialProcesses.AddRange(ProcessMonitorStub.CreateProcesses(0, 2));
            List<int> expectedRuleApplications = [1, 4];

            // Arrange
            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses);
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
            // Simulate process changes
            processMonitor.StartSimulation(
            [
                ProcessMonitorStub.CreateAction(Action.Terminate, 0),
                ProcessMonitorStub.CreateAction(Action.Terminate, 5),
                ProcessMonitorStub.CreateAction(Action.Terminate, 7),
                ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                ProcessMonitorStub.CreateAction(Action.Terminate, 6),
            ])?.Wait();

            // Assert
            Assert.AreEqual(
                expectedRuleApplications.Count,
                ruleApplicationCount);
        }

        [TestMethod]
        public void BaselineChangeBeforeFallback()
        {
            // Data
            var powerRules = CreateRules(1, 4);
            var initialProcesses = ProcessMonitorStub.CreateProcesses(3, 7);
            List<int> expectedRuleApplications = [3, 4, -100, 2, -102];

            // Arrange
            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses);
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
            // Simulate user changing PowerScheme
            ruleManager.PowerManager.SetActivePowerScheme(
                ruleManager.PowerManager.GetPowerSchemeGuids().Skip(3).First());
            // Simulate process changes
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ])?.Wait();
            // Simulate user changing PowerScheme
            ruleManager.PowerManager.SetActivePowerScheme(
                ruleManager.PowerManager.GetPowerSchemeGuids().Skip(2).First());
            // Simulate process changes
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ])?.Wait();

            // Assert
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
            var initialProcesses = ProcessMonitorStub.CreateProcesses(4, 6);
            initialProcesses.AddRange(ProcessMonitorStub.CreateProcesses(0, 2));
            List<int> expectedRuleApplications = [1, 2, 4, 1, 4];

            // Arrange
            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses);
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
            // Simulate process changes
            processMonitor.StartSimulation(
            [
                ProcessMonitorStub.CreateAction(Action.Create, 2),
                ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ProcessMonitorStub.CreateAction(Action.Create, 1),
                ProcessMonitorStub.CreateAction(Action.Terminate, 1),
            ])?.Wait();

            // Assert
            Assert.AreEqual(
                expectedRuleApplications.Count,
                ruleApplicationCount);
        }
    }
}
