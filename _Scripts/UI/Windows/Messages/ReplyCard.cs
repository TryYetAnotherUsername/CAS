using Godot;
using System;

public partial class ReplyCard : PanelContainer
{
	[Export] private Label _text;
	[Export] private Button _button;
    [Export] private string _nextMessageID;
	[Export] private string _conversationId;
	private Conversation.OutboundReply _reply;

	public void Init(string convoId, Conversation.OutboundReply r)
	{
		_conversationId = convoId;
		_reply = r;
		
		_text.Text = _reply.Text;
		_button.Pressed += Reply;
	}

	private void Reply()
	{
		MessageService.I.Reply(_conversationId, _reply);
	}
	
}
