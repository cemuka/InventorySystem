using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData", order = 0)]
public class ItemData : ScriptableObject 
{
    public int id;
    public string itemName;
    public Sprite icon;
    public bool stackable;
    public ItemType type;
}