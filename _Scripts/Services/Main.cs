using Godot;
using System;

public partial class Main : Node
{
    public override void _Ready()
    {
        ProductConfig.Init();
		RunShelfTests();
        WindowService.I.NewWindow(WindowService.EWindowContent.Properties);
        GD.Print("\n==== Main: Scene tree ready. Hello, world!");
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

        var product = ProductConfig.FindByUID("testUid");
		// Stock a product for fun
        GD.Print("Stock a product for fun");
        shelf.ChangeProductStockStatus(product, true);
        shelf.AddProduct(product, 10);

        GD.Print("==>> Shelf tests complete!");
    }
}
