using UnityEngine;
using System.Collections.Generic;

public class VendorController : MonoBehaviour 
{
    public Transform vendorParent;

    private Dictionary<int, InventorySlot> vendorSlots = new Dictionary<int, InventorySlot>();

    private GameResources gameResources;
    private InventorySettings _settings;

    public void Init(InventorySettings settings)
    {
        gameResources = Utils.GetResources();
        _settings = settings;
        _settings.slotAmount = gameResources.GetVendorInventoryAmount();

        BuildVendor();
    }

    private void BuildVendor()
    {
        var slotPrefab = Utils.GetVendorSlotPrefab();

        for (int i = 0; i < _settings.slotAmount; i++)
        {
            var slotGO = Instantiate(slotPrefab, vendorParent) as GameObject;
            var slot = slotGO.GetComponent<VendorSlot>();
            slot.InitSlot(i);
            vendorSlots.Add(i, slot);
        }

        foreach (var savedSlot in _settings.slots)
        {
            var newItem = new InventoryItemData();

            newItem.data = Utils.GetItemDataById(savedSlot.item.itemId);
            if (newItem.data.stackable)
            {
                newItem.amount = savedSlot.item.amount;
            }
            vendorSlots[savedSlot.slotIndex].CreateItem(newItem);
        }
    }
}