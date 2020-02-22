using UnityEngine;

[CreateAssetMenu(menuName= "Inventory/Slot Data")]
public class SlotData : ScriptableObject
{
    [SerializeField]private InventoryItemData inventoryItemData;

    public void PlaceInsideThis(InventoryItemData item)
    {
        this.inventoryItemData = item;
    }

    public bool IsOccupied() => inventoryItemData.metadata != null;

    public InventoryItemData GetItemData() => inventoryItemData;
}