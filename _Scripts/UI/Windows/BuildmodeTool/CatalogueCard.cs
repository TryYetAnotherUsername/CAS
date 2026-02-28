using Godot;
using System;
using System.Runtime.InteropServices.Marshalling;

public partial class CatalogueCard : Button
{
	[Export] Label _dispNameLabel;
	[Export] Label _costLabel;
	private CatalogEntity _catEnt;

	public void Init(CatalogEntity catalogEntity)
	{
		_catEnt = catalogEntity;
		_costLabel.Text = $"£{catalogEntity.Cost.ToString()} / unit";
		_dispNameLabel.Text = catalogEntity.DispName;
		Pressed += ReqPlace;
	}

	private void ReqPlace()
	{
		GetViewport().SetInputAsHandled();
		BuildToolService.I.StartPlacingFromCatalog(_catEnt);
	}
}
