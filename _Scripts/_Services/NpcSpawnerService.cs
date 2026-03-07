using Godot;
using System;
using System.Threading;

public partial class NpcSpawnerService : Node
{
    [Export] private Node3D _npcRoot;
    [Export] private PackedScene _customerNpc;
    public static NpcSpawnerService I;
    public static event Action OnClearAll;
    public int CustomerCount;

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

    [Export] private float _facProductVariety = 0;
    [Export] private float _facCustomerCount = 0;
    [Export] private float _baseDelay = 3;
    [Export] private float _finalWaitTime;

    public async void StartSpawning()
    {
        while (true)
        {
            var products = WorldService.I.GetProducts();
            if (products is null || products.Count == 0)
            {
                return;
            }

            float varietyBonus = products.Count * _facProductVariety; // Each product avalible speeds spawnrate up by 0.2s
            float crowdDelay = CustomerCount * _facCustomerCount; // Each customer slows spawnrate down by 0.5s

            float _finalWaitTime = _baseDelay + varietyBonus - crowdDelay;
            _finalWaitTime = MathF.Min(_finalWaitTime, 3f);
            _finalWaitTime += GD.RandRange(1,5);

            await ToSignal(GetTree().CreateTimer(_finalWaitTime), SceneTreeTimer.SignalName.Timeout);
            I.SpawnCustomer();
        }
    }

    public void AddCount()
    {
        CustomerCount ++;
    }

    public void SubtractCount()
    {
        CustomerCount --;
    }

}
