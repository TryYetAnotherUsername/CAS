using Godot;
using System;

/// <summary>
/// "Main"ly for lazy testing. Attatched to root node so the entire scene tree is generated before this runs.
/// </summary>

public partial class Main : Node
{
    public override void _Ready()
    {
        PathfindingService.Init();
        ProductConfig.Init();

        GD.Print("\n==== Main: Scene tree ready. Hello, world!");
        EconomyService.I.AddCash(100000f);
    }  
}
