using Godot;
using System;

public partial class S2dRenderedVert : Sprite2D
{
	[Export] public float Size = 0.25f;
	[Export] public Color Colour;
	[Export] private float smoothFac = 2f;
	[Export] private Area2D area2D;

	private WinContSketch2d MyController;
	private Vector2 targScale;
	private bool isDragging;
	private int id;
	private Vector2 posTarg;

	// Called when the node enters the scene tree for the first time.
	public override void _Process(double delta)
	{
		Scale = Scale.Lerp(targScale, (float)(smoothFac * delta));
	}

	public override void _Input(InputEvent @event)
	{
		if (!isDragging) return;
		
		if (@event.IsActionReleased("viewport_select"))
		{
			isDragging = false;
			SaveMyPosition();
			return;
		}

		if (isDragging && @event is InputEventMouseMotion mouseMovement)
		{
			var p = Position;
			posTarg += mouseMovement.Relative / MyController.CurrentCamZoom;
			p.X = MathF.Round(posTarg.X / MyController.SnapMovement) * MyController.SnapMovement;
			p.Y = MathF.Round(posTarg.Y / MyController.SnapMovement) * MyController.SnapMovement;
			Position = p;
		}
	}

	public void StartDragging()
	{
		posTarg = Position;
		isDragging = true;
	}

	public void Init(int inid, Vector2 pos, WinContSketch2d controller)
	{
		Position = pos;
		MyController = controller;
		
		float currentZoom = MyController.CurrentCamZoom;
		targScale = new Vector2(Size / currentZoom, Size / currentZoom);

		Scale = targScale; 

		id = inid;

		MyController.OnCamZoomChangedTo += ScaleTo;
	}

	private void ScaleTo(float zoomLevel)
    {
        targScale = new Vector2(Size/zoomLevel, Size/zoomLevel); 
    }

    public override void _ExitTree()
    {
        MyController.OnCamZoomChangedTo -= ScaleTo;
    }

	private void SaveMyPosition()
	{
		Vector3 dataPos = new Vector3(Position.X, 0, Position.Y);

		if (CurrentSession.ProjectData.Vertices.ContainsKey(id))
		{
			CurrentSession.ProjectData.Vertices[id].Position = dataPos;
		}
	}
}
