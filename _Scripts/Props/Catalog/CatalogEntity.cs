using Godot;
using System;
using System.ComponentModel;


public partial class CatalogEntity
{
    // === Enums ===
    public enum EGroup
    {
        // Retail infrastructure:
        Amenities,  // bins, seating, atm
        Logistics,  // depot, storage
        Services,   // checkouts
        Shelves,

        // Design and architecture:
        Decorations,
        Floors,
        Lighting,
        Primitives,
        Signage,
        Walls,
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
