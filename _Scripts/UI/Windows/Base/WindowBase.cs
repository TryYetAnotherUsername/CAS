//NOTE TO SELF
//	BUGS
//		When a window is spawned directly on top of another, when dragging, both windows move at the same time with each other
//		This is not game breaking, can be worked around by user if they close the top window, move it, and then try spawning again.
//		


using Godot;
using System;

public partial class WindowBase : Panel
{
	// export
	// All the drag bars:
	[Export] private Control _DragT;
	[Export] private Control _DragL;
	[Export] private Control _DragR;
	[Export] private Control _DragB;
	[Export] private Control _DragBR;
	[Export] private Control _DragBar;
	
	[Export] private PanelContainer _Outline; // window outline
	[Export] private Label _WindowTitle; // window title shown on dragbar (titlebar)

	[Export] private AnimationPlayer _aniPlayer;

	// Window controls:
	[Export] private Button _CloseButton;

	// global vars
	private enum Dir{T, L, R, B, BR, Dragbar, none}
	private Dir currentDir = Dir.none;
	private Vector2 winPos;
	private Vector2 winSize;
	private Vector2 lastMousePos;

	public void Init(string title, Node content)
	{
		_WindowTitle.Text = title;

		if (content != null)
    	{
       		GetNode<Control>("%ContentLayer").AddChild(content);
    	}
	}

    public override void _Ready()
    {
		GiveUpFocus();
        FocusEntered += BringToFocus;
		FocusExited += GiveUpFocus;
		_CloseButton.Pressed += PlayCloseWindow;
		PlayShowWindow();
    }

    public override void _ExitTree()
    {
        FocusEntered -= BringToFocus;
		FocusExited -= GiveUpFocus;
		_CloseButton.Pressed -= PlayCloseWindow;
    }


	// NOTE TO SELF:	This bit is kinda slow and not snappy, could probably do it in Process instead.
	//					just a minor visual anoyance though
	private void PlayShowWindow()
	{
		_aniPlayer.Play("open_window");
	}

	private void PlayCloseWindow()
	{
		_aniPlayer.Play("close_window");
	}

	public override void _Input(InputEvent @event)
	{
		Vector2 currentMousePos = GetGlobalMousePosition();
		if(Input.IsActionJustReleased("window_drag"))
		{
			currentDir = Dir.none;
			var screenSize = GetViewportRect().Size;

			var clampedSize = new Vector2(
				Mathf.Clamp(Size.X, 100, screenSize.X),
				Mathf.Clamp(Size.Y, 100, screenSize.Y)
			);

			var clampedPos = new Vector2(
				Mathf.Clamp(Position.X, 0, screenSize.X - clampedSize.X),
				Mathf.Clamp(Position.Y, 0, screenSize.Y - clampedSize.Y)
			);

			var tween = CreateTween().SetParallel();
			tween.TweenProperty(this, "size", clampedSize, 0.2f).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
			tween.TweenProperty(this, "position", clampedPos, 0.25f).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
			return;
		}

		if(Input.IsActionJustPressed("window_drag"))
		{
			lastMousePos = currentMousePos;
			if (_DragT.GetGlobalRect().HasPoint(currentMousePos))
			{
				currentDir = Dir.T;
			}
			else if (_DragB.GetGlobalRect().HasPoint(currentMousePos))
			{
				currentDir = Dir.B;
			}
			else if (_DragL.GetGlobalRect().HasPoint(currentMousePos))
			{
				currentDir = Dir.L;
			}
			else if (_DragR.GetGlobalRect().HasPoint(currentMousePos))
			{
				currentDir = Dir.R;
			}
			else if (_DragBR.GetGlobalRect().HasPoint(currentMousePos))
			{
				currentDir = Dir.BR;
			}
			else if (_DragBar.GetGlobalRect().HasPoint(currentMousePos))
			{
				currentDir = Dir.Dragbar;
			}
		}

		Vector2 mousePosChange = lastMousePos - currentMousePos;
		Vector2 size = Size;
		Vector2 pos = Position;

		switch(currentDir)
		{
			case Dir.T:
				size.Y += mousePosChange.Y;
				pos.Y -= mousePosChange.Y;
				break;
			case Dir.B:
				size.Y -= mousePosChange.Y;
				//pos.Y -= mousePosChange.Y;
				break;
			case Dir.L:
				size.X += mousePosChange.X;
				pos.X -= mousePosChange.X;
				break;
			case Dir.R:
				size.X -= mousePosChange.X;
				break;
			case Dir.BR:
				size.X -= mousePosChange.X;
				size.Y -= mousePosChange.Y;
				break;
			case Dir.Dragbar:
				pos -= mousePosChange;
				break;
		}

		Size = size;
		Position = pos;
		lastMousePos = currentMousePos;
	}

	private void BringToFocus()
	{
		MoveToFront();
		_Outline.Visible = true;
	}

	private void GiveUpFocus()
	{
		_Outline.Visible = false;
	}

}
