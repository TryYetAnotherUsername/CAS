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
                if (CheckSpawnStatus())
                {
                    StartSpawning();   
                }
            }
        };

        //StartSpawning();
    }

    public void SpawnCustomer()
    {
        if (WorldService.I.GetCheckout() is null)
        {
            GD.PushWarning("No one can shop at your shop because there is no checkout.");
            OnClearAll?.Invoke();
            return;
        }
        if (WorldService.I.GetCheckoutQueue() is null)
        {
            GD.PushWarning("No one can shop at your shop because they have no where to queue.");
            OnClearAll?.Invoke();
            return;
        }

        GD.Print("NpcSpawnerService: Starting to spawn a new customer.");
        var customerInst = _customerNpc.Instantiate();
        _npcRoot.AddChild(customerInst);

        if (customerInst is Customer customer)
        {
            customer.Init();
        }
    }

    public bool CheckSpawnStatus()
    {
        if (WorldService.I.GetCheckout() is null)
        {
            NotificationService.I.Print("No one can shop at your shop because there is no checkout\n\nTo build a new checkout, open the build tool from the menu and go to Misc > Checkout.");
            OnClearAll?.Invoke();
            return false;
        }
        if (WorldService.I.GetCheckoutQueue() is null)
        {
            NotificationService.I.Print("No one can shop at your shop because they have no where to queue\n\nTo build a new queue point, open the build tool from the menu and go to Misc > Queue point.");
            OnClearAll?.Invoke();
            return false;
        }
        return true;
    }

    public async void StartSpawning()
    {
        while (true)
        {
            await ToSignal(GetTree().CreateTimer(GD.RandRange(1,5)), SceneTreeTimer.SignalName.Timeout);
            I.SpawnCustomer();
        }
    }

}
