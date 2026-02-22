using Godot;
using System.Collections.Generic;

public class CasProj
{
	public Metadata Meta { get; set; }
    public List<PropData> Props { get; set; } = new();
}

public class Metadata
{
	long LastModifiedUnix { get; set; }
	long FileCreatedUnix { get; set; }
}

// needed because json has beef with Node3Ds :( 
public class PropData
{
    public string Uid { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
	public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }
}

