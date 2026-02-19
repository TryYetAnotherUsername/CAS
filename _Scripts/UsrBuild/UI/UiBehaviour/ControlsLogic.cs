using Godot;
using Godot.Collections;
using System;

public partial class ControlsLogic : TabContainer
{
	Dictionary<BuildmodeService.Tool, int>_enumToTabMap = new()
	{
		{BuildmodeService.Tool.Selection, 0},
		{BuildmodeService.Tool.Object, 1},
		{BuildmodeService.Tool.Freeform, 2},
		{BuildmodeService.Tool.Move, 3},
		{BuildmodeService.Tool.Rotate, 4},
		{BuildmodeService.Tool.Scale, 5},
	};

	private BuildmodeService.Tool GetToolFromTab(int tabIndex)
	{
		foreach (var kvp in _enumToTabMap)
		{
			if (kvp.Value == tabIndex)
			{
				return kvp.Key;
			}	
		}
		return BuildmodeService.Tool.Selection;
	}

	private int GetTabFromTool(BuildmodeService.Tool tool)
	{
		foreach (var kvp in _enumToTabMap)
		{
			if (kvp.Key == tool)
			{
				return kvp.Value;
			}	
		}
		return 0;
	}

	public override void _Ready()
	{
		TabClicked += (t) =>
		{
			BuildmodeService.I.SwitchToolTo(GetToolFromTab((int)t));
			if (t == 0)
			{
				SetTabTitle(0, "Selecting...");
				SetTabDisabled(0, false);
				SetTabDisabled(1, true);
				SetTabTitle(1, "Object");
				SetTabDisabled(2, true);
				SetTabDisabled(3, true);
				SetTabDisabled(4, true);
				SetTabDisabled(5, true);
			}
		};

		BuildmodeService.OnToolSelected += (tool) =>
		{
			if (tool == BuildmodeService.Tool.Selection) // if it is selection then grey out all other tabs for clarity
			{
				SetTabTitle(0, "Selecting...");
				SetTabDisabled(0, false);
				SetTabDisabled(1, true);
				SetTabTitle(1, "Object");
				SetTabDisabled(2, true);
				SetTabDisabled(3, true);
				SetTabDisabled(4, true);
				SetTabDisabled(5, true);
			}
			else // switch to the tab
			{
				CurrentTab = GetTabFromTool(tool);
			}
		};

		

		BuildmodeService.OnObjectSelected += (node) =>
		{
			GD.Print("hi");
			if (node is Prop prop)
			{
				SetTabTitle(0, "Select new");
				SetTabTitle(1, prop.Identity.DispName);
				SetTabDisabled(1, false);
				SetTabDisabled(2, false);
				SetTabDisabled(3, false);
				SetTabDisabled(4, false);
				SetTabDisabled(5, false);
				
				//Set tab to freeform for convenience
				CurrentTab = GetTabFromTool(BuildmodeService.Tool.Freeform);
				BuildmodeService.I.SwitchToolTo(BuildmodeService.Tool.Freeform);
			}
		};
	}
}
