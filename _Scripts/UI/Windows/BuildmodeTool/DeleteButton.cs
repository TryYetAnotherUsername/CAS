using Godot;
using System;

public partial class DeleteButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;
		Pressed += () => 
		{
			Visible = false;
			BuildToolService.I.DeleteSelected();
		};
		BuildToolService.OnPropSelect += SetVisTrue;
		BuildToolService.OnOpenForSelect += SetVisFalse;
	}

    public override void _ExitTree()
    {
        BuildToolService.OnPropSelect -= SetVisTrue;
		BuildToolService.OnOpenForSelect -= SetVisFalse;
    }


	private void SetVisTrue(Prop _)
	{
		Visible = true;
	}

	
	private void SetVisFalse()
	{
		Visible = false;
	}
}
