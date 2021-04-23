using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int id;
    public int parentSlotId;
    public int currentStack;
    public ItemOrigin origin;
    
    public string definitionId;
    public ItemView view;

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
        Signals.Get<OnItemBeginDragSignal>().Invoke(origin, id, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Signals.Get<OnItemDragSignal>().Invoke(origin, id, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Signals.Get<OnItemEndDragSignal>().Invoke(origin, id, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Signals.Get<OnItemPointerEnterSignal>().Invoke(origin, id, eventData);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        Signals.Get<OnItemPointerExitSignal>().Invoke(origin, id, eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Signals.Get<OnItemPointerClickSignal>().Invoke(origin, id, eventData);
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

    public void CleanStackText()
    {
        view.stackText.text = "";
    }

    private void UpdateStackText()
    {
        view.stackText.text = currentStack.ToString();
    }

}