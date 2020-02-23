﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class ItemCarryHandler 
{
    private static Canvas carryCanvas;

    private static ItemCarry currentItemCarry;
    private static GameObject carryPrefab;

    private static GameObject canvasObj;

    public static void Initiate()
    {
        canvasObj = new GameObject("ItemCarryCanvas");
        carryCanvas = canvasObj.AddComponent<Canvas>();
        carryCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        carryCanvas.sortingOrder = 100;

        carryPrefab = Resources.Load<GameObject>("Prefabs/CarryItem") as GameObject;
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

        return itemCarryGO;
    }

    public static void Clear()
    {
        MonoBehaviour.Destroy(currentItemCarry.gameObject);
    }
}
