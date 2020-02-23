using UnityEngine;
using UnityEngine.UI;

public class ItemCarry : MonoBehaviour
{
    public InventoryItemData data;
    public InventorySlot parentSlot;
    public Image icon;

    public void Init(InventoryItemData data, InventorySlot parentSlot)
    {
        this.data = data;
        this.parentSlot = parentSlot;
        icon.sprite = data.metadata.icon;
        this.transform.localScale = Vector3.one * 1.5f;
    }
}