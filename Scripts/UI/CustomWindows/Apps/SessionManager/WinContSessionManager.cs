using Godot;
using System;
using System.ComponentModel;

public partial class WinContSessionManager : Control
{
	// Dependencies
	[Export] Button ReloadButton;
	[Export] Label CurrentFile;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        ReloadButton.Pressed += RefreshInfo;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void RefreshInfo()
	{
		CurrentFile.Text = 
		$"""
		Path currently loaded:	{CurrentSession.Path}
		Created (Unix): 		{CurrentSession.ProjectData.Metadata.CreatedUnix}
		Last modified (Unix): 	{CurrentSession.ProjectData.Metadata.LastModifiedUnix}
		Next avalible ID:		{CurrentSession.ProjectData.NextAvailableId}
		""";
	}
}
