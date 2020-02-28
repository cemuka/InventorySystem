using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public int slotId;
    public InventoryItem currentItem;
    public SlotState state = SlotState.Empty;

    public void InitSlot(int id)
    {
        this.slotId = id;
    }

    public void CreateItem(InventoryItemData itemData) 
    {
        if (currentItem)
        {
            Destroy(currentItem.gameObject);
        }

        var prefab = Resources.Load<GameObject>("Prefabs/InventoryItem") as GameObject;
        var itemGO = Instantiate(prefab, this.transform);
        
        itemGO.transform.localPosition = Vector3.zero;
        itemGO.transform.localScale = Vector3.one;

        currentItem = itemGO.GetComponent<InventoryItem>();
        currentItem.Init(itemData, this);

        ChangeStateTo(SlotState.Occupied);
    }

    public void OnItemOnCarry()
    {
        currentItem.icon.enabled = false;
        ChangeStateTo(SlotState.OnCarry);
    }

    public void OnItemLost()
    {
        Destroy(currentItem.gameObject);
        ChangeStateTo(SlotState.Empty);
    }


    public virtual void OnItemReceived(ItemCarry recievedItem)
    {
        switch (state)
        {
            case SlotState.OnCarry: 

                currentItem.icon.enabled = true;
                ChangeStateTo(SlotState.Occupied);
                ItemCarryHandler.Clear();
                break;

            case SlotState.Empty: 

                CreateItem(recievedItem.data);
                ChangeStateTo(SlotState.Occupied);
                break;

            case SlotState.Occupied: 
                if (currentItem.GetInventoryItemData().metadata.id == recievedItem.data.metadata.id && 
                    currentItem.GetInventoryItemData().metadata.stackable)
                {
                    //Merged
                    Debug.Log("Merged");
                }
                else
                {
                    //Swap Items
                    Debug.Log("Swap");
                    var target = recievedItem.data;
                    recievedItem.parentSlot.CreateItem(currentItem.GetInventoryItemData());
                    this.CreateItem(target);
                }
                break;
        }
    }

    private void ChangeStateTo(SlotState targetState)
    {
        state = targetState;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        var recievedItem = ItemCarryHandler.GetCurrent();
        
        if (recievedItem.parentSlot is VendorSlot)
        {
            //item bought
            //keep in vendor
            recievedItem.parentSlot.OnItemReceived(recievedItem);

            //item adopted from this slot
            OnItemReceived(recievedItem);
        }
        else if (recievedItem.parentSlot is InventorySlot)
        {
            //notify old parent
            recievedItem.parentSlot.OnItemLost();

            //item adopted from this slot
            OnItemReceived(recievedItem);
        }

        ItemCarryHandler.Clear();
    }
}