using Godot;
using System;
using System.Linq;

public partial class BmVisuals : Node
{
	[Export] Shader _OutlineShader;
	private ShaderMaterial _shaderMat;


	public override void _Ready()
	{
		BuildmodeService.OnObjectSelected += (obj) =>
		{
			ChangeOutline(obj, true);
		};

		BuildmodeService.OnObjectDeselected += (obj) =>
		{
			ChangeOutline(obj, false);
		};

		_shaderMat = new ShaderMaterial();
		_shaderMat.Shader = _OutlineShader;
		_shaderMat.SetShaderParameter("color", new Color("#ff8800"));
	}

	private void ChangeOutline(Node3D targObj, bool apply)
	{
		if (targObj is null) return;

		var mainMesh = targObj.FindChild("MainMesh", recursive: true) as MeshInstance3D;
		if (mainMesh is null) return;

		if (apply == true)
		{
			mainMesh.MaterialOverlay = _shaderMat;
			GD.Print(mainMesh.MaterialOverlay);
		}
		else
		{
			mainMesh.MaterialOverlay = null;
		}
		
	}
}
