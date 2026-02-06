using Godot;
using System;
using System.Dynamic;
using System.Net.Sockets;

public partial class BuildmodeService : Node
{
	public static BuildmodeService I;

    // events
	public static event Action<Tool> OnNewToolSelected;

    public static event Action<Node3D> OnNewObjectSelected;
    public static event Action OnObjectDeselected;

    // enums
    public enum BuildingMode { Simple, Advanced }
    public enum Tool { Selection, Object, Freeform, Move, Rotate, Scale }

    // states
    public BuildingMode CurrentMode { get; private set; } = BuildingMode.Simple;
    public Tool CurrentTool { get; private set; } = Tool.Selection;
	public Node3D CurrentSelected { get; private set; } = null;

	public void Select(Node3D obj)
	{
		if (obj is null) return;
		if (obj == CurrentSelected) return;
		CurrentSelected = obj;
    	OnNewObjectSelected?.Invoke(obj);
		GD.Print("hi");
	}

	public void Deselect()
	{
    	CurrentSelected = null;
    	OnObjectDeselected?.Invoke();
	}

	public void SwitchToolTo(Tool tool)
	{
		if (!(CurrentTool == tool))
		{
			CurrentTool = tool;
			OnNewToolSelected?.Invoke(tool);
			if (tool == Tool.Selection)
				Deselect();
		}
		
	}

    public override void _Ready()
    {
        I = this;
		OnNewToolSelected?.Invoke(Tool.Selection);
    }

}
