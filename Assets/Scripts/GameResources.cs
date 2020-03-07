using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName="GameResources")]
public class GameResources : ScriptableObject
{
    [Header("Configuration")]
    [SerializeField] private int playerGold;
    [SerializeField] private int playerInventorySlotAmount;
    [SerializeField] private int vendorInventorySlotAmount;

    private InventorySave saveFile;

    //set from editor
    [Header("Item Database")]
    [SerializeField]private List<ItemData> itemResources = new List<ItemData>();


    public void Init()
    {
        Debug.Log("resources initialized.");
        saveFile = JsonUtility.FromJson<InventorySave>(Resources.Load<TextAsset>("Save").text);
    }

    //getters setters
    public int GetPlayerGold() => playerGold;
    public int GetInventorySlotAmount() => playerInventorySlotAmount;
    public int GetVendorInventoryAmount() => vendorInventorySlotAmount;
    public InventorySave GetSaveResource() => saveFile;

    public ItemData GetItemDataById(int itemId)
    {
        return itemResources.Where( item => item.id == itemId).FirstOrDefault();
    }

    public void InventoryTransaction(int price)
    {
        playerGold += price;
    }
}

[System.Serializable]
public class InventorySave
{
    public List<SlotSave> slots;
}

[Serializable]
public class SlotSave
{
    public int slotIndex;
    public ItemSave item;
}

[Serializable]
public class ItemSave
{
    public int itemId;
    public int amount;
}
