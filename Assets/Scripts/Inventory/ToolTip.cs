using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public Text tip;
    public Transform pivot;

    public void SetText(string txt)
    {
        tip.text = txt; 
    }
}