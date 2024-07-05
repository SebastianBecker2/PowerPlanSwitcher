namespace PowerPlanSwitcherTests
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    using PowerPlanSwitcher.PowerManagement;
    using PowerPlanSwitcher.RuleManagement;

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
            Expectation expectation)
        {
            if (expectation.Reason == Reason.BaselineApplied)
            {
                Assert.IsNull(e.Rule);
                StringAssert.Contains(e.Reason!, "No rule applies");
                Assert.AreEqual(
                    expectation.GetPowerSchemeGuid(),
                    e.PowerSchemeGuid);
                return;
            }
            if (expectation.Reason == Reason.PowerLineChanged)
            {
                Assert.IsNull(e.Rule);
                Assert.IsNull(e.Reason);
                Assert.AreEqual(
                    expectation.GetPowerSchemeGuid(),
                    e.PowerSchemeGuid);
                return;
            }

            Assert.AreEqual(expectation.Index, e.Rule!.Index);
            StringAssert.Contains(
                e.Reason!,
                $"Rule {expectation.Index + 1} applies");
            Assert.AreEqual(
                expectation.GetPowerSchemeGuid(),
                e.PowerSchemeGuid);
        }

        private static List<PowerRule> CreateRules(int start, int count) =>
            Enumerable.Range(start, count).Select(CreatePowerRule).ToList();

        [TestMethod]
        public void NoRules()
        {
            var initialProcesses = ProcessMonitorStub.CreateProcesses(4, 6);
            initialProcesses.AddRange(ProcessMonitorStub.CreateProcesses(0, 2));
            List<int> expectedRuleApplications = [];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses);
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
            };

            ruleManager.StartEngine([]);

            Assert.AreEqual(
                expectedRuleApplications.Count,
                ruleApplicationCount);
        }

        [TestMethod]
        public void InitialProcesses()
        {
            var initialProcesses = ProcessMonitorStub.CreateProcesses(4, 6);
            initialProcesses.AddRange(ProcessMonitorStub.CreateProcesses(0, 2));
            List<Expectation> expectations = [new(Reason.RuleApplied, 1)];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses);
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(CreateRules(1, 4));

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
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
                ProcessMonitorStub.CreateProcesses(3, 7));
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(CreateRules(1, 4));
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }

        [TestMethod]
        public void MissingPowerManager()
        {
            var ruleManager = new RuleManager();

            try
            {
                ruleManager.StartEngine(CreateRules(1, 4));
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
            var initialProcesses = ProcessMonitorStub.CreateProcesses(3, 7);
            initialProcesses.AddRange(ProcessMonitorStub.CreateProcesses(0, 2));

            List<Expectation> expectations = [
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 4),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses);
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(CreateRules(1, 4));
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 0),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 5),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 7),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 6),
                ]);

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }

        // Rules           [0,1,2,3            ]
        // Init Processes  [0,1,2,3,4,5,6,7,8,9]
        // Terminate Event [  1                ]
        // Terminate Event [0                  ]
        // Rules           [            6,7,8,9]
        // Create Event    [0                  ]
        // Terminate Event [            6      ]
        // Terminate Event [0                  ]
        // Rules           [                   ]
        // Create Event    [            6      ]
        [TestMethod]
        public void ChangingRules()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 0),
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 6),
                new(Reason.RuleApplied, 7),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                ProcessMonitorStub.CreateProcesses(0, 10));
            var powerManager = new PowerManagerStub();
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = powerManager,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Debug.Print($"Switched to {e.PowerSchemeGuid}");
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(CreateRules(0, 4));
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 0),
                ]);

            ruleManager.StartEngine(CreateRules(6, 4));
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

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }

        [TestMethod]
        public void RestartingEngine()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_003),
                new(Reason.RuleApplied, 2),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                ProcessMonitorStub.CreateProcesses(3, 7));
            var powerManager = new PowerManagerStub();
            var rules = CreateRules(1, 4);
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = powerManager,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(rules);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                ]);

            ruleManager.StopEngine();
            powerManager.SetActivePowerScheme(
                powerManager.GetPowerSchemeGuids().Skip(3).First());
            ruleManager.StartEngine(rules);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);

            ruleManager.StopEngine();
            ruleManager.StartEngine(rules);
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                ]);

            ruleManager.StopEngine();

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }

        [TestMethod]
        public void BatteryManagerWithoutBattery()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                ProcessMonitorStub.CreateProcesses(3, 7));
            var batteryManager = new BatteryManagerStub(false);
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
                BatteryMonitor = batteryManager,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(CreateRules(1, 4));
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }

        [TestMethod]
        public void PowerLineChangeWithActiveRule()
        {
            List<Expectation> expectations = [
                new(Reason.PowerLineChanged,1_000_000),
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_000_001),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                ProcessMonitorStub.CreateProcesses(3, 7));
            var batteryManager = new BatteryManagerStub();
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
                BatteryMonitor = batteryManager,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(CreateRules(1, 4));
            batteryManager.PowerLineStatus = PowerLineStatus.Offline;
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }

        [TestMethod]
        public void UnknownPowerLineStatus()
        {
            List<Expectation> expectations = [
                new(Reason.PowerLineChanged,1_000_000),
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_000_000),
                new(Reason.RuleApplied, 2),
                new(Reason.BaselineApplied, 1_000_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                ProcessMonitorStub.CreateProcesses(3, 7));
            var batteryManager = new BatteryManagerStub();
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
                BatteryMonitor = batteryManager,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(CreateRules(1, 4));
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);
            batteryManager.PowerLineStatus = PowerLineStatus.Unknown;
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ]);

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }

        [TestMethod]
        public void PowerLineChangeWithoutActiveRule()
        {
            List<Expectation> expectations = [
                new(Reason.PowerLineChanged,1_000_000),
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_000_000),
                new(Reason.PowerLineChanged,1_000_001),
                new(Reason.RuleApplied, 2),
                new(Reason.BaselineApplied, 1_000_001),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                ProcessMonitorStub.CreateProcesses(3, 7));
            var batteryManager = new BatteryManagerStub();
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
                BatteryMonitor = batteryManager,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(CreateRules(1, 4));
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);
            batteryManager.PowerLineStatus = PowerLineStatus.Offline;
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ]);

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
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
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                ProcessMonitorStub.CreateProcesses(3, 7));
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(CreateRules(1, 4));
            ruleManager.PowerManager.SetActivePowerScheme(
                ruleManager.PowerManager.GetPowerSchemeGuids().Skip(3).First());
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);
            ruleManager.PowerManager.SetActivePowerScheme(
                ruleManager.PowerManager.GetPowerSchemeGuids().Skip(2).First());
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ]);

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
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
            var initialProcesses = ProcessMonitorStub.CreateProcesses(4, 6);
            initialProcesses.AddRange(ProcessMonitorStub.CreateProcesses(0, 2));
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 4),
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 4),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses);
            var ruleManager = new RuleManager()
            {
                ProcessMonitor = processMonitor,
                PowerManager = new PowerManagerStub(),
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                AssertRuleApplication(e, expectations[ruleApplicationCount]);
                ruleApplicationCount++;
            };

            ruleManager.StartEngine(CreateRules(1, 4));
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                ]);

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }
    }
}
