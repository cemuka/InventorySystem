using System.Collections.Generic;

public interface IInventoryQuery
{
    IEnumerable<InventoryItem> TakeSnapshot();
    bool Contains(string defId);
}