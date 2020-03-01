using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public Text inventoryGold;
    public Text vendorGold;

    public void SetInventoryGold(string txt)
    {
        inventoryGold.text = txt;
    }

    public void SetVendorGold(string txt)
    {
        vendorGold.text = txt;
    }
}