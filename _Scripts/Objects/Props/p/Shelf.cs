using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;

/// <summary>
/// Each shelf has fields for its stored products, exposes methods for adding/ subtracting products, and logic to render out the products on the shelf.
/// </summary>

// Issue #11 - unify checklists

public partial class Shelf : Prop
{
	private class Visuals
	{
		private void Clear()
		{
			
		}
	}

	#region Stuff

	/// <summary>
	/// Stores the quantity of each product.
	//  NTS- It dosen't need a class, but probably more scaleable, for expiry dates, batch quality, etc.
	/// </summary>
	
    public class StockEntry
    {
        public ProductEntity Product = ProductConfig.Catalog[0];
        public int Quantity = 0;
    }

    public List<StockEntry> StockedProductsList = new();

	[Export] public Node3D NavTarget;
	[Export] private Node3D _productAreasRoot;

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
			}

		}
		else // want to unstock
		{
			if (entry != null) // stocked before, unstock now
			{
				StockedProductsList.Remove(entry);
				GD.Print("A shelf: Unstocked a product.");
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
				return amount;
			}
			else // quantity > 0 but < amount
			{
				int taken = entry.Quantity;
				entry.Quantity = 0;
				GD.Print($"A shelf: Only had {taken}, took all that were left.");
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