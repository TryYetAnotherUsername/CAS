using Godot;
using System;
using System.Collections.Generic;

public partial class Checkout : Prop
{
    [Export] public Node3D NavTarg {get; private set;}
    [Export] private Label _display;
    [Export] private MeshInstance3D _light;
    [Export] public bool IsFree;
    [Export] public bool IsFinishedPaying;
    [Export] public bool IsQueueTarg {get; private set;}

    public override void _Ready()
    {
        IsFree = true;
        IsFinishedPaying = false;
    }


    public async void UseCheckout(List<Customer.ShoppingItem> shoppedItems)
    {
        IsFinishedPaying = false;
        IsFree = false;

        await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);

        // <material boilercode>

        Material rawMat = _light.GetSurfaceOverrideMaterial(0);
        if (rawMat == null)
        {
            rawMat = _light.Mesh.SurfaceGetMaterial(0);
        }

        var material = rawMat.Duplicate() as StandardMaterial3D;
        _light.SetSurfaceOverrideMaterial(0, material);

        // </material boilercode>

        if (material != null)
        {
            material.AlbedoColor = Colors.Red;
            material.EmissionEnabled = true;
            material.Emission = Colors.Red;
        }

        foreach (var item in shoppedItems)
        {
            _display.Text = $"{item.Product.DispName} x{item.Quantity}";
            await ToSignal(GetTree().CreateTimer(1f), SceneTreeTimer.SignalName.Timeout);
        }

        _display.Text = "Processing Payment...";
        await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);

        if (material != null)
        {
            material.AlbedoColor = Colors.Green;
            material.EmissionEnabled = true;
            material.Emission = Colors.Green;
        }

        _display.Text = "Next customer please!";

        await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);

        IsFree = true;
        IsFinishedPaying = true;
    }
}
