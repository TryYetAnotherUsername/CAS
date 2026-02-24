using Godot;


public partial class CatalogueObjectCard : Panel
{
	public CatalogEntity Item { private get; set; } = null;
	[Export] Button _itemButton;

	public override void _Ready()
	{
		_itemButton.Pressed += ()=> BuildmodeService.I?.SpawnNewObject(Item.UID);
		_itemButton.Text = Item.DispName;
	}
}
