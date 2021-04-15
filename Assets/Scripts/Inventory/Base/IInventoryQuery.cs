using System.Collections.Generic;

public interface IInventoryQuery
{
    IEnumerable<InventoryItem> TakeSnapshot();
}