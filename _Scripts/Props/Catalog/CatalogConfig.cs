using Godot;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static CatalogEntity;

public static class CatalogConfig
{
    public static readonly Dictionary<string, CatalogEntity> BuildModeItems = new()
    {
        // THE SCRIPT WILL AUTOMATICALLY FILL IN THE REMAINING VALUES.
        // THE VALUES ARE: Scene, Thumbnail

        // ========== WALLS ==========
        { "single_wall",         new CatalogEntity { DispName = "Wall",              Cat = ECat.Walls }},
        { "double_wall",         new CatalogEntity { DispName = "Wall (Double)",     Cat = ECat.Walls }},
        { "double_wall_window",  new CatalogEntity { DispName = "Wall (Windowed)",   Cat = ECat.Walls }},
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
        GD.Print("::== BuildModeConfig: Starting init...");

        foreach (var kvp in BuildModeItems)
        {
            // try find a scene for it
            var foundScene = FindSceneOrNull(kvp.Key);
            if (foundScene is not null)
            {
                kvp.Value.Scene = foundScene;
                GD.Print($"BuildModeConfig: Scene {foundScene} matched to name <{kvp.Key}>.");
            }
            else
            {
                GD.PrintErr($"BuildModeConfig: Could not find matching scene for name <{kvp.Value.DispName}>, the result is null.");
            }
        }

        GD.Print("==>> BuildModeConfig: Init done!");

    }
}