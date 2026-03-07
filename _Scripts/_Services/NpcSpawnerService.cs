using Godot;
using System;

public partial class NpcSpawnerService : Node
{
    [Export] private Node3D _npcRoot;
    [Export] private PackedScene _customerNpc;
    public static NpcSpawnerService I;
    public static event Action OnClearAll;
    public int CustomerCount;

    private bool _spawning;


    public override void _Ready()
    {
        I = this;

        Performance.AddCustomMonitor("NpcSpawnerService/FinalWaitTime", Callable.From(() => (Variant)           _finalWaitTime));
        Performance.AddCustomMonitor("NpcSpawnerService/CustomerCount", Callable.From(() => (Variant)           CustomerCount));
        Performance.AddCustomMonitor("NpcSpawnerService/VarietyBonus", Callable.From(() => (Variant)            (EconomyService.I.Variety * _facVariety)));
        Performance.AddCustomMonitor("NpcSpawnerService/AttractivenessBonus", Callable.From(() => (Variant)     EconomyService.I.Attractiveness));
        // Performance.AddCustomMonitor("NpcSpawnerService/IsSpawning?", Callable.From(() => (Variant)              _spawning));
        // Performance.AddCustomMonitor("NpcSpawnerService/AllowedToSpawn?", Callable.From(() => (Variant)          _allowedToSpawn));
        // Performance.AddCustomMonitor("NpcSpawnerService/FactoredCusCount?", Callable.From(() => (Variant)          (CustomerCount * _facCustomerCount)));

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
        if (!CheckSpawnStatus()) return;
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
        bool allowed = true;
        if (WorldService.I.GetCheckout() is null)
        {
            NotificationService.I.Print("No one can shop at your shop because there is no checkout\n\nTo build a new checkout, open the build tool from the menu and go to Misc > Checkout.");
            OnClearAll?.Invoke();
            allowed = false;
        }
        if (WorldService.I.GetCheckoutQueue() is null)
        {
            NotificationService.I.Print("No one can shop at your shop because they have no where to queue\n\nTo build a new queue point, open the build tool from the menu and go to Misc > Queue point.");
            OnClearAll?.Invoke();
            allowed = false;
        }
        if (WorldService.I.GetEntrance() is null)
        {
            NotificationService.I.Print("No one can shop at your shop because there is no entrance\n\nTo build a new entrance, open the build tool from the menu and go to Walls > Entrance.");
            OnClearAll?.Invoke();
            allowed = false;
        }
        _allowedToSpawn = allowed;
        return allowed;
    }

    [Export] private float _facVariety = 0.05f;       // 100 variety = 5s bonus
    [Export] private float _facAttractiveness = 0.03f; // 100 attract = 3s bonus
    [Export] private float _facCustomerCount = 0.5f;   // each customer adds 0.5s
    [Export] private float _minDelay = 2f;
    [Export] private float _maxDelay = 30f;
    private float _finalWaitTime;

    private bool _currentlySpawning;      // loop is running
    private bool _allowedToSpawn;

    public async void StartSpawning()
    {

        if (_currentlySpawning) return;
        _allowedToSpawn = true;
        _currentlySpawning = true;
        
        while (_allowedToSpawn)
        {
            var products = WorldService.I.GetProducts();
            if (products is null || products.Count == 0)
            {
                _allowedToSpawn = false;
                continue;
            }

            if (!CheckSpawnStatus())
            {
                _allowedToSpawn = false;
                continue;
            }

            float varietyBonus = EconomyService.I.Variety * _facVariety;
            float attractivenessBonus = EconomyService.I.Attractiveness * _facAttractiveness;

            float crowdDelay = CustomerCount * _facCustomerCount; // Each customer slows spawnrate down

            _finalWaitTime = crowdDelay - varietyBonus;
            _finalWaitTime = Mathf.Clamp(_finalWaitTime, 3, 60);
            //_finalWaitTime += (float)GD.RandRange(0, 2);

            await ToSignal(GetTree().CreateTimer(_finalWaitTime), SceneTreeTimer.SignalName.Timeout);
            I.SpawnCustomer();
        }
        OnClearAll?.Invoke();
        _currentlySpawning = false;
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
