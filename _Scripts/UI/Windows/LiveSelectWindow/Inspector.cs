using Godot;
using System;
using System.Collections.Generic;

public partial class Inspector : Control
{
	private Prop _prop;
	private Shelf _shelf;
	[Export] private PackedScene _productCard;
	[Export] private Control _productAddCard;
	[Export] private Control _productCardRoot;

	[Export] private Button _Reload;
	[Export] private Button _Add;
	[Export] private Button _Apply;

	[Export] private AnimationPlayer _aniPlayer;

	// lifecycle kinda methods
	public void Init(Prop prop)
	{
		_prop = prop;
		if (prop is Shelf shelf)
		{
			_shelf = shelf;
			updateWithAnimation(shelf.StockedProductsList);

			var productAddCard = (StockNewProductCard)_productAddCard;
			productAddCard.Start(_shelf, ()=> updateWithAnimation(shelf.StockedProductsList));

			_Add.Pressed += () => 
			{
				productAddCard.Visible = true;
				productAddCard.GenProductOptions();
			};

			_Reload.Pressed += () => 
			{
				updateWithAnimation(shelf.StockedProductsList);
			};
		}
	}

	// private methods

	private List<Shelf.StockEntry> _stockListA; // bad practice but if it works it works :/

	private void updateWithAnimation(List<Shelf.StockEntry> stockList)
	{
		_stockListA = stockList;
		_aniPlayer.Play("refresh_list");
	}

	public void updateShelfProductsA()
	{
		clearAllCards();

		foreach (Shelf.StockEntry stockEntry in _stockListA)
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
