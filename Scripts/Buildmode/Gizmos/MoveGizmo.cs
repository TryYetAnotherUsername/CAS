using System;
using System.Diagnostics.Tracing;
using System.Security;
using Godot;

public partial class MoveGizmo : BuildmodeGizmo
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

	enum LineDir
	{
		None, Negative, Positive
	}

	// Global variables
	[Export] private float _sensitivity = 0.05f;
	[Export] private bool _showDebug;
	private bool _isDragging;
	private Direction _currentDragDir;
	private Vector2 _lineStart;
	private Vector2 _lineEnd;
	private Vector2 _initalPosOnLine;
	private Vector3 _targPos;
	private Vector3 _initialPos;

	// ========== MAIN PROGRAM ==========

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("build_gizmo_drag"))
		{
			var dir = GetMouseOverDir();
			if (dir is null) return;

			_currentDragDir = dir.Value;
			_isDragging = true;
			SetDragging(true);
			
			_initialPos = BuildmodeService.I.CurrentSelected.Position;
			_targPos = BuildmodeService.I.CurrentSelected.Position; 
			_initalPosOnLine = GetClosestPointOnLine(_lineStart, _lineEnd);

		}
		else if (Input.IsActionJustReleased("build_gizmo_drag"))
		{
			_isDragging = false;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		_debugLine = new Line2D();
		_debugLine.Width = 2;
		_debugLine.DefaultColor = Color.FromHtml("#ffd194");
		_debugLine.Visible = false;
		CallDeferred("add_child", _debugLine);

		_debugPoint = new ColorRect();
		_debugPoint.Color = Colors.Red;
		_debugPoint.Size = new Vector2(10, 10);
		_debugPoint.Visible = false;
		CallDeferred("add_child", _debugPoint);
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

	private void MoveObject(Direction? dir, float amount)
	{
		var p = BuildmodeService.I.CurrentSelected.Position;
		var a = amount * _sensitivity;

		if (dir == Direction.X)
		{
			_targPos.X = _initialPos.X + a;
		}
		else if (dir == Direction.Y)
		{
			_targPos.Y = _initialPos.Y + a;
		}
		else if (dir == Direction.Z)
		{
			_targPos.Z = _initialPos.Z + a;
		}
		p = _targPos;
		BuildmodeService.I.CurrentSelected.Position = p;
		Position = BuildmodeService.I.CurrentSelected.Position;
	}

	private void SetDragging(bool state)
	{
		if (state)
		{
			var cam = GetViewport().GetCamera3D();
			_debugLine.ClearPoints();
			
			var start = cam.UnprojectPosition(GlobalPosition);
			var end = GetOffsetEndpoint(_currentDragDir, cam);

			var direction = (start - end).Normalized();
			var farEnd = start + direction * 10000;
			var farStart = start - direction * 10000;

			if (_showDebug)
			{
				_debugLine.AddPoint(farEnd);
				_debugLine.AddPoint(farStart);
				_debugLine.Visible = true;
			}

			_lineEnd = farEnd;
			_lineStart = farStart;

		}
		else
		{
			_debugLine.Visible = false;
			_debugPoint.Visible = false;
		}

        Vector2 GetOffsetEndpoint(Direction? dir, Camera3D cam)
        {
			Vector2 endP = Vector2.Inf;
            if (dir == Direction.X)
            {
                endP = cam.UnprojectPosition(GlobalPosition + GlobalBasis.X * 2);
            }
            else if (dir == Direction.Y)
            {
                endP = cam.UnprojectPosition(GlobalPosition + GlobalBasis.Y * 2);
            }
            else if (dir == Direction.Z)
            {
                endP = cam.UnprojectPosition(GlobalPosition + GlobalBasis.Z * 2);
            }
            return endP;
        }
    }

	private Vector2 GetClosestPointOnLine(Vector2 start, Vector2 end)
	{
		var intersect = Geometry2D.GetClosestPointToSegment(GetViewport().GetMousePosition(), start, end);
		_debugPoint.Position = intersect;
		return intersect;
	}

	public override void _Process(double delta)
	{
		if (!_isDragging) 
		{
			SetDragging(false);
			return;
		}

		//If user is dragging:
		Vector2 currentIntersect = GetClosestPointOnLine(_lineStart, _lineEnd);
		
		float initialDist = _initalPosOnLine.DistanceTo(_lineStart);
		float currentDist = currentIntersect.DistanceTo(_lineStart);
		float dragDelta = initialDist - currentDist;
		
		MoveObject(_currentDragDir, dragDelta); // note dragDelta can be negative so no need for dot product.
	}
}