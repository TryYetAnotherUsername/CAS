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

    // Get a shelf with specified product. Not nessasarily the closest one, though.
    public Shelf GetShelf(ProductEntity targetProduct)
    {
        List<Shelf> shelvesWithThisProduct = new();

        foreach (Node3D node in _shelvesRoot.GetChildren())
        {
            if (node is Shelf shelf) // found a new shelf
            {
                var stockList = shelf.StockedProductsList;
                foreach (Shelf.StockEntry entry in stockList)
                {
                    if (entry.Product == targetProduct) // Has product
                    {
                        shelvesWithThisProduct.Add(shelf);
                    }
                }
            }
        }

        if (shelvesWithThisProduct.Count == 0) return null;

        var randomisedIndex = (int) GD.RandRange(0, shelvesWithThisProduct.Count - 1);
        Shelf luckyShelf = shelvesWithThisProduct[randomisedIndex];
        
        return luckyShelf;
    }

    public Checkout GetCheckout()
    {
        List<Checkout> freeCheckouts = new();

        foreach (Node3D node in _shelvesRoot.GetChildren())
        {
            if (node is Checkout checkout) // found a new checkout
            {
                if (!checkout.IsQueueTarg)
                {
                    if (checkout.IsFree)
                    {
                        freeCheckouts.Add(checkout);
                    }   
                }
            }
        }

        if (freeCheckouts.Count == 0) return null;
        var randomisedIndex = (int) GD.RandRange(0, freeCheckouts.Count - 1);
        Checkout luckyCheckout = freeCheckouts[randomisedIndex];
        
        return luckyCheckout;
    }

    public Checkout GetCheckoutQueue()
    {
        List<Checkout> queuePoints = new();

        foreach (Node3D node in _shelvesRoot.GetChildren())
        {
            if (node is Checkout target) // found a new checkout/ queuepoint (they share a class, not good practice but oh well)
            {
                if (target.IsQueueTarg)
                {
                    queuePoints.Add(target);
                }
            }
        }

        if (queuePoints.Count == 0) return null;
        var randomisedIndex = (int) GD.RandRange(0, queuePoints.Count - 1);
        Checkout luckyQueuePoint = queuePoints[randomisedIndex];
        
        return luckyQueuePoint;
    }
}
