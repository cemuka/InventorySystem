using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorSlot : InventorySlot
{
    public void Init()
    {

    }

    public override void OnDrop(PointerEventData eventData)
    {
        var recievedItem = ItemCarryHandler.GetCurrent();
        if (recievedItem.parentSlot is VendorSlot)
        {
            //do nothing
            recievedItem.parentSlot.OnItemReceived(recievedItem);
        }
        
        else if (recievedItem.parentSlot is InventorySlot)
        {
            //sell item to vendor
            //notify old parent
            recievedItem.parentSlot.OnItemLost(); 
            Debug.Log("item sold");
            InventoryEventHandler.InvokeSellEvent(recievedItem.data.metadata.price);
        }
        
        ItemCarryHandler.Clear();
    }

    public override void OnItemReceived(ItemCarry recievedItem)
    {
        base.OnItemReceived(recievedItem);
    }
}