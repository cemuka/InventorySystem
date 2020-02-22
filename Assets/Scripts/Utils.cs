using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utils
{
    private static GameResources gameResources;

    public static void Initiate()
    {
        gameResources = Resources.Load<GameResources>("InventoryConfiguration");
        gameResources.Init();
    }

    public static GameResources GetResources()
    {
        return gameResources;
    }

    public static ItemData GetItemDataById(int id)
    {
        return gameResources.GetItemDataById(id);
    }

    public static List<SlotData> GetInventory()
    {
        return gameResources.GetInventorySlots();
    }
}