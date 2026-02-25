using Godot;
using System;

public partial class EconomyService : Node
{
    public static EconomyService I;

    public float Cash { get; private set; } = 0;
    public static event Action <float> OnCashChanged;

    public override void _Ready()
    {
        I = this;
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
}
