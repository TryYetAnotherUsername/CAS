using Godot;
using System;

public partial class ProductCard : FoldableContainer
{
	// export vars
	[Export] Button _unstockButton;
	[Export] OptionButton _optionButton;
	[Export] SpinBox _quantityField;

	// private vars
	private Shelf.StockEntry _entry;
	private Shelf _shelf;


	// lifecycle methods
	public void Init(Shelf s , Shelf.StockEntry e)
	{
		GD.Print("A ProductCard: Starting init");
		
		_entry = e;
		_shelf = s;

		Title = e.Product.DispName;
		_quantityField.SetValueNoSignal((int) e.Quantity);

		GenProductOptions();

		_unstockButton.Pressed += RemoveEntry;
		_optionButton.ItemSelected += ChangeEntryProduct;
		_quantityField.ValueChanged += ChangeEntryQuantity;
		GD.Print("A ProductCard: Init finished");
	}

	// Private/ helper methods
	private void RemoveEntry()
	{
		GD.Print("try remove" + _entry.Product);
		_shelf.ChangeProductStockStatus(_entry.Product, false);
		QueueFree();
	}

	private void ChangeEntryProduct(long index)
	{
		var uid = ResourceUid.IdToText(_optionButton.GetItemId((int)index)); // converts the index of dropdown to (previously set) uid then to text uid
		GD.Print(ProductConfig.FindByUID(uid));
		_entry.Product = ProductConfig.FindByUID(uid); // then use a method on ProductConfig... clean!
	}

	private void ChangeEntryQuantity(double quantity)
	{
		_entry.Quantity = (int) quantity;
	}

	private void GenProductOptions()
	{
		_optionButton.Clear();
		foreach (var product in ProductConfig.Catalog)
		{
			_optionButton.AddItem(product.DispName, (int) ResourceUid.TextToId(product.UID));
		}
	}
}
