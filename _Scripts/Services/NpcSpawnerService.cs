using Godot;
using System;

public partial class NpcSpawnerService : Node
{
    [Export] private Node3D _npcRoot;
    [Export] private PackedScene _customerNpc;

    public override void _Ready()
    {
        SpawnCustomers(10, 10);
    }

    public async void SpawnCustomers(int count, float interval)
    {
        for (int i = 0; i < count; i++)
        {
            var customerInst = _customerNpc.Instantiate();
            _npcRoot.AddChild(customerInst);
            if (customerInst is Customer customer)
            {
                customer.Init();
            }
            
            await ToSignal(GetTree().CreateTimer(interval), SceneTreeTimer.SignalName.Timeout);
        }
    }
}
