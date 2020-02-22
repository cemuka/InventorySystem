using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{

    public InventoryController inventory;

    void Start()
    {
        Utils.Initiate();
        inventory.Init();
    }
}
