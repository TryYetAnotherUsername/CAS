using Godot;

public partial class FactoryService : Node
{
	public static FactoryService I;
    [Export] private Node3D _floorsRoot;

    public override void _Ready()
    {
        I = this;   
    }

    public Node3D TrySpawningUidAndGetNode(string uid)
    {
        // Convert the string uid into non-human friendly uid, then into a path:
        long numberUid = ResourceUid.TextToId(uid);
        if (!ResourceUid.HasId(numberUid)) // validation
        {
            GD.PrintErr($"FactoryService: Could not find UID <{uid}>. Returning.");
            return null;
        }
        var path = ResourceUid.GetIdPath(numberUid);

        // Instantiate:
        var scene = GD.Load<PackedScene>(path);
        var newInstance = scene.Instantiate();

        // Modify the new instance:
        Prop newProp = newInstance as Prop; // Cast to (the class) Prop.
        if (newProp is not Prop) // validation
        {
            GD.PrintErr($"FactoryService: Item of UID <{uid}> is not a Prop. Returning.");
            newInstance.QueueFree();
            return null;
        }
        CatalogEntity entityDataObject = CatalogConfig.FindByUID(uid); // Find the CatalogEntity object.
        newProp.Identity = entityDataObject; // Hand back to the new object's Prop script.

        // Reset location on Node3D level:
        Node3D newNode = newInstance as Node3D;
        if (newNode is null) // validation
        {
            GD.PrintErr($"FactoryService: Node3D of UID <{uid}> is null. Returning.");
            newInstance.QueueFree();
            return null;
        }
        newNode.Position = new Vector3(0f,0f,0f);

        // Add as child of building root in main scene tree, and return the node:
        _floorsRoot.AddChild(newInstance);        
        return newNode;
    }

    public void ClearAll()
    {
        foreach (Node node in _floorsRoot.GetChildren())
        {
            node.QueueFree();
        } 
    }
}
