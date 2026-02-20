using Godot;
using System;

public partial class MenuItemButtons : Button
{
	[Export] private ToolService.ETools _tool;
    public override void _Ready()
    {
        Pressed += () => ToolService.I.UseTool(_tool);
    }

}
