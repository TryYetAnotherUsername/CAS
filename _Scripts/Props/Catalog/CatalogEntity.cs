using Godot;
using System;
using System.ComponentModel;


public partial class CatalogEntity
{
    // === Enums ===
    public enum EGroup
    {
        Shelves,
        Decorations,
        Floors,
        Misc,
        Walls,
        Primitives,
    }
    public enum EPlacementMode
	{
    	Free,
    	GridLocked
	}

    // === Fields ===
    
    public string UID;
    public string DispName;
    public EGroup CatalogGroup;
    public EPlacementMode AllowedPlacementMode;
}
