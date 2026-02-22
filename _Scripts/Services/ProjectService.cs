using Godot;
using System;
using System.Collections.Generic;

public class ProjectService
{
	public static ProjectService I;
	public CasProj Current;
	public Node3D _BuildingRoot;

	public void In(CasProj projectData)
	{
		foreach (PropData prop in projectData.Props)
		{
			Node3D node = FactoryService.I.TrySpawningUidAndGetNode(prop.Uid);
			node.GlobalPosition = new Vector3(prop.X, prop.Y, prop.Z);
			node.GlobalRotation = new Vector3(prop.RotX, prop.RotY, prop.RotZ);
		}
	}

	public void Out()
	{
		foreach (PropData prop in _)
		{
			Node3D node = FactoryService.I.TrySpawningUidAndGetNode(prop.Uid);
			node.GlobalPosition = new Vector3(prop.X, prop.Y, prop.Z);
			node.GlobalRotation = new Vector3(prop.RotX, prop.RotY, prop.RotZ);
		}
	}

	public void Load(string path)
	{
		// ... Deserialize your json ...

		foreach (var data in project.Props)
		{
			var node = FactoryService.I.TrySpawningUidAndGetNode(data.Uid);
			if (node is Prop p)
			{
				p.GlobalPosition = new Vector3(data.X, data.Y, data.Z);
				p.GlobalRotation = new Vector3(0, data.RotY, 0);

				// Re-stock if the data says it's a shelf with items
				if (p is Shelf s && !string.IsNullOrEmpty(data.ProductUid))
				{
					var prod = ProductConfig.FindByUID(data.ProductUid);
					s.SetProductStockStatus(prod, true);
					s.AddProduct(prod, data.Quantity);
				}
			}
		}
	}
}