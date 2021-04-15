using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class EventSignals 
{
    public static event Action<int, string> OnItemSoldToVendor;
    public static event Action<int, string> OnItemSoldToPlayer;

    public static event Action<EventType,int> OnItemOnBeginDragEvent;
    public static event Action<EventType,int> OnItemOnDragEvent;
    public static event Action<EventType,int> OnItemOnEndDragEvent;
    public static event Action<EventType, int, PointerEventData> OnItemDropEvent;
    

    public static event Action<string> OnItemTooltipShow;
    public static event Action         OnItemTooltipHide;

    

    public static void InvokeItemSoldToVendor(int itemId, string defId)
    {
        OnItemSoldToVendor?.Invoke(itemId, defId);
    }
    
    public static void InvokeItemSoldToPlayer(int itemId, string defId)
    {
        OnItemSoldToPlayer?.Invoke(itemId, defId);
    }

    //  invoke player inventory 
    public static void InvokeItemOnBeginDragEvent(EventType type, int itemId)
    {
        OnItemOnBeginDragEvent?.Invoke(type, itemId);
    }
    
    public static void InvokeItemOnDragEvent(EventType type, int itemId)
    {
        OnItemOnDragEvent?.Invoke(type, itemId);
    }

    public static void InvokeItemOnEndDragEvent(EventType type, int itemId)
    {
        OnItemOnEndDragEvent?.Invoke(type, itemId);
    }
    
    public static void InvokeItemDropEvent(EventType type, int slotIndex, PointerEventData data)
    {
        OnItemDropEvent?.Invoke(type, slotIndex, data);
    }

    //  tooltip
    public static void InvokeOnItemTooltipShowEvent(string definitionId)
    {
        OnItemTooltipShow?.Invoke(definitionId);
    }
    
    public static void InvokeOnItemTooltipHideEvent()
    {
        OnItemTooltipHide?.Invoke();
    }

}