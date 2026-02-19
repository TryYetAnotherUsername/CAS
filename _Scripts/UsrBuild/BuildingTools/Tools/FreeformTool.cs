using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class FreeformTool : BuildmodeTool
{
    private Camera3D _camera;
    private bool _active = false;
    private bool _isDragging = false;

    public override void _Ready()
    {
        _camera = GetViewport().GetCamera3D();

        BuildmodeService.OnToolSelected += (tool) =>
        {
            if (tool == BuildmodeService.Tool.Freeform)
            {
                SetActive(true);
            }
            else
            {
                SetActive(false);
            }
        };
    }

    private void SetActive(bool state)
    {
        _active = state;
        _isDragging = false;
        SetProcessInput(state);
    }

    public override void _Input(InputEvent e)
    {
        if (!_active)
            return;

        if (e.IsActionPressed("build_gizmo_drag"))
        {
            if (IsMouseOverSelected())
                _isDragging = true;
        }

        // Mouse released
        if (e.IsActionReleased("build_gizmo_drag"))
        {
            _isDragging = false;
        }

        // Mouse motion while dragging
        if (_isDragging && e is InputEventMouseMotion)
        {
            TryPlaceAtMouse();
        }

        if (e.IsActionPressed("build_gizmo_rotate"))
        {
            Node3D selected = BuildmodeService.I.CurrentSelected;
            if (selected == null)
                return;
            var tween = CreateTween();
            tween.TweenProperty(selected, "rotation_degrees:y", selected.RotationDegrees.Y + 90, 0.15);
        }
    }

    private bool IsMouseOverSelected()
    {
        Node3D selected = BuildmodeService.I.CurrentSelected;
        if (selected == null)
            return false;

        var result = Raycast(false);
        if (result.Count == 0)
            return false;

        if (!result.ContainsKey("collider"))
            return false;

        var hit = result["collider"].As<Node>();


        var body1 = FindBodyByKeyword(selected, "mesh");
        var body2 = FindBodyByKeyword(selected, "physics");

        return hit == body1 || hit == body2;
    }

    private StaticBody3D FindBodyByKeyword(Node3D parent, string keyword)
    {
        foreach (Node meshInst in parent.GetChildren())
        {
            if (meshInst.Name.ToString().Contains(keyword))
                return meshInst.GetChild(0) as StaticBody3D;
        }
        return null;
    }

    private void TryPlaceAtMouse()
    {
        var result = Raycast(true);

        if (result.Count == 0)
            return;

        if (!result.ContainsKey("position"))
            return;

        Node3D selected = BuildmodeService.I.CurrentSelected;
        if (selected == null)
            return;


        var tween = CreateTween();
        tween.TweenProperty(selected, "global_position", ((Vector3)result["position"]).Snapped(BuildmodeService.I.GridLockStepVal), 0.1);
    }


    private Godot.Collections.Dictionary Raycast(bool excludeSelected)
    {
        var mousePos = GetViewport().GetMousePosition();
        var rayOrigin = _camera.ProjectRayOrigin(mousePos);
        var rayEnd = rayOrigin + _camera.ProjectRayNormal(mousePos) * BuildmodeService.RayLength;

        var query = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);
        query.CollisionMask = 1;

        if (excludeSelected)
        {
            Node3D selected = BuildmodeService.I.CurrentSelected;

            if (selected != null)
            {
                var exclusions = new Godot.Collections.Array<Rid>();

                var body1 = FindBodyByKeyword(selected, "mesh");
                var body2 = FindBodyByKeyword(selected, "physics");

                if (body1 != null) exclusions.Add(body1.GetRid());
                if (body2 != null) exclusions.Add(body2.GetRid());

                query.Exclude = exclusions;
            }
        }

        return GetViewport()
            .GetWorld3D()
            .DirectSpaceState
            .IntersectRay(query);
    }
}
