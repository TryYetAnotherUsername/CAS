using Godot;
using System;

public partial class NpcSpawnerService : Node
{
    [Export] private Node3D _npcRoot;
    [Export] private PackedScene _customerNpc;

    public override void _Ready()
    {
        ToolService.OnUpdate += (tool) =>
        {
            if (tool == ToolService.ETools.SpawnACustomer)
            {
                SpawnCustomer();
            }
        };
    }

    public void SpawnCustomer()
    {
        var customerInst = _customerNpc.Instantiate();
        _npcRoot.AddChild(customerInst);

        if (customerInst is Customer customer)
        {
            customer.Init();
        }
    }
}
