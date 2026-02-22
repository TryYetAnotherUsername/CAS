using Godot;

public partial class NPC : CharacterBody3D
{
    [Export] public float MovementSpeed { get; set; } = 4.0f;
    [Export] NavigationAgent3D _navigationAgent;
    [Export] private float _movementDelta;

    public override void _Ready()
    {
        _navigationAgent.VelocityComputed += OnVelocityComputed;
    }

    private void SetMovementTarget(Vector3 movementTarget)
    {
        _navigationAgent.TargetPosition = movementTarget;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Do not query when the map has never synchronized and is empty.
        if (NavigationServer3D.MapGetIterationId(_navigationAgent.GetNavigationMap()) == 0)
        {
            return;
        }

        if (_navigationAgent.IsNavigationFinished())
        {
            return;
        }

        _movementDelta = MovementSpeed * (float)delta;
        Vector3 nextPathPosition = _navigationAgent.GetNextPathPosition();
        Vector3 newVelocity = GlobalPosition.DirectionTo(nextPathPosition) * _movementDelta;
        if (_navigationAgent.AvoidanceEnabled)
        {
            _navigationAgent.Velocity = newVelocity;
        }
        else
        {
            OnVelocityComputed(newVelocity);
        }
    }

    private void OnVelocityComputed(Vector3 safeVelocity)
    {
        GlobalPosition = GlobalPosition.MoveToward(GlobalPosition + safeVelocity, _movementDelta);
    }
}