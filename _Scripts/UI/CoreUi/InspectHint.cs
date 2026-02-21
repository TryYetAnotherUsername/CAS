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
        //var mousePos = GetViewport().GetMousePosition();
		//Position = Position.Lerp(mousePos, 0.2f);
    }

	private void Start()
	{
		SetProcess(true);
		_aniPlayer.Play("enter");
	}

	private void End()
	{
		SetProcess(false);
		_aniPlayer.PlayBackwards("enter");
	}
}
