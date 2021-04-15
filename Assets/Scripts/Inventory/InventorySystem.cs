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
    public Transform slotsParent;
    public Transform itemDragParent;
    public TMP_Text goldText;
    public PlayerInventory  playerInventory;

    private int _currentGold;
    private GameResources _resources;
    private IInventoryQuery _inventoryQuery;
    
    public void Init(GameResources resources)
    {
        _resources = resources;
        
        playerInventory.Init(_resources, slotsParent, itemDragParent, 20);
        
        playerInventory.AddItem(_resources.itemDatabase.definitions[0].Id, 0);
        playerInventory.AddItem(_resources.itemDatabase.definitions[1].Id, 1);
        playerInventory.AddItem(_resources.itemDatabase.definitions[2].Id, 2);



        _inventoryQuery = playerInventory;

        EventSignals.OnItemSoldToPlayer             += OnItemSoldToPlayer;
        EventSignals.OnItemSoldToVendor             += OnItemSoldToVendor;
        EventSignals.OnItemDropEvent                += OnItemDrop;


        _currentGold = _resources.playerGold;
        UpdateGold();
    }

    private void UpdateGold()
    {
        goldText.text = "Gold: " + _currentGold;
    }
    
    private void OnItemSoldToPlayer(int id, string defId)
    {
        var item = _resources.itemDatabase.FetchItem(defId);
        _currentGold -= item.price;
        UpdateGold();
    }

    private void OnItemSoldToVendor(int id, string defId)
    {
        var item = _resources.itemDatabase.FetchItem(defId);
        _currentGold += item.price;
        UpdateGold();
    }

    private void OnItemDrop(EventType type, int id, PointerEventData data)
    {

    }

}
