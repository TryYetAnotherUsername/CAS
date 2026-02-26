using Godot;
using System;
using System.Collections.Generic;

public partial class WindowService : Node
{
	public static WindowService I;

	// Export
	[Export] private Control _windowsRoot;
	[Export] private PackedScene _windowBase;

	// Links WindowContent enums to content PackedScenes and also display names
	private static readonly Dictionary<EWindowContent, (string dispName, string UID)> _windowContentIndex = new()
    {
        { EWindowContent.None, ("New window", "") },
		{ EWindowContent.Properties, ("Inspect", "uid://dga1dp2fut3cf") },
		{ EWindowContent.Messages, ("Messages", "uid://bh0t8bq1lj75o") }
    };


	public enum EWindowContent
	{
		None, Properties, Messages
	};

    public override void _Ready()
    {
		I = this;

    }

	/// <Summary>
	/// Spawns a new window
	/// </Summary>
	/// <returns>
	/// (Control) content scene root
	/// </returns>
	public Control NewWindow(EWindowContent content= EWindowContent.None)
    {
        GD.Print("WindowService: Trying to spawn a new window");

        var windowBase = _windowBase.Instantiate();
        _windowsRoot.AddChild(windowBase);

        var contLayer = windowBase.FindChild("ContentLayer", true);
        Label title = windowBase.FindChild("Title", true) as Label;

        var scene = TryGetScene(content);
        if (scene is null) return null;
        var sceneInst = scene.Instantiate();

        title.Text = GetDispName(content);

        contLayer.AddChild(sceneInst);

        GD.Print("WindowService: Finished spawning a new window. Returned the scene.");
		return sceneInst as Control;

		// ===== local methods =====
        static string GetDispName(EWindowContent targCont)
        {
            if (_windowContentIndex.TryGetValue(targCont, out (string, string) value))
            {
                return value.Item1;
            }
			else
			{
				return "New window";
			}
        }
    }

    private PackedScene TryGetScene(EWindowContent targCont)
	{
		string uid = "";

		if (_windowContentIndex.TryGetValue(targCont, out (string, string) value))
		{
			uid = value.Item2;
		}

		// Convert the string uid into non-human friendly uid, then into a path:
		long numberUid = ResourceUid.TextToId(uid);
		if (!ResourceUid.HasId(numberUid)) // validation
		{
			GD.PrintErr($"WindowService: Could not find UID <{uid}>. Returning.");
			return null;
		}
		var path = ResourceUid.GetIdPath(numberUid);

		// Return packedscene:
		var scene = GD.Load<PackedScene>(path);
		return scene;
	}
}
