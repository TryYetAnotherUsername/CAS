using Godot;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

public partial class BuildToolService : Node
{
    // Buildmode revamp- old buildmode is called OldBuildMode. Gud luck timmy :)

    public static BuildToolService I;

    // enums
    public enum EState
    {
        OpenForSelection, PlacingFromCatalog, Freeform, Sleep
    }

    // public vars
    public float GridLockStep = 0.5f;
    public const int RayLength = 2000;

    // private vars
    private EState _currentState = EState.Sleep;
    private Prop _currentProp;
    private CatalogEntity _reqPropEnt;

    // events
    public static Action <Prop> OnPlacingNewPropType;
    public static Action OnOpenForSelect;
    public static Action<Prop> OnPropSelect;

    #region <Godot funcs>
    public override void _Ready()
    {
        I = this;
        ToolService.OnUpdate += (tool) =>
        {
            if (tool == ToolService.ETools.BuildToolWindow)
            {
                WindowService.I.NewWindow(WindowService.EWindowContent.BuildWindow);
            }
        };
    }

    public override void _Process(double delta)
    {
        switch (_currentState)
        {
            case EState.PlacingFromCatalog:
                var targPos = RayGetPos();
                // snap to grid
                var tween = CreateTween();
                tween.TweenProperty(_currentProp, "global_position", SnapToGrid(targPos, GridLockStep), 0.1);
                break;
            case EState.Freeform:
                var targPos2 = RayGetPos();
                // snap to grid
                var tween2 = CreateTween();
                tween2.TweenProperty(_currentProp, "global_position", SnapToGrid(targPos2, GridLockStep), 0.1);
                break;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("build_select")) // clicks
        {
            // this handles clicks during the auto place from catalogue freeform, so this creates a new instance
            if (_currentState == EState.PlacingFromCatalog)
            {
                if (EconomyService.I.TryTakeCash(_reqPropEnt.Cost) == false)
                {
                    _currentProp.QueueFree();
                    NotificationService.I.Print($"⚠️ You are £{MathF.Round(_reqPropEnt.Cost - EconomyService.I.Cash, 2)} short to place {_reqPropEnt.DispName}.)");
                    OpenForSelection();
                    return;
                }
                _currentProp.SetCollision(true);
                _currentProp.AnimatePlace();
                GD.Print("Placed- Continue placing");
                KeepPlacing();
            }

            // this handles clicks while in freeform (place object)
            else if (_currentState == EState.Freeform)
            {
                GD.Print("Freeform- placed");
                OpenForSelection();
            }

            // this handles clicks while nothing is active (so new editing target)
            else if (_currentState == EState.OpenForSelection)
            {
                var prop = RayGetProp();
                if (prop is null) return;
                GD.Print("new select");
                StartFreeform(prop);
            }

        }
        if (@event.IsActionPressed("build_gizmo_rotate"))
        {
            if (_currentState == EState.PlacingFromCatalog)
            {
                // rotate
                var tween = CreateTween();
                tween.TweenProperty(_currentProp, "rotation_degrees:y", _currentProp.RotationDegrees.Y + 90, 0.2);
            }
        }
    }

    #endregion <godot funcs>


    #region <public methods>
    public void StartPlacingFromCatalog(CatalogEntity catEnt)
    {
        OpenForSelection();
        _reqPropEnt = catEnt;

        GD.Print("BuildTool: Starting placement");

        var node = FactoryService.I.TrySpawningUidAndGetNode(catEnt.UID);

        if (node is Prop prop)
        {
            _currentProp = prop;
            _currentProp.SetCollision(false);

            var targPos = RayGetPos();
            _currentProp.GlobalPosition = targPos;

            SetState(EState.PlacingFromCatalog);
            OnPlacingNewPropType?.Invoke(prop);
        }
    }

    public void KeepPlacing()
    {
        GD.Print("BuildTool: Keep placing the same prop");
        var node = FactoryService.I.TrySpawningUidAndGetNode(_reqPropEnt.UID);

        if (node is Prop prop)
        {
            _currentProp = prop;
            _currentProp.SetCollision(false);

            var targPos = RayGetPos();
            _currentProp.GlobalPosition = targPos;

            SetState(EState.PlacingFromCatalog);
        }
    }

    public void StartFreeform(Prop prop)
    {
        GD.Print("BuildTool: Starting freeform placement");
        _currentProp = prop;
        _currentProp.SetCollision(false);
        _currentProp.AnimateSelect();

        SetState(EState.Freeform);
        OnPlacingNewPropType?.Invoke(prop);
        OnPropSelect?.Invoke(prop);
    }

    public void OpenForSelection()
    {
        if (_currentState == EState.PlacingFromCatalog)
        {
            _currentProp?.QueueFree();
        }
        else if (_currentState == EState.Freeform)
        {
            _currentProp.SetCollision(true);
            _currentProp.AnimatePlace();
            _reqPropEnt = null;
            _currentProp = null;
        }

        OnOpenForSelect?.Invoke();
        SetState(EState.OpenForSelection);
    }

    public void DeleteSelected()
    {
        if (_currentState == EState.OpenForSelection || _currentState == EState.PlacingFromCatalog || _currentState== EState.Sleep)
        {
            return;
        }

        _currentProp?.QueueFree();
        OnOpenForSelect?.Invoke();
        SetState(EState.OpenForSelection);
    }

    public void Sleep()
    {
        OpenForSelection();
        SetState(EState.Sleep);
    }

    public void Wake()
    {
        OpenForSelection();
    }

    #endregion <public methods>

    #region <priv methods>
    public void SetState(EState state)
    {
        _currentState = state;
    }

    private Prop RayGetProp()
	{
		var camera3D = GetViewport().GetCamera3D();
		var from = camera3D.ProjectRayOrigin(GetViewport().GetMousePosition());
		var to = from + camera3D.ProjectRayNormal(GetViewport().GetMousePosition()) * RayLength;

		var spaceState = GetViewport().World3D.DirectSpaceState;
		var options = PhysicsRayQueryParameters3D.Create(from, to);
		options.CollisionMask = 2;
		var result = spaceState.IntersectRay(options);
		Node3D selectedNode;

		if (result.Count > 0)
		{
			selectedNode = (Node3D)result["collider"];
            selectedNode = selectedNode.GetOwnerOrNull<Node3D>();
		}
		else
		{
			return null;
		}

        if (selectedNode is Prop prop)
        {
            return prop;
        }

        return null;
	}

    private Vector3 RayGetPos()
    {
        // adapted from gd docs
        var camera3D = GetViewport().GetCamera3D();
        var from = camera3D.ProjectRayOrigin(GetViewport().GetMousePosition());
        var to = from + camera3D.ProjectRayNormal(GetViewport().GetMousePosition()) * 1000f;
        var spaceState = GetViewport().World3D.DirectSpaceState;
        var options = PhysicsRayQueryParameters3D.Create(from, to);
        options.CollisionMask = 1;
        var result = spaceState.IntersectRay(options);


        if (result.Count > 0)
            return (Vector3)result["position"];

        return Vector3.Zero;
    }

    private Vector3 SnapToGrid(Vector3 pos, float step = 0.5f)
    {
        return pos.Snapped(step);
    }

    #endregion <priv methods>
}
