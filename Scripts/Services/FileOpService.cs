using Godot;
using System;
using System.Text.Json;

public partial class FileOpService : Node
{
    // Private global variables
    private string _currentIntention;
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        IncludeFields = true,
        WriteIndented = true,
    };
    
    // Built in methods
	public override void _Ready()
    {
        Relay.FileOpService = this;
    }
    
    // Public methods
    public void SaveFile()
    {
        ConsoleService.Print($"FileService: Attempting to save file.");
        
        string currentFilePath = CurrentSession.Path;

        if (currentFilePath == null)
        {
            NewFile();
            ConsoleService.Print($"FileService: No file loaded, creating new file.");
        }
        else
        {
            CurrentSession.ProjectData.Metadata.LastModifiedUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // Used to throw error here, I think its fixed
            using var file = FileAccess.Open(currentFilePath, FileAccess.ModeFlags.Write);
            string jsonData = JsonSerializer.Serialize(CurrentSession.ProjectData, JsonOptions);
            file.StoreString(jsonData);
            ConsoleService.Print($"FileService: Saved current file.");
            NotificationService.Print("File saved", $"Active session has been saved to path: {currentFilePath}", 1);
            GD.Print("hi");
        }

    }
    public void NewFile()
    {
        ConsoleService.Print($"FileService: Attempting to create file.");

        FileDialog dialog = NewDialog();
        Modify(dialog);

        dialog.FileSelected += (path) =>
        {
            CreateNewFile(path);
            CurrentSession.Path = path;
            dialog.QueueFree();
        };

        dialog.Popup();


        static void CreateNewFile(string path)
        {
            // Create blank ProjectData and initialise
            CurrentSession.ProjectData = new();
            CurrentSession.ProjectData.Metadata.CreatedUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            CurrentSession.ProjectData.Metadata.LastModifiedUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            CurrentSession.ProjectData.NextAvailableId = 0;

            // Make new .casproj file at the path
            path = System.IO.Path.ChangeExtension(path, ".casproj");
            using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);

            // Write ProjectData to file
            string jsonData = JsonSerializer.Serialize(CurrentSession.ProjectData, JsonOptions);         
            file.StoreString(jsonData);
            
            ConsoleService.Print($"FileService: New file created at <{path}>.");
            NotificationService.Print("New file created", $"New file is now loaded: {path}", 1);
        }

        static void Modify(FileDialog dialog)
        {
            dialog.Mode = Window.ModeEnum.Windowed;
            dialog.InitialPosition = Window.WindowInitialPosition.CenterPrimaryScreen;
            dialog.UseNativeDialog = true;
            dialog.FileMode = FileDialog.FileModeEnum.SaveFile;
            dialog.Title = "Select a place to place the new file";
            dialog.Access = FileDialog.AccessEnum.Filesystem;
            dialog.Filters = ["*.casproj ; CAS Project Files"];
        }
    }
    public void OpenFile()
    {
        ConsoleService.Print($"FileService: Attempting to open file.");

        FileDialog dialog = NewDialog();
        Modify(dialog);

        dialog.FileSelected += (selectedPath) =>
        {
            LoadFileToMemory(selectedPath);
            CurrentSession.Path = selectedPath;
            dialog.QueueFree();
        };

        dialog.Popup();


        static void LoadFileToMemory(string path)
        {
            if (System.IO.Path.GetExtension(path) != ".casproj")
            {
                ConsoleService.PrintErr($"FileService: Not a .casproj file.");
            }
            else
           {
                string content = System.IO.File.ReadAllText(path);

                // JSON => C# data (yay!!!)
                ProjectData importedData = JsonSerializer.Deserialize<ProjectData>(content, JsonOptions);

                if (importedData != null)
                {
                    CurrentSession.ProjectData = importedData;
                    ConsoleService.Print($"FileService: Successfully loaded {importedData.Vertices.Count} vertices.");
                    NotificationService.Print("File loaded", $"This file has {importedData.Vertices.Count} vertices", 1);
                } 
           }
        }

        static void Modify(FileDialog dialog)
        {
            dialog.Mode = Window.ModeEnum.Windowed;
            dialog.InitialPosition = Window.WindowInitialPosition.CenterPrimaryScreen;
            dialog.UseNativeDialog = true;
            dialog.FileMode = FileDialog.FileModeEnum.OpenFile;
            dialog.Title = "Open a file";
            dialog.Access = FileDialog.AccessEnum.Filesystem;
            dialog.Filters = ["*.casproj ; CAS Project Files"];
        }
    }
    private FileDialog NewDialog()
    {
        FileDialog dialog = new FileDialog();
        AddChild(dialog);
        return dialog;
    }

}