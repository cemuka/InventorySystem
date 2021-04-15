using System;
using System.Collections.Generic;

public class TooltipBuilder
{
    private TooltipRecipe _tooltip;
    public TooltipBuilder()
    {
        _tooltip = new TooltipRecipe();
    }
    public TooltipBuilder AddLine(string line)
    {
        _tooltip.lines.Add(line);
        return this;
    }
    public TooltipBuilder AddSpace()
    {
        _tooltip.lines.Add("\n");
        return this;
    }
    public TooltipBuilder AddSectionListing(string header, string[] items)
    {
        if (items.Length == 0)
        {
            AddLine(header.AsBold() + "-");
        }
        else
        {
            AddLine(header.AsBold());

            for (int i = 0; i < items.Length; i++)
            {
                AddLine("" + items[i]);
            }
        }
        return this;
    }

    public TooltipBuilder AddIcon(string name)
    {
        AddLine("<sprite=\"TooltipIcons\" name=\""+ name +"\">");
        return this;
    }
    
    public TooltipBuilder When(bool condition)
    {
        if (condition == false)
        {
            _tooltip.lines.RemoveAt(_tooltip.lines.Count-1);
        }
        return this;
    }
    
    public TooltipRecipe Build()
    {
        return _tooltip;
    }
}

public class TooltipRecipe
{
    public List<string> lines;

    public TooltipRecipe()
    {
        lines = new List<string>();
    }

    public string Extract()
    {
        return String.Join("\n", lines);
    }
}