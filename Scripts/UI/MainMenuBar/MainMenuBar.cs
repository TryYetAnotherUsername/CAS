using Godot;
using System.Collections.Generic;
using System;

public partial class MainMenuBar : PopupMenu
{

	public override void _Ready()
    {
        IdPressed += CallServ;
    }

    private readonly Dictionary <string,Action> CmdMap = new()
    {
        {"sketch_2d", () => Relay.Exe("window.new","sketch2d")},
        {"new_console_window", () => Relay.Exe("window.new","console")},
        {"save", () => Relay.Exe("fileop.save")},
        {"open_file", () => Relay.Exe("fileop.open")},
        {"new_file", () => Relay.Exe("fileop.new")},
        {"file_options", () => Relay.Exe("window.new","fileoptions")},
        
    };

	private void CallServ(long id)
    {
		int index = GetItemIndex((int)id);
        string key = GetItemText(index);

        key = key.ToSnakeCase();
        
		CmdMap.TryGetValue(key ,out var action);
        action?.Invoke();
    }
}
