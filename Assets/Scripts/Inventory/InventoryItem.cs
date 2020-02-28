using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public int id;
    public InventorySlot parentSlot;
    public Image icon;
    public Text stackCountText;

    private GameObject itemToCarry;
    private ToolTip toolTip;
    
    private bool toolTipActivated;

    private InventoryItemData dataHolder;

    public void Init(InventoryItemData itemData, InventorySlot slot)
    {
        this.parentSlot = slot;
        dataHolder = itemData;
        icon.sprite = dataHolder.metadata.icon;

        if (dataHolder.metadata.stackable)
        {
            stackCountText.gameObject.SetActive(true);
            stackCountText.text = dataHolder.amount.ToString();
        }
        else
        {
            stackCountText.gameObject.SetActive(false);
        }
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
            if (item.GetComponentInChildren<InventorySlot>())
            {
                //dropped on a slot
                //slot handles the drop event itself
                Destroy(itemToCarry.gameObject);
                ItemCarryHandler.Clear();
                break;
            }
            
        }
        
        //if no result here put item back to parent slot
        parentSlot.OnItemReceived(itemToCarry.GetComponent<ItemCarry>());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip = ItemCarryHandler.CreateToolTip();
        toolTipActivated = true;

        toolTip.SetText(dataHolder.GetToolTipContent());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toolTipActivated)
        {
            toolTipActivated = false;
            ItemCarryHandler.ClearTooltip();
        }
    }

    private void Update()
    {
        if (toolTipActivated && toolTip != null)
        {
            toolTip.transform.position = Input.mousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        toolTipActivated = false;
        ItemCarryHandler.ClearTooltip();
    }
}