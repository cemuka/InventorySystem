using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{

    public InventoryController inventory;
    public VendorController vendor;

    void Start()
    {
        Utils.Initiate();
        ItemCarryHandler.Initiate();

        inventory.Init();
        vendor.Init();
    }
}
