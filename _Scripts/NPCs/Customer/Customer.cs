using Godot;
using System;

public partial class Customer : NPC
{
    // Enums
    public enum State
    {
        WalkingToEntrance,
        Browsing,
        WalkingToShelf,
        BrowsingShelf,
        WalkingToCheckout,
        WalkingToExit,
        Leaving
    }
    // Private Variables
    private State CurrentState;

    
}
