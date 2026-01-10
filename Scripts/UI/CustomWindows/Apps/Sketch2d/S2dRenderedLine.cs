using Godot;
using System;

public partial class S2dRenderedLine : Line2D
{
	[Export] public float Thickness = 1f;
	[Export] public Color LineColor = Color.FromHtml("#ffffffff");


	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
    {
        GD.Print(Owner.Name);
		WinContSketch2d MyController = Owner as WinContSketch2d;
		
		ScaleTo(MyController.CurrentCamZoom);
		MyController.OnCamZoomChangedTo += ScaleTo;
    }

	public void InitWall(Vector2 start, Vector2 end)
	{
		Points = new Vector2[] {start, end};
	}

	private void ScaleTo(float zoomLevel)
    {
        Width = Thickness / zoomLevel; 
    }
}
