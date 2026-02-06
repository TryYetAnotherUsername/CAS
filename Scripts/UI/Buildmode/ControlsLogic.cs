using Godot;
using System;

public partial class ControlsLogic : TabContainer
{
	public enum BuildingMode
	{
		simple,
		advanced
	}

	public enum Tool
	{
		freeform,
		move,
		rotate,
		scale,
		none
	}

	[Export] public CheckButton _ModeToggle;
	[Export] public Control _ConfHint;

	public static event Action<Tool> OnToolChanged;
	public bool listenToConf;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_ModeToggle.Toggled += (mode) =>
		{
			if (mode == true)
			{
				SetBuildingMode(BuildingMode.advanced);
			}
			else
			{
				SetBuildingMode(BuildingMode.simple);
			}
		};

		TabClicked += (i) =>
		{
			if (i == 0)
			{
				UseTool(Tool.none);
			}
			else if (i == 1)
			{
				UseTool(Tool.freeform);
			}
			else if (i == 2)
			{
				UseTool(Tool.move);
			}
			else if (i == 3)
			{
				UseTool(Tool.rotate);
			}
			else if (i == 4)
			{
				UseTool(Tool.scale);
			}
		};

		SetTabDisabled(2,true);
		SetTabDisabled(3,true);
		SetTabDisabled(4,true);

		_ConfHint.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent @event)
    {
		if (listenToConf)
		{
			if (Input.IsActionJustPressed("build_conf"))
			{
				UseTool(Tool.none);
			}
		}
    }



	private void UseTool(Tool tool)
	{
		if (tool != Tool.none)
		{
			_ConfHint.Visible = true;
			OnToolChanged?.Invoke(tool);
			listenToConf = true;
			GD.Print(tool);
		}
		else if (tool == Tool.none)
		{
			_ConfHint.Visible = false;
			OnToolChanged?.Invoke(Tool.none);
			listenToConf = false;
			CurrentTab = 0;
			GD.Print("end tool use");
		}
	}


	public void SetBuildingMode(BuildingMode bm)
	{
		if (bm == BuildingMode.simple)
		{
			SetTabDisabled(2,true);
			SetTabDisabled(3,true);
			SetTabDisabled(4,true);
		}
		else
		{
			SetTabDisabled(2,false);
			SetTabDisabled(3,false);
			SetTabDisabled(4,false);
		}
	}
}
