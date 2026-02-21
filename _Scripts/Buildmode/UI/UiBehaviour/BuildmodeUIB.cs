using Godot;
using System;

public partial class BuildmodeUIB : Control
{
	[Export] private Button _exitButton;
	[Export] private AnimationPlayer _aniPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;
		_exitButton.Pressed += End;
		BuildmodeService.OnBuildModeStart += Start;
	}

	private void Start()
	{
		GD.Print("Buildmenu Ui: starting start animation");
		_aniPlayer.Play("open_menu");
	}

	private void End()
	{
		GD.Print("Buildmenu Ui: starting close animation");
		BuildmodeService.I.SetBuildModeEnabled(false);
		_aniPlayer.PlayBackwards("open_menu");
	}


}
