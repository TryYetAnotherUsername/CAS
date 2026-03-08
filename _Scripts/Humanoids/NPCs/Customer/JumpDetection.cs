using Godot;
using System;

// Detects the player jumping on the Customer NPC to flatten it. unethically gives money per squash.
public partial class JumpDetection : Area3D
{
	[Export] Area3D _area3D;
	[Export] Customer _customer;
	float _yOffset = 0.5f;

    public override void _Ready()
    {
        _area3D.BodyEntered += CheckBody;
    }

	private void CheckBody(Node3D node)
	{
		if (node.Name == "PlayerChar")
		{
			float detected = node.GlobalPosition.Y - GlobalPosition.Y;
			if (detected > _yOffset)
			{
				EconomyService.I.AddCash(GD.RandRange(1,5));
				_customer.Flatten();
			}

		}
	}

}
