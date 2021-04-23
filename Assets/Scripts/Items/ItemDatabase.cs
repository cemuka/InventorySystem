using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Blacksmith/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemDefinition> definitions;

    public ItemDefinition FetchItem(string id)
    {
        return definitions.Find(i => i.DefId == id);
    }

    public void SortByPrice()
    {
        definitions = definitions.OrderBy(item => item.price).ToList();
    }
}