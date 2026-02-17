using Godot;
using System;
using System.ComponentModel;


public partial class CatalogEntity
{
    public enum ECat
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

    public string Name;
    public string DispName;
    public PackedScene Scene;
    public Texture2D Thumbnail;
    public ECat Cat;
    public EPlacementMode PlacementMode;
}
