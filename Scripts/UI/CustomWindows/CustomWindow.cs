using Godot;
using System;

/*

NOTES TO SELF

----------Issues----------
-	[FIXED WITH NEW ISSUES] Dragging sometimes turns into drag top, probably when it detects 
	mouse down it clips the top bar. Add a var to not detect when dragging.
-	Previous window resizes on new window spawn (!??)
-	Needs 2 clicks to drag window after focus

-	[TO-DO] This is the most messy script in the project, needs full switch to Godot focus system.
	(I think it should work, I think.)

----------To do----------
-	[DONE] Needs service to spawn this scene. Additional scene to be put into
	ContentLayer.

*/

public partial class CustomWindow : Control
{
	// Important
	[Export] public bool AllowClose = true; // when this is set to false the window will not close

	// Export (dependencies)
	[Export] private Control Drag_Corner;

	[Export] private Control Drag_Right;
	[Export] private Control Drag_Bottom;
	[Export] private Control Drag_Top;
	[Export] private Control Drag_Left;
	[Export] private Control Drag_Bar;

	[Export] private Label windowTitle;
	[Export] private Control OutlineLayer;
	[Export] private Color defocusedModulate;
	[Export] private Button CloseButton;
	[Export] private PanelContainer _PanelContainer;

	//  global variables
	private Vector2 _winSize, _winPos;
	public static CustomWindow ActiveWindow;
	private Dir _lastKnownDir;
	private Dir _currentDir;
	private Vector2 _mousePosPrevious;
	private Vector2 _mousePosChange;
	public static event Action OnNewFocus;
	private enum Dir
    {
        Corner, Left, Right, Top, Bottom, DragBar, None
    }

	// Public methods
	public void Init(string title)
    {
		_lastKnownDir = Dir.None;
		_currentDir = Dir.None;
		windowTitle.Text = title;
		WindowService.DefocusAll += Defocus;
		OnNewFocus += CheckIfFocused;
		CloseButton.Pressed += () => Relay.Exe("window.closefocused");
		
		OutlineLayer.FocusExited += () =>
		{
			_PanelContainer.Visible = false;
		};

		OutlineLayer.FocusEntered += () =>
		{
			_PanelContainer.Visible = true;
			BringToFocus();
		};


		BringToFocus();
	}
	public void CloseWindow()
    {
		if (AllowClose)
        {
			QueueFree();
			ConsoleService.Print("Active window instance: Window queued free.");
        }
		else
        {
            ConsoleService.Print("Active window instance: Window closing is blocked, returning.");
			return;
        }
    }
	public void BringToFocus()
	{
		ActiveWindow = this;
		OnNewFocus?.Invoke();
		MoveToFront();
	}

	// Godot native methods
    public override void _Process(double delta)
    {
		Vector2 mousePosCurrent = GetGlobalMousePosition();
    	_mousePosChange = mousePosCurrent - _mousePosPrevious;
    	_mousePosPrevious = mousePosCurrent;

		if (ActiveWindow != this)
        {
            return;
        }
	
		_winPos = OutlineLayer.Position;
		_winSize = OutlineLayer.Size;

		if (Input.IsActionJustPressed("window_drag_side"))
        {
            CheckMousePos();
        }

		if (Input.IsActionPressed("window_drag_side"))
        {
			transform();
        }
		else
        {
            CheckMousePos();
        }

		OutlineLayer.Position = _winPos;
		OutlineLayer.Size = _winSize;
    }
	public override void _ExitTree()
	{
		// Always unsubscribe when the window is destroyed
		OnNewFocus -= CheckIfFocused;
		WindowService.DefocusAll -= Defocus;
	}

	// Private Methods
	private void CheckMousePos()
    {
        if (Drag_Bar.GetGlobalRect().HasPoint(GetGlobalMousePosition()))
        {
            _currentDir = Dir.DragBar;
        }
		else if (Drag_Bottom.GetGlobalRect().HasPoint(GetGlobalMousePosition()))
        {
            _currentDir = Dir.Bottom;
        }
		else if (Drag_Corner.GetGlobalRect().HasPoint(GetGlobalMousePosition()))
        {
            _currentDir = Dir.Corner;
        }
		else if (Drag_Right.GetGlobalRect().HasPoint(GetGlobalMousePosition()))
        {
            _currentDir = Dir.Right;
        }
		else if (Drag_Left.GetGlobalRect().HasPoint(GetGlobalMousePosition()))
        {
            _currentDir = Dir.Left;
        }
		else if (Drag_Top.GetGlobalRect().HasPoint(GetGlobalMousePosition()))
        {
            _currentDir = Dir.Top;
        }
		else
        {
            _currentDir = Dir.None;
        }
    }
	private void transform()
    {
        switch (_currentDir)
			{
				case Dir.Right:
					_winSize.X += _mousePosChange.X;
					break;

				case Dir.Left:
					_winSize.X -= _mousePosChange.X;
					_winPos.X += _mousePosChange.X;
					break;

				case Dir.Top:
					_winSize.Y -= _mousePosChange.Y;
					_winPos.Y += _mousePosChange.Y;
					break;

				case Dir.Bottom:
					_winSize.Y += _mousePosChange.Y;
					break;

				case Dir.Corner:
					_winSize.Y += _mousePosChange.Y;
					_winSize.X += _mousePosChange.X;
					break;

				case Dir.DragBar:
					_winPos += _mousePosChange;
					break;
			}
    }
	private void CheckIfFocused()
	{
		if (ActiveWindow == this)
		{
			OutlineLayer.Modulate = new Color(1, 1, 1, 1);
		}
		else
		{
			Defocus();
		}
	}
	private void Defocus()
    {
        _lastKnownDir = Dir.None;
		_currentDir = Dir.None;
		OutlineLayer.Modulate = defocusedModulate;
    }
}
