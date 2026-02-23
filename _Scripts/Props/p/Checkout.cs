using Godot;
using System;

public partial class Checkout : Prop
{
    [Export] public Node3D NavTarg {get; private set;}
    [Export] private Label Display;
    [Export] public bool IsFree {get; private set;}
    [Export] public bool IsQueueTarg {get; private set;}
    
    public async void UseCheckout(Action callback)
    {
        IsFree = false;
        Display.Text = "Using checkout...";
        await ToSignal(GetTree().CreateTimer(10), SceneTreeTimer.SignalName.Timeout);
        Display.Text = "Next customer please!";
        callback?.Invoke();
    }
}
