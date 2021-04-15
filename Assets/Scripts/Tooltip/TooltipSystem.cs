using System;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public Tooltip tooltip;

    private GameResources _resources;

    public void Init(GameResources resources)
    {
        _resources = resources;
        tooltip.Hide();

        EventSignals.OnItemTooltipShow += OnTooltipShow;
        EventSignals.OnItemTooltipHide += OnTooltipHide;
        EventSignals.OnItemOnDragEvent += OnItemDrag;
    }

    private void OnItemDrag(EventType type, int id)
    {
        tooltip.Hide();
    }

    private void OnTooltipShow(string defId)
    {
        var item = _resources.itemDatabase.FetchItem(defId);
        var recipe = new TooltipBuilder().AddLine(item.itemName.WithSize(20).AsColor("yellow").AsBold())
                                        .AddLine(item.itemType.ToString())
                                        .AddLine("Price: " + item.price.ToString().AsColor("orange"))
                                        .Build();
        tooltip.Show(recipe.Extract());
    }

    private void OnTooltipHide()
    {
        tooltip.Hide();
    }
}