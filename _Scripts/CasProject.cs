using Godot;
using System.Collections.Generic;

public class CasProj
{
	public Metadata Meta;
    public List<PropData> Props;
}

public class Metadata
{
	long LastModifiedUnix;
	long FileCreatedUnix;
}

public class PropData
{
    public string Uid;
    public float X;
    public float Y;
    public float Z;
	public float RotX;
    public float RotY;
    public float RotZ;
}

