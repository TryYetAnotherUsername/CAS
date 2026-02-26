using System.Collections.Generic;

public static class ConversationConfig
{
    private static readonly Dictionary <string, Conversation> Conversations = new()
    {
        {
            "c_john", new Conversation
            {
                CurrentMessageId = "m_first",
                DispTitle = "Jon",
                Messages = new()
                {
                    { "m_first", new Conversation.InboundMessage()
                        {
                            Text = "This is the first message. Hello, world!",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "What is this? (go to second message)",
                                    NextMessageID = "m_second"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "Introduce yourself! (repeats this message)",
                                    NextMessageID = "m_first"
                                }
                            }
                        }},

                    { "m_second", new Conversation.InboundMessage()
                        {
                            Text = "Hello there. This is the second message.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Yo! (goes back to start.)",
                                    NextMessageID = "m_first"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "Hi! (Also goes back to start.)",
                                    NextMessageID = "m_first"
                                }
                            }
                        }},
                }
            }
        },

        {
            "c_tim", new Conversation
            {
                CurrentMessageId = "m_first",
                DispTitle = "pilotimothy",
                Messages = new()
                {
                    { "m_first", new Conversation.InboundMessage()
                        {
                            Text = "This is the first message from Tim. Hello, world!",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Hey Tim, What is this? (go to second message)",
                                    NextMessageID = "m_second"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "Introduce yourself, Tim! (repeats this message)",
                                    NextMessageID = "m_first"
                                }
                            }
                        }},

                    { "m_second", new Conversation.InboundMessage()
                        {
                            Text = "Hello there, I'm Tim. This is the second message.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Yo, Tim! (goes back to start.)",
                                    NextMessageID = "m_first"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "Hi, Tim! (Also goes back to start.)",
                                    NextMessageID = "m_first"
                                }
                            }
                        }},
                }
            }
        }
    };
    
    public static Dictionary <string, Conversation> GetCopy()
    {
        return Conversations;
    }
}