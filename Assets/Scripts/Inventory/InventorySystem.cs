using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public Transform vendorSlotsParent;
    public Transform playerSlotsParent;
    public Transform itemDragParent;
    public TMP_Text goldText;
    public PlayerInventory  playerInventory;
    public VendorInventory  vendorInventory;

    private int _currentGold;
    private GameResources _resources;
    private IInventoryQuery _inventoryQuery;
    private IInventoryQuery _vendorQuery;
    
    public void Init(GameResources resources)
    {
        _resources = resources;
        _currentGold = _resources.playerGold;
        UpdateGold();
        InitPlayer();
        InitVendor();

        Signals.Get<OnItemDragSignal>().AddListener(OnItemDrag);
        Signals.Get<OnItemPointerEnterSignal>().AddListener(OnItemPointerEnter);
        Signals.Get<OnItemPointerExitSignal>().AddListener(OnItemPointerExit);
        Signals.Get<OnItemPointerClickSignal>().AddListener(OnItemPointerClick);
    }


    private void InitVendor()
    {
        vendorInventory.Init(new InventoryOptions()
        {
            origin = ItemOrigin.Vendor,
            resources = _resources,
            contentTransform = vendorSlotsParent,
            itemDragTransform = itemDragParent,
            slotCount = 10,
            allowInternalSwap = false
        });
        vendorInventory.OnItemDroppedEvent += OnVendorItemReceive;
        vendorInventory.AddItem(_resources.itemDatabase.definitions[0].DefId, 0);
        vendorInventory.AddItem(_resources.itemDatabase.definitions[1].DefId, 1);
        vendorInventory.AddItem(_resources.itemDatabase.definitions[2].DefId, 2);
        vendorInventory.AddItem(_resources.itemDatabase.definitions[3].DefId, 3);
        vendorInventory.AddItem(_resources.itemDatabase.definitions[4].DefId, 4);
        vendorInventory.AddItem(_resources.itemDatabase.definitions[5].DefId, 5);
        vendorInventory.AddItem(_resources.itemDatabase.definitions[6].DefId, 6);
        vendorInventory.AddItem(_resources.itemDatabase.definitions[7].DefId, 7);
        _vendorQuery = vendorInventory;
    }

    private void InitPlayer()
    {
        playerInventory.Init(new InventoryOptions()
        {
            origin = ItemOrigin.PlayerInventory,
            resources = _resources,
            contentTransform = playerSlotsParent,
            itemDragTransform = itemDragParent,
            slotCount = 12,
            allowInternalSwap = true
        });
        playerInventory.OnItemDroppedEvent += OnPlayerItemReceive;
        playerInventory.AddItem(_resources.itemDatabase.definitions[0].DefId, 0);
        playerInventory.AddItem(_resources.itemDatabase.definitions[1].DefId, 1);
        playerInventory.AddItem(_resources.itemDatabase.definitions[2].DefId, 2);
        _inventoryQuery = playerInventory;
    }

    //  signal listeners
    private void OnItemDrag(ItemOrigin origin, int itemId, PointerEventData eventData)
    {
        Signals.Get<OnHideTooltipSignal>().Invoke();
    }

    private void OnItemPointerEnter(ItemOrigin origin, int itemId, PointerEventData eventData)
    {
        string itemDefId = string.Empty;
        if (origin == ItemOrigin.PlayerInventory)
        {
            itemDefId = _inventoryQuery.TakeSnapshot().Single(item => item.id == itemId).definitionId;
        }
        
        if (origin == ItemOrigin.Vendor)
        {
            itemDefId = _vendorQuery.TakeSnapshot().Single(item => item.id == itemId).definitionId;
        }


        Signals.Get<OnShowTooltipSignal>().Invoke(itemDefId);
    }

    private void OnItemPointerExit(ItemOrigin origin, int itemId, PointerEventData eventData)
    {
        Signals.Get<OnHideTooltipSignal>().Invoke();
    }

    private void OnItemPointerClick(ItemOrigin origin, int itemId, PointerEventData eventData)
    {
        Signals.Get<OnHideTooltipSignal>().Invoke();

        if (origin == ItemOrigin.PlayerInventory)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var item    = _inventoryQuery.TakeSnapshot().Single(x => x.id == itemId);
                var itemDef = _resources.itemDatabase.FetchItem(item.definitionId);
                
                playerInventory.RemoveItem(item.id, 1);

                _currentGold += itemDef.price;
                UpdateGold();
            }
        }
    }

    //  event listeners
    private void OnVendorItemReceive(InventoryItem item, int slotIndex)
    {
        if (item.origin == ItemOrigin.PlayerInventory)
        {
            playerInventory.RemoveItem(item.id, 1);

            //  OnEndDrag event invokes after OnDrop
            //  Inventory destroys when remove method called, thus OnEndDrag couldn't invoke 
            //  Eventually I needed to set dragged dummy false

            playerInventory.DisplayDragItemDummy(false);
            
            var itemDef = _resources.itemDatabase.FetchItem(item.definitionId);
            _currentGold += itemDef.price;
            UpdateGold();
        }
    }

    private void OnPlayerItemReceive(InventoryItem item, int slotIndex)
    {
        if (item.origin == ItemOrigin.Vendor)
        {
            if (playerInventory.HandleItemReceive(slotIndex, item))
            {
                var itemDef = _resources.itemDatabase.FetchItem(item.definitionId);
                _currentGold -= itemDef.price;
                UpdateGold();
            }
        }
    }

    private void UpdateGold()
    {
        goldText.text = "Gold: " + _currentGold;
    }

}

public class InventoryOptions
{
    public ItemOrigin origin;
    public GameResources resources;
    public Transform contentTransform;
    public Transform itemDragTransform;
    public int slotCount;
    public bool allowInternalSwap;
}
