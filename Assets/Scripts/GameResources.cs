﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="GameResources")]
public class GameResources : ScriptableObject
{
    [Header("Player gold")]
    [SerializeField] private int playerGold;

    //set from editor
    [Header("Slot data")]
    [SerializeField]private List<ItemData> itemResources = new List<ItemData>();
    [SerializeField]private List<SlotData> playerInventorySlots = new List<SlotData>();
    [SerializeField]private List<SlotData> vendorSlots = new List<SlotData>();


    public void Init()
    {
        Debug.Log("resources initialized.");
    }

    internal List<SlotData> GetVendorSlots()
    {
        return vendorSlots;
    }

    //getters setters
    public int GetPlayerGold() => playerGold;

    public void InventoryTransaction(int price)
    {
        playerGold += price;
    }

    public ItemData GetItemDataById(int itemId)
    {
        return itemResources.Where( item => item.id == itemId).FirstOrDefault();
    }

    public List<SlotData> GetInventorySlots()
    {
        return playerInventorySlots;
    }

    public SlotData GetFirstEmptySlot()
    {
        return playerInventorySlots.Where(slot => slot.IsOccupied() == false ).FirstOrDefault();
    }

    public void RecieveItemToInventory(InventoryItemData itemData)
    {
        var emptySlot = GetFirstEmptySlot();
        if (!emptySlot)
        {
            Debug.Log("Inventory full.");
        }
        else
        {
            emptySlot.PlaceInsideThis(itemData);
        }
    }

    public void UpdatePlayerInventory(int id, InventoryItemData data)
    {
        playerInventorySlots[id].PlaceInsideThis(data);
    }
}
