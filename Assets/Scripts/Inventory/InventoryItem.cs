using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public int id;
    public InventorySlot parentSlot;
    public Image icon;
    public Text stackCountText;

    private GameObject itemToCarry;
    private ToolTip toolTip;

    private InventoryItemData dataHolder;
    private IEnumerator toolTipCoroutine;

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

        toolTipCoroutine = ShowToolTipCoroutine();
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
        if (ItemCarryHandler.isTooltipAvailable())
        {
            toolTip = ItemCarryHandler.CreateToolTip();
            ShowTooltip();       
            toolTip.SetText(dataHolder.GetToolTipContent());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip = null;
        StopCoroutine(toolTipCoroutine);
        ItemCarryHandler.ClearTooltip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        toolTip = null;
        StopCoroutine(toolTipCoroutine);
        ItemCarryHandler.ClearTooltip();
    }
    
    private void ShowTooltip()
    {
        toolTipCoroutine = ShowToolTipCoroutine();
        StartCoroutine(toolTipCoroutine);
    }
    
    IEnumerator ShowToolTipCoroutine()
    {
        toolTip.gameObject.SetActive(false);
        yield return new WaitForSeconds(.55f);

        while (toolTip != null)
        {
            toolTip.gameObject.SetActive(true);
            toolTip.transform.position = Input.mousePosition;
            yield return null;
        }
    }
}