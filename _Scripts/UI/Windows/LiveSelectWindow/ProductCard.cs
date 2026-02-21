using Godot;
using System;

public partial class ProductCard : FoldableContainer
{
	[Export] Button _unstockButton;
	[Export] OptionButton _optionButton;
	[Export] SpinBox _quantityField;

	private Shelf.StockEntry _entry;

	public void Init(Shelf.StockEntry entry)
	{
		GD.Print("A ProductCard: Starting init");
		

		_entry = entry;
		ProductEntity product = entry.Product;
		int quantity = entry.Quantity;

		GetOwner<FoldableContainer>().Title = entry.Product.DispName;
		GenProductOptions();
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
