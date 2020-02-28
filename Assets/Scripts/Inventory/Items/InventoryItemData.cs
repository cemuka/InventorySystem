
using System;
using System.Text;

[System.Serializable]
public class InventoryItemData  
{
    public int upgradeState;
    public int amount;
    public ItemData metadata;

    public string GetToolTipContent()
    {
        var contentBuilder = new StringBuilder();

        contentBuilder
            .Append("name: " + metadata.itemName).Append("\n")
            .Append("type: " + metadata.type.ToString()).Append("\n\n")
            .Append("power: " + metadata.power).Append("\n")
            .Append("description: " + metadata.description).Append("\n\n")
            .Append("price: " + metadata.price).Append("\n");
            

        return contentBuilder.ToString();
    }
}