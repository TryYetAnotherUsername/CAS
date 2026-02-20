using Godot;
using System;

public partial class LiveSelectService : Node
{


    // code copied from selection tool, needs proper implementation soon. :(

    public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("build_select"))
		{
            var rayResult = Raycast();
            if (rayResult is null) return;
            if (rayResult.GetOwner<Node3D>() is Prop prop)
            {
                if (prop is Shelf shelf)
                {
                    foreach (var entry in shelf.stockedProducts)
                    {
                        GD.Print($"{entry.Product.DispName}: {entry.Quantity}");
                    }
                }
            }
		}
	}
    
    private Node3D Raycast()
	{
		var camera3D = GetViewport().GetCamera3D();
		var from = camera3D.ProjectRayOrigin(GetViewport().GetMousePosition());
		var to = from + camera3D.ProjectRayNormal(GetViewport().GetMousePosition()) * BuildmodeService.RayLength;

		var spaceState = GetViewport().World3D.DirectSpaceState;
		var options = PhysicsRayQueryParameters3D.Create(from, to);
		options.CollisionMask = 2;
		var result = spaceState.IntersectRay(options);
		
		if (result.Count > 0)
		{
			return (Node3D)result["collider"];
		}
		else
		{
			return null;
		}
	}
}
