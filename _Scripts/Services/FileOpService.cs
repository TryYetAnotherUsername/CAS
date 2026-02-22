using Godot;
using System;
using System.Text.Json;

public partial class FileOpService : Node
{
    public static FileOpService I;
    private FileDialog _fDialog;
    
    public override void _Ready()
    {
        I = this;
    }
    
    public void Open()
    {
        if (_fDialog != null)
        {
            _fDialog.QueueFree();
            _fDialog = null;
        }

        _fDialog = new FileDialog();
        _fDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
        _fDialog.Access = FileDialog.AccessEnum.Filesystem;
        _fDialog.Filters = ["*.casproj"];
		_fDialog.Title = "Open a CAS project file";
		_fDialog.ForceNative = true;
		_fDialog.UseNativeDialog = true;
        AddChild(_fDialog);
        
        _fDialog.FileSelected += OnFileSelected;
        _fDialog.Canceled += OnCanceled;
        
        _fDialog.Popup();

		void OnFileSelected(string p)
		{
			using var file = Godot.FileAccess.Open(p, Godot.FileAccess.ModeFlags.Read);
			
			if (file == null || System.IO.Path.GetExtension(p) != ".casproj")
			{
				GD.PrintErr("Invalid file!");
				return;
			}
			
			string contents = file.GetAsText();
			var deserialised = JsonSerializer.Deserialize<CasProj>(contents);
			
			if (deserialised != null)
			{
				ProjectService.I.In(deserialised);
				GD.Print("Successfully deserialised!");
			}
		}
    
		void OnCanceled()
		{
			GD.Print("File selection canceled");
		}
    }
    

}