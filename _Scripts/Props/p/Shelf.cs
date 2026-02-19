using System;
using System.Collections.Generic;
using Godot;

public partial class Shelf : Prop
{
    public class StockEntry
    {
        public ProductEntity product;
        public int quantity;
    }

    private List<StockEntry> stockedProducts = new();

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
			entry.quantity += 1;
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
			if (entry.quantity == 0)
			{
				GD.Print("A shelf: Failed to take any products, shelf is empty.");
				return 0;
			}
			else if (entry.quantity >= amount)
			{
				entry.quantity -= amount;
				GD.Print($"A shelf: Took {amount} products. {entry.quantity} remaining.");
				return amount;
			}
			else // quantity > 0 but < amount
			{
				int taken = entry.quantity;
				entry.quantity = 0;
				GD.Print($"A shelf: Only had {taken}, took all that were left.");
				return taken;
			}
			
		}
	}

	private StockEntry GetEntryFromStocked(ProductEntity targProduct)
	{
		foreach (StockEntry entry in stockedProducts)
		{
			if (entry.product == targProduct)
			{
				return entry;
			}
		}
		return null;
	}
}