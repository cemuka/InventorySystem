using System;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public TMP_Text tipText;

    private bool _activated = false;

    public void Show(string text)
    {
        this.gameObject.transform.position = Input.mousePosition;
        tipText.text = text;
        this.gameObject.SetActive(true);
        _activated = true;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        _activated = false;
    }

    private void Update()
    {
        if (_activated)
        {
            this.gameObject.transform.position = Input.mousePosition;
        }
    }
}