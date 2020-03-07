using UnityEngine;

public static class InventoryDisplayHelper 
{
    private static Canvas carryCanvas;

    private static ItemCarry currentItemCarry;
    private static ToolTip   currentToolTip;
    
    private static GameObject carryPrefab;
    private static GameObject toolTipPrefab;

    private static GameObject canvasObj;

    private static bool tooltipAvailable;

    public static void Initiate()
    {
        canvasObj = new GameObject("ItemCarryCanvas");
        carryCanvas = canvasObj.AddComponent<Canvas>();
        carryCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        carryCanvas.sortingOrder = 100;

        carryPrefab = Utils.GetItemCarryPrefab();
        toolTipPrefab = Utils.GetTooltipPrefab();
        tooltipAvailable = true;
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

    public static bool TooltipIsAvailable()
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

    public static void ClearItemCarry()
    {
        MonoBehaviour.Destroy(currentItemCarry.gameObject);
        tooltipAvailable = true;
    }
}
