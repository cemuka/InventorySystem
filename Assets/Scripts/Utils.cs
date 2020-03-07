using UnityEngine;

public static class Utils
{
    private static GameResources gameResources;

    public static void Initiate()
    {
        gameResources = Resources.Load<GameResources>("InventoryConfiguration");
        gameResources.Init();
    }

    public static GameResources GetResources()        => gameResources;
    public static GameObject GetInventoryItemPrefab() => Resources.Load<GameObject>("Prefabs/InventoryItem") as GameObject;
    public static GameObject GetInventorySlotPrefab() => Resources.Load<GameObject>("Prefabs/InventorySlot") as GameObject;
    public static GameObject GetVendorSlotPrefab()    => Resources.Load<GameObject>("Prefabs/VendorSlot")    as GameObject;
    public static GameObject GetItemCarryPrefab()     => Resources.Load<GameObject>("Prefabs/CarryItem")     as GameObject;
    public static GameObject GetTooltipPrefab()       => Resources.Load<GameObject>("Prefabs/ToolTip")       as GameObject;

    public static ItemData GetItemDataById(int id)
    {
        return gameResources.GetItemDataById(id);
    }
}