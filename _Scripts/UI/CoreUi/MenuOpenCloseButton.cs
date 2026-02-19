using Godot;
using System;

public partial class MenuOpenCloseButton : Button
{
	public enum ButtonAction
	{
		Open, Close
	} 
	[Export] ButtonAction _actionMode;
	[Export] AnimationPlayer _animationPlayer;
	
	public override void _Ready()
	{
		if (_actionMode == ButtonAction.Open)
		{
			Pressed += () => _animationPlayer.Play("open_menu");
		}
		else
		{
			Pressed += () => _animationPlayer.Play("close_menu");
		}
	}
}
