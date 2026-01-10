using Godot;
using System;

public partial class Sketch2dService : Node
{
	public static WinContSketch2d CurrentWindow;

	// Called when the node enters the scene tree for the first time.
	public void MakeVertex(string arguments)
    {
        Vector3 position = ParseUtil.ToVector3(arguments);

		var key = CurrentSession.ProjectData.NextAvailableId;
		CurrentSession.ProjectData.Vertices.Add(key, new Vertex{Position = position});

		ConsoleService.Print($"Sketch2dService: New vertex created at <{position}>");

		CurrentSession.ProjectData.NextAvailableId += 1;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Ready()
    {
        Relay.Sketch2DService = this;
    }
}
