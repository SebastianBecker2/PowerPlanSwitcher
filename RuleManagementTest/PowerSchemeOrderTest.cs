namespace RuleManagementTest;

using PowerManagement;

[TestClass]
public sealed class PowerSchemeOrderTest
{
    [TestMethod]
    public void Apply_NoSavedOrder_KeepsOriginalSequence()
    {
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();
        var third = Guid.NewGuid();
        var items = new[] { first, second, third };

        var ordered = PowerSchemeOrder.Apply(items, guid => guid, _ => null);

        CollectionAssert.AreEqual(items, ordered.ToList());
    }

    [TestMethod]
    public void Apply_SavedOrder_SortsByOrderThenOriginalIndex()
    {
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();
        var third = Guid.NewGuid();
        var items = new[] { first, second, third };
        var orders = new Dictionary<Guid, int>
        {
            [first] = 2,
            [second] = 0,
            [third] = 1,
        };

        var ordered = PowerSchemeOrder.Apply(
            items,
            guid => guid,
            guid => orders[guid]);

        CollectionAssert.AreEqual(new[] { second, third, first }, ordered.ToList());
    }

    [TestMethod]
    public void Apply_MixedOrder_PlacesUnorderedItemsLastInOriginalRelativeOrder()
    {
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();
        var third = Guid.NewGuid();
        var fourth = Guid.NewGuid();
        var items = new[] { first, second, third, fourth };
        var orders = new Dictionary<Guid, int>
        {
            [third] = 0,
            [first] = 1,
        };

        var ordered = PowerSchemeOrder.Apply(
            items,
            guid => guid,
            guid => orders.TryGetValue(guid, out var order) ? order : null);

        CollectionAssert.AreEqual(new[] { third, first, second, fourth }, ordered.ToList());
    }
}
