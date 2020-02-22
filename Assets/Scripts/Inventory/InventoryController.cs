using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour 
{
    public Transform inventoryParent;
    public Transform infoPanelParent;

    private Dictionary<int, Slot> inventorySlots = new Dictionary<int, Slot>();
    private List<SlotData> slotDataList = new List<SlotData>();

    private GameResources gameResources;

    public void Init() 
    {
        ItemCarryHandler.Initiate();

        slotDataList = Utils.GetInventory();
        gameResources = Utils.GetResources();

        BuildInventory();
        //BuildUpgradePanel();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(GetFirstEmptySlot().slotId);
        }
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
            var slot = slotGO.GetComponent<Slot>();
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
    private Slot GetFirstEmptySlot()
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