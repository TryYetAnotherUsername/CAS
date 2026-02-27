using Godot;
using Godot.Collections;
using Microsoft.VisualBasic;
using System;

public partial class Chat : Control
{
	[Export] VBoxContainer _textsRoot;
	[Export] VBoxContainer _replyRoot;
	[Export] PackedScene _outboundBubble;
	[Export] PackedScene _inboundBubble;
	[Export] PackedScene _replyCard;
	[Export] ScrollContainer _scrollCont;
	[Export] Panel _repliesArea;

	[Export] Panel _loadingAnimationThing;
	[Export] AnimationPlayer _aniPlayer;
	[Export] Label _loadingPhrase;

	private bool _scrollToBottom;
	private string _currentChatConvoId;

	private string[] _pickFromPhrases =
	{
		"ummm...",
		"umm...",
		"umm...",
		"umm...",
		"umm...",
		"umm...",
		"umm...",
		"umm...",								

		"yea umm..",
		"mhm",
		"i'm thinking",
		"hang on...",
		"alright..",
		"give me a sec.",
		"typing...",
		"uhh...",
		"ye um",
		"yes",
		"aha,",
	};

	#region <Godot lifecycle>
	public override void _Ready()
	{
		MessageService.OnDisplayConversation += OpenChat;
		MessageService.OnNewMessage += InterceptMessage;
		MessageService.OnNewReply += OnReply;
	}
	public override void _ExitTree()
	{
		MessageService.OnDisplayConversation -= OpenChat;
		MessageService.OnNewMessage -= InterceptMessage;
		MessageService.OnNewReply -= OnReply;
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
		_loadingAnimationThing.Visible = false;
		_aniPlayer.Play("RESET");

		if (data.convoId == _currentChatConvoId)
		{
			AddInboundBubble(data.message);
			if (data.message.AvalibleReplies is not null)
			{
				ShowReplyOptions(data.message);
			}
		}
	}

	private void OpenChat(string conversationId)
	{
		_loadingAnimationThing.Visible = false;

		_currentChatConvoId = conversationId;

		foreach (Node child in _textsRoot.GetChildren())
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
			ShowReplyOptions(latestMessage);
		}
	}

	private void OnReply(Conversation.OutboundReply reply)
	{
		_loadingAnimationThing.Visible = true;
		_aniPlayer.Play("tick");
		_loadingPhrase.Text = _pickFromPhrases[GD.RandRange(0,_pickFromPhrases.Length-1)];

		foreach (Node node in _replyRoot.GetChildren())
		{
			node.QueueFree();
		}
		AddOutboundBubble(reply.Text);

		var tween = CreateTween();

		tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(_repliesArea, "custom_minimum_size:y", 115f, 1);
	}

	#region <helper code>

	private void ShowReplyOptions(Conversation.InboundMessage message)
	{
		foreach (Node node in _replyRoot.GetChildren())
		{
			node.QueueFree();
		}

		foreach (Conversation.OutboundReply reply in message.AvalibleReplies)
		{
			var node = _replyCard.Instantiate();
			_replyRoot.AddChild(node);

			if (node is ReplyCard replyCard)
			{
				replyCard.Init(_currentChatConvoId, reply);
			}
		}

		var tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
   		tween.TweenProperty(_repliesArea, "custom_minimum_size:y", 150f, 0.4);
	}

	private void AddInboundBubble(Conversation.InboundMessage message)
	{
		var node = _inboundBubble.Instantiate();
		_textsRoot.AddChild(node);

		if (node is MessageBubble messageBubble)
		{
			messageBubble.Init(message.Text);
		}

		_scrollToBottom = true;
	}

	private void AddOutboundBubble(string text)
	{
		var node = _outboundBubble.Instantiate();
		_textsRoot.AddChild(node);
		if (node is MessageBubble messageBubble)
		{
			messageBubble.Init(text);
		}
		_scrollToBottom = true;
	}

	private void ScrollToBottom()
	{
		var tween = CreateTween();

		tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
		tween.TweenProperty(_scrollCont, "scroll_vertical", (int)_scrollCont.GetVScrollBar().MaxValue, 0.75);
	}

	#endregion <helper code>
}
