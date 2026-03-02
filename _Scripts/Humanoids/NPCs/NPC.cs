using Godot;

public partial class NPC : CharacterBody3D
{
    [Export] public float MovementSpeed = 0.5f;
    [Export] public NavigationAgent3D _navigationAgent;
    [Export] private float _movementDelta;

    // adapted from gd docs

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

        Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();
        Vector3 direction = GlobalPosition.DirectionTo(nextPathPosition);
        Velocity = direction * MovementSpeed;

        Vector3 flatNext = new Vector3(nextPathPosition.X, GlobalPosition.Y, nextPathPosition.Z);
        if (GlobalPosition.DistanceTo(flatNext) > 0.1f)
        {
            LookAt(flatNext, Vector3.Up);
        }

        MoveAndSlide();
    }

    
}