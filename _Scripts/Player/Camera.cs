using Godot;
using System;

public partial class Camera : Node3D
{
    [Export] private Node3D _PivotH;
    [Export] private Node3D _PivotV;
    [Export] private SpringArm3D _SpringArm;
	[Export] private Camera3D _PlayerCam;
    [Export] public float zoomSpeed = 0.2f;
    [Export] public float mouseSens = 0.005f;
    
    private float TargSpringArmLength;
    private bool _isDragging = false;

    private Vector2 _initialMousePos;
    
    public override void _UnhandledInput(InputEvent @event)
    {
        // Handle drag button press/release
        if (@event.IsActionPressed("player_camera_drag"))
        {
            _initialMousePos = GetViewport().GetMousePosition();
            _isDragging = true;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
        else if (@event.IsActionReleased("player_camera_drag"))
        {
            ResetMouse();
        }
        
        // Handle mouse motion while dragging
        if (@event is InputEventMouseMotion motion && _isDragging)
        {
            _PivotH.RotateY(-motion.Relative.X * mouseSens);
            _PivotV.RotateX(-motion.Relative.Y * mouseSens);
            
            // Clamp vertical rotation
            Vector3 rotation = _PivotV.Rotation;
            rotation.X = Mathf.Clamp(rotation.X, Mathf.DegToRad(-60), Mathf.DegToRad(70));
            _PivotV.Rotation = rotation;
        }
        
        // Camera zoom in/out controls
        if (@event is InputEventMouseButton eventAction)
        {
            if (eventAction.IsActionPressed("player_camera_zoom_in"))
            {
                TargSpringArmLength -= zoomSpeed*_SpringArm.SpringLength;
            }
            else if (eventAction.IsActionPressed("player_camera_zoom_out"))
            {
                TargSpringArmLength += zoomSpeed*_SpringArm.SpringLength;
            }
        }
    }

    public override void _Process(double delta)
    {
        _SpringArm.SpringLength = Mathf.Lerp(_SpringArm.SpringLength, TargSpringArmLength, 0.3f);

        if (Input.IsActionJustReleased("player_camera_drag"))
        {
            ResetMouse();
            Input.WarpMouse(_initialMousePos);
        }
    }

    private void ResetMouse()
    {
        _isDragging = false;
        Input.MouseMode = Input.MouseModeEnum.Visible;
        GD.Print(_initialMousePos);
        Input.WarpMouse(_initialMousePos);
    }


    public override void _Ready()
    {
        TargSpringArmLength = 5f;
    }


}
