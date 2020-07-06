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

    private InventoryItemData content;
    private IEnumerator toolTipCoroutine;
    private WaitForSeconds repaintRate;

    public void Init(InventoryItemData itemData, InventorySlot slot)
    {
        this.parentSlot = slot;
        content = itemData;
        icon.sprite = content.data.icon;

        if (content.data.stackable)
        {
            stackCountText.gameObject.SetActive(true);
            stackCountText.text = content.amount.ToString();
        }
        else
        {
            stackCountText.gameObject.SetActive(false);
        }

        toolTipCoroutine = ShowToolTipCoroutine();
        repaintRate = new WaitForSeconds(.02f);
    }

    public InventoryItemData GetInventoryItemData()
    {
        return content;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemToCarry = InventoryDisplayHelper.CreateCarryItem(parentSlot, content);
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
                InventoryDisplayHelper.ClearItemCarry();
                break;
            }
            
        }
        
        //if no result here put item back to parent slot
        parentSlot.OnItemReceived(itemToCarry.GetComponent<ItemCarry>());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InventoryDisplayHelper.TooltipIsAvailable())
        {
            toolTip = InventoryDisplayHelper.CreateToolTip();
            ShowTooltip();       
            toolTip.SetText(content.GetToolTipContent());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip = null;
        StopCoroutine(toolTipCoroutine);
        InventoryDisplayHelper.ClearTooltip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        toolTip = null;
        StopCoroutine(toolTipCoroutine);
        InventoryDisplayHelper.ClearTooltip();
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
            yield return repaintRate;
        }
    }
}