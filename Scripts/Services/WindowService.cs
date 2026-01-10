using Godot;
using System;
using System.Collections.Generic;

public partial class WindowService : Node
{
	Control WindowsRoot;
    private PackedScene targetScene;
    public static event Action DefocusAll;

    private static readonly Dictionary<string, (string dispName, string path)> AppData = new()
    {
        { "launchpad", ("Launchpad", "res://Scenes/UI/CustomWindow/Launcher/win_cont_launcher.tscn")},
        { "sketch2d", ("Sketch2D", "res://Scenes/UI/CustomWindow/Sketch2D/win_cont_sketch_2d.tscn")},
        { "console", ("Console", "res://Scenes/UI/CustomWindow/Console/win_cont_console.tscn")},
        { "fileoptions", ("File options", "res://Scenes/UI/CustomWindow/SessionManager/win_cont_session_manager.tscn")}
    };

    public override void _Ready()
    {
        Relay.WindowService = this;
        WindowsRoot = GetNode("/root/Main/WindowsRoot") as Control;
        targetScene = GD.Load<PackedScene>("res://Scenes/UI/CustomWindow/CustomWindow.tscn");
    }

    public void CloseFocusedWindow()
    {
        if (CustomWindow.ActiveWindow != null)
        {
            ConsoleService.Print("WindowService: Attempting to close active window.");
            CustomWindow.ActiveWindow.CloseWindow();
        }
        else
        {
            ConsoleService.Print("WindowService: Cannot close active window because there is no currently focused window, returning.");
        }
    }

	public void NewWindow(string appName)
    {

        if (appName == "")
        {
            ConsoleService.PrintErr("WindowService: No app defined, returning.");
            return;
        }
        
        else
        {
            string path;
            string dispName;

            if (!AppData.TryGetValue(appName, out var value))
            {
                ConsoleService.PrintErr("WindowService: No app found with this name, returning.");
                return;
            }
            else
            {
                path = value.path;
                dispName = value.dispName;
            }
            
            var newWindow = targetScene.Instantiate();

            DefocusAll?.Invoke();
            WindowsRoot.AddChild(newWindow);

            CustomWindow newWindowScript = newWindow as CustomWindow;
            Control newWindowControl = newWindow as Control;

            PackedScene targetContentScene = GD.Load<PackedScene>(path);
            var newContent = targetContentScene.Instantiate();
            newWindowControl.GetNode("%ContentLayer").AddChild(newContent);
            newWindowScript.Init(dispName);
            ConsoleService.Print($"WindowService: Finished creating new app <{appName}>");
        }
    }
}
