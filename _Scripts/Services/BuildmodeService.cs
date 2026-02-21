using Godot;
using System;

public partial class BuildmodeService : Node
{
    // Singleton:
    public static BuildmodeService I;

    // Dependencies:
    [Export] public Control _BuildToolUi;

    // Enums:
    public enum BuildingMode { Simple, Advanced }
    public enum Tool { None, Selection, Object, Freeform, Move, Rotate, Scale }

    // Events:
    public static event Action<Tool> OnToolSelected;
    public static event Action OnBuildModeStart;
    public static event Action<Node3D> OnObjectSelected;
    public static event Action<Node3D> OnObjectDeselected;

    // States:
    public BuildingMode CurrentMode { get; private set; } = BuildingMode.Simple;
    public Tool CurrentTool { get; private set; } = Tool.None;
    public Node3D CurrentSelected { get; private set; } = null;

    // Vars:
    public const int RayLength = 1000;
    public float GridLockStepVal { get; set; } = 0.5f;

    // Godot native methods:
    public override void _Ready()
    {
        I = this;
        ToolService.OnUpdate += (tool) =>
        {
            if (tool == ToolService.ETools.Buildmode)
                SetBuildModeEnabled(true);
        };
        SetToolTo(Tool.None);
        GD.Print("==== BuildmodeService: Init");
    }

    // API methods:
    public void SetBuildModeEnabled(bool status)
    {
        Deselect();
        if (status)
        {
            GD.Print("BuildmodeSevice: Start buildmode");
            SetToolTo(Tool.Selection);
            OnBuildModeStart?.Invoke();
        }
        else
        {
            GD.Print("BuildmodeSevice: End buildmode");
            SetToolTo(Tool.None);
        }
    }

    public void SetToolTo(Tool tool)
    {
        if (CurrentTool != tool)
        {
            if (tool == Tool.Selection)
                Deselect();
            CurrentTool = tool;
            OnToolSelected?.Invoke(tool);
        }
        if (CurrentTool == Tool.None)
        {
            Deselect();
            CurrentTool = Tool.None;
            OnToolSelected?.Invoke(tool);
        }
        GD.Print($"BuildmodeService: Switched tool to {tool}");
    }

    public void Select(Node3D obj)
    {
        if (obj is null) return;
        if (obj == CurrentSelected) return;
        CurrentSelected = obj;
        Prop currentProp = CurrentSelected as Prop;
        OnObjectSelected?.Invoke(obj);
        GD.Print($"BuildModeService: New Prop selected, UID <{currentProp.Identity.UID}>, DispName <{currentProp.Identity.DispName}>");
    }

    public void SpawnNewObject(string uid)
    {
        Select(FactoryService.I?.TrySpawningUidAndGetNode(uid));
    }

    // Internal methods:

    private void Deselect()
    {
        OnObjectDeselected?.Invoke(CurrentSelected);
        CurrentSelected = null;
        if (PathfindingService.I is not null)
        {
            PathfindingService.I?.StartBakingRegion();
        }
    }
}