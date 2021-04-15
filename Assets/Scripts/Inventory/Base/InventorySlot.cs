using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Transform itemHolder;

    private int _index;
    private SlotState _state;
    private InventoryItem _item;

    public event Action<int, PointerEventData> OnItemDropEvent;

    public virtual void Init(int index)
    {
        this._index = index;
        _state = SlotState.Empty;
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDropEvent?.Invoke(_index, eventData);
    }

    public void PlaceItem(InventoryItem item)
    {
        item.transform.SetParent(itemHolder);
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one;

        this._item = item;
        _state = SlotState.Filled;
    }

    public SlotState GetState()     => _state;
    public int       GetIndex()     => _index;
    public int       GetItemId()    => _item.id;

    public void      SetEmpty()     => _state = SlotState.Empty;
}