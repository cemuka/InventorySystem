using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{

    public InventoryController inventory;
    public VendorController vendor;

    void Start()
    {
        Utils.Initiate();
        InventoryDisplayHelper.Initiate();

        var resources = Utils.GetResources();
        var save = resources.GetSaveResource();
        var inventorySettings = new InventorySettings();
        inventorySettings.slotAmount = resources.GetInventorySlotAmount();
        inventorySettings.slots = save.slots;

        inventory.Init(inventorySettings);

        var vendorSettings = new InventorySettings();
        vendorSettings.slotAmount = resources.GetVendorInventoryAmount();

        vendorSettings.slots = new List<SlotSave>();
        vendorSettings.slots.Add( new SlotSave(){
            slotIndex = 0,
            item = new ItemSave(){itemId = 0}
        });

        vendorSettings.slots.Add( new SlotSave(){
            slotIndex = 1,
            item = new ItemSave(){itemId = 3}
        });

        vendorSettings.slots.Add( new SlotSave(){
            slotIndex = 2,
            item = new ItemSave(){itemId = 2}
        });

        vendorSettings.slots.Add( new SlotSave(){
            slotIndex = 3,
            item = new ItemSave(){itemId = 4}
        });



        vendor.Init(vendorSettings);
    }
}
