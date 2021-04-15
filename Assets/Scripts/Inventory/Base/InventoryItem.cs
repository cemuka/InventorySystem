using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public int id;
    public int parentSlotId;
    public int currentStack;
    
    public string definitionId;
    public ItemView view;

    public event Action<int, PointerEventData> OnItemBeginDragEvent;
    public event Action<int, PointerEventData> OnItemDragEvent;
    public event Action<int, PointerEventData> OnItemEndDragEvent;

    //  0 - id
    //  1 - parentIndexSlot
    public virtual void Init(int id, int parentSlotId)
    {
        this.id = id;
        SetParentSlot(parentSlotId);
        currentStack = 1;
        UpdateStackText();
    }

    public void SetParentSlot(int slotIndex)
    {
        parentSlotId = slotIndex;
    }

    public void SetIcon(Sprite icon)
    {
        view.iconImage.sprite = icon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnItemBeginDragEvent?.Invoke(id, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnItemDragEvent?.Invoke(id, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDragEvent?.Invoke(id, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSignals.InvokeOnItemTooltipShowEvent(definitionId);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        EventSignals.InvokeOnItemTooltipHideEvent();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EventSignals.InvokeOnItemTooltipHideEvent();
    }

    public void IncrementStack()
    {
        currentStack ++;
        UpdateStackText();
    }

    public void DecrementStack()
    {
        currentStack --;
        UpdateStackText();
    }

    private void UpdateStackText()
    {
        view.stackText.text = currentStack.ToString();
    }

    public void CleanStackText()
    {
        view.stackText.text = "";
    }

}