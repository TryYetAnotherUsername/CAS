using Godot;
using System;

public partial class MessageBubble : HBoxContainer
{
	[Export] RichTextLabel _label;
	
	public void Init(string text)
	{
		_label.Text = text;
	}

    public override void _Ready()
    {
    }

}
