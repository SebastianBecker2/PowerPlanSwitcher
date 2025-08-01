#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace PowerPlanSwitcherTests
{
    using System;
    using System.Windows.Forms;
    using FakeItEasy;
    using PowerPlanSwitcher.PowerManagement;
    using PowerPlanSwitcher.ProcessManagement;
    using PowerPlanSwitcher.RuleManagement;
    using PowerPlanSwitcher.RuleManagement.Rules;

    [TestClass]
    public class RuleManagerTests
    {
        private record RuleSnapshot(Guid SchemeGuid, int ActivationCount)
        {
            public static RuleSnapshot? Create(IRule? rule)
            {
                if (rule is null)
                {
                    return null;
                }
                return new RuleSnapshot(rule.SchemeGuid, rule.ActivationCount);
            }
        }

        private static int ruleIndex;
        private static ProcessRule CreateTestProcessRule(
            string path,
            Guid scheme = default,
            ComparisonType type = ComparisonType.EndsWith,
            int? index = null)
        {
            if (scheme == Guid.Empty)
            {
                scheme = Guid.NewGuid();
            }

            if (!index.HasValue)
            {
                index = ruleIndex++;
            }

            return new ProcessRule
            {
                FilePath = path,
                Index = index.Value,
                SchemeGuid = scheme,
                Type = type,
            };
        }


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
                StringAssert.Contains(
                    e.Reason,
                    "Battery Management",
                    "Expected reason to be \"Battery Management\"");
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

        [TestMethod]
        public void MultipleProcessesForProcessRule()
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
            processMonitor.StartMonitoring();
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
        public void ProcessForMultipleProcessRules()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 10),
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
            processMonitor.StartMonitoring();
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
            processMonitor.StartMonitoring();
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
                new(Reason.RuleApplied, 3),
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
            var batteryMonitor = new BatteryMonitorStub();
            var powerManager = new PowerManagerStub();
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
            processMonitor.StartMonitoring();
            ruleManager.StartEngine(
                [
                    CreateProcessRule(0),
                    CreateProcessRule(1),
                ]);
            processMonitor.StartMonitoring();
            ruleManager.StartEngine(
                [
                    CreateProcessRule(0),
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                ]);
            processMonitor.StartMonitoring();
            ruleManager.StartEngine(
                [
                    CreateProcessRule(0),
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                ]);
            processMonitor.StartMonitoring();
            ruleManager.StopEngine();

            powerManager.SetActivePowerScheme(
                powerManager.GetPowerSchemeGuids().ToList()[3]);

            ruleManager.StartEngine(
                [
                    CreateProcessRule(0),
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                ]);
            processMonitor.StartMonitoring();
            ruleManager.StopEngine();

            ruleManager.StartEngine(
                [
                    CreatePowerLineRule(PowerLineStatus.Offline, 0),
                    CreatePowerLineRule(PowerLineStatus.Unknown, 1),
                ]);
            processMonitor.StartMonitoring();
            ruleManager.StopEngine();

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void NoRules()
        {
            List<Expectation> expectations = [];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(4),
                    ProcessMonitorStub.CreateProcess(5),
                ]);
            var batteryMonitor = new BatteryMonitorStub();
            var ruleManager = new RuleManager(new PowerManagerStub())
            {
                ProcessMonitor = processMonitor,
                BatteryMonitor = batteryMonitor,
            };

            ruleManager.StartEngine([]);

            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 1),
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 1),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 5),
                ]);

            batteryMonitor.PowerLineStatus = PowerLineStatus.Offline;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Online;

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount);
        }

        [TestMethod]
        public void InitialProcesses()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 4),
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
            processMonitor.StartMonitoring();

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
                new(Reason.RuleApplied, 5),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 6),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
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
                    CreatePowerLineRule(PowerLineStatus.Offline, 5),
                    CreatePowerLineRule(PowerLineStatus.Unknown, 6),
                ]);
            processMonitor.StartMonitoring();
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);
            batteryMonitor.PowerLineStatus = PowerLineStatus.Offline;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Online;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Online;

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void UnknownActivePowerScheme()
        {
            var powerManager = new PowerManagerStub(Guid.Empty);
            var ruleManager = new RuleManager(powerManager);

            _ = Assert.ThrowsExactly<InvalidOperationException>(() =>
                ruleManager.StartEngine([])
            );
        }

        [TestMethod]
        public void ChangeToUnknownPowerScheme()
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
            processMonitor.StartMonitoring();
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
                new(Reason.RuleApplied, 3),
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
            processMonitor.StartMonitoring();
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
                new(Reason.RuleApplied, 0),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 0),
                new(Reason.RuleApplied, 8),
                new(Reason.RuleApplied, 10),
                new(Reason.RuleApplied, 1),
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
                    CreateProcessRule(0),
                    CreateProcessRule(1),
                    CreateProcessRule(2),
                    CreateProcessRule(3),
                ]);
            processMonitor.StartMonitoring();
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
            processMonitor.StartMonitoring();
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 0),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 6),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 0),
                ]);

            ruleManager.StartEngine([]);
            processMonitor.StartMonitoring();
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 6),
                ]);
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Offline;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Online;

            ruleManager.StartEngine(
                [
                    CreatePowerLineRule(PowerLineStatus.Online, 0),
                    CreatePowerLineRule(PowerLineStatus.Offline, 1),
                ]);
            processMonitor.StartMonitoring();
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Offline;

            ruleManager.StartEngine(
                [
                    CreateProcessRule(10),
                    CreateProcessRule(11),
                    CreatePowerLineRule(PowerLineStatus.Offline, 0),
                    CreatePowerLineRule(PowerLineStatus.Unknown, 1),
                    CreateProcessRule(8),
                    CreateProcessRule(9),
                ]);
            processMonitor.StartMonitoring();
            batteryMonitor.PowerLineStatus = PowerLineStatus.Online;
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 10),
                ]);
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 10),
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
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 3),
                new(Reason.BaselineApplied, 1_003),
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 3),
                new(Reason.BaselineApplied, 1_003),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(3),
                    ProcessMonitorStub.CreateProcess(4),
                ]);
            var batteryMonitor = new BatteryMonitorStub();
            var powerManager = new PowerManagerStub();
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
            processMonitor.StartMonitoring();
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
            processMonitor.StartMonitoring();
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
            processMonitor.StartMonitoring();
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                ]);

            ruleManager.StopEngine();
            ruleManager.StartEngine(
                [
                    CreatePowerLineRule(PowerLineStatus.Online, 1),
                    CreatePowerLineRule(PowerLineStatus.Offline, 2),
                    CreatePowerLineRule(PowerLineStatus.Unknown, 3),
                ]);
            processMonitor.StartMonitoring();
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;

            ruleManager.StopEngine();
            batteryMonitor.PowerLineStatus = PowerLineStatus.Offline;
            ruleManager.StartEngine(
                [
                    CreatePowerLineRule(PowerLineStatus.Online, 1),
                    CreatePowerLineRule(PowerLineStatus.Offline, 2),
                    CreatePowerLineRule(PowerLineStatus.Unknown, 3),
                ]);
            processMonitor.StartMonitoring();
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;

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
                    CreatePowerLineRule(PowerLineStatus.Online, 5),
                    CreatePowerLineRule(PowerLineStatus.Offline, 6),
                    CreatePowerLineRule(PowerLineStatus.Unknown, 7),
                ]);
            processMonitor.StartMonitoring();
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 3),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                ]);
            batteryMonitor.PowerLineStatus = PowerLineStatus.Offline;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Online;

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
            processMonitor.StartMonitoring();
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

        [TestMethod]
        public void InitialPowerLineStatus()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 2),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 3),
                new(Reason.BaselineApplied, 1_000),
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var powerManager = new PowerManagerStub();
            var batteryMonitor = new BatteryMonitorStub(PowerLineStatus.Offline);
            var ruleManager = new RuleManager(powerManager)
            {
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
                    CreatePowerLineRule(PowerLineStatus.Online, 1),
                    CreatePowerLineRule(PowerLineStatus.Offline, 2),
                    CreatePowerLineRule(PowerLineStatus.Unknown, 3),
                ]);
            ruleManager.StopEngine();

            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            ruleManager.StartEngine(
                [
                    CreatePowerLineRule(PowerLineStatus.Online, 1),
                    CreatePowerLineRule(PowerLineStatus.Offline, 2),
                    CreatePowerLineRule(PowerLineStatus.Unknown, 3),
                ]);
            ruleManager.StopEngine();

            batteryMonitor.PowerLineStatus = PowerLineStatus.Online;
            ruleManager.StartEngine(
                [
                    CreatePowerLineRule(PowerLineStatus.Online, 1),
                    CreatePowerLineRule(PowerLineStatus.Offline, 2),
                    CreatePowerLineRule(PowerLineStatus.Unknown, 3),
                ]);
            ruleManager.StopEngine();

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void ChangingPowerLineStatus()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 3),
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 2),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var powerManager = new PowerManagerStub();
            var batteryMonitor = new BatteryMonitorStub(PowerLineStatus.Offline);
            var ruleManager = new RuleManager(powerManager)
            {
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
                    CreatePowerLineRule(PowerLineStatus.Online, 1),
                    CreatePowerLineRule(PowerLineStatus.Offline, 2),
                    CreatePowerLineRule(PowerLineStatus.Unknown, 3),
                ]);
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Online;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Offline;

            ruleManager.StopEngine();

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount,
                "Unexpected count of rule applications");
        }

        [TestMethod]
        public void NoMonitors()
        {
            List<Expectation> expectations = [];

            var ruleApplicationCount = 0;
            var ruleManager = new RuleManager(new PowerManagerStub());

            ruleManager.StartEngine(
                [
                    CreatePowerLineRule(PowerLineStatus.Online, 1),
                    CreateProcessRule(2),
                ]);
            ruleManager.StopEngine();

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount);
        }

        [TestMethod]
        public void MixRuleTypes()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 1),
                new(Reason.RuleApplied, 4),
                new(Reason.RuleApplied, 5),
                new(Reason.RuleApplied, 2),
                new(Reason.RuleApplied, 5),
                new(Reason.RuleApplied, 3),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(1),
                    ProcessMonitorStub.CreateProcess(2),
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
                    CreatePowerLineRule(PowerLineStatus.Online, 1),
                    CreateProcessRule(2),
                    CreatePowerLineRule(PowerLineStatus.Offline, 3),
                    CreateProcessRule(4),
                    CreatePowerLineRule(PowerLineStatus.Unknown, 5),
                    CreateProcessRule(6),
                ]);

            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Create, 4),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ]);
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            processMonitor.StartSimulation(
                [
                    ProcessMonitorStub.CreateAction(Action.Terminate, 4),
                    ProcessMonitorStub.CreateAction(Action.Create, 2),
                    ProcessMonitorStub.CreateAction(Action.Terminate, 2),
                ]);
            batteryMonitor.PowerLineStatus = PowerLineStatus.Offline;

            ruleManager.StopEngine();

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount);
        }

        [TestMethod]
        public void PowerLineSwitchWhileProcessRuleApplied()
        {
            List<Expectation> expectations = [
                new(Reason.RuleApplied, 1),
                new(Reason.BaselineApplied, 1_000),
            ];

            var ruleApplicationCount = 0;
            var processMonitor = new ProcessMonitorStub(
                [
                    ProcessMonitorStub.CreateProcess(1),
                    ProcessMonitorStub.CreateProcess(2),
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
                    CreatePowerLineRule(PowerLineStatus.Offline, 3),
                ]);
            processMonitor.StartMonitoring();

            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Online;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Unknown;
            batteryMonitor.PowerLineStatus = PowerLineStatus.Online;

            ruleManager.StopEngine();

            Assert.AreEqual(
                expectations.Count,
                ruleApplicationCount);
        }

        /// <summary>
        /// Verifies that a process already running at RuleManager startup
        /// triggers its associated rule, and that the rule is properly
        /// deactivated when the process terminates.
        /// </summary>
        [TestMethod]
        public void
            ProcessIsRunningBeforeStartup_OnProcessTermination_ShouldDeactivateRule()
        {
            // Arrange
            var initialScheme = Guid.NewGuid();
            var testRuleScheme = Guid.NewGuid();
            var testExecutable = "test.exe";

            var fakeProcess = A.Fake<IProcess>();
            _ = A.CallTo(() => fakeProcess.ExecutablePath).Returns(testExecutable);

            var fakePowerManager = A.Fake<IPowerManager>();
            _ = A.CallTo(() => fakePowerManager.GetActivePowerSchemeGuid())
                .Returns(initialScheme);

            var fakeProcessMonitor = A.Fake<IProcessMonitor>();
            // fakeProcessMonitor returning fakeProcess simulates process
            // already running at RuleManager startup.
            _ = A.CallTo(() => fakeProcessMonitor.GetUsersProcesses())
                .Returns([fakeProcess]);

            var ruleManager = new RuleManager(fakePowerManager)
            {
                ProcessMonitor = fakeProcessMonitor,
            };
            List<RuleSnapshot?> ruleTimeline = [];
            ruleManager.RuleApplicationChanged += (s, e) =>
                ruleTimeline.Add(RuleSnapshot.Create(e?.Rule));
            ruleManager.StartEngine(
            [
                CreateTestProcessRule(testExecutable, testRuleScheme),
            ]);

            // Act
            fakeProcessMonitor.ProcessCreated += Raise
                .With(new ProcessEventArgs(fakeProcess));
            fakeProcessMonitor.ProcessTerminated += Raise
                .With(new ProcessEventArgs(fakeProcess));

            // Assert 
            Assert.AreEqual(
                2,
                ruleTimeline.Count,
                "Unexpected number of rules changes after process was terminated.");
            Assert.IsNotNull(
                ruleTimeline[0],
                "No process rule was applied for already running process.");
            Assert.AreEqual(
                testRuleScheme,
                ruleTimeline[0]!.SchemeGuid,
                "Appropriate process rule was not applied for already running process.");
            Assert.AreEqual(
                1,
                ruleTimeline[0]!.ActivationCount,
                "Process rule activation count is not 1 for already running process.");
            Assert.IsNull(
                ruleTimeline[1],
                "Default PowerScheme was not applied when process was terminated.");
        }
    }
}

#pragma warning restore CA1707 // Identifiers should not contain underscores
