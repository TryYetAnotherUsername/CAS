using Godot;
using System;

public partial class WinContSketch2d : Control
{
	// Global vars
	private bool _isMouseOnViewport;
	private Vector2 _ = Vector2.Zero;
	[Export] public float CurrentCamZoom = 1f;
	[Export] private float _zoomStep = 0.2f;
	public Action <float> OnCamZoomChangedTo;
	public Node2D ClosestNode;
	public float SnapMovement = 5f;

	[Export] private float _camZoomMax;
	[Export] private float _camZoomMin;


	// Dependencies
	[Export] private SubViewportContainer _viewport;
	[Export] private Camera2D _camera;
	[Export] private Node2D _drawedItemsRoot;
	[Export] private Label _posLabel;
	[Export] private Button _updButton;
	[Export] private Button _vertexButton;
	[Export] private SpinBox _SnapTo;

	// draw elements
	[Export] private PackedScene _line;
	[Export] private PackedScene _vert;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        //Input.WarpMouse(new Vector2(100,100)); //can use this in viewport thing
		_viewport.MouseEntered += () => 
		{
			_isMouseOnViewport = true;
		};
		_viewport.MouseExited += () => 
		{
			_isMouseOnViewport = false;
		};

		_SnapTo.Value = 1;

		_updButton.Pressed += UiUpdateAll;
		_vertexButton.Pressed += () =>
        {
			Relay.Exe("sketch2d.newvertex","0,0,0");
            UiUpdateAll();
        };

		_SnapTo.ValueChanged += (_) =>
		{
			if (_SnapTo.Value == 0)
			{
				_SnapTo.Value = 0.001;
				GD.Print("SnapMovement" + _SnapTo.Value);
			}

			SnapMovement = (float) _SnapTo.Value;
			GD.Print(SnapMovement);
		};
    }

	public override void _GuiInput(InputEvent @event)
	{

		// if the mouse isn't in the viewport
		if (!_isMouseOnViewport) 
		{
			_posLabel.Text = "";
			return;
		}

		

		var mousePosInWorld = _camera.GetGlobalMousePosition();
		_posLabel.Text = $"X ({MathF.Round(mousePosInWorld.X)}) Y ({MathF.Round(mousePosInWorld.Y)})";

	if (@event.IsActionPressed("viewport_select"))
	{
		var mousePos = _camera.GetGlobalMousePosition();
		Node2D node = GetClosestNode2dTo(mousePos);
		
		if (node is S2dRenderedVert vert)
		{
			vert.StartDragging(); // The handshake
		}
	}


		// Zoom
		if (@event is InputEventMouseButton)
		{
			if (@event.IsActionPressed("viewport_zoom_out"))
				CurrentCamZoom -= _zoomStep;
			else if (@event.IsActionPressed("viewport_zoom_in"))
				CurrentCamZoom += _zoomStep;

			CurrentCamZoom = Mathf.Clamp(CurrentCamZoom, _camZoomMin, _camZoomMax);
			OnCamZoomChangedTo?.Invoke(CurrentCamZoom);
		}

		// Dragging
		if (@event is InputEventMouseMotion mouseMotion && Input.IsActionPressed("viewport_drag_cam"))
		{
			PushPosToCamera(mouseMotion.Relative);
		}
		GetViewport().SetInputAsHandled();
		
		return;
	}

	private Node2D GetClosestNode2dTo(Vector2 clickPos)
	{
		Node2D bestMatch = null;
		float closestDist = 999999f;

		foreach (Node child in _drawedItemsRoot.GetChildren())
		{
			var item = (Node2D)child;
			float dist = clickPos.DistanceTo(item.GlobalPosition);
			
			if (dist < closestDist)
			{
				closestDist = dist;
				bestMatch = item;
			}
		}
		return bestMatch;
	}

	private void PushPosToCamera(Vector2 posChange)
    {
        _camera.Position = _camera.Position -= posChange / new Vector2(CurrentCamZoom,CurrentCamZoom);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
		_camera.Zoom = _camera.Zoom.Lerp(new Vector2(CurrentCamZoom, CurrentCamZoom), 0.2f);
    }

	private void UiUpdateAll()
    {
		UiClearAll();

		foreach (var key in CurrentSession.ProjectData.Vertices.Keys)
		{
			if (CurrentSession.ProjectData.Vertices.TryGetValue(key, out Vertex vertex))
			{
				Vector2 pos2d = new Vector2(vertex.Position.X, vertex.Position.Z);
				UiDrawVertexAt(key, pos2d);
			}
		}
        
		Generate3dService.GenerateAll();

        ConsoleService.Print("Sketch2DWindow: View updated with project data.");
    }

	private void UiClearAll()
    {
        foreach (var child in _drawedItemsRoot.GetChildren())
        {
			//DrawedItemsRoot.RemoveChild(child);
            child.QueueFree();
        }
    }

	private void UiDrawVertexAt(int id , Vector2 pos)
    {
        var newScene = _vert.Instantiate();
		_drawedItemsRoot.AddChild(newScene);
		S2dRenderedVert VertScript = newScene as S2dRenderedVert;
		VertScript.Init(id, pos, this);
    }
}
