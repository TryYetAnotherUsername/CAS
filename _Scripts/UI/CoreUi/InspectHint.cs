using Godot;
using System;

public partial class InspectHint : Panel
{
	[Export] AnimationPlayer _aniPlayer;

	public override void _Ready()
	{
		LiveSelectService.OnHoverStart += Start;
		LiveSelectService.OnHoverEnd += End; 
	}

    public override void _Process(double delta)
    {
        var mousePos = GetViewport().GetMousePosition();
		Position = mousePos;
    }

	private void Start()
	{
		_aniPlayer.Play("enter");
	}

	private void End()
	{
		_aniPlayer.PlayBackwards("enter");
	}
}
