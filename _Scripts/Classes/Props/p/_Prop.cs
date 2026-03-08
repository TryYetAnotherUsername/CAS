using System;
using Godot;

/// <summary>
/// Base class for all props (placeables)
/// </summary>
public partial class Prop : Node3D
{
	public CatalogEntity Identity;
	[Export] public CollisionShape3D PhyCol;
	[Export] public CollisionShape3D RayCol;

	public void SetCollision(bool state)
	{
		if (!state) // if true, set col to enable
		{
			PhyCol.Disabled = true;
			RayCol.Disabled = true;
		}
		else
		{
			PhyCol.Disabled = false;
			RayCol.Disabled = false;
		}
	}

	public void AnimatePlace()
	{
		var tween = CreateTween();

		tween.SetEase(Tween.EaseType.In);
		tween.SetTrans(Tween.TransitionType.Sine);
    	tween.TweenProperty(this, "scale", Scale * 1.25f, 0.1f);
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Back);
    	tween.TweenProperty(this, "scale", new Vector3(1,1,1), 0.3f);
	}

	public void AnimateSelect()
	{
		var tween = CreateTween();

		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Back);
    	tween.TweenProperty(this, "scale", Scale * 0.75f, 0.1f);
    	tween.TweenProperty(this, "scale", new Vector3(1,1,1), 0.2f);
	}

}
