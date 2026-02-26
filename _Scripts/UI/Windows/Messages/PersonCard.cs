using Godot;
using System;

public partial class PersonCard : PanelContainer
{
	[Export] private Button _theButton;

	private string _dispName;
	private string _conversationId;
	
	public void Init(string convoId, string dispName)
	{
		_dispName = dispName;
		_conversationId = convoId;

		_theButton.Text = dispName;
		_theButton.Pressed += () => MessageService.I.Display(_conversationId);
	} 
}
