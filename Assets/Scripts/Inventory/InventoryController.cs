using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour 
{
    public Transform inventoryParent;
    public Transform infoPanelParent;

    public Text playerGold;

    private Dictionary<int, InventorySlot> inventorySlots = new Dictionary<int, InventorySlot>();
    private List<SlotData> slotDataList = new List<SlotData>();

    private GameResources gameResources;

    public void Init() 
    {
        slotDataList = Utils.GetInventory();
        gameResources = Utils.GetResources();

        BuildInventory();
        UpdateGoldText(gameResources.GetPlayerGold().ToString());

        InventoryEventHandler.PlayerBoughtItem += PurchaseItem;
        InventoryEventHandler.PlayerSoldItem += SellItem;
    }

    private void UpdateGoldText(string txt)
    {
        playerGold.text = txt + " gold";
    }

    private void PurchaseItem(int price)
    {
        gameResources.InventoryTransaction(-price);
        UpdateGoldText(gameResources.GetPlayerGold().ToString());
    }

    private void SellItem(int price)
    {
        gameResources.InventoryTransaction(price);
        UpdateGoldText(gameResources.GetPlayerGold().ToString());
    }

    private void OnDisable() 
    {
        ItemCarryHandler.DestroyCarrierCanvas();

        SaveInventoryState();
    }

    private void BuildInventory()
    {
        Debug.Log("inventory build started");
        var slotPrefab = Resources.Load<GameObject>("Prefabs/InventorySlot");

        for (int i = 0; i < slotDataList.Count; i++)
        {
            var slotGO = Instantiate(slotPrefab, inventoryParent) as GameObject;
            var slot = slotGO.GetComponent<InventorySlot>();
            slot.InitSlot(i);
            inventorySlots.Add(i, slot);
        }

        for (int i = 0; i < slotDataList.Count; i++)
        {
            if (slotDataList[i].IsOccupied())
            {
                inventorySlots[i].CreateItem(slotDataList[i].GetItemData());
            }
        }
    }

    public void SaveInventoryState()
    {
        foreach (var slot in inventorySlots.Values)
        {
            if (slot.currentItem)
            {
                gameResources.UpdatePlayerInventory(slot.slotId, slot.currentItem.GetInventoryItemData());
            }
            else
            {
                gameResources.UpdatePlayerInventory(slot.slotId, null);
            }
        }
        Debug.Log("Inventory state saved.");
    }

    //helper methods
    private InventorySlot GetFirstEmptySlot()
    {
        foreach (var slot in inventorySlots.Values)
        {
            if (!slot.currentItem)
            {
                return slot;
            }
        }

        return null;
    }
}