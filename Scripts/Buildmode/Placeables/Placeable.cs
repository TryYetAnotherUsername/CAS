using Godot;

public partial class Placeable : Node3D
{
	public string DisplayName;
	public string ScenePath { get; init; }

	public enum PlacementMode
	{
    	Free,
    	GridLocked
	}

	public PlacementMode myPlacementMode;

}
