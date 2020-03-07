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

    private GameResources _gameResources;
    private InventorySettings _settings;

    public void Init(InventorySettings settings) 
    {
        _settings = settings;
        _gameResources = Utils.GetResources();

        BuildInventory();
        UpdateGoldText(_gameResources.GetPlayerGold().ToString());

        InventoryEventHandler.PlayerBoughtItem += PurchaseItem;
        InventoryEventHandler.PlayerSoldItem += SellItem;
    }

    private void BuildInventory()
    {
        Debug.Log("inventory build started");
        var slotPrefab = Utils.GetInventorySlotPrefab();

        for (int i = 0; i < _settings.slotAmount; i++)
        {
            var slotGO = Instantiate(slotPrefab, inventoryParent) as GameObject;
            var slot = slotGO.GetComponent<InventorySlot>();
            slot.InitSlot(i);
            inventorySlots.Add(i, slot);
        }

        foreach (var savedSlot in _settings.slots)
        {
            var newItem = new InventoryItemData();

            newItem.data = Utils.GetItemDataById(savedSlot.item.itemId);
            if (newItem.data.stackable)
            {
                newItem.amount = savedSlot.item.amount;
            }
            inventorySlots[savedSlot.slotIndex].CreateItem(newItem);
        }
    }

    private void UpdateGoldText(string txt)
    {
        playerGold.text = txt + " gold";
    }

    private void PurchaseItem(int price)
    {
        _gameResources.InventoryTransaction(-price);
        UpdateGoldText(_gameResources.GetPlayerGold().ToString());
    }

    private void SellItem(int price)
    {
        _gameResources.InventoryTransaction(price);
        UpdateGoldText(_gameResources.GetPlayerGold().ToString());
    }

    private void OnDisable() 
    {
        InventoryDisplayHelper.DestroyCarrierCanvas();

        SaveInventoryState();
    }


    public void SaveInventoryState()
    {
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

public class InventorySettings
{
    public int slotAmount;
    public List<SlotSave> slots;
}