using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName="GameResources")]
public class GameResources : ScriptableObject
{
    //setted from editor
    [SerializeField]private List<ItemData> itemResources = new List<ItemData>();
    [SerializeField]private List<SlotData> playerInventory = new List<SlotData>();

    public void Init()
    {
        Debug.Log("resources initialized.");
    }

    //getters setters
    public ItemData GetItemDataById(int itemId)
    {
        return itemResources.Where( item => item.id == itemId).FirstOrDefault();
    }

    public List<SlotData> GetPlayerInventory()
    {
        return playerInventory;
    }

    public SlotData GetFirstEmptySlot()
    {
        return playerInventory.Where(slot => slot.occupied == false).FirstOrDefault();
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

    public void UpdatePlayerInventory(int id, InventoryItemData data, bool occupied)
    {
        playerInventory[id].data = data;
        playerInventory[id].occupied = occupied;
    }
}
