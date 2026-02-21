using Godot;
using System;

public partial class BuildmodeTool : Node
{
	public override void _UnhandledInput(InputEvent @event)
    {
        // Global logic: All tools listen for 'E' to quit
        if (@event.IsActionPressed("build_conf")) 
        {
            BuildmodeService.I.SetToolTo(BuildmodeService.Tool.Object);
			GD.Print("exiting to object tool");
        }
    }
}
