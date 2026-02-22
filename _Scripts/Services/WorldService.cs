using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class WorldService : Node
{
    // instance
    public static WorldService I {get; private set;}

    // export vars
    [Export] private Node3D _shelvesRoot;

    // ========== Godot-lifecycle ==========

    public override void _Ready()
    {
        I = this;
    }

    // ========== public methods ==========

    // Get all products in this store
    public List<ProductEntity> GetProducts()
    {
        List<ProductEntity> products = new(); // init a list

        foreach (Node3D node in _shelvesRoot.GetChildren())
        {
            if (node is Shelf shelf) // found a new shelf
            {
                var stockList = shelf.StockedProductsList;
                foreach (Shelf.StockEntry entry in stockList)
                {
                    bool alreadyAdded = false;
                    foreach (ProductEntity p in products) // check if this product is already added into the list
                    {
                        if (p == entry.Product)
                        {
                            alreadyAdded = true;
                        }
                    }
                    if (alreadyAdded == false) // not added yet- add.
                    {
                        products.Add(entry.Product);
                    }
                }
            }
        }

        return products; // return!
    }

}
