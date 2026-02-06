using Godot;
using System;
using System.Collections.Generic;

public partial class MenuCommandBindings : PopupMenu
{
	private Dictionary<int, Action> commandIndex;

	public override void _Ready()
	{
		commandIndex = new()
		{
			// File
			{0, () => FileOpService.I.Open()}, // Save
			{1, () => FileOpService.I.Open()}, // Open
			{2, () => FileOpService.I.Open()}, // New project
			{3, () => WindowService.I.NewWindow()}, // Get info

			// Tools
			{4, () => WindowService.I.NewWindow()}, // Build
		};

		IdPressed += IdToMethod;
	}

	private void IdToMethod(long id)
	{
		GD.Print(id);
		int iId = (int)id;  // Cast instead of ToString().ToInt()
		
		if (commandIndex.TryGetValue(iId, out Action command))
		{
			command?.Invoke();
		}
	}
}
