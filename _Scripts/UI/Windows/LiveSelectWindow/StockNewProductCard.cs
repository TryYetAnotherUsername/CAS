using Godot;
using System;

public partial class StockNewProductCard : Panel
{
	[Export] private OptionButton _optionButton;
	[Export] private Button _button;
	private Shelf _shelf;

	ProductEntity _targProductEntity;

	private Action _onProductAdded;

	public void Start(Shelf shelf, Action onProductAdded)
	{
		_targProductEntity = null;
		_shelf = shelf;
		_onProductAdded = onProductAdded;

		GenProductOptions();

		_button.Pressed += AddProduct;
		_optionButton.ItemSelected += ChangeTargProduct;
	}

	private void AddProduct()
	{
		int catalogIndex = _optionButton.GetItemId(_optionButton.Selected);
		_targProductEntity = ProductConfig.Catalog[catalogIndex];

		if (_targProductEntity is null)
		{
			GD.PrintErr("StockNewProductCard: An error occured"); return;
		}
		if (_shelf.IsStocked(_targProductEntity))
		{
			GD.PrintErr("StockNewProductCard: Selected product already stocked."); 
			return;
		}

		_shelf.SetProductStock(_targProductEntity, true);
		_onProductAdded?.Invoke();
		Visible = false;
	}

	private void ChangeTargProduct(long index)
	{
		int catalogIndex = _optionButton.GetItemId((int)index);
		_targProductEntity = ProductConfig.Catalog[catalogIndex];
	}

	public void GenProductOptions()
	{
		_optionButton.Clear();
		for (int i = 0; i < ProductConfig.Catalog.Count; i += 1)
		{
			if (_shelf.IsStocked(ProductConfig.Catalog[i]) == false)
			{
				_optionButton.AddItem(ProductConfig.Catalog[i].DispName, i);
			}
		}
	}
}
