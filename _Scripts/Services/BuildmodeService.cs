using Godot;
using System;

public partial class BuildmodeService : Node
{
	public static BuildmodeService I;

    // events
	public static event Action<Tool> OnToolSelected;

    public static event Action<Node3D> OnObjectSelected;
    public static event Action<Node3D> OnObjectDeselected;

    // enums
    public enum BuildingMode { Simple, Advanced }
    public enum Tool { Selection, Object, Freeform, Move, Rotate, Scale }

    // states
    public BuildingMode CurrentMode { get; private set; } = BuildingMode.Simple;
    public Tool CurrentTool { get; private set; } = Tool.Selection;
	public Node3D CurrentSelected { get; private set; } = null;

	// vars
	public float GridLockStepVal { get; set; } = 0.5f;

	public const int RayLength = 1000;

	public void Select(Node3D obj)
	{
		if (obj is null) return;
		if (obj == CurrentSelected) return;
		CurrentSelected = obj;
		Prop currentProp = CurrentSelected as Prop;
    	OnObjectSelected?.Invoke(obj);
		GD.Print($"BuildModeService: New Prop selected, UID <{currentProp.Identity.UID}>, DispName <{currentProp.Identity.DispName}>");
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
			if (tool == Tool.Selection)
				Deselect();

			CurrentTool = tool;
			OnToolSelected?.Invoke(tool);
		}
		GD.Print($"BuildmodeService: Switched tool to {tool}");
		
	}

    public override void _Ready()
    {
        I = this;
		OnToolSelected?.Invoke(Tool.Selection);
    }

	public void SpawnNewObject(string uid)
	{
		Select(FactoryService.I?.TrySpawningUidAndGetNode(uid));
	}

}
