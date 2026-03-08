using Godot;
using System;

/// <summary>
/// Handles opening the shelf stocking UI.
/// </summary>
// Not a very useful class. Shouldn't even be a service. I dont have time to change it.
public partial class LiveSelectService : Node
{
    // Events:
    public static event Action OnHoverStart;
    public static event Action OnHoverEnd;

    // States:
    private Node3D _currentHovered = null;

    // Godot native methods:
	public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("inspect_pressed"))
		{
			HandleSelect();
		}

        var hit = Raycast();

		if (hit == _currentHovered)
		{
			return;
		} 
		else
		{
			if (hit == null)
			{
				OnHoverEnd?.Invoke();
				_currentHovered = hit;
				return;
			}

			_currentHovered = hit;
			
			if (hit.GetOwner<Node3D>() is Shelf shelf)
        	{
				OnHoverStart?.Invoke();
        	}
			else
			{
				OnHoverEnd?.Invoke();
			}
		}
    }

    // Private methods:
    private void HandleSelect()
    {
        var hit = Raycast();
        if (hit is null) return;
        if (hit.GetOwner<Node3D>() is Shelf shelf)
        {
			Control window = WindowService.I.NewWindow(WindowService.EWindowContent.Properties);
			var inspector = window as Inspector;
            inspector?.Init(shelf);

			
            foreach (var entry in shelf.StockedProductsList)
			{
				GD.Print($"{entry.Product.DispName}: {entry.Quantity}");
			}
        }

    }

    private Node3D Raycast()
    {
        var camera3D = GetViewport().GetCamera3D();
        var from = camera3D.ProjectRayOrigin(GetViewport().GetMousePosition());
		
        var to = from + camera3D.ProjectRayNormal(GetViewport().GetMousePosition()) * BuildToolService.RayLength;
        var spaceState = GetViewport().World3D.DirectSpaceState;

        var options = PhysicsRayQueryParameters3D.Create(from, to);
        options.CollisionMask = 2;

        var result = spaceState.IntersectRay(options);
        return result.Count > 0 ? (Node3D)result["collider"] : null;

    }
}