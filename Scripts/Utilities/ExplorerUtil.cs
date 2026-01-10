using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class ExplorerUtil : Node
{
	public event Action<string> OnFileOpenConfirmed;
	public event Action<string> OnFileSaveConfirmed;
	// public event Action<string> OnFailed;
 
private void CreateDialog(FileDialog.FileModeEnum mode)
{
	ConsoleService.Print("ExplorerUtil: Attempting to create dialogue");

    FileDialog dialog = new FileDialog();
    AddChild(dialog);

    dialog.Mode = Window.ModeEnum.Windowed;
    dialog.InitialPosition = Window.WindowInitialPosition.CenterPrimaryScreen;
    dialog.UseNativeDialog = true;
    dialog.FileMode = mode;
    dialog.Title = (mode == FileDialog.FileModeEnum.SaveFile) ? "Save File" : "Open File";
	dialog.Access = FileDialog.AccessEnum.Filesystem;
    dialog.Filters = ["*.casproj ; CAS Project Files"];

    dialog.FileSelected += (path) => 
    {
        if (mode == FileDialog.FileModeEnum.SaveFile)
            OnFileSaveConfirmed?.Invoke(path);
        else
            OnFileOpenConfirmed?.Invoke(path);
            
        dialog.QueueFree();
    };

    dialog.Canceled += () => 
	{
		dialog.QueueFree();
	};

    dialog.Popup();
}
}
