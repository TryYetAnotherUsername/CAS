using System;
using System.Collections.Generic;
using Godot;

/// <summary>
/// Each shelf has fields for its stored products, exposes methods for adding/ subtracting products, and logic to render out the products on the shelf.
/// </summary>

// Issue #11 - unify checklists

public partial class Shelf : Prop
{
	/// <summary>
	/// Contains all things related to rendering products out onto shelves.
	/// </summary>
	private class ShelfVisuals
	{
		// vars
		private Node3D _productAreasRoot;
		private Shelf _shelf;
		private List<Area3D> _productAreas = new();

		// constructor
		public ShelfVisuals(Shelf theClass)
		{
			_productAreasRoot = theClass._productAreasRoot;
			_shelf = theClass;
			ScanAreas();
		}

		// private methods
		public void Refresh()
		{
			Clear();

			// Get the data needed from the shelf class
			if (_shelf.StockedProductsList is null || _shelf.StockedProductsList.Count == 0 || _shelf.StockedProductsList[0] is null || _shelf.StockedProductsList[0].Quantity == 0) return;
			string productUid = _shelf.StockedProductsList[0].Product.UID;
			int totalQuantity = _shelf.StockedProductsList[0].Quantity;
			PackedScene scene = GD.Load<PackedScene>(ResourceUid.GetIdPath(ResourceUid.TextToId(productUid)));

			// For the product
			var newNode = scene.Instantiate() as Node3D;
			if (newNode is null) return;
			var mesh = newNode.GetChild(0) as MeshInstance3D;
			if (mesh is null) return;
				
			var sizeOfProduct = mesh.GetAabb().Size;
			var productOffset = sizeOfProduct / 2;
			newNode.Free();

			// For each area
			foreach (Area3D area in _productAreas)
			{
				if (totalQuantity <= 0) return;

				// Get the shelf area object
				var targAreaCol = area.GetChild(0) as CollisionShape3D;
				if (targAreaCol is null) return;
				var box = targAreaCol.Shape as BoxShape3D;
				if (box is null) return;
				
				// Calculate how many products will fit in this area
				var fitHorizontal = (int) MathF.Floor(box.Size.Z / sizeOfProduct.Z);
				var fitDepth = (int) MathF.Floor(box.Size.X / sizeOfProduct.X);

				// go along the horizontal of the area
				for (int column = 0; column < fitHorizontal; column++)
				{
					if (totalQuantity <= 0) return;

					// place products along the depth of the area
					for (int row = 0; row < fitDepth; row++)
					{
						if (totalQuantity <= 0) return;

						var node = scene.Instantiate() as Node3D;
						if (node is null) continue;
						area.AddChild(node);

						var p = node.Position;
						p.X += sizeOfProduct.X * row + productOffset.X;
						p.Z -= sizeOfProduct.Z * column + productOffset.Z;
						node.Position = p;

						// decrease total quantity
						totalQuantity --;
					}
				}
			}
		}

		private void ScanAreas()
		{
			foreach (Node node in _productAreasRoot.GetChildren())
			{
				if (node is Area3D area)
				{
					_productAreas.Add(area);
				}
			}
		}

		private void Clear()
		{
			foreach (Area3D area in _productAreas)
			{
				foreach (Node node in area.GetChildren())
				{
					if (node is not CollisionShape3D)
					{
						node.Free();
					}
				}
			}
		}

	}

	[Export] private Node3D _productAreasRoot;
	private ShelfVisuals _Visuals;

    #region Godot methods
    public override void _Ready()
    {
        _Visuals = new ShelfVisuals(this);
		_Visuals.Refresh();
    }
	#endregion Godot methods

	#region Stuff

	/// <summary>
	/// Stores the quantity of each product.
	//  NTS- It dosen't need a class, but probably more scaleable, for expiry dates, batch quality, etc.
	/// </summary>
	
    public class StockEntry
    {
    	public ProductEntity Product {get; set;} = ProductConfig.Catalog[0];
        public int Quantity {get; set;} = 0;
    }

    public List<StockEntry> StockedProductsList {get; set;}= new();

	[Export] public Node3D NavTarget;

	#endregion Stuff

	#region Public methods

	public void SetProductStock(ProductEntity targProduct, bool status)
	{
		var entry = GetEntryFromStocked(targProduct);
		if (status == true) // want to stock
		{
			if (entry != null) // already stocked, do nothing.
			{
				GD.Print("A shelf: Tried to stock a product, but that product is already stocked on this shelf.");
				return;
			}
			else // not stocked yet, stock.
			{
				StockedProductsList.Add(new StockEntry{Product = targProduct, Quantity = 0});
				GD.Print("A shelf: Stocked a new product");
				_Visuals.Refresh();
			}

		}
		else // want to unstock
		{
			if (entry != null) // stocked before, unstock now
			{
				StockedProductsList.Remove(entry);
				GD.Print("A shelf: Unstocked a product.");
				_Visuals.Refresh();
			}
			else // no such stock, do nothing
			{
				GD.Print("A shelf: Could not unstock, that product is not stocked.");
			}
		}
	}

	public bool IsStocked(ProductEntity targProduct)
	{
		var entry = GetEntryFromStocked(targProduct);
		if (entry is null)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public void AddProduct(ProductEntity targProduct, int amount)
	{
		var entry = GetEntryFromStocked(targProduct);
		if (entry is null)
		{
			GD.Print("A shelf: Tried to add a product, but that product is not stocked on this shelf.");
			return;
		}
		else
		{
			entry.Quantity += amount;
			GD.Print("A shelf: Added some products.");
			_Visuals.Refresh();
			return;
		}
	}

	public int TakeProduct(ProductEntity targProduct, int amount)
	{
		var entry = GetEntryFromStocked(targProduct);
		if (entry is null)
		{
			GD.Print("A shelf: Tried to take a product, but that product is not stocked on this shelf.");
			return 0;
		}
		else
		{
			if (entry.Quantity == 0)
			{
				GD.Print("A shelf: Failed to take any products, shelf is empty.");
				return 0;
			}
			else if (entry.Quantity >= amount)
			{
				entry.Quantity -= amount;
				GD.Print($"A shelf: Took {amount} products. {entry.Quantity} remaining.");
				_Visuals.Refresh();
				return amount;
			}
			else // quantity > 0 but < amount
			{
				int taken = entry.Quantity;
				entry.Quantity = 0;
				GD.Print($"A shelf: Only had {taken}, took all that were left.");
				_Visuals.Refresh();
				return taken;
			}
			
		}
	}
	#endregion Public methods
	
	#region Private methods
	private StockEntry GetEntryFromStocked(ProductEntity targProduct)
	{
		foreach (StockEntry entry in StockedProductsList)
		{
			if (entry.Product == targProduct)
			{
				return entry;
			}
		}
		return null;
	}
	#endregion Private methods
}