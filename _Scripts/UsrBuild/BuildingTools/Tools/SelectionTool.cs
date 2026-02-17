using Godot;
using System;
using System.Data.Common;



public partial class SelectionTool : BuildmodeTool
{

	public override void _Ready()
	{
		BuildmodeService.OnToolSelected += (tool) => 
		{
			if (tool == BuildmodeService.Tool.Selection)
			{
				SetSelecting(true);
			}
			else
			{
				SetSelecting(false);
			}
		};
	}

/*
    public override void _UnhandledInput(InputEvent @event)
    {
		if (Input.IsActionJustPressed("build_select"))
		{
			if (CurrentHovering != null)
			{
				var owner = CurrentHovering.GetOwner<Node3D>();
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
		}

		if (Input.IsActionJustPressed("build_select") && CurrentHoveringNoneDetected)
		{
			if (BuildmodeService.CurrentSelected != null)
			{
				var mesh = BuildmodeService.CurrentSelected.FindChild("Mesh") as MeshInstance3D;
				if (mesh != null)
				{
					mesh.MaterialOverlay = null;
				}
				BuildmodeService.CurrentSelected = null;
				GD.Print("Selection cleared.");
			}
		}

    }

	public override void _PhysicsProcess(double delta)
	{
		var camera3D = GetViewport().GetCamera3D();
		var from = camera3D.ProjectRayOrigin(GetViewport().GetMousePosition());
		var to = from + camera3D.ProjectRayNormal(GetViewport().GetMousePosition()) * RayLength;
		var spaceState = GetViewport().World3D.DirectSpaceState;
		var options = PhysicsRayQueryParameters3D.Create(from, to);
		options.CollisionMask = 2;
		var result = spaceState.IntersectRay(options);
		
		if (result.Count > 0)
		{
			CurrentHoveringNoneDetected = false;
			CurrentHovering = (Node3D)result["collider"];
			DisplayServer.CursorSetShape(DisplayServer.CursorShape.PointingHand);
		}
		else
		{
			DisplayServer.CursorSetShape(DisplayServer.CursorShape.Arrow);
			CurrentHovering = null;
			CurrentHoveringNoneDetected = true;
		}
	}
*/

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("build_select"))
		{
			if (Raycast() is Node3D collider)
			{
				var root = collider.GetOwner<Prop>();
				if (root is null) return;  // hit something that isn't a placeable
				BuildmodeService.I.Select(root);
			}
		}
	}

	public void SetSelecting(bool state)
	{
		SetProcessUnhandledInput(state);
		SetProcess(state);
	}

    public override void _Process(double delta)
    {
    	Node3D collider = Raycast();
    	if (collider is not null)
    	{
			DisplayServer.CursorSetShape(DisplayServer.CursorShape.PointingHand);
    	}
		else
		{
			DisplayServer.CursorSetShape(DisplayServer.CursorShape.Arrow);
		}
    }

	private Node3D Raycast()
	{
		var camera3D = GetViewport().GetCamera3D();
		var from = camera3D.ProjectRayOrigin(GetViewport().GetMousePosition());
		var to = from + camera3D.ProjectRayNormal(GetViewport().GetMousePosition()) * BuildmodeService.RayLength;

		var spaceState = GetViewport().World3D.DirectSpaceState;
		var options = PhysicsRayQueryParameters3D.Create(from, to);
		options.CollisionMask = 2;
		var result = spaceState.IntersectRay(options);
		
		if (result.Count > 0)
		{
			return (Node3D)result["collider"];
		}
		else
		{
			return null;
		}
	}
}
