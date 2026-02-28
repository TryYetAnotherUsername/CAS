using Godot;

public partial class NPC : CharacterBody3D
{
    [Export] public float MovementSpeed { get; set; } = 4.0f;
    [Export] public NavigationAgent3D _navigationAgent;
    [Export] private float _movementDelta;

    public void SetMovementTarget(Vector3 movementTarget)
    {
        _navigationAgent.TargetPosition = movementTarget;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_navigationAgent.IsNavigationFinished())
        {
            Velocity = Vector3.Zero;
            return; 
        }
        
        _movementDelta = MovementSpeed * (float)delta;
        Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();
        Vector3 newVelocity = GlobalPosition.DirectionTo(nextPathPosition);

        Vector3 flatNext = new Vector3(nextPathPosition.X, GlobalPosition.Y, nextPathPosition.Z);
        if (GlobalPosition.DistanceTo(flatNext) > 0.1f)
        {
            LookAt(flatNext, Vector3.Up);
        }

        Velocity = newVelocity;
        MoveAndSlide();
    }

    
}