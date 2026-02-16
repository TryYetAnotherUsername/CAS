using Godot;
using System;
using System.ComponentModel;


public partial class ItemConfig
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
    // enough? lol

    public string Name;
    public string DispName;
    public PackedScene Scene;
    public Texture2D Thumbnail;
    public ECat Category;
}
