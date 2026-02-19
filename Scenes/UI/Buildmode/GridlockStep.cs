using Godot;
using System;

public partial class GridlockStep : SpinBox
{
	public override void _Ready()
	{
		ValueChanged += UpdatePrecision;
	}

	public void UpdatePrecision(double value)
	{
		BuildmodeService.I.GridLockStepVal = (float) value;
	}
}
