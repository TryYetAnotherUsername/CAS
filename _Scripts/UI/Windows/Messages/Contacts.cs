using Godot;
using System;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices.Marshalling;

public partial class Contacts : Control
{
	[Export] private VBoxContainer _root;
	[Export] private PackedScene _personCard;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MessageService.OnNewConvo += AddCard;
		foreach (var item in MessageService.I.GetConversationList())
		{
			AddCard((item.conversationId, item.dispName));
		}	
	}

	public void AddCard((string id, string dispName) data)
	{
		var newCard = _personCard.Instantiate();
		_root.AddChild(newCard);
		if (newCard is PersonCard personCard)
		{
			personCard.Init(data.id, data.dispName);
		}
	}
}
