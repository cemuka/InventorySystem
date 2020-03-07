
using System.Text;

[System.Serializable]
public class InventoryItemData  
{
    public int upgradeState;
    public int amount;
    public ItemData data;

    public string GetToolTipContent()
    {
        var contentBuilder = new StringBuilder();

        contentBuilder
            .Append("name: " + data.itemName).Append("\n")
            .Append("type: " + data.type.ToString()).Append("\n\n")
            .Append("power: " + data.power).Append("\n")
            .Append("description: " + data.description).Append("\n\n")
            .Append("price: " + data.price).Append("\n");
            

        return contentBuilder.ToString();
    }
}