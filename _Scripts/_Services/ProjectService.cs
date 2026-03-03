using Godot;
using System;
using System.Collections.Generic;

public partial class ProjectService : Node
{
	public static ProjectService I;
	[Export] public Node3D _BuildingRoot;

    public override void _Ready()
    {
        I = this;
    }


	public void In(CasProj project)
	{
		FactoryService.I.ClearAll();
		
		foreach (var prop in project.Props)
		{
			var node = FactoryService.I.TrySpawningUidAndGetNode(prop.Uid);
			if (node is Prop p)
			{
				p.GlobalPosition = new Vector3(prop.X, prop.Y, prop.Z);
				p.GlobalRotation = new Vector3(0, prop.RotY, 0);

				if (node is Shelf shelf && prop.StockListData != null)
				{
					foreach (var entry in prop.StockListData)
					{
						shelf.SetProductStock(entry.Product, true);
						shelf.AddProduct(entry.Product, entry.Quantity);
					}
				}
			}
		}
	}

	public CasProj Out()
	{
		CasProj project = new();
		foreach (Node3D node in _BuildingRoot.GetChildren())
		{
			if (node is Prop prop)
			{
				PropData propDataPack = new()
				{
					Uid = prop.Identity.UID,
					X = node.GlobalPosition.X,
					Y = node.GlobalPosition.Y,
					Z = node.GlobalPosition.Z,
					RotX = node.GlobalRotation.X,
					RotY = node.GlobalRotation.Y,
					RotZ = node.GlobalRotation.Z
				};

				// Save shelf stock asw if it is
				if (node is Shelf shelf)
				{
					propDataPack.StockListData = shelf.StockedProductsList;
				}
				project.Props.Add(propDataPack);
			}
		}
		return project;
	}
}