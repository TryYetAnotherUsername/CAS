using Godot;

// Godot engine movement script
public partial class CharacterBody3d : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 8f;

	[Export] private Node3D _PivotH;
	[Export] private AnimationTree _aniTree;

	public override void _PhysicsProcess(double delta)
	{
		int aniBlendVal = 0;

		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("player_move_left", "player_move_right", "player_move_forward", "player_move_backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			aniBlendVal = 0;

			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;

			GlobalRotation = _PivotH.GlobalRotation;
            var rot = _PivotH.Rotation;
            rot.Y = 0;

            _PivotH.Rotation = rot;
		}
		else
		{
			aniBlendVal = 1;

			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;

		var tween1 = CreateTween();
        tween1.TweenProperty(_aniTree, "parameters/blend_2/blend_amount", aniBlendVal, 0.3f);

		MoveAndSlide();
	}
}
