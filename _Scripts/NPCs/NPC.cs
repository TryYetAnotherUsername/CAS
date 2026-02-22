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
        _movementDelta = MovementSpeed * (float)delta;
        Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();
        Vector3 newVelocity = GlobalPosition.DirectionTo(nextPathPosition);
        Velocity = newVelocity;
        MoveAndSlide();
    }
}