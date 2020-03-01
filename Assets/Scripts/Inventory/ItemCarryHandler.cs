using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class ItemCarryHandler 
{
    private static Canvas carryCanvas;

    private static ItemCarry currentItemCarry;
    private static ToolTip   currentToolTip;
    
    private static GameObject carryPrefab;
    private static GameObject toolTipPrefab;

    private static GameObject canvasObj;

    private static bool tooltipAvailable = true;

    public static void Initiate()
    {
        canvasObj = new GameObject("ItemCarryCanvas");
        carryCanvas = canvasObj.AddComponent<Canvas>();
        carryCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        carryCanvas.sortingOrder = 100;

        carryPrefab = Resources.Load<GameObject>("Prefabs/CarryItem") as GameObject;
        toolTipPrefab = Resources.Load<GameObject>("Prefabs/ToolTipContainer") as GameObject;
    }

    public static void DestroyCarrierCanvas()
    {
        MonoBehaviour.Destroy(canvasObj);
    }

    public static ItemCarry GetCurrent()
    {
        return currentItemCarry;
    }


    public static GameObject CreateCarryItem(InventorySlot parentSlot, InventoryItemData data)
    {
        //Debug.Log(itemId);
        var itemCarryGO = MonoBehaviour.Instantiate(carryPrefab, carryCanvas.transform);

        itemCarryGO.transform.position = Input.mousePosition;
        
        currentItemCarry = itemCarryGO.GetComponent<ItemCarry>();

        currentItemCarry.Init(data, parentSlot);

        tooltipAvailable = false;
        return itemCarryGO;
    }

    public static bool isTooltipAvailable()
    {
        return tooltipAvailable;
    }

    public static ToolTip CreateToolTip()
    {
        var toolTopGO = MonoBehaviour.Instantiate(toolTipPrefab, carryCanvas.transform);
        currentToolTip = toolTopGO.GetComponent<ToolTip>();
        tooltipAvailable = false;
        return currentToolTip; 
    }

    public static void ClearTooltip()
    {
        if (currentToolTip != null)
        {
            MonoBehaviour.Destroy(currentToolTip.gameObject);
            currentToolTip = null;
            tooltipAvailable = true;
        }
    }

    public static void Clear()
    {
        MonoBehaviour.Destroy(currentItemCarry.gameObject);
        tooltipAvailable = true;
    }
}
