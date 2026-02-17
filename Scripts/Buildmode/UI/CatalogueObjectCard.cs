using Godot;


public partial class CatalogueObjectCard : Panel
{
	public ItemConfig Item { private get; set; } = null;
	[Export] Button _itemButton;

	public override void _Ready()
	{
		_itemButton.Text = Item.DispName;
	}
}
