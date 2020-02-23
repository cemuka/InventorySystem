using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VendorController : MonoBehaviour 
{
    public Transform vendorParent;

    private Dictionary<int, InventorySlot> vendorSlots = new Dictionary<int, InventorySlot>();
    private List<SlotData> slotDataList = new List<SlotData>();

    private GameResources gameResources;

    public void Init()
    {
        slotDataList = Utils.GetVendorInventory();
        gameResources = Utils.GetResources();

        BuildVendor();
    }

    private void BuildVendor()
    {
        var slotPrefab = Resources.Load<GameObject>("Prefabs/VendorSlot");

        for (int i = 0; i < slotDataList.Count; i++)
        {
            var slotGO = Instantiate(slotPrefab, vendorParent) as GameObject;
            var slot = slotGO.GetComponent<VendorSlot>();
            slot.InitSlot(i);
            vendorSlots.Add(i, slot);
        }

        for (int i = 0; i < slotDataList.Count; i++)
        {
            if (slotDataList[i].IsOccupied())
            {
                vendorSlots[i].CreateItem(slotDataList[i].GetItemData());
            }
        }
    }
}