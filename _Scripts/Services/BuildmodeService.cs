using Godot;
using System;

public partial class BuildmodeService : Node
{
	public static BuildmodeService I;

    // events
	public static event Action<Tool> OnToolSelected;

    public static event Action<Prop> OnObjectSelected;
    public static event Action<Prop> OnObjectDeselected;

    // enums
    public enum BuildingMode { Simple, Advanced }
    public enum Tool { Selection, Object, Freeform, Move, Rotate, Scale }

    // states
    public BuildingMode CurrentMode { get; private set; } = BuildingMode.Simple;
    public Tool CurrentTool { get; private set; } = Tool.Selection;
	public Prop CurrentSelected { get; private set; } = null;

	public const int RayLength = 1000;

	public void Select(Prop obj)
	{
		if (obj is null) return;
		if (obj == CurrentSelected) return;
		CurrentSelected = obj;
    	OnObjectSelected?.Invoke(obj);
		GD.Print($"BuildModeService: New object of name <{obj.Name}> selected.");
	}

	public void Deselect()
	{
    	OnObjectDeselected?.Invoke(CurrentSelected);
		CurrentSelected = null;
	}

	public void SwitchToolTo(Tool tool)
	{
		if (!(CurrentTool == tool))
		{
			CurrentTool = tool;
			OnToolSelected?.Invoke(tool);
			if (tool == Tool.Selection)
				Deselect();
		}
		GD.Print($"BuildmodeService: Switched tool to {tool}");
		
	}

    public override void _Ready()
    {
        I = this;
		OnToolSelected?.Invoke(Tool.Selection);
    }

	public void NewObject(PackedScene scene)
	{
		
	}

}
