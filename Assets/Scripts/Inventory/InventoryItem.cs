using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventoryItem : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int id;
    public Slot parentSlot;
    public Image icon;

    private GameObject itemToCarry;

    private InventoryItemData dataHolder;

    public void Init(InventoryItemData itemData, Slot slot)
    {
        this.parentSlot = slot;
        dataHolder = itemData;
        icon.sprite = dataHolder.metadata.icon;
    }

    public InventoryItemData GetInventoryItemData()
    {
        return dataHolder;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemToCarry = ItemCarryHandler.CreateCarryItem(parentSlot, dataHolder);
        parentSlot.OnItemOnCarry();
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemToCarry.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (var item in eventData.hovered)
        {
            if (item.GetComponentInChildren<Slot>())
            {
                //dropped on a slot
                Destroy(itemToCarry.gameObject);
                ItemCarryHandler.Clear();
                break;
            }
            
        }
        
        //if no result here put item back to parent slot
        parentSlot.OnItemReceived(itemToCarry.GetComponent<ItemCarry>());
    }
}