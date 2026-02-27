using System.Collections.Generic;

public static class ConversationConfig
{
    private static readonly Dictionary <string, Conversation> Conversations = new()
    {
        {
            "c_matilda", new Conversation
            {
                CurrentMessageId = "m_intro",
                DispTitle = "Matilda (Crimson town)",
                Messages = new()
                {
                    { "m_intro", new Conversation.InboundMessage()
                        {
                            Text = "hiya im Matilda\nspeaking to you on behalf of the Crimson Town community.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Ya?",
                                    NextMessageID = "m_toddler"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "Can I help you?",
                                    NextMessageID = "m_toddler"
                                }
                            }
                        }
                    },
                    { "m_toddler", new Conversation.InboundMessage()
                        {
                            Text = "Kdkdl🔥🔥🔥 he 👋 🤣 khehehejhe",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Excuse me?",
                                    NextMessageID = "m_sorry"
                                }
                            }
                        }
                    },
                    { "m_sorry", new Conversation.InboundMessage()
                        {
                            Text = "no no no no ahh,\noh, no, so, so sorry.\nThat was my toddler.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "...",
                                    NextMessageID = "m_donuts"
                                }
                            }
                        }
                    },
                    { "m_donuts", new Conversation.InboundMessage()
                        {
                            Text = "We were looking to inquire about the sugar donuts from your store.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "We put those off the shelf a while ago. They were really affecting the grades of the students at the community school next to our shop.",
                                    NextMessageID = "m_notillegal"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "What's your problem?",
                                    NextMessageID = "m_notillegal"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "I didn't have a choice, really.",
                                    NextMessageID = "m_notillegal"
                                }
                            }
                        }
                    },
                    { "m_notillegal", new Conversation.InboundMessage()
                        {
                            Text = "Well, I would like to remind you that sugar donuts aren't illegal.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "As a shop devoted to serving our community, it would not be right for me to continue selling those products.",
                                    NextMessageID = "m_parents"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "That's a valid point.",
                                    NextMessageID = "m_kant"
                                }
                            }
                        }
                    },
                    { "m_parents", new Conversation.InboundMessage()
                        {
                            Text = "That's your problem. Do you realise that it is a parent's responsibility to limit their child's sugar intake?\nYou really don't have the right to limit what your customers can or cannot buy. Let alone fully grown adults like myself, who are more than capeable of their own judgement!",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "also, I would love to point out the fact that you are being quite very disrespectful.",
                                    NextMessageID = "m_coperate_garbage"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "Well, anyway, it's been...\nIt's been good speaking to you.",
                                    NextMessageID = "m_coperate_garbage"
                                }
                            }
                        }
                    },
                    { "m_kant", new Conversation.InboundMessage()
                        {
                            Text = "urgh. I'm glad you've gotten past your weird Kantian ethics arc then.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Don't you dare disrespect the wonderful Kant.",
                                    NextMessageID = "m_parents"
                                },
                            }
                        }
                    },
                    { "m_coperate_garbage", new Conversation.InboundMessage()
                        {
                            Text = "Yea. yea, of course! you unhelpful peice of coperate garbage.",
                            AvalibleReplies = null
                        }
                    },
                }
            }
        },

    };

    public static Dictionary <string, Conversation> GetCopy()
    {
        return Conversations;
    }
}