using Godot;
using System.Collections.Generic;

public static class BuildModeConfig
{
    public static readonly List<BuildModeItem> Items = new()
    {
        new BuildModeItem()
        { 
            Name = "basic_wall",
            DisplayName = "Basic Wall",
            Scene = 
            Thumbnail = null
        }
    };
}