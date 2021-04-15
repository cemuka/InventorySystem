using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IInventoryQuery
{
    private Dictionary<int, InventorySlot> _slots;
    private Dictionary<int, InventoryItem> _items;
    

    private GameResources _resources;
    private Transform _slotsParent;
    private Transform _itemDragParent;
    private InventoryItem _itemOnDrag;

    public void Init(GameResources resources, Transform slotsParent, Transform itemDragParent, int slotCount)
    {
        _resources = resources;
        _slots = new Dictionary<int, InventorySlot>();
        _items = new Dictionary<int, InventoryItem>();
        
        _slotsParent = slotsParent;
        _itemDragParent = itemDragParent;
        CreateItemForDrag();

        for (int i = 0; i < slotCount; i++)
        {
            CreateSlot(i);
        }
    }

    public IEnumerable<InventoryItem> TakeSnapshot()
    {
        return _items.Values;
    }

    private void CreateItemForDrag()
    {
        _itemOnDrag = PrefabLoader.CreatePrefabAs<InventoryItem>("Prefabs/InventoryItem", _itemDragParent);
        _itemOnDrag.gameObject.SetActive(false);
        _itemOnDrag.CleanStackText();
    }

    private void CreateSlot(int index)
    {
        var slotPrefab = PrefabLoader.LoadPrefab("Prefabs/InventorySlot");
        var slot = Instantiate(slotPrefab, _slotsParent).GetComponent<InventorySlot>();
        slot.Init(index);
        _slots.Add(index, slot);
        
        slot.OnItemDropEvent += OnItemDrop;
    }

    public void AddItem(string defId, int slotIndex)
    {
        var item = PrefabLoader.CreatePrefabAs<InventoryItem>("Prefabs/InventoryItem", transform);
        var itemDef = _resources.itemDatabase.FetchItem(defId);

        item.Init(IdFactory.CreateInstanceId(), slotIndex);
        item.SetIcon(itemDef.icon);
        item.definitionId = itemDef.Id;
        item.CleanStackText();
        
        _items.Add(item.id, item);
        _slots[slotIndex].PlaceItem(item);

        item.OnItemBeginDragEvent   += OnItemBeginDrag;
        item.OnItemDragEvent        += OnItemDrag;
        item.OnItemEndDragEvent     += OnItemEndDrag;
    }

    public void RemoveItem(int id, int stackAmount)
    {
        var item = _items[_itemOnDrag.id];

        if (item.currentStack > 1)
        {
            item.DecrementStack();
        }
        else
        {
            _slots[_items[id].parentSlotId].SetEmpty();

            Destroy(_items[id].gameObject);
            _items.Remove(id);
        }
    }


    //  events
    public void OnItemBeginDrag(int id, PointerEventData eventData)
    {
        var item = _items[id];
        var itemDef = _resources.itemDatabase.FetchItem(item.definitionId);

        _itemOnDrag.definitionId = itemDef.Id;
        _itemOnDrag.id = item.id;
        _itemOnDrag.SetIcon(itemDef.icon);
        _itemOnDrag.gameObject.SetActive(true);
        _itemOnDrag.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnItemDrag(int id, PointerEventData eventData)
    {
        _itemOnDrag.transform.position = Input.mousePosition;
    }

    public void OnItemEndDrag(int id, PointerEventData eventData)
    {
        _itemOnDrag.gameObject.SetActive(false);
        _itemOnDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private void OnItemDrop(int slotId, PointerEventData eventData)
    {
        var item = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (item != null)
        {
            if (IsSlotEmpty(slotId))
            {
                _slots[item.parentSlotId].SetEmpty();

                _slots[slotId].PlaceItem(item);
                item.SetParentSlot(slotId);                
            }
            else
            {
                SwapItems(_slots[slotId].GetItemId(), _slots[item.parentSlotId].GetItemId());
            }
            item.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }


    //  helpers
    private bool IsSlotEmpty(int slotIndex)
    {
        return _slots[slotIndex].GetState() == SlotState.Empty;
    }

    private void SwapItems(int itemId1, int itemId2)
    {
        var item1 = _items[itemId1];
        var item2 = _items[itemId2];

        var slotId1 = item1.parentSlotId;
        var slotId2 = item2.parentSlotId;

        _slots[slotId1].SetEmpty();
        _slots[slotId2].SetEmpty();

        _slots[slotId1].PlaceItem(item2);
        _slots[slotId2].PlaceItem(item1);

        item1.SetParentSlot(slotId2);
        item2.SetParentSlot(slotId1);
    }

    // private bool IsItemInInventoryAlready(string defId)
    // {
    //     return _items.Values.Any(item => item.definitionId == defId);
    // }

    // private int GetItemIdWithAvailableStack(string defId)
    // {
    //     int itemId = Constants.INVALID;

    //     var itemDef     = _resources.itemDatabase.FetchItem(defId);
    //     var maxStack    = _resources.GetMaxStackByType(itemDef.itemType);
    //     var foundItem   = _items.Values.Where(item => item.definitionId == defId)
    //                                     .FirstOrDefault(item => item.currentStack < maxStack); 

    //     if (foundItem != null)
    //     {
    //         itemId = foundItem.id;
    //     }

    //     return itemId;
    // }

    // private int FindFirstEmptySlot()
    // {
    //     int index = Constants.INVALID;

    //     var emptySlot = _slots.Values.Where(slot => slot.GetState() == SlotState.Empty).FirstOrDefault();
    //     if (emptySlot != null)
    //     {
    //         index = emptySlot.GetIndex();
    //     }

    //     return index;
    // }
    
    // private void HandleItemReceive(int slotIndex, InventoryItem receivedItem)
    // {
    //     var itemDef = _resources.itemDatabase.FetchItem(receivedItem.definitionId);

    //     //  check if item already in inventory
    //     if (IsItemInInventoryAlready(itemDef.Id))
    //     {
    //         int itemId = GetItemIdWithAvailableStack(itemDef.Id);
    //         if (itemId == Constants.INVALID)
    //         {
    //             int emptySlotIndex = FindFirstEmptySlot();
    //             if (emptySlotIndex == Constants.INVALID)
    //             {
    //                 Debug.Log("Inventory full.");
    //             }
    //             else
    //             {
    //                 CreateItem(emptySlotIndex, receivedItem.definitionId);
    //             }
    //         }
    //         else
    //         {
    //             _items[itemId].IncrementStack();
    //         }

    //         return;
    //     }

    //     //  check if slot is filled
    //     if (IsSlotEmpty(slotIndex))
    //     {
    //         int emptySlotIndex = FindFirstEmptySlot();
    //         if (emptySlotIndex == Constants.INVALID)
    //         {
    //             Debug.Log("Inventory full.");
    //         }
    //         else
    //         {
    //             CreateItem(emptySlotIndex, receivedItem.definitionId);
    //         }
    //     }
    //     else
    //     {
    //         CreateItem(slotIndex, receivedItem.definitionId);
    //     }
    // }

}