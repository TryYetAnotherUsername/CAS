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
			}
		}
	}

	public CasProj Out()
	{
		CasProj project = new();

		foreach (Node3D node in _BuildingRoot.GetChildren())
		{
			GD.Print($"  - {node.Name} is Prop: {node is Prop}");

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
				project.Props.Add(propDataPack);
			} 
		}

		return project;
	}
}