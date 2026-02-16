using Godot;
using System;

public partial class ScaleTool : BuildmodeTool
{
    [Export] PackedScene _scaleGizmo;
    private Node _scaleGizmoI;

    public override void _Ready()
    {
        _scaleGizmoI = _scaleGizmo.Instantiate();

        BuildmodeService.OnToolSelected += (tool) =>
        {
			GD.Print("scale tool selected");
            SetScaling(tool == BuildmodeService.Tool.Scale);
        };
    }

    public void SetScaling(bool state)
    {
        if (state)
        {
            Node selected = BuildmodeService.I.CurrentSelected;

            // Guard: nothing selected
            if (selected == null) return;

            // Guard: already parented (avoid duplicate add)
            if (_scaleGizmoI.GetParent() != null)
                _scaleGizmoI.GetParent().RemoveChild(_scaleGizmoI);

            selected.AddChild(_scaleGizmoI);
        }
        else
        {
            // Remove gizmo if it has a parent
            if (_scaleGizmoI.GetParent() != null)
                _scaleGizmoI.GetParent().RemoveChild(_scaleGizmoI);
        }
    }
}