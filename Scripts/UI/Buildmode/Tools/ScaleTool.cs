using Godot;
using System;

public partial class ScaleTool : BuildmodeTool
{
	[Export] PackedScene _ScaleGizmo;
	public override void _Ready()
	{
		BuildmodeService.OnToolSelected += (tool) => 
		{
			if (tool == BuildmodeService.Tool.Scale)
			{
				SetScaling(true);
			}
			else
			{
				SetScaling(false);
			}
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void SetScaling(bool state)
	{
		SetProcessUnhandledInput(state);
		SetProcess(state);
	}


}
