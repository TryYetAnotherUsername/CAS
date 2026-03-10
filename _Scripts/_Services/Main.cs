using Godot;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

/// <summary>
/// "Main"ly for lazy testing. Attatched to root node so the entire scene tree is generated before this runs.
/// </summary>
public partial class Main : Node
{
    [Export] private AudioStreamPlayer _bgm;

    public override void _Ready()
    {
        PathfindingService.Init();
        ProductConfig.Init();

        GD.Print("\n==== Main: Scene tree ready. Hello, world!");
        EconomyService.I.AddCash(8550f);

        PlayCrowd();
    }  

    public async void PlayCrowd()
    {
        while (true)
        {
            await ToSignal(GetTree().CreateTimer(10f), SceneTreeTimer.SignalName.Timeout);
            if (!IsInstanceValid(this)) return;

            var cusCount = NpcSpawnerService.I.CustomerCount;

            if (cusCount < 2)
            {
                _bgm.VolumeDb = -80;
            }
            else
            {
                _bgm.VolumeDb = -25 + cusCount;
            }

            _bgm.VolumeDb = Mathf.Clamp(_bgm.VolumeDb, -80, -5);
        }
    }
}
