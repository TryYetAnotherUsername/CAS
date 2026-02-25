using System.Collections.Generic;
using Godot;

/// <summary>
/// Each shelf has fields for its stored products, exposes methods for adding/ subtracting products, and logic to render out the products on the shelf.
/// </summary>

// Issue #11 - unify checklists

public partial class Shelf : Prop
{
	private class ShelfVisuals
	{
		// vars
		private Node3D _productAreasRoot;
		private Shelf _shelf;
		private List<Area3D> _productAreas = new();

		// constructor
		public ShelfVisuals(Shelf theClass)
		{
			_productAreasRoot = theClass._productAreasRoot;
			_shelf = theClass;
			ScanAreas();
		}

		// private methods
		public void Refresh()
		{
			Clear();
			if (_shelf.StockedProductsList is null || _shelf.StockedProductsList.Count == 0 || _shelf.StockedProductsList[0] is null || _shelf.StockedProductsList[0].Quantity == 0) return;
			string targProductUid = _shelf.StockedProductsList[0].Product.UID;
			int targProductQuantity = _shelf.StockedProductsList[0].Quantity;
			PackedScene scene = GD.Load<PackedScene>(ResourceUid.GetIdPath(ResourceUid.TextToId(targProductUid)));

			foreach (Area3D area in _productAreas) // for each row
			{
				var targAreaCol = area.GetChild(0) as CollisionShape3D;
				if (targAreaCol is null) return;
				var box = targAreaCol.Shape as BoxShape3D;
				if (box is null) return;

				float rowWidth = box.Size.X;
				float rowDepth = box.Size.Z;

				Vector3 accumOffset = Vector3.Zero;

				// for each product entity
				for (int i = 1; i <= targProductQuantity; i++)
				{
					var newNode = scene.Instantiate() as Node3D;
					if (newNode is null) return;
					var mesh = newNode.GetChild(0) as MeshInstance3D;
					if (mesh is null) return;

					area.AddChild(newNode);

					Vector3 baseOffset = new Vector3(mesh.GetAabb().Size.X / 2, 0, -mesh.GetAabb().Size.Z / 2);

					newNode.Position = accumOffset + baseOffset;
					accumOffset += new Vector3(0 , 0, -mesh.GetAabb().Size.Z - 0.005f);
				}
			}
		}

		private void ScanAreas()
		{
			foreach (Node node in _productAreasRoot.GetChildren())
			{
				if (node is Area3D area)
				{
					_productAreas.Add(area);
				}
			}
		}

		private void Clear()
		{
			foreach (Area3D area in _productAreas)
			{
				foreach (Node node in area.GetChildren())
				{
					if (node is not CollisionShape3D)
					{
						node.Free();
					}
				}
			}
		}

	}


	[Export] private Node3D _productAreasRoot;
	private ShelfVisuals _Visuals;

    #region Godot methods
    public override void _Ready()
    {
        _Visuals = new ShelfVisuals(this);
		_Visuals.Refresh();
    }
	#endregion Godot methods

	#region Stuff

	/// <summary>
	/// Stores the quantity of each product.
	//  NTS- It dosen't need a class, but probably more scaleable, for expiry dates, batch quality, etc.
	/// </summary>
	
    public class StockEntry
    {
        public ProductEntity Product = ProductConfig.Catalog[0];
        public int Quantity = 0;
    }

    public List<StockEntry> StockedProductsList = new();

	[Export] public Node3D NavTarget;

	#endregion Stuff

	#region Public methods

	public void SetProductStock(ProductEntity targProduct, bool status)
	{
		var entry = GetEntryFromStocked(targProduct);
		if (status == true) // want to stock
		{
			if (entry != null) // already stocked, do nothing.
			{
				GD.Print("A shelf: Tried to stock a product, but that product is already stocked on this shelf.");
				return;
			}
			else // not stocked yet, stock.
			{
				StockedProductsList.Add(new StockEntry{Product = targProduct, Quantity = 0});
				GD.Print("A shelf: Stocked a new product");
				_Visuals.Refresh();
			}

		}
		else // want to unstock
		{
			if (entry != null) // stocked before, unstock now
			{
				StockedProductsList.Remove(entry);
				GD.Print("A shelf: Unstocked a product.");
				_Visuals.Refresh();
			}
			else // no such stock, do nothing
			{
				GD.Print("A shelf: Could not unstock, that product is not stocked.");
			}
		}
	}

	public bool IsStocked(ProductEntity targProduct)
	{
		var entry = GetEntryFromStocked(targProduct);
		if (entry is null)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public void AddProduct(ProductEntity targProduct, int amount)
	{
		var entry = GetEntryFromStocked(targProduct);
		if (entry is null)
		{
			GD.Print("A shelf: Tried to add a product, but that product is not stocked on this shelf.");
			return;
		}
		else
		{
			entry.Quantity += amount;
			GD.Print("A shelf: Added some products.");
			_Visuals.Refresh();
			return;
		}
	}

	public int TakeProduct(ProductEntity targProduct, int amount)
	{
		var entry = GetEntryFromStocked(targProduct);
		if (entry is null)
		{
			GD.Print("A shelf: Tried to take a product, but that product is not stocked on this shelf.");
			return 0;
		}
		else
		{
			if (entry.Quantity == 0)
			{
				GD.Print("A shelf: Failed to take any products, shelf is empty.");
				return 0;
			}
			else if (entry.Quantity >= amount)
			{
				entry.Quantity -= amount;
				GD.Print($"A shelf: Took {amount} products. {entry.Quantity} remaining.");
				_Visuals.Refresh();
				return amount;
			}
			else // quantity > 0 but < amount
			{
				int taken = entry.Quantity;
				entry.Quantity = 0;
				GD.Print($"A shelf: Only had {taken}, took all that were left.");
				_Visuals.Refresh();
				return taken;
			}
			
		}
	}
	#endregion Public methods
	
	#region Private methods
	private StockEntry GetEntryFromStocked(ProductEntity targProduct)
	{
		foreach (StockEntry entry in StockedProductsList)
		{
			if (entry.Product == targProduct)
			{
				return entry;
			}
		}
		return null;
	}
	#endregion Private methods
}