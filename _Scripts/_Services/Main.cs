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
		RunShelfTests();
        GD.Print("\n==== Main: Scene tree ready. Hello, world!");
        EconomyService.I.AddCash(100000f);
    }

    private void RunShelfTests()
    {
        GD.Print("::== Main: Starting shelf test");
        var newShelf = FactoryService.I?.TrySpawningUidAndGetNode("uid://c3rwyrjlanyer");
        if (newShelf is not Shelf shelf)
        {
            GD.PrintErr("[FAIL] Could not spawn shelf.");
            return;
        }
        else
        {
            GD.Print("[OK] Added a new shelf");
        }

        var product = ProductConfig.FindByUID("uid://cxcijsn8eyi38");
		// Stock a product for fun
        GD.Print("Stock a product for fun");
        shelf.SetProductStock(product, true);
        shelf.AddProduct(product, 10);

        GD.Print("==>> Shelf tests complete!");
    }

    
}
