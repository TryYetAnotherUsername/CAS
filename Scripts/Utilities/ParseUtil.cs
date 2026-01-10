using Godot;
using System;

public static class ParseUtil
{
    public static Vector3 ToVector3(string input)
    {
        Vector3 result = new Vector3();
        string[] inputSplit = input.Split(",");

        if (result.Length() > 3)
        {
            ConsoleService.PrintErr($"ParseUtil: Failed to convert string {input} to Vector3.");
            result.X = 12345;
            result.Y = 12345;
            result.Z = 12345;
            return result;
        }

        result.X = inputSplit[0].ToFloat();
        result.Y = inputSplit[1].ToFloat();
        result.Z = inputSplit[2].ToFloat();

        return result;
    }
}