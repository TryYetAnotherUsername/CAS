using Godot;
using Godot.Collections;
using System;

public partial class ToolService : Node
{
    public static ToolService I { get; set; }

    public override void _Ready()
    {
        I = this;
    }

    public enum ETools
    {
        Buildmode, Settings, BakePathfinding, SpawnACustomer, None
    }

    public static event Action <ETools> OnUpdate;
    public ETools CurrentTool;

    public void UseTool(ETools tool)
    {
        OnUpdate?.Invoke(tool);
    }

}
