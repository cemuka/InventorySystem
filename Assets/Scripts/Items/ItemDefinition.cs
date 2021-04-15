using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemDefinition
{
    public string itemName;
    
    [SerializeField]private string _id;
    public  string  Id { get {   return _id; }   }

    public ItemType itemType;
    public int level;
    public Sprite icon;
    public string description;
    public int price;

    public void CreateNew()
    {
        _id = IdFactory.CreateDefinitionId();
    }
}
