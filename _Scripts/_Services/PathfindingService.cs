using Godot;
using System;

public partial class PathfindingService : Node
{
    // instance
    public static PathfindingService I {get; set;}

    // export vars
    [Export] NavigationRegion3D _navRegion;
    [Export] Node3D _shelvesRoot;

    // Vars
    private bool _subscribed = false;

    // ========== Godot-lifecycle ==========

    public override void _Ready()
    {
        I = this;
    }

    // ========== public methods ==========

    // Nav mesh baking
    public static void Init()
    {
        GD.Print("==== PathfindingService init");
        ToolService.OnUpdate += (tool) =>
        {
            if (tool == ToolService.ETools.BakePathfinding)
            {
                I?.StartBakingRegion();
            }
        };
    }

    public void StartBakingRegion()
    {
        if (_navRegion.IsBaking()) return;
        
        GD.Print("PathfindingService: Started baking.");
        _navRegion.BakeNavigationMesh(true);
        
        if (_subscribed == false)
        {
            _navRegion.BakeFinished += FinishBake;
            _subscribed = true;
        }
        
        NavigationServer3D.SetDebugEnabled(true);
    }

    // Helper funcs for NPCs
    
    // ========== Private methods ==========
    private void FinishBake()
    {
        _navRegion.BakeFinished -= FinishBake;
        _subscribed = false;

        var navMesh = _navRegion.NavigationMesh;

        if (navMesh is null) return;

        GD.Print("PathfindingService: Finished baking.");
    }

}
