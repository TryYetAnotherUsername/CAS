using System;
using System.Collections.Generic;
using Godot;

public partial class Shelf : Prop
{
    public class StockEntry
    {
        public ProductEntity Product = ProductConfig.Catalog[0];
        public int Quantity = 0;
    }

    public List<StockEntry> StockedProductsList = new();

	public void ChangeProductStockStatus(ProductEntity targProduct, bool status)
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
}