using Godot;
using System;
using System.ComponentModel;

public partial class EconomyView : Control
{
	[Export] private Label _attractiveness;
	[Export] private Label _variety;
	[Export] private Label _queueTime;
	[Export] private Label _crowdCap;
	[Export] private Label _money;
	[Export] private Label _ratingString;
	[Export] private Label _numberOfPeople;

	public override void _Ready()
	{
		Refresh();
		EconomyService.OnStatsUpdate += Refresh;
	}

    public override void _ExitTree()
    {
        EconomyService.OnStatsUpdate -= Refresh;
    }

	private void Refresh()
	{
		GD.Print("Cuscount" + NpcSpawnerService.I.CustomerCount);
		_attractiveness.Text = $"{EconomyService.I.Attractiveness}%";
		_variety.Text = $"{EconomyService.I.Variety}%";
		_queueTime.Text = $"{EconomyService.I.QueueTime}%";
		_crowdCap.Text = $"{EconomyService.I.CrowdCap}%";
		_money.Text = $"Cash: £{Mathf.Round(EconomyService.I.Cash)}";
		_numberOfPeople.Text = $"{NpcSpawnerService.I.CustomerCount} people in your store";
		_ratingString.Text = $"Rating: {EconomyService.I.OverallRating} - (Bonus +{EconomyService.I.EarningBonus}% income)";
	}
}
