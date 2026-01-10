using Godot;
using System;

public partial class NotificationMenu : Button
{
	[Export] private Panel menu;
	[Export] private Button clearAll;

	public override void _Ready()
    {
        Pressed += ToggleNotifCentre;
		clearAll.Pressed += ClearAllNotifs;
		menu.Visible = false;

        menu.FocusExited += () =>
        {
            menu.Visible = false;
        };
    }

	private void ClearAllNotifs()
    {
        NotificationService.ClearAllNotifs();
    }

	private void ToggleNotifCentre()
    {
        if (menu.Visible)
        {
            menu.Visible = false;
        }
		else
        {
            menu.Visible = true;
            menu.GrabFocus();
        }
    }
}
