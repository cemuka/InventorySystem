using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Action<PointerEventData, bool> _onHover;
    
    public void InitForHover(Action<PointerEventData, bool> onHover)
    {
        _onHover = onHover;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _onHover?.Invoke((eventData), true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _onHover?.Invoke((eventData), false);
    }
}