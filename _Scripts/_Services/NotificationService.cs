using Godot;
using System;
public partial class NotificationService : Node
{
    public static NotificationService I;
    [Export] private PackedScene _notifCardScene;
    [Export] private VBoxContainer _root;

    public override void _Ready()
    {
        I = this;
    }

    public void Print(string message)
    {
        var instance = _notifCardScene.Instantiate();
        _root.AddChild(instance);
        if (instance is NotifCard card)
        {
            card.Init(message);
        }
    }
}