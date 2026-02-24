using Godot;
using System;

public partial class Camera : Node3D
{
    #region Export Variables
    [Export] private Node3D _PivotH;
    [Export] private Node3D _PivotV;
    [Export] private SpringArm3D _SpringArm;
	[Export] private Camera3D _PlayerCam;
    [Export] public float zoomSpeed = 0.2f;
    [Export] public float mouseSens = 0.005f;
    #endregion Export Variables

    #region Private Variables
    private float TargSpringArmLength;
    private bool _isDragging = false;
    private Vector2 _lastFrameMousePos;
    private Vector2 _initialMousePos;
    #endregion Private Variables
    
    #region Godot methods
    public override void _Ready()
    {
        TargSpringArmLength = 5f;
        _isDragging = false;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        // Camera zoom in/out controls
        if (@event.IsActionPressed("player_camera_zoom_in"))
        {
            TargSpringArmLength -= zoomSpeed*_SpringArm.SpringLength;
        }
        if (@event.IsActionPressed("player_camera_zoom_out"))
        {
            TargSpringArmLength += zoomSpeed*_SpringArm.SpringLength;
        }

        // Handle drag button press/release
        if (@event.IsActionPressed("player_camera_drag"))
        {
            _isDragging = true;
            _initialMousePos = GetViewport().GetMousePosition();
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
        else if (@event.IsActionReleased("player_camera_drag"))
        {
            _isDragging = false;
            ResetMouse();
        }

        // handle on mouse movement
        if (@event is InputEventMouseMotion motion)
        {
            if (_isDragging)
            {
                _PivotH.RotateY(-motion.Relative.X * mouseSens);
                _PivotV.RotateX(-motion.Relative.Y * mouseSens);
                    
                // Clamp vert rot
                Vector3 rotation = _PivotV.Rotation;

                rotation.X = Mathf.Clamp(rotation.X, Mathf.DegToRad(-60), Mathf.DegToRad(70));
                
                // Apply vert rot
                _PivotV.Rotation = rotation;
            }
        }
    }

    public override void _Process(double delta)
    {
        _SpringArm.SpringLength = Mathf.Lerp(_SpringArm.SpringLength, TargSpringArmLength, 0.3f);
    }
    #endregion Godot methods

    #region Private methods
    private void ResetMouse()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        Input.WarpMouse(_initialMousePos);
    }
    #endregion Private methods
}
