using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public class Vertex 
{
    public Vector3 Position;
}

public class CasProjMetadata 
{
    public long CreatedUnix;
    public long LastModifiedUnix;
}

public class Wall 
{
    public int Start;
    public int End;
    public float Thickness;
}

public class ProjectData 
{
    public CasProjMetadata Metadata = new();
    public int NextAvailableId = 0;
    public List<int> FreedIds = new();
    public Dictionary<int, Vertex> Vertices = new();
    public Dictionary<int, Wall> Walls = new();
}

public static class CurrentSession 
{
    public static ProjectData ProjectData = new();
    public static string Path;
}