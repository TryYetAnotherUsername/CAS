using System;
using System.Collections.Generic;

/// <summary>
/// All the conversation content
/// </summary>
// lots and lots of typos. I think i'm deslexsic.
public static class ConversationConfig
{
    private static readonly Dictionary <string, Conversation> Conversations = new()
    {
        {
            "c_matilda", new Conversation
            {
                CurrentMessageId = "m_intro",
                DispTitle = "Matilda",
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
        {
            "c_tom", new Conversation
            {
                CurrentMessageId = "m_intro",
                DispTitle = "tom@bradfordfamilydairies.co",
                Messages = new()
                {
                    { "m_intro", new Conversation.InboundMessage()
                        {
                            Text = "Hey, can I ask you for a favour?",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "For you? Of course!",
                                    NextMessageID = "m_honestly"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "Maybe... is it good or bad?",
                                    NextMessageID = "m_fine"
                                }
                            }
                        }
                    },
                    { "m_fine", new Conversation.InboundMessage()
                        {
                            Text = "It's... Fine. Everything will be just fine\nBut I really need your help.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Sure thing.",
                                    NextMessageID = "m_honestly"
                                }
                            }
                        }
                    },
                    { "m_honestly", new Conversation.InboundMessage()
                        {
                            Text = "Well...\nI honestly don't know who to say this to. Our family is going through some really tough times this week.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Hey, Hey, I'm here.\nYou can...\nTrust me.",
                                    NextMessageID = "m_count"
                                }
                            }
                        }
                    },
                    { "m_count", new Conversation.InboundMessage()
                        {
                            Text = "I knew I could count on you.\nWell.\nThere was a major virus outbreak on the farm last week.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Please don't tell me what I think you're gonna tell me...",
                                    NextMessageID = "m_ik_its_bad"
                                }
                            }
                        }
                    },
                    { "m_ik_its_bad", new Conversation.InboundMessage()
                        {
                            Text = "I know, buddy. It's... Bad.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "I'm so sorry for the trouble. Can I lend you a hand?",
                                    NextMessageID = "m_cows"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "I've got more important things to care about. So?",
                                    NextMessageID = "m_quickly"
                                }
                            }
                        }
                    },
                    { "m_quickly", new Conversation.InboundMessage()
                        {
                            Text = "Sorry, I will make this as quick as I can.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Oh, come on then.",
                                    NextMessageID = "m_cows"
                                }
                            }
                        }
                    },
                    { "m_cows", new Conversation.InboundMessage()
                        {
                            Text = "Half of our cows are in critical conditions, and we think most of them will be gone by the end of the week.\nIf you would be so kind to be able to lend us a little extra money, it could mean that our beloved farm wouldn't go under.\nA £2500 lend would be great.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "I'm more than happy to!",
                                    Consequence = new Action (() => EconomyService.I.TryTakeCash(2500)),
                                    NextMessageID = "m_accept"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "Urgh. Just this one time then.",
                                    Consequence = new Action (() => EconomyService.I.TryTakeCash(2500)),
                                    NextMessageID = "m_accept"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "That's a big ask, and- you know it.\nI'm sorry, but I'm going to have to pass.",
                                    NextMessageID = "m_pass"
                                }
                            }
                        }
                    },
                    { "m_accept", new Conversation.InboundMessage()
                        {
                            Text = "I can't thank you enough. I will repay you some day.\nThat's a promise.",
                            AvalibleReplies = null
                        }
                    },
                    { "m_pass", new Conversation.InboundMessage()
                        {
                            Text = "I thought...\nI could...\nTrust you.",
                            AvalibleReplies = null
                        }
                    },
                }
            }
        },
        {
            "c_hmrc", new Conversation
            {
                CurrentMessageId = "m_intro",
                DispTitle = "HMRC",
                Messages = new()
                {
                    { "m_intro", new Conversation.InboundMessage()
                        {
                            Text = "HMRC: You have a £1500 tax payment, conveniently rounded to £2000, due to be paid by the end of this week. if you do not pay it, you will face unimaginable consequences.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Fair enough... heres £2000. Thanks for reminding me!",
                                    NextMessageID = "m_goodlad",
                                    Consequence = new Action(()=> EconomyService.I.TryTakeCash(2000))
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "I will consider it. Give me a day or two.",
                                    NextMessageID = "m_verwell"
                                }
                            }
                        }
                    },
                    { "m_goodlad", new Conversation.InboundMessage()
                        {
                            Text = "Good lad.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Hey- I never knew HMRC was so chill!?",
                                    NextMessageID = "m_humour"
                                }
                            }
                        }
                    },
                    { "m_verwell", new Conversation.InboundMessage()
                        {
                            Text = "Very well. We shall be back soon.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Hey- I never knew HMRC was so chill!?",
                                    NextMessageID = "m_humour"
                                }
                            }
                        }
                    },
                    { "m_humour", new Conversation.InboundMessage()
                        {
                            Text = "Haha. You humour me sometimes...\n... I'm not even the HMRC. The HMRC does not communicate to you by text.",
                            AvalibleReplies = new()
                            {
                                new Conversation.OutboundReply()
                                {
                                    Text = "Thats it. Im telling mommy.",
                                    NextMessageID = "m_assemblies"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "ARGHHHHHHH\nDARETH NOT STEAL MINE WEALTH!!",
                                    NextMessageID = "m_assemblies"
                                },
                                new Conversation.OutboundReply()
                                {
                                    Text = "I guess it wasn't that much anyway.",
                                    NextMessageID = "m_assemblies"
                                }
                            }
                        }
                    },
                    { "m_assemblies", new Conversation.InboundMessage()
                        {
                            Text = "Either way... u should have listened to those online safety assemblies.\n🙂",
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