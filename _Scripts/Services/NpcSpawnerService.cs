using Godot;
using System;

public partial class NpcSpawnerService : Node
{
    [Export] private Node3D _npcRoot;
    [Export] private PackedScene _customerNpc;
    public static NpcSpawnerService I;
    public static event Action OnClearAll;

    public override void _Ready()
    {
        I = this;
        
        ToolService.OnUpdate += (tool) =>
        {
            if (tool == ToolService.ETools.SpawnACustomer)
            {
                SpawnCustomer();
            }
        };

        StartSpawning();
    }

    public void SpawnCustomer()
    {
        GD.Print("NpcSpawnerService: Starting to spawn a new customer.");
        var customerInst = _customerNpc.Instantiate();
        _npcRoot.AddChild(customerInst);

        if (customerInst is Customer customer)
        {
            customer.Init();
        }
    }

    public async void StartSpawning()
    {
        while (true)
        {
            if (WorldService.I.GetCheckout() is null)
            {
                GD.PrintErr("No one can shop at your shop because you dont have a Checkout.");
                OnClearAll?.Invoke();
                await ToSignal(GetTree().CreateTimer(5f), SceneTreeTimer.SignalName.Timeout);
                continue;
            }
            if (WorldService.I.GetCheckoutQueue() is null)
            {
                GD.PrintErr("No one can shop at your shop because they have no where to queue.");
                OnClearAll?.Invoke();
                await ToSignal(GetTree().CreateTimer(5f), SceneTreeTimer.SignalName.Timeout);
                continue;
            }

            await ToSignal(GetTree().CreateTimer(GD.RandRange(1,5)), SceneTreeTimer.SignalName.Timeout);
            I.SpawnCustomer();
        }
    }

    
}
