using Godot;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static ItemConfig;

public static class BuildModeConfig
{
    public static readonly Dictionary<string, ItemConfig> BuildModeItems = new()
    {
        // THE SCRIPT WILL AUTOMATICALLY FILL IN THE REMAINING VALUES.
        // THE VALUES ARE: Scene, Thumbnail

        // ========== WALLS ==========
        { "single_wall",         new ItemConfig { DispName = "Wall",              Category = ECat.Walls }},
        { "double_wall",         new ItemConfig { DispName = "Wall (Double)",     Category = ECat.Walls }},
        { "double_wall_window",  new ItemConfig { DispName = "Wall (Windowed)",   Category = ECat.Walls }},
    };

    private static PackedScene FindSceneOrNull(string name)
    {
        var dir = DirAccess.Open("res://Scenes/3D/Placeables_auto/");
        foreach (var file in dir.GetFiles())
        {
            if (file.Replace(".tscn", "") == name)
                return GD.Load<PackedScene>($"res://Scenes/3D/Placeables_auto/{file}");
        }
        return null;
    }

    public static void Init()
    {
        GD.Print("BuildModeConfig: starting init");

        foreach (var kvp in BuildModeItems)
        {
            GD.Print(kvp); // remove

            // try find a scene for it
            var foundScene = FindSceneOrNull(kvp.Key);
            if (foundScene is not null)
            {
                kvp.Value.Scene = foundScene;
            }
            else
            {
                GD.PrintErr($"BuildModeConfig: Could not find matching scene for name <{kvp.Value.DispName}>, the result is null.");
            }

            
        }

    }
}