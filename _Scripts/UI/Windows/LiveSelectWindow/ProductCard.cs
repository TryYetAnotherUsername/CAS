using Godot;
using System;

public partial class ProductCard : FoldableContainer
{
	// export vars
	[Export] Button _unstockButton;
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

		_unstockButton.Pressed += RemoveEntry;
		_quantityField.ValueChanged += ChangeEntryQuantity;
		GD.Print("A ProductCard: Init finished");
	}

	// Private/ helper methods
	private void RemoveEntry()
	{
		GD.Print("try remove" + _entry.Product);
		_shelf.SetProductStock(_entry.Product, false);
		QueueFree();
	}

	private void ChangeEntryProduct(long index)
	{
		_entry.Product = ProductConfig.Catalog[(int)index];
	}

	private void ChangeEntryQuantity(double quantity)
	{
		_entry.Quantity = (int) quantity;
	}
}
