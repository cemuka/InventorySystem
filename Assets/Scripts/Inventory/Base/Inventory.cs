using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using System.Linq;

public class Inventory : MonoBehaviour, IInventoryQuery
{
    public event Action<InventoryItem, int>     OnItemDroppedEvent;

    private Dictionary<int, InventorySlot> _slots;
    private Dictionary<int, InventoryItem> _items;
    

    private GameResources _resources;
    private Transform _itemDragParent;
    private InventoryItem _itemOnDrag;
    private InventoryOptions _options;

    public void Init(InventoryOptions options)
    {
        _options = options;
        _resources = _options.resources;
        _slots = new Dictionary<int, InventorySlot>();
        _items = new Dictionary<int, InventoryItem>();

        Signals.Get<OnItemBeginDragSignal>().AddListener(OnItemBeginDrag);
        Signals.Get<OnItemDragSignal>().AddListener(OnItemDrag);
        Signals.Get<OnItemEndDragSignal>().AddListener(OnItemEndDrag);
        
        CreateItemForDrag();

        for (int i = 0; i < _options.slotCount; i++)
        {
            CreateSlot(i);
        }
    }

    private void CreateItemForDrag()
    {
        _itemOnDrag = PrefabLoader.CreatePrefabAs<InventoryItem>("Prefabs/InventoryItem", _options.itemDragTransform);
        _itemOnDrag.gameObject.SetActive(false);
        _itemOnDrag.CleanStackText();
    }

    private void CreateSlot(int index)
    {
        var slotPrefab = PrefabLoader.LoadPrefab("Prefabs/InventorySlot");
        var slot = Instantiate(slotPrefab, _options.contentTransform).GetComponent<InventorySlot>();
        slot.Init(index);
        _slots.Add(index, slot);
        
        slot.OnItemDropEvent += OnItemDrop;
    }

    //  IInventoryQuery implement
    public IEnumerable<InventoryItem> TakeSnapshot()
    {
        return _items.Values;
    }

    public bool Contains(string defId)
    {
        return _items.Values.Any(item => item.definitionId == defId);
    }


    //  inventory actions
    public void AddItem(string defId, int slotIndex)
    {
        var item = PrefabLoader.CreatePrefabAs<InventoryItem>("Prefabs/InventoryItem", transform);
        var itemDef = _resources.itemDatabase.FetchItem(defId);

        item.Init(IdFactory.CreateInstanceId(), slotIndex);
        item.SetIcon(itemDef.icon);
        item.definitionId = itemDef.DefId;
        item.CleanStackText();
        item.origin = _options.origin;
        
        _items.Add(item.id, item);
        _slots[slotIndex].PlaceItem(item);
    }

    public void RemoveItem(int id, int stackAmount)
    {
        var item = _items[id];

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
    
    public bool HandleItemReceive(int slotIndex, InventoryItem receivedItem)
    {
        var itemDef = _resources.itemDatabase.FetchItem(receivedItem.definitionId);

        //  check if item already in inventory
        if (Contains(itemDef.DefId))
        {
            var item = GetItemWithAvailableStack(itemDef.DefId);
            if (item == null)
            {
                var emptySlot = FindFirstEmptySlot();
                if (emptySlot == null)
                {
                    Debug.Log("Inventory full.");
                    return false;
                }
                else
                {
                    AddItem(itemDef.DefId, slotIndex);
                    return true;
                }
            }
            else
            {
                _items[item.id].IncrementStack();
                return true;
            }
        }

        //  check if slot is filled already
        if (IsSlotEmpty(slotIndex))
        {
            AddItem(receivedItem.definitionId, slotIndex);
            return true;
        }
        else
        {
            var emptySlot = FindFirstEmptySlot();
            if (emptySlot == null)
            {
                Debug.Log("Inventory full.");
                return false;
            }
            else
            {
                AddItem(receivedItem.definitionId, emptySlot.GetIndex());
                return true;
            }
        }
    }
    
    public void DisplayDragItemDummy(bool display)
    {
        if (display)
        {
            _itemOnDrag.gameObject.SetActive(true);
            _itemOnDrag.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            _itemOnDrag.gameObject.SetActive(false);
            _itemOnDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }


    //  events  - local
    private void OnItemDrop(int slotId, PointerEventData eventData)
    {
        var item = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (item != null)
        {
            if (item.origin == _options.origin)
            {
                if (_options.allowInternalSwap)
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
                }
            }
            else
            {
                OnItemDroppedEvent?.Invoke(item, slotId);   
            }
        }
    }


    //  signals - global
    private void OnItemBeginDrag(ItemOrigin origin, int id, PointerEventData eventData)
    {
        if (origin == _options.origin)
        {
            var item = _items[id];
            var itemDef = _resources.itemDatabase.FetchItem(item.definitionId);

            _itemOnDrag.definitionId = itemDef.DefId;
            _itemOnDrag.id = item.id;
            _itemOnDrag.origin = _options.origin;
            _itemOnDrag.SetIcon(itemDef.icon);
            DisplayDragItemDummy(true);
        }
    }

    private void OnItemDrag(ItemOrigin origin, int id, PointerEventData eventData)
    {
        if (origin == _options.origin)
        {
            _itemOnDrag.transform.position = Input.mousePosition;
        }
    }

    private void OnItemEndDrag(ItemOrigin origin, int id, PointerEventData eventData)
    {
        if (origin == _options.origin)
        {
            DisplayDragItemDummy(false);
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

    private InventoryItem GetItemWithAvailableStack(string defId)
    {
        var itemDef     = _resources.itemDatabase.FetchItem(defId);
        var maxStack    = _resources.GetMaxStackByType(itemDef.itemType);
        var foundItem   = _items.Values.Where(item => item.definitionId == defId)
                                        .FirstOrDefault(item => item.currentStack < maxStack); 

        return foundItem;
    }

    private InventorySlot FindFirstEmptySlot()
    {
        var emptySlot = _slots.Values.Where(slot => slot.GetState() == SlotState.Empty).FirstOrDefault();
        return emptySlot;
    }

}