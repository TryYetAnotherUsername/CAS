using Godot;
using System;

// Stats and all
public partial class EconomyService : Node
{
    public static EconomyService I;

    public float Cash { get; private set; } = 0;
    public float Variety { get; private set; } = 0;
    public float Attractiveness { get; private set; } = 0;
    public float QueueTime { get; private set; } = 100;
    public float CrowdCap { get; private set; } = 100;
    public float OverallRating { get; private set; } = 0;
    public float EarningBonus { get; private set; } = 0;

    public static event Action <float> OnCashChanged;
    public static event Action OnStatsUpdate;

    public override void _Ready()
    {
        I = this;
        ToolService.OnUpdate += (tool) =>
        {
            if (tool != ToolService.ETools.Economy) return;
            GD.Print("hi");
            WindowService.I.NewWindow(WindowService.EWindowContent.Economy);
            Refresh();
        };
        StartLoop();
    }

	public void Refresh()
	{
        float maxPopularity = 0;
        // calc max popularity
        foreach(var product in ProductConfig.Catalog)
        {
            maxPopularity += product.Popularity;
        }

        // calc variety
        Variety = 0;
        if (WorldService.I.GetProducts() is not null)
        {
            foreach (var product in WorldService.I.GetProducts())
            {
                ModVariety(product.Popularity);
            }
        }
        else
        {
            ModVariety(-100);
        }

        Variety = Mathf.Clamp(Variety / maxPopularity * 100f, 0f, 100f);

        OverallRating = Math.Clamp((Variety + Attractiveness + (100f - QueueTime) + CrowdCap) / 4f, 0f, 100f);
        OnStatsUpdate?.Invoke();
	}

    public async void StartLoop()
    {
        while (true)
        {
            await ToSignal(GetTree().CreateTimer(10f), SceneTreeTimer.SignalName.Timeout);
            Refresh();   
        }
    }

    public void AddCash(float amount)
    {
        Cash += amount;
        MathF.Round(Cash, 2);

        OnCashChanged?.Invoke(Cash);
    }

    public bool TryTakeCash(float amount)
    {
        if (amount > Cash)
        {
            return false;
        } 

        Cash -= amount;
        MathF.Round(Cash, 2);
        
        OnCashChanged?.Invoke(Cash);
        return true;
    }

    public void ModVariety(float percent) => Variety = Math.Clamp(Variety + percent, 0f, 100f);
    public void ModAttractiveness(float percent) => Attractiveness = Math.Clamp(Attractiveness + percent, 0f, 100f);
    public void ModQueueTime(float percent) => QueueTime = Math.Clamp(QueueTime + percent, 0f, 100f);
    public void ModCrowdCap(float percent) => CrowdCap = Math.Clamp(CrowdCap + percent, 0f, 100f);
}
