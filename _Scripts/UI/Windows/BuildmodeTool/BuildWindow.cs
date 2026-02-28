using Godot;
using System;

public partial class BuildWindow : Control
{
	[Export] FoldableContainer _infoPanel;
	[Export] RichTextLabel _infoTextBox;

	// Called when the node enters the scene tree for the first time.
	public override void _ExitTree()
	{
		
		BuildToolService.OnPropSelect -= ShowSelect;
		BuildToolService.OnOpenForSelect -= HideSelect;
		BuildToolService.OnPlacingNewPropType -= ShowSelect;

		BuildToolService.I.Sleep();
	}

    public override void _Ready()
    {
		HideSelect();
        BuildToolService.I.Wake();
		BuildToolService.OnPropSelect += ShowSelect;
		BuildToolService.OnOpenForSelect += HideSelect;
		BuildToolService.OnPlacingNewPropType += ShowSelect;
    }

	private void ShowSelect(Prop prop)
	{
		_infoPanel.Visible = true;
		_infoPanel.Title = prop.Identity.DispName;
		_infoTextBox.Text = 
		$"""
		"{prop.Identity.Discription}"
		
		Category: {prop.Identity.CatalogGroup.ToString()}
		Cost: £{prop.Identity.Cost} / unit
		""";
	}

	private void HideSelect()
	{
		_infoPanel.Visible = false;
	}

}
