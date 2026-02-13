using Godot;
using System;

public partial class ControlsLogic : TabContainer
{
	public override void _Ready()
	{
		TabChanged += (t) =>
		{
			switch (t)
			{
				case 0:
					BuildmodeService.I.SwitchToolTo(BuildmodeService.Tool.Selection);
					break;
				case 1:
					BuildmodeService.I.SwitchToolTo(BuildmodeService.Tool.Freeform);
					break;
			}
		};

		BuildmodeService.OnToolSelected += (tool) =>
		{
			if (tool == BuildmodeService.Tool.Selection)
			{
				SetTabTitle(0, "Selecting...");
				SetTabDisabled(0, false);
				SetTabDisabled(1, true);
				SetTabTitle(1, "Object");
				SetTabDisabled(2, true);
				SetTabDisabled(3, true);
				SetTabDisabled(4, true);
				SetTabDisabled(5, true);
			};
		};

		BuildmodeService.OnObjectSelected += (name) =>
		{
			SetTabTitle(0, "Select new");
			SetTabTitle(1, name.ToString());
			SetTabDisabled(1, false);
			SetTabDisabled(2, false);
        	SetTabDisabled(3, false);
        	SetTabDisabled(4, false);
			SetTabDisabled(5, false);
			BuildmodeService.I.SwitchToolTo(BuildmodeService.Tool.Object);
			CurrentTab = 1;
		};
	}
}
