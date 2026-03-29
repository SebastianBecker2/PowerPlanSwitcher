namespace RuleManagementTest;

using System.Diagnostics;
using System.Drawing;
using PowerPlanSwitcher;

[TestClass]
public sealed class PowerPlanSwitcherPerformanceTest
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public void NormalizeForPowerSchemeIcon_BatchCompletesWithinBudget()
    {
        using var source = new Bitmap(512, 256);
        using (var graphics = Graphics.FromImage(source))
        {
            graphics.Clear(Color.DarkOliveGreen);
            graphics.FillEllipse(Brushes.OrangeRed, 32, 32, 300, 160);
            graphics.DrawRectangle(Pens.Black, 0, 0, source.Width - 1, source.Height - 1);
        }

        const int iterations = 1000;
        var stopwatch = Stopwatch.StartNew();

        for (var i = 0; i < iterations; i++)
        {
            using var normalized = IconUtilities.NormalizeForPowerSchemeIcon(source);
            Assert.AreEqual(32, normalized.Width);
            Assert.AreEqual(32, normalized.Height);
        }

        stopwatch.Stop();

        TestContext.WriteLine(
            "NormalizeForPowerSchemeIcon elapsed: {0} ms for {1} iterations",
            stopwatch.ElapsedMilliseconds,
            iterations);

        Assert.IsTrue(
            stopwatch.Elapsed < TimeSpan.FromSeconds(8),
            $"NormalizeForPowerSchemeIcon took too long: {stopwatch.Elapsed} for {iterations} iterations.");
    }

    [TestMethod]
    public void CreateNotifyIcon_BatchCompletesWithinBudget()
    {
        using var source = new Bitmap(512, 256);
        using (var graphics = Graphics.FromImage(source))
        {
            graphics.Clear(Color.DarkSlateBlue);
            graphics.FillEllipse(Brushes.Gold, 40, 40, 200, 120);
            graphics.DrawLine(Pens.White, 0, 0, source.Width - 1, source.Height - 1);
        }

        const int iterations = 250;
        var stopwatch = Stopwatch.StartNew();

        for (var i = 0; i < iterations; i++)
        {
            using var icon = IconUtilities.CreateNotifyIcon(source);
            Assert.IsGreaterThan(0, icon.Size.Width);
            Assert.IsGreaterThan(0, icon.Size.Height);
        }

        stopwatch.Stop();

        TestContext.WriteLine(
            "CreateNotifyIcon elapsed: {0} ms for {1} iterations",
            stopwatch.ElapsedMilliseconds,
            iterations);

        Assert.IsTrue(
            stopwatch.Elapsed < TimeSpan.FromSeconds(8),
            $"CreateNotifyIcon took too long: {stopwatch.Elapsed} for {iterations} iterations.");
    }
}
