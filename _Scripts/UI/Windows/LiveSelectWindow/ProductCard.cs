using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class ProductCard : FoldableContainer
{
	// export vars
	[Export] private Button _unstockButton;
	[Export] private Label _quantityField;
	[Export] private Button _add;
	[Export] private Button _take;
	[Export] private Button _add1;

	// All the fields
	[Export] private Label _supplier;
	[Export] private Label _category;
	[Export] private Label _importPrice;
	[Export] private Label _sellPrice;
	[Export] private Label _popularity;
	[Export] private Label _localImports;
	[Export] private Label _ageRestriction;

	// private vars
	private Shelf.StockEntry _entry;
	private Shelf _shelf;


	// lifecycle methods
	public void Init(Shelf s, Shelf.StockEntry e)
	{
		_entry = e;
		_shelf = s;
		Title = e.Product.DispName;
		_quantityField.Text = e.Quantity.ToString() + " units";

		_supplier.Text = e.Product.Supplier;
		_category.Text = e.Product.Category.ToString();
		_importPrice.Text = $"£ {e.Product.PriceImport}";
		_sellPrice.Text = $"£ {e.Product.PriceSell}";
		_popularity.Text = e.Product.Popularity.ToString();

		_localImports.Visible = e.Product.LocalImport;
		_ageRestriction.Visible = e.Product.AgeRestricted;

		_unstockButton.Pressed += RemoveEntry;
		_add.Pressed += Add;
		_add1.Pressed += Add1;
		_take.Pressed += Take;
	}

	// Private/ helper methods
	private void RemoveEntry()
	{
		_shelf.SetProductStock(_entry.Product, false);
		QueueFree();
	}

	private void Add()
	{
		if (EconomyService.I.TryTakeCash(_entry.Product.PriceImport * 10) == false) return;
		_shelf.AddProduct(_entry.Product, 10);
		_quantityField.Text = _entry.Quantity.ToString() + " units";
	}

	private void Add1()
	{
		if (EconomyService.I.TryTakeCash(_entry.Product.PriceImport) == false) return;
		_shelf.AddProduct(_entry.Product, 1);
		_quantityField.Text = _entry.Quantity.ToString() + " units";
	}

	private void Take()
	{
		if(_entry.Quantity <= 0)
		{
			return;
		}
		else if(_entry.Quantity >= 10)
		{
			EconomyService.I.AddCash(_entry.Product.PriceImport / 2 * 10);
			_shelf.TakeProduct(_entry.Product, 10);
		}
		else if(_entry.Quantity < 10)
		{
			EconomyService.I.AddCash(_entry.Product.PriceImport / 2 * _entry.Quantity);
			_shelf.TakeProduct(_entry.Product, _entry.Quantity);
		}

		_quantityField.Text = _entry.Quantity.ToString() + " units";
	}
}
