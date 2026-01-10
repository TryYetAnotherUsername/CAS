using Godot;
using System;
using System.ComponentModel;
using System.Security.AccessControl;

public partial class Generate3dService : Node
{
    [Export] private Node3D _drawnItemsRoot;

    private static Generate3dService _internalThis;

    [Export] private PackedScene _vertexScene;

    public override void _Ready()
    {
        _internalThis = this;
 
    }


    public static void GenerateAll()
    {
        _internalThis?.ClearAllBuilt();
        _internalThis?.Generate();
    }

    private void ClearAllBuilt()
    {
        foreach(Node3D node in _drawnItemsRoot.GetChildren())
        {
            node.QueueFree();
        }
    }

    private void Generate()
    {
        foreach(Vertex vertex in CurrentSession.ProjectData.Vertices.Values)
        {
            Vector3 pos = vertex.Position;
            var vertInst = _vertexScene.Instantiate();
            _drawnItemsRoot.AddChild(vertInst,true);
            var vertexN3d = (Node3D) vertInst;
            vertexN3d.Position = pos;
        }
    }
}
