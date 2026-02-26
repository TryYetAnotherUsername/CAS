using Godot;
using Godot.Collections;
using Microsoft.VisualBasic;
using System;

public partial class Chat : Control
{
	[Export] VBoxContainer _root;
	[Export] PackedScene _outboundBubble;
	[Export] PackedScene _inboundBubble;
	[Export] ScrollContainer _scrollCont;

	private bool _scrollToBottom;
	private string _currentChatConvoId;

	#region <Godot lifecycle>
	public override void _Ready()
	{
		MessageService.OnDisplayConversation += OpenChat;
		MessageService.OnNewMessage += InterceptMessage;
	}
	public override void _ExitTree()
	{
		MessageService.OnDisplayConversation -= OpenChat;
		MessageService.OnNewMessage -= InterceptMessage;
	}

    public override void _Process(double delta)
    {
        if (_scrollToBottom)
		{
			CallDeferred("ScrollToBottom");
			_scrollToBottom = false;
		}
    }

	#endregion <Godot lifecycle>

	private void InterceptMessage((string convoId, Conversation.InboundMessage message)data)
	{
		if (data.convoId == _currentChatConvoId)
		{
			AddInboundBubble(data.message);
		}
	}

	private void OpenChat(string conversationId)
	{
		foreach (Node child in _root.GetChildren())
		{
			child.QueueFree();
		}

		foreach (Conversation.HistoryEntry entry in MessageService.I.GetHistory(conversationId))
		{
			if (entry.IsInboundMessage)
			{
				AddInboundBubble(new Conversation.InboundMessage {Text = entry.Text, AvalibleReplies = null});
			}
			else
			{
				AddOutboundBubble(entry.Text);
			}
		}
		
		var latestMessage = MessageService.I.GetCurrentOrNull(conversationId);
		if (latestMessage is not null)
		{
			AddInboundBubble(latestMessage);
		}
	}



	#region <helper code>

	private void AddInboundBubble(Conversation.InboundMessage message)
	{
		var node = _inboundBubble.Instantiate();
		_root.AddChild(node);
		if (node is MessageBubble messageBubble)
		{
			messageBubble.Init(message.Text);
		}
		_scrollToBottom = true;
	}

	private void AddOutboundBubble(string text)
	{
		var node = _outboundBubble.Instantiate();
		_root.AddChild(node);
		if (node is MessageBubble messageBubble)
		{
			messageBubble.Init(text);
		}
		_scrollToBottom = true;
	}

	private void ScrollToBottom()
	{
		var tween = CreateTween();
        tween.TweenProperty(_scrollCont, "scroll_vertical", (int)_scrollCont.GetVScrollBar().MaxValue, 0.5);
	}

	#endregion <helper code>
}
