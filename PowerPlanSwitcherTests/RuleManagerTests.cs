namespace PowerPlanSwitcherTests
{
    using System;
    using System.Windows.Forms;
    using PowerPlanSwitcher.RuleManagement;
    using PowerPlanSwitcher.RuleManagement.Rules;

    [TestClass]
    public class RuleManagerTests
    {
        private enum Reason
        {
            RuleApplied,
            BaselineApplied,
            PowerLineChanged,
        }
        private class Expectation(Reason reason, int index)
        {
            public Reason Reason { get; set; } = reason;
            public int Index { get; set; } = index;
            public Guid GetPowerSchemeGuid() =>
                PowerManagerStub.CreatePowerSchemeGuid(Index);
        }

        private static ProcessRule CreateProcessRule(int i) =>
            new()
            {
                FilePath = $"{i}",
                Index = i,
                Type = ComparisonType.StartsWith,
                SchemeGuid = PowerManagerStub.CreatePowerSchemeGuid(i),
            };

        private static PowerLineRule CreatePowerLineRule(
            PowerLineStatus powerLineStatus, int i) =>
            new()
            {
                Index = i,
                PowerLineStatus = powerLineStatus,
                SchemeGuid = PowerManagerStub.CreatePowerSchemeGuid(i),
            };

        private static void AssertRuleApplication(
            RuleApplicationChangedEventArgs e,
            Expectation expectation)
        {
            if (expectation.Reason == Reason.BaselineApplied)
            {
                Assert.IsNull(e.Rule, "Expected baseline to be applied");
                Assert.IsNotNull(e.Reason, "Expected a reason to be provided");
                StringAssert.Contains(
                    e.Reason,
                    "No rule applies",
                    "Reason not as expected");
                Assert.AreEqual(
                    expectation.GetPowerSchemeGuid(),
                    e.PowerSchemeGuid,
                    "PowerSchemeGuid doesn't match expectation");
                return;
            }
            if (expectation.Reason == Reason.PowerLineChanged)
            {
                Assert.IsNull(e.Rule, "Expected PowerLineChange");
                Assert.IsNull(e.Reason, "Expected no reason to be provided");
                Assert.AreEqual(
                    expectation.GetPowerSchemeGuid(),
                    e.PowerSchemeGuid,
                    "PowerSchemeGuid doesn't match expectation");
                return;
            }

            Assert.IsNotNull(e.Rule, "Expected rule to be applied");
            Assert.AreEqual(
                expectation.Index,
                e.Rule.Index,
                "Wrong rule applied");
            StringAssert.Contains(
                e.Reason,
                $"Rule {expectation.Index + 1} applies");
            Assert.AreEqual(
                expectation.GetPowerSchemeGuid(),
                e.PowerSchemeGuid,
                "PowerSchemeGuid doesn't match expectation");
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
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 4),
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 4),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                    ProcessMonitorStub.CreateProcess(0),
                    ProcessMonitorStub.CreateProcess(1),
                ]);
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                ]);

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void MultipleProcessesForRule()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 2),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 3),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 3),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 20),
                new(Reason.RuleApplied, 200),
                new(Reason.RuleApplied, 100),
                new(Reason.RuleApplied, 200),
                new(Reason.RuleApplied, 20),
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 100),
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 100),
                new(Reason.RuleApplied, 2),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(1),
                    ProcessMonitorStub.CreateProcess(1),
                    ProcessMonitorStub.CreateProcess(1),
                    ProcessMonitorStub.CreateProcess(2),
                    ProcessMonitorStub.CreateProcess(2),
                    ProcessMonitorStub.CreateProcess(2),
                ]);
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(100),
                    CreateProcessRule(200),
                    CreateProcessRule(300),
                    CreateProcessRule(10),
                    CreateProcessRule(20),
                    CreateProcessRule(30),
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                    ProcessMonitorStub.CreateAction(Action.Create, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Create, 3),
                    ProcessMonitorStub.CreateAction(Action.Create, 3),
                    ProcessMonitorStub.CreateAction(Action.Create, 3),
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Create, 20),
                    ProcessMonitorStub.CreateAction(Action.Create, 20),
                    ProcessMonitorStub.CreateAction(Action.Create, 200),
                    ProcessMonitorStub.CreateAction(Action.Create, 200),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 200),
                    ProcessMonitorStub.CreateAction(Action.Create, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 20),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 200),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 20),
                    ProcessMonitorStub.CreateAction(Action.Create, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                    ProcessMonitorStub.CreateAction(Action.Create, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ]);

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void ProcessForMultipleRules()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 100),
                new(Reason.RuleApplied, 10),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 100),
                new(Reason.RuleApplied, 10),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 10),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 10),
                new(Reason.RuleApplied, 100),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 10),
                new(Reason.RuleApplied, 100),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 10),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 10),
                new(Reason.RuleApplied, 100),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(1),
                    ProcessMonitorStub.CreateProcess(10),
                    ProcessMonitorStub.CreateProcess(100),
                ]);
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(100),
                    CreateProcessRule(10),
                    CreateProcessRule(1),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 100),
                    ProcessMonitorStub.CreateAction(Action.Create, 10),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 10),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 10),
                    ProcessMonitorStub.CreateAction(Action.Create, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 100),
                    ProcessMonitorStub.CreateAction(Action.Create, 10),
                    ProcessMonitorStub.CreateAction(Action.Create, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 100),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 10),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
                    ProcessMonitorStub.CreateAction(Action.Create, 10),
                    ProcessMonitorStub.CreateAction(Action.Create, 100),
                ]);

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(10),
                    CreateProcessRule(100),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 100),
                    ProcessMonitorStub.CreateAction(Action.Create, 100),
                    ProcessMonitorStub.CreateAction(Action.Create, 10),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 10),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 10),
                    ProcessMonitorStub.CreateAction(Action.Create, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 100),
                    ProcessMonitorStub.CreateAction(Action.Create, 10),
                    ProcessMonitorStub.CreateAction(Action.Create, 100),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 100),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 10),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
                ]);

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void FallbackWhenNewRulesDoNotApply()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 3),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 3),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 3),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                ]);
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                ]);
            ruleManager.StartEngine(
                [
                    CreateProcessRule(0),
                    CreateProcessRule(1),
                ]);
            ruleManager.StartEngine(
                [
                    CreateProcessRule(0),
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                ]);
            ruleManager.StartEngine(
                [
                    CreateProcessRule(0),
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                ]);
            ruleManager.StopEngine();

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void NoRules()
        {
            List<int> expectedRuleApplications = [];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                    ProcessMonitorStub.CreateProcess(0),
                    ProcessMonitorStub.CreateProcess(1),
                ]);
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
            };

            ruleManager.StartEngine([]);

            Assert.AreEqual(
                expectedRuleApplications.Count,
                ruleApplicationCount);
        }

        [TestMethod]
        public void InitialProcesses()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 1)
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                    ProcessMonitorStub.CreateProcess(0),
                    ProcessMonitorStub.CreateProcess(1),
                ]);
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                ]);

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void FallbackToBaseline()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                ]);
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void UnknownActivePowerScheme()
        {
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                ]);
            var powerManager = new PowerManagerStub(Guid.Empty);

            _ = Assert.ThrowsException<InvalidOperationException>(() =>
                new RuleManager(powerManager)
                {
                    ProcessMonitor = processMonitor,
                }
            );
        }

        [TestMethod]
        public void ChangeToUnkownPowerScheme()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                ]);
            var powerManager = new PowerManagerStub();
            var ruleManager = new RuleManager(powerManager)
            {
                ProcessMonitor = processMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);
            powerManager.SetActivePowerScheme(Guid.Empty);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                ]);

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void ProcessTermination()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                    ProcessMonitorStub.CreateProcess(0),
                    ProcessMonitorStub.CreateProcess(1),
                ]);
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 0),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 5),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 7),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 6),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void ChangingRules()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 0),
                new(Reason.RuleApplied, 2),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 6),
                new(Reason.RuleApplied, 7),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(0),
                    ProcessMonitorStub.CreateProcess(1),
                    ProcessMonitorStub.CreateProcess(2),
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                ]);
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(0),
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 0),
                ]);

            ruleManager.StartEngine(
                [
                    CreateProcessRule(6),
                    CreateProcessRule(7),
                    CreateProcessRule(8),
                    CreateProcessRule(9),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 0),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 6),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 0),
                ]);

            ruleManager.StartEngine([]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 6),
                ]);

            ruleManager.StopEngine();

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void RestartingEngine()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_003),
                new(Reason.RuleApplied, 2),
                new(Reason.BaselineApplied, 1_003),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                ]);
            var powerManager = new PowerManagerStub();
            var ruleManager = new RuleManager(powerManager)
            {
                ProcessMonitor = processMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                ]);

            ruleManager.StopEngine();
            powerManager.SetActivePowerScheme(
                powerManager.GetPowerSchemeGuids().ToList()[3]);
            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);

            ruleManager.StopEngine();
            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                ]);

            ruleManager.StopEngine();

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void BatteryMonitorWithoutBattery()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                ]);
            var batteryMonitor = new BatteryMonitorStub(
                PowerLineStatus.Online,
                false);
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
                BatteryMonitor = batteryMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void PowerLineChangeWithActiveRule()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 5),
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.RuleApplied, 6),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                ]);
            var batteryMonitor = new BatteryMonitorStub();
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
                BatteryMonitor = batteryMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                    CreatePowerLineRule(PowerLineStatus.Online, 5),
                    CreatePowerLineRule(PowerLineStatus.Offline, 6),
                ]);
            batteryMonitor.PowerLineStatus = PowerLineStatus.Offline;
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void UnknownPowerLineStatus()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 5),
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.RuleApplied, 5),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 2),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                ]);
            var batteryMonitor = new BatteryMonitorStub();
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
                BatteryMonitor = batteryMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                    CreatePowerLineRule(PowerLineStatus.Online, 5),
                    CreatePowerLineRule(PowerLineStatus.Offline, 6),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ]);

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void PowerLineChangeWithoutActiveRule()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 5),
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.RuleApplied, 5),
                new(Reason.RuleApplied, 6),
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 6),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                ]);
            var batteryMonitor = new BatteryMonitorStub();
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
                BatteryMonitor = batteryMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                    CreatePowerLineRule(PowerLineStatus.Online, 5),
                    CreatePowerLineRule(PowerLineStatus.Offline, 6),
                ]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);
            batteryMonitor.PowerLineStatus = PowerLineStatus.Offline;
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ]);

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void BaselineChangeBeforeFallback()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 2),
                new(Reason.BaselineApplied, 1_002),
                new(Reason.RuleApplied, 2),
                new(Reason.BaselineApplied, 1_002),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_004),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                    ProcessMonitorStub.CreateProcess(6),
                    ProcessMonitorStub.CreateProcess(7),
                    ProcessMonitorStub.CreateProcess(8),
                    ProcessMonitorStub.CreateProcess(9),
                ]);
            var powerManager = new PowerManagerStub();
            var batteryMonitor = new BatteryMonitorStub();
            var ruleManager = new RuleManager(powerManager)
            {
                ProcessMonitor = processMonitor,
                BatteryMonitor = batteryMonitor,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(
                [
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                    CreateProcessRule(4),
                ]);
            powerManager.SetActivePowerScheme(
                powerManager.GetPowerSchemeGuids().ToList()[3]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);
            powerManager.SetActivePowerScheme(
                powerManager.GetPowerSchemeGuids().ToList()[2]);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ]);
            ruleManager.StartEngine(
                [
                    CreatePowerLineRule(PowerLineStatus.Offline, 1),
                    CreatePowerLineRule(PowerLineStatus.Online, 2),
                ]);
            powerManager.SetActivePowerScheme(
                powerManager.GetPowerSchemeGuids().ToList()[3]);
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            powerManager.SetActivePowerScheme(
                powerManager.GetPowerSchemeGuids().ToList()[4]);
            batteryMonitor.PowerLineStatus = PowerLineStatus.Offline;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }
    }
}
