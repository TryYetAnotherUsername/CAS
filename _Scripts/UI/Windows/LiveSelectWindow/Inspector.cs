using Godot;
using System;
using System.Collections.Generic;

public partial class Inspector : Control
{
	private Prop _prop;
	private Shelf _shelf;
	[Export] private PackedScene _productCard;
	[Export] private Control _productCardRoot;

	[Export] private Button _Reload;
	[Export] private Button _Add;
	[Export] private Button _Apply;

	// lifecycle kinda methods
	public void Init(Prop prop)
	{
		_prop = prop;
		if (prop is Shelf shelf)
		{
			GD.Print(shelf.StockedProductsList);
			_shelf = shelf;
			updateShelfProducts(shelf.StockedProductsList);
		}
	}

	public void QueueFree(Prop prop)
	{
		
	}

	// private methods

	private void updateShelfProducts(List<Shelf.StockEntry> stockList)
	{
		clearAllCards();

		foreach (Shelf.StockEntry stockEntry in stockList)
		{
			var productCard = _productCard.Instantiate();
			_productCardRoot.AddChild(productCard);

			GD.Print("card: " + productCard.Name);
			
			var productCardScript = (ProductCard) productCard;
			GD.Print("script: " + productCardScript);

			productCardScript.Init(_shelf, stockEntry);
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
