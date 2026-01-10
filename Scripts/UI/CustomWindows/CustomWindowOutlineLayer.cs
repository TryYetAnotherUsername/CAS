using Godot;
using System;

public partial class CustomWindowOutlineLayer : Panel
{
	[Export] private CustomWindow _customWindow;
	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton) 
		{
			if (mouseButton.Pressed)
			{
				_customWindow.BringToFocus();
				
			}
		}
	}
}
