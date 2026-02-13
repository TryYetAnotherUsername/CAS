using System;
using System.Diagnostics.Tracing;
using Godot;

public partial class ScaleGizmo : BuildmodeGizmo
{
	// Editor dependencies
	[Export] MeshInstance3D _x;
	[Export] MeshInstance3D _y;
	[Export] MeshInstance3D _z;

	// Debug objects
	private Line2D _debugLine;
	private ColorRect _debugPoint;

	// enums
	enum Direction
	{
		X, Y, Z
	}

	// Global variables
	private bool _isDragging;
	private Direction _currentDragDir;
	private Node3D _currentTargetObj;

	// ========== MAIN PROGRAM ==========

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("build_gizmo_drag"))
		{
			var dir = GetMouseOverDir();
			if (dir is null) return;

			_currentDragDir = dir.Value;  // lock direction on click
			_isDragging = true;

			SetDragging(true);
		}
		else if (Input.IsActionJustReleased("build_gizmo_drag"))
		{
			_isDragging = false;
		}
	}

	public override void _Ready()
	{
		_debugLine = new Line2D();
		_debugLine.Width = 1;
		_debugLine.DefaultColor = Color.FromHtml("#ff9100");
		_debugLine.Visible = false;  // hidden by default
		CallDeferred("add_child", _debugLine);
	}

	// This method uses GetMouseOverPart() to get the handle the mouse is currently over, and returns a Direction enum.
	// If the mouse is not hovering on any hendle, returns null.
	private Direction? GetMouseOverDir() // Apparently this "?" makes a function nullabel, interesting!
	{
		var part = GetMouseOverPart();
		if (part is null) return null;

		if (part == _x) return Direction.X;
		else if (part == _y) return Direction.Y;
		else if (part == _z) return Direction.Z;
		else return null;
	}

	private void ScaleObject(Direction? dir, float amount)
	{
		if (dir == Direction.X)
		{
			
		}
		else if (dir == Direction.Y)
		{
			
		}
		else if (dir == Direction.Z)
		{
			
		}
	}

	private void SetDragging(bool state)
	{
		if (state)
		{
			var cam = GetViewport().GetCamera3D();
			_debugLine.ClearPoints();
			_debugLine.AddPoint(cam.UnprojectPosition(GlobalPosition));
			_debugLine.AddPoint(GetOffsetEndpoint(_currentDragDir, cam));
			_debugLine.Visible = true;
		}
		else
		{
			_debugLine.Visible = false;
		}

        Vector2 GetOffsetEndpoint(Direction? dir, Camera3D cam)
        {
			Vector2 endP = Vector2.Inf;
            if (dir == Direction.X)
            {
				GD.Print("x");
                endP = cam.UnprojectPosition(GlobalPosition + GlobalBasis.X * 2);
            }
            else if (dir == Direction.Y)
            {
				GD.Print("y");
                endP = cam.UnprojectPosition(GlobalPosition + GlobalBasis.Y * 2);
            }
            else if (dir == Direction.Z)
            {
				GD.Print("z");
                endP = cam.UnprojectPosition(GlobalPosition + GlobalBasis.Z * 2);
            }
            return endP;
        }
    }

	public override void _Process(double delta)
	{
		if (!_isDragging) 
		{
			SetDragging(false);
			return;
		}
		ScaleObject(_currentDragDir, 100);
	}
}