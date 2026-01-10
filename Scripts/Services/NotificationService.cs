using Godot;

public partial class NotificationService : Node
{
    // colour coding:
    // 1= blue; 2= orange; none= grey

    [Export] private VBoxContainer NotifRoot;
    [Export] private PackedScene targetNotifScene;

    private static NotificationService myself;
    public override void _Ready()
    {
        myself = this;
    }
    public static void Print(string title, string body, int colour)
    {
        myself?.ExecutePrint(title, body, colour);
    }
    public static void ClearAllNotifs()
    {
        myself?.ExecuteClearAllNotifs();
    }
    private void ExecuteClearAllNotifs()
    {
        foreach(Control child in NotifRoot.GetChildren())
        {
            var notifScript = (NotificationTile) child;
            notifScript.Clear();
        }
    } 
    private void ExecutePrint(string title, string body, int colour)
    {
        var newContent = targetNotifScene.Instantiate();
        NotifRoot.AddChild(newContent);
        var newNotifScript = newContent as NotificationTile; 
        newNotifScript?.Init(title, body, 0, colour);
    }
}