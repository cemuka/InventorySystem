using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipSystem : MonoBehaviour
{
    public Tooltip tooltip;

    private GameResources _resources;

    public void Init(GameResources resources)
    {
        _resources = resources;
        tooltip.Hide();

        Signals.Get<OnShowTooltipSignal>().AddListener(Show);
        Signals.Get<OnHideTooltipSignal>().AddListener(Hide);
    }

    private void Show(string defId)
    {
        var item = _resources.itemDatabase.FetchItem(defId);
        var recipe = new TooltipBuilder().AddLine(item.itemName.WithSize(20).AsColor("yellow").AsBold())
                                        .AddLine(item.itemType.ToString())
                                        .AddLine("Price: " + item.price.ToString().AsColor("orange"))
                                        .Build();
        tooltip.Show(recipe.Extract());
    }

    private void Hide()
    {
        tooltip.Hide();
    }
}