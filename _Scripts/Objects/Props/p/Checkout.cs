using Godot;
using System;
using System.Collections.Generic;

public partial class Checkout : Prop
{
    [Export] public Node3D NavTarg {get; private set;}
    [Export] private Label _display;
    [Export] private MeshInstance3D _light;
    [Export] private float _grandTotal = 0;
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
        _grandTotal = 0;

        IsFinishedPaying = false;
        IsFree = false;

        await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);

        #region <material boilercode>

        Material rawMat = _light.GetSurfaceOverrideMaterial(0);
        if (rawMat == null)
        {
            rawMat = _light.Mesh.SurfaceGetMaterial(0);
        }

        var material = rawMat.Duplicate() as StandardMaterial3D;
        _light.SetSurfaceOverrideMaterial(0, material);

        #endregion <material boilercode>

        if (material != null)
        {
            material.AlbedoColor = Colors.Red;
            material.EmissionEnabled = true;
            material.Emission = Colors.Red;
        }

        foreach (var item in shoppedItems)
        {
            _display.Text = $"{item.Product.DispName} x{item.Quantity}\n@ £ {item.Product.PriceSell * item.Quantity}";
            _grandTotal += item.Product.PriceSell * item.Quantity;
            await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);
        }

        _display.Text = $"Total: £ {_grandTotal}\nProcessing Payment...";
        await ToSignal(GetTree().CreateTimer(3f), SceneTreeTimer.SignalName.Timeout);

        if (material != null)
        {
            material.AlbedoColor = Colors.Green;
            material.EmissionEnabled = true;
            material.Emission = Colors.Green;
        }

        EconomyService.I.AddCash(_grandTotal);
        _display.Text = "Next customer please!";

        await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);

        IsFree = true;
        IsFinishedPaying = true;
    }
}
