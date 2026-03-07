using Godot;

public partial class NPC : CharacterBody3D
{
    [Export] public float MovementSpeed = 0.5f;
    [Export] public NavigationAgent3D _navigationAgent;
    private float _movementDelta;
    
    [Export] private Sprite3D _talkBubble;
    [Export] private Label _talkText;

    private Vector3 _targPos;

    public override void _Ready()
    {
        _targPos = _talkBubble.Position;
    }


    public async void Print(string message)
    {
        _talkText.Text = message;
        _talkBubble.Visible = true;

        var startPos = _targPos;
        startPos.Y -= 0.5f;
        _talkBubble.Position = startPos;
        _talkBubble.Modulate = Color.FromHtml("#ffffff00");

        Tween tween = CreateTween();
        tween.SetEase(Tween.EaseType.Out);
        tween.SetTrans(Tween.TransitionType.Sine);
        tween.SetParallel(true);
        tween.TweenProperty(_talkBubble, "modulate", Color.FromHtml("#ffffff"), 1f);
        tween.TweenProperty(_talkBubble, "position", _targPos, 1f);

        await ToSignal(GetTree().CreateTimer(5f), "timeout");
        if (!IsInstanceValid(this)) return;

        _talkBubble.Visible = false;
        _talkBubble.Position = _targPos;
    }

    // adapted from gd docs

    public void SetMovementTarget(Vector3 movementTarget)
    {
        _navigationAgent.TargetPosition = movementTarget;
        GD.Print($"Movement target set: <{movementTarget}>");
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