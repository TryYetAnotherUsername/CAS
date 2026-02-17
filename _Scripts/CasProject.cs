using Godot;
using System.Collections.Generic;

public class CasProj
{
	public Metadata Meta;
    public List<Prop> PlacedObjects;
}

public class Metadata
{
	long LastModifiedUnix;
	long FileCreatedUnix;
}