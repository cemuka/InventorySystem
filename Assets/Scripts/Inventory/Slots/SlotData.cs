using UnityEngine;

[CreateAssetMenu(menuName= "Inventory/Slot Data")]
public class SlotData : ScriptableObject
{
    public bool occupied;
    public InventoryItemData data;

    public void PlaceInsideThis(InventoryItemData item)
    {
        this.data = item;
        occupied = true;
    }
}