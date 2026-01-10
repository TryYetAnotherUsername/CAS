using System;
using System.Collections.Generic;

public static class Relay
{
    // Dependencies
	public static FileOpService FileOpService { get; set; }
    public static WindowService WindowService { get; set; }
    public static Sketch2dService Sketch2DService { get; set; }
    public static NotificationService NotifService { get; set; }

    // Initialisation
    private static readonly Dictionary<string, Action> CommandMap = new();
    private static readonly Dictionary<string, Action<string>> DataCommandMap = new();

    static Relay()
    {

        // ================= COMMAND MAPS =================

        // window
        DataCommandMap.Add("window.new", (appName) => WindowService.NewWindow(appName));
        CommandMap.Add("window.closefocused", () => WindowService.CloseFocusedWindow());

        // sketch2d
        DataCommandMap.Add("sketch2d.newvertex", (position) => Sketch2DService.MakeVertex(position));
        
        // fileop
        CommandMap.Add("fileop.save", () => FileOpService.SaveFile());
        CommandMap.Add("fileop.new", () => FileOpService.NewFile());
        CommandMap.Add("fileop.open", () => FileOpService.OpenFile());

        // ================================================

        ConsoleService.Print("exec: Commands ready");
        ConsoleService.LineBreak();
    }
    public static void Exe(string key,string argument = "")
    {
		key = key.ToLower();
        key = key.Trim();
        
        ConsoleService.Print($"Relay: Attempting to invoke command <{key}>");
        if (DataCommandMap.TryGetValue(key, out var daction))
        {
            daction.Invoke(argument);
            ConsoleService.Print($"Relay: Finished invoking data command <{key}> with argument <{argument}>.");
        }
        else if (CommandMap.TryGetValue(key, out var action))
        {
            action.Invoke();
            ConsoleService.Print($"Relay: Finished invoking command <{key}>.");
        }
        else
        {
            ConsoleService.PrintErr($"Relay: Command <{key}> not found.");
        }
        ConsoleService.LineBreak();
    }
}