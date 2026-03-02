using Godot;
using System;
using System.Collections.Generic;

public partial class Checkout : Prop
{
    [Export] public Node3D NavTarg {get; private set;}
    [Export] private Node3D _productTarg;
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


    public async void UseCheckout(List<ShoppingListService.ShoppingItem> shoppedItems)
    {
        _grandTotal = 0;

        IsFinishedPaying = false;
        IsFree = false;

        await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);
        if (!IsInstanceValid(this)) return;

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
            string productUid = item.Product.UID;
            PackedScene scene = GD.Load<PackedScene>(ResourceUid.GetIdPath(ResourceUid.TextToId(productUid)));
            var node = scene.Instantiate();

            if (node is Node3D node3D)
            {
                _productTarg.AddChild(node3D);
                node3D.Position = new Vector3(0, 0, 0.065f);

                var tweenIn = CreateTween();
                tweenIn.SetTrans(Tween.TransitionType.Sine);
                tweenIn.SetEase(Tween.EaseType.Out);
                tweenIn.TweenProperty(node3D, "position", Vector3.Zero, 0.3f);
            }

            _display.Text = $"{item.Product.DispName} x{item.Quantity}\n@ £{item.Product.PriceSell * item.Quantity}";
            _grandTotal += item.Product.PriceSell * item.Quantity;

            await ToSignal(GetTree().CreateTimer(1.5f), SceneTreeTimer.SignalName.Timeout);
            if (!IsInstanceValid(this)) return;

            if (node is Node3D node3D2)
            {
                var tweenOut = CreateTween();
                tweenOut.SetTrans(Tween.TransitionType.Sine);
                tweenOut.SetEase(Tween.EaseType.In);
                tweenOut.TweenProperty(node3D2, "position", new Vector3(0, 0, -0.065f), 0.3f);
                await ToSignal(tweenOut, Tween.SignalName.Finished);
                if (!IsInstanceValid(this)) return;
                node3D2.QueueFree();
            }
        }

        _display.Text = $"Total: £ {MathF.Round(_grandTotal,2)}\nProcessing Payment...";
        await ToSignal(GetTree().CreateTimer(3f), SceneTreeTimer.SignalName.Timeout);
        if (!IsInstanceValid(this)) return;

        if (material != null)
        {
            material.AlbedoColor = Colors.Green;
            material.EmissionEnabled = true;
            material.Emission = Colors.Green;
        }

        EconomyService.I.AddCash(_grandTotal);
        _display.Text = "Scan an item on the scanner to start";

        await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
        if (!IsInstanceValid(this)) return;
        
        IsFree = true;
        IsFinishedPaying = true;
    }
}
