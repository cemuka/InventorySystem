public static class TooltipStringExtensions
{
    public static string AsBold(this string value)
    {
        return "<b>" + value + "</b>";
    }

    public static string AsColor(this string value, string color)
    {
        return "<color=\""+ color +"\">" + value + "</color>";
    }
    
    public static string WithSize(this string value, int size)
    {
        return "<size=\""+ size +"\">" + value + "</size>";
    }
}