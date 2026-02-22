using Godot;
using System;

public partial class CustomerSpawnerService : Node
{
    public override void _Ready()
    {
        StartSpawning();
    }

    public async void StartSpawning()
    {
        while (true)
        {
            await ToSignal(GetTree().CreateTimer(GD.RandRange(1,5)), SceneTreeTimer.SignalName.Timeout);
            NpcSpawnerService.I.SpawnCustomer();
        }
    }
}
