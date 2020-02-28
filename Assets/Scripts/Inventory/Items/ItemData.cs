using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData", order = 0)]
public class ItemData : ScriptableObject 
{
    public int id;
    public string itemName;
    public int price;
    public Sprite icon;
    public bool stackable;
    public ItemType type;

    [Multiline(3)]
    public string description;
    public int power;
}