namespace PowerManagement;

public static class PowerSchemeOrder
{
    public static IReadOnlyList<T> Apply<T>(
        IEnumerable<T> items,
        Func<T, Guid> getGuid,
        Func<Guid, int?> getOrder)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(getGuid);
        ArgumentNullException.ThrowIfNull(getOrder);

        return
        [
            .. items
                .Select((item, index) => (
                    item,
                    index,
                    order: getOrder(getGuid(item))))
                .OrderBy(entry => entry.order ?? int.MaxValue)
                .ThenBy(entry => entry.index)
                .Select(entry => entry.item)
        ];
    }
}
