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
        Signals.Register<OnItemPointerEnterSignal>();
        Signals.Register<OnItemPointerExitSignal>();
        Signals.Register<OnItemPointerClickSignal>();

        Signals.Register<OnItemBeginDragSignal>();
        Signals.Register<OnItemDragSignal>();
        Signals.Register<OnItemEndDragSignal>();
     
        Signals.Register<OnShowTooltipSignal>();
        Signals.Register<OnHideTooltipSignal>();

        _resources = Resources.Load<GameResources>("GameResources");
        inventorySystem.Init(_resources);
        tooltipSystem.Init(_resources);

        Application.targetFrameRate = 30;
    }

}

