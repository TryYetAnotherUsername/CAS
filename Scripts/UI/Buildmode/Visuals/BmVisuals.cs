using Godot;
using System;

public partial class BmVisuals : Node
{
	[Export] Shader OutlineShader;

	public override void _Ready()
	{
		//BuildmodeService.OnNewObjectSelected += (obj) =>
			
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

/*
	ApplyOutline(Node3D targObj)
	{
		var owner = targObj.GetOwner<Node3D>();
		if (owner?.GetParent()?.Name == "BuildingRoot")
		{
			GD.Print($"New object selected: {owner.Name}");
			BuildmodeService.CurrentSelected = (Node3D) CurrentHovering.GetOwner();
			var Mesh = (MeshInstance3D)BuildmodeService.CurrentSelected.FindChild("Mesh");
			var newShaderMat = new ShaderMaterial();
			newShaderMat.Shader = OutlineShader;
			Mesh.MaterialOverlay = newShaderMat;
		}
	}
*/
}
