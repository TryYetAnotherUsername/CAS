using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Backend for the messaging window.
/// </summary>
public partial class MessageService : Node
{
    #region <fields>
    public static MessageService I;

    private Dictionary <string, Conversation> _convosList;
    private List<(string id, string dispName)> _avalibleConvosIds = new();

    #endregion <fields>

    #region <events>
    public static Action <(string conversationId, Conversation.InboundMessage)> OnNewMessage;
    public static Action <Conversation.OutboundReply> OnNewReply;

    public static Action <(string ID, string dispName)> OnNewConvo;
    public static Action <string> OnDisplayConversation;

    #endregion <events>

    #region <Godot methods>
    public override void _Ready()
    {
        I = this;
        ToolService.OnUpdate += (tool) =>
        {
            if (tool is not ToolService.ETools.Messages) return;
            NewWindow();
        };

        // copy of conversation from config
        _convosList = ConversationConfig.GetCopy();

        PlayConversations();
    }

    #endregion <Godot methods>

    #region <Private methods>
    private void NewWindow()
    {
        GD.Print("spawning new messages window");
        WindowService.I.NewWindow(WindowService.EWindowContent.Messages);
    }

    private async void PlayConversations()
    {
        foreach (var conversation in _convosList)
        {
            _avalibleConvosIds.Add((conversation.Key, conversation.Value.DispTitle));
            OnNewConvo?.Invoke((conversation.Key, conversation.Value.DispTitle));
        };
    }
    
    #endregion <Private methods>

    #region <Public methods>
    public List<(string conversationId, string dispName)> GetConversationList() 
        => _avalibleConvosIds;

    public List<Conversation.HistoryEntry> GetHistory(string conversationId)
    {
        return _convosList[conversationId].History;
    }

    /// <returns>
    /// (Message) The current (most recent) InboundMessage, or null.
    /// </returns>
    /// <summary>
    /// If AwaitingInboundReply is True, this method will return null.
    /// </summary>
    public Conversation.InboundMessage GetCurrentOrNull(string conversationId)
    {
        Conversation conversation = _convosList[conversationId];
        if (!conversation.AwaitingInboundReply)
        {
            return conversation.Messages[conversation.CurrentMessageId];
        }
        else
        {
            return null;
        }
    }

    public void Display(string conversationId)
    {
        OnDisplayConversation?.Invoke(conversationId);
    }

    public async void Reply(string conversationId, Conversation.OutboundReply reply)
    {
        var convo = _convosList[conversationId];
        
        // add to history
        convo.History.Add(new Conversation.HistoryEntry { Text = convo.Messages[convo.CurrentMessageId].Text, IsInboundMessage = true });
        convo.History.Add(new Conversation.HistoryEntry { Text = reply.Text, IsInboundMessage = false });
        
        reply.Consequence?.Invoke();
        OnNewReply?.Invoke(reply);
        
        if (reply.NextMessageID == null) return;
        var nextMessage = convo.Messages[reply.NextMessageID];
        var waitTime = nextMessage.Text.Length * 0.05;
        Mathf.Clamp(waitTime, 1,10);

        await ToSignal(GetTree().CreateTimer(waitTime), SceneTreeTimer.SignalName.Timeout);
        
        convo.CurrentMessageId = reply.NextMessageID;
        OnNewMessage?.Invoke((conversationId, nextMessage));
    }

    #endregion <Public methods>

}
