using PowerManagement;

using var monitor = new BatteryMonitor();
monitor.PowerLineStatusChanged += (s, e) =>
    Console.WriteLine($"Power line status changed to: {e.PowerLineStatus}");

Console.WriteLine("Monitoring power line status changes. Press Enter to exit.");
Console.ReadLine();
