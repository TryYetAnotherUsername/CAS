using Godot;
using System;

public partial class MoveTool : BuildmodeTool
{
    [Export] PackedScene _moveGizmo;
    private Node _moveGizmoI;

    public override void _Ready()
    {
        _moveGizmoI = _moveGizmo.Instantiate();

        BuildmodeService.OnToolSelected += (tool) =>
        {
            SetMoving(tool == BuildmodeService.Tool.Move);
        };

    }

    public void SetMoving(bool state)
    {
        if (state)
        {
            Node selected = BuildmodeService.I.CurrentSelected;

            // Guard: nothing selected
            if (selected == null) return;

            // Guard: already parented (avoid duplicate add)
            if (_moveGizmoI.GetParent() != null)
                _moveGizmoI.GetParent().RemoveChild(_moveGizmoI);

            selected.AddChild(_moveGizmoI);
        }
        else
        {
            // Remove gizmo if it has a parent
            if (_moveGizmoI.GetParent() != null)
                _moveGizmoI.GetParent().RemoveChild(_moveGizmoI);
        }
    }
}