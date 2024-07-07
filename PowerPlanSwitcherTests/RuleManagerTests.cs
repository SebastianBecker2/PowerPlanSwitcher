namespace PowerPlanSwitcherTests
{
    using System;
    using System.Windows.Forms;
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
                Type = RuleType.StartsWith,
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

        private static List<PowerRule> CreateRules(int start, int count) =>
            Enumerable.Range(start, count).Select(CreatePowerRule).ToList();

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
            var processMonitor = new ProcessMonitorStub([
                .. ProcessMonitorStub.CreateProcesses(4, 6),
                .. ProcessMonitorStub.CreateProcesses(0, 2)]);
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

        [TestMethod]
        public void MultipleProcessesForRule()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 2),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub([]);
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

            ruleManager.StartEngine(CreateRules(1, 4));
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ]);

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }

        [TestMethod]
        public void ProcessForMultipleRules()
        {
            //Assert.Fail("UnitTest not fully implemented yet");

            List<Expectation> expectations = [
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 2),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub([]);
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

            ruleManager.StartEngine(CreateRules(1, 2));
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ]);

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }

        [TestMethod]
        public void FallbackWhenNewRulesDoNotApply()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 3),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 3),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                ProcessMonitorStub.CreateProcesses(3, 7));
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

            ruleManager.StartEngine(CreateRules(1, 4));
            ruleManager.StartEngine(CreateRules(0, 2));
            ruleManager.StartEngine(CreateRules(0, 4));

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }

        [TestMethod]
        public void NoRules()
        {
            var initialProcesses = ProcessMonitorStub.CreateProcesses(4, 6);
            initialProcesses.AddRange(ProcessMonitorStub.CreateProcesses(0, 2));
            List<int> expectedRuleApplications = [];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses);
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
            var initialProcesses = ProcessMonitorStub.CreateProcesses(4, 6);
            initialProcesses.AddRange(ProcessMonitorStub.CreateProcesses(0, 2));
            List<Expectation> expectations = [new(Reason.RuleApplied, 1)];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(initialProcesses);
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

            ruleManager.StartEngine(CreateRules(1, 4));
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }

        [TestMethod]
        public void UnknownActivePowerScheme()
        {
            var processMonitor = new ProcessMonitorStub(
                ProcessMonitorStub.CreateProcesses(3, 7));
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
                ProcessMonitorStub.CreateProcesses(3, 7));
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

            ruleManager.StartEngine(CreateRules(1, 4));
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

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
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
                ProcessMonitorStub.CreateProcesses(0, 10));
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
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 4),
                new(Reason.BaselineApplied, 1_003),
                new(Reason.RuleApplied, 2),
                new(Reason.BaselineApplied, 1_003),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                ProcessMonitorStub.CreateProcesses(3, 7));
            var powerManager = new PowerManagerStub();
            var rules = CreateRules(1, 4);
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
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
                BatteryMonitor = batteryManager,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
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
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
                BatteryMonitor = batteryManager,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
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
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
                BatteryMonitor = batteryManager,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
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
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
                BatteryMonitor = batteryManager,
            };
            ruleManager.RuleApplicationChanged += (s, e) =>
            {
                Console.WriteLine($"Applied {e.PowerSchemeGuid}");
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

            ruleManager.StartEngine(CreateRules(1, 4));
            powerManager.SetActivePowerScheme(
                powerManager.GetPowerSchemeGuids().Skip(3).First());
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);
            powerManager.SetActivePowerScheme(
                powerManager.GetPowerSchemeGuids().Skip(2).First());
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ]);

            Assert.AreEqual(expectations.Count, ruleApplicationCount);
        }
    }
}
