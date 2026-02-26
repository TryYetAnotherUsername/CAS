using System;
using System.Collections.Generic;

public class Conversation
{
    public string DispTitle;
    public string CurrentMessageId = null;
    
    public bool AwaitingInboundReply = false;

    public Dictionary <string, InboundMessage> Messages = new();
    public List<HistoryEntry> History = new();

    public class HistoryEntry{ public string Text; public bool IsInboundMessage;}

    public class InboundMessage
    {
        public string Text;
        public List<OutboundReply> AvalibleReplies = new();
    }

    public class OutboundReply
    {
        public string Text;
        public Action Consequence = null;
        public string NextMessageID = null;
    }
}