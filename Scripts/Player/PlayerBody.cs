using Godot;
using System;

public partial class PlayerBody : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	[Export] private Node3D _pivH;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Gravity:
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Jump:
		if (Input.IsActionJustPressed("player_move_jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Input dir + direction:
		Vector2 inputDir = Input.GetVector("player_move_l", "player_move_r", "player_move_f", "player_move_b");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			GlobalRotation = _pivH.GlobalRotation;
            var rot = _pivH.Rotation;
            rot.Y = 0;

            _pivH.Rotation = rot;
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
