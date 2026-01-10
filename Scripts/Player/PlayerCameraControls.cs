using Godot;
using System;

public partial class PlayerCameraControls : Camera3D
{
    [Export] private Node3D _pivotH;
    [Export] private Node3D _pivotV;
    [Export] private SpringArm3D _springArm3D;
    [Export] public float zoomSpeed = 0.1f;
    [Export] public float mouseSens = 0.005f;
    
    private bool _isDragging = false;
    
    public override void _UnhandledInput(InputEvent @event)
    {
        // Handle drag button press/release
        if (@event.IsActionPressed("player_camera_drag"))
        {
            _isDragging = true;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
        else if (@event.IsActionReleased("player_camera_drag"))
        {
            _isDragging = false;
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        
        // Handle mouse motion while dragging
        if (@event is InputEventMouseMotion motion && _isDragging)
        {
            _pivotH.RotateY(-motion.Relative.X * mouseSens);
            _pivotV.RotateX(-motion.Relative.Y * mouseSens);
            
            // Clamp vertical rotation
            Vector3 rotation = _pivotV.Rotation;
            rotation.X = Mathf.Clamp(rotation.X, Mathf.DegToRad(-60), Mathf.DegToRad(70));
            _pivotV.Rotation = rotation;
        }
        
        // Camera zoom in/out controls
        if (@event is InputEventMouseButton eventAction)
        {
            if (eventAction.IsActionPressed("player_camera_zoom_in"))
            {
                _springArm3D.SpringLength -= zoomSpeed;
            }
            else if (eventAction.IsActionPressed("player_camera_zoom_out"))
            {
                _springArm3D.SpringLength += zoomSpeed;
            }
        }
    }
}

