using Godot;
using System;

public partial class ControlsLogic : TabContainer
{
	public override void _Ready()
	{
		TabClicked += (t) =>
		{
			switch (t)
			{
				case 0:
					BuildmodeService.I.SwitchToolTo(BuildmodeService.Tool.Selection);
					break;
				case 1:
					BuildmodeService.I.SwitchToolTo(BuildmodeService.Tool.Object);
					break;
				case 2:
					BuildmodeService.I.SwitchToolTo(BuildmodeService.Tool.Freeform);
					break;
				case 3:
					BuildmodeService.I.SwitchToolTo(BuildmodeService.Tool.Move);
					break;
				case 4:
					BuildmodeService.I.SwitchToolTo(BuildmodeService.Tool.Rotate);
					break;
				case 5:
					BuildmodeService.I.SwitchToolTo(BuildmodeService.Tool.Scale);
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
