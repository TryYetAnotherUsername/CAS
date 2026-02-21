using Godot;
using System;
public partial class MenuOpenCloseButton : Button
{
    public enum ButtonAction
    {
        Open, Close, ExitTool
    } 
    [Export] ButtonAction _actionMode;
    [Export] AnimationPlayer _animationPlayer;
    
    public override void _Ready()
    {
        if (_actionMode == ButtonAction.Open)
        {
            Pressed += OpenMenu;
        }
        else if (_actionMode == ButtonAction.Close)
        {
            Pressed += CloseMenu;
        }

        ToolService.OnUpdate += (tool) => CloseMenu();
    }

    // MIXED UP ANIMATION TRACKS PLS FIX!!!

    public void OpenMenu()
    {
        _animationPlayer.Play("close_menu");
    }

    public void CloseMenu()
    {
        _animationPlayer.PlayBackwards("close_menu");
    }
}