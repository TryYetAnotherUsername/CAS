using Godot;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class MoneyCounter : Panel
{
	[Export] private AnimationPlayer _aniPlayer;
	[Export] private Label _label;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EconomyService.OnCashChanged += ShowNewCash; 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void ShowNewCash(float cashAmount)
	{
		_label.Text = "£ " + MathF.Round(cashAmount).ToString();
		_aniPlayer.Play("tick");
	}
}
