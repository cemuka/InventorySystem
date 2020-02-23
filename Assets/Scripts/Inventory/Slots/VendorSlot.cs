using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorSlot : InventorySlot
{
    public void Init()
    {

    }

    public override void OnItemReceived(ItemCarry recievedItem)
    {
        Debug.Log(recievedItem.data.metadata.price);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var recievedItem = ItemCarryHandler.GetCurrent();
        if (recievedItem.parentSlot is VendorSlot)
        {
            //do nothing
            recievedItem.parentSlot.OnItemReceived(recievedItem);
        }
        
        if (recievedItem.parentSlot is InventorySlot)
        {
            //sell item to vendor
            //notify old parent
            recievedItem.parentSlot.OnItemLost();   
        }
        
        ItemCarryHandler.Clear();
    }
}