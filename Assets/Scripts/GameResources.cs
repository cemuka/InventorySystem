using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameResources", menuName = "Blacksmith/GameResources")]
public class GameResources : ScriptableObject
{
    public ItemDatabase itemDatabase;
    public int playerGold;
    public InventoryConfigurations inventoryConfigs;


    public int GetMaxStackByType(ItemType type)
    {
        switch (type)
        {
            case ItemType.BasicMaterial:        return inventoryConfigs.maxStackBasicMaterial;
            case ItemType.PolishingMaterial:    return inventoryConfigs.maxStackPolishMaterial;
            
            default: return 1;
        }
    }
}

[System.Serializable]
public class InventoryConfigurations
{
    public int maxStackBasicMaterial;
    public int maxStackPolishMaterial;
}