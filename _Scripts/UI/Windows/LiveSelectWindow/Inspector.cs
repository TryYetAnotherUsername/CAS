using Godot;
using System;
using System.Collections.Generic;

public partial class Inspector : Control
{
	private Prop _prop;
	[Export] private PackedScene _productCard;
	[Export] private Control _productCardRoot;

	public void Init(Prop prop)
	{
		_prop = prop;
		if (prop is Shelf shelf)
		{
			GD.Print(shelf.StockedProductsList);
			updateShelfProducts(shelf.StockedProductsList);
		}
	}

	private void updateShelfProducts(List<Shelf.StockEntry> stockList)
	{
		clearAllCards();

		foreach (Shelf.StockEntry stockEntry in stockList)
		{
			var productCard = _productCard.Instantiate();
			var productCardScript = productCard as ProductCard;

			productCardScript.Init(stockEntry);
		}
	}

	private void clearAllCards()
	{
		foreach (Node node in _productCardRoot.GetChildren())
		{
			node.QueueFree();
		}
	}
}
