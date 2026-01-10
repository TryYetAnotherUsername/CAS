using Godot;
using System;

public partial class NotificationTile : PanelContainer
{
	private int myId;

	[Export] private Label Title;
	[Export] private RichTextLabel Body;
	[Export] private Button Dismiss;
	[Export] private Panel Line;

	public void Clear()
    {
		
        QueueFree();
    }

	public void Init(string title, string message, int id, int mode = 0)
    {
        myId = id;
		Title.Text = title;
		Body.Text = message;

		GetParent().MoveChild(this, 0);

		switch (mode)
        {
            case 1:
				Line.Modulate = Color.FromHtml("#006dd9ff");
				break;
			case 2:
				Line.Modulate = Color.FromHtml("#bd6d30ff");
				break;
			default:
				Line.Modulate = Color.FromHtml("#62646cff");
				break;
        }
		Dismiss.Pressed += () =>
        {
			
        };
    }
}
