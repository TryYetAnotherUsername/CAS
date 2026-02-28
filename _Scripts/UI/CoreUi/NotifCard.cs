using Godot;
using System;
public partial class NotifCard : PanelContainer
{
    [Export] private Button _closeButton;
	[Export] private RichTextLabel _label;

    public void Init(string message)
    {
		_label.Text = message;
        Visible = true;
        _closeButton.Pressed += QueueFree;
    }
}