using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class BuildmodeGizmo : Node3D
{
    public override void _Ready()
    {
		BuildmodeService.OnToolSelected += InitialisePostition;

    }

	public void InitialisePostition(BuildmodeService.Tool _)
	{
		Position = BuildmodeService.I.CurrentSelected.Position;
	}

	// This method returns the gizmo handle, or else null.
	public MeshInstance3D GetMouseOverPart()
	{
		// <Code adapted from Godot docs>
		var camera3D = GetViewport().GetCamera3D();
		var mousePos = GetViewport().GetMousePosition();
		var from = camera3D.ProjectRayOrigin(mousePos);
		var to = from + camera3D.ProjectRayNormal(mousePos) * BuildmodeService.RayLength;

		var spaceState = GetViewport().World3D.DirectSpaceState;
		var options = PhysicsRayQueryParameters3D.Create(from, to);
		options.CollisionMask = 4;
		options.CollideWithAreas = true;
		
		var result = spaceState.IntersectRay(options);
		// <Code adapted from Godot docs>

		if (result.Count == 0) return null;

		var hitCollider = (Node3D)result["collider"];
		var hitPart = hitCollider.GetParent() as MeshInstance3D;
		return hitPart;
	}
}
