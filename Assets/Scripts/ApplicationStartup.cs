using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationStartup : MonoBehaviour
{
    public InventorySystem inventorySystem;
    public TooltipSystem tooltipSystem;
    private GameResources _resources;

    private void Start()
    {
        _resources = Resources.Load<GameResources>("GameResources");
        inventorySystem.Init(_resources);
        tooltipSystem.Init(_resources);

        Application.targetFrameRate = 30;
    }

}

