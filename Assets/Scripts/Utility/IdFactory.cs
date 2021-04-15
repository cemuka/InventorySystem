using System;
using System.Text.RegularExpressions;
using UnityEngine;

public static class IdFactory 
{
    public static int       MaxId = 0;
    public static int       CreateInstanceId()    => MaxId++;
    public static string    CreateDefinitionId()   => Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
}