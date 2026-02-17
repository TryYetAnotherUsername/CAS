using Godot;
using System;
using System.Collections.Generic;

public partial class WindowService : Node
{
	public static WindowService I;

	// Export
	[Export] private Control _WindowsRoot;
	[Export] private PackedScene _WindowBase;

	// All window content packed scenes:
	[Export] private PackedScene _Content_Console;

	// Links WindowContent enums to content PackedScenes and also display names

	private Dictionary<WindowContent, (string dispName, PackedScene scene)> windowContentIndex;

	public enum WindowContent
	{
		None, Console, 
	};

    public override void _Ready()
    {
		I = this;
        windowContentIndex = new()
		{
			{WindowContent.Console, ("CAS User console", _Content_Console)},
			{WindowContent.None, ("New Window", null)}
		};
    }


	public void NewWindow(WindowContent content= WindowContent.None)
    {
        var newWindowInst = _WindowBase.Instantiate();

		if (_WindowBase == null || newWindowInst == null)
		{
			GD.Print("hi");
		}
		
		var newWindowScript = newWindowInst as WindowBase;
		
		PackedScene contentScene = windowContentIndex[content].scene;
		var contSceneNode = (Control) contentScene.Instantiate();

		newWindowScript.Init(windowContentIndex[content].dispName, contSceneNode);

		_WindowsRoot.AddChild(newWindowInst);

		if (content == WindowContent.None)
		{
			
		}
		else
		{
			
		}
	}
}
