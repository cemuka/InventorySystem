using System;

public static class InventoryEventHandler
{
    public static event Action<int> PlayerSoldItem; 
    public static event Action<int> PlayerBoughtItem;

    public static void InvokeSellEvent(int price)
    {
        PlayerSoldItem.Invoke(price);
    }

    public static void InvokeBuyEvent(int price)
    {
        PlayerBoughtItem.Invoke(price);
    }
}