using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public RectTransform parentRect;
    public RectTransform displayRect;
    public LayoutGroup layout;
    public TMP_Text tipText;

    private bool _activated = false;
    private Vector3 _lastTooltipPosition;
    private Vector2 _tooltipPosition;


    public void Show(string text)
    {
        _activated = true;
        this.gameObject.transform.position = Input.mousePosition;
        tipText.text = text;
        CheckBounds();
        
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        _activated = false;
    }
    
    private void CheckBounds()
    {
        Vector3[] corners = new Vector3[4];
        parentRect.GetLocalCorners(corners);
        Vector3 screenSize = corners[corners.Length - 1];

        ResetContentPosition(displayRect);

        layout.CalculateLayoutInputHorizontal();
        layout.CalculateLayoutInputVertical();
        LayoutRebuilder.ForceRebuildLayoutImmediate(displayRect);


        float width  = displayRect.sizeDelta.x;
        float height = displayRect.sizeDelta.y;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect,
                                                                Input.mousePosition,
                                                                null,
                                                                out _tooltipPosition);
        
        float verticalDiff = Mathf.Abs(screenSize.y - _tooltipPosition.y);
        if (height > verticalDiff)
        {
            displayRect.pivot           = new Vector2(displayRect.pivot.x, 0);
            displayRect.localPosition   = Vector3.zero;
        }

        float horizontalDiff = Mathf.Abs(screenSize.x - _tooltipPosition.x);
        if (width > horizontalDiff)
        {
            displayRect.pivot           = new Vector2(1, displayRect.pivot.y);
            displayRect.localPosition   = Vector3.zero;
        }
    }
    
    private void ResetContentPosition(RectTransform rt)
    {
        rt.offsetMax    = new Vector2(0,1);
        rt.offsetMin    = new Vector2(0,1);
        rt.pivot        = new Vector2(0,1);
        rt.localPosition= new Vector3(10, 0, 0);
    }
    
    private void Update()
    {
        if (_activated)
        {
            this.transform.position = Input.mousePosition;
            if (this.transform.position != _lastTooltipPosition)
            {
                _lastTooltipPosition = this.transform.position;
                CheckBounds();
            }
        }
    }
}