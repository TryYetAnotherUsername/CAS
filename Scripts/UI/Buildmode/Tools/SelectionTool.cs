using Godot;
using System;
using System.Data.Common;



public partial class SelectionTool : Node
{
	private const float RayLength = 1000.0f;
	[Export] Node3D _Marker;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ControlsLogic.OnToolChanged += (tool) =>
		{
			if (tool == ControlsLogic.Tool.none)
			{
				StartUsing();
			}
			else
			{
				EndUsing();
			}
		};
		EndUsing();
	}

    public override void _Input(InputEvent @event)
    {
		if (Input.IsActionJustPressed("build_select"))
		{
			
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
			_Marker.Visible = true;
			_Marker.Position = (Vector3)result["position"];
		}
		else
		{
			_Marker.Visible = false;
		}
	}

	public void StartUsing()
	{
		SetProcessInput(true);
		SetProcess(true);
		SetPhysicsProcess(true);
	}

	public void EndUsing()
	{
		SetProcessInput(false);
		SetProcess(false);
		SetPhysicsProcess(false);
	}
}
