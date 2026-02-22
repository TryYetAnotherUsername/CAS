using Godot;
using System;
using System.Collections.Generic;

public partial class Customer : NPC
{
    // Enums
    public enum State 
    {
        WalkingToEntrance,
        Browsing,
        WalkingToShelf,
        BrowsingShelf,
        WalkingToCheckout,
        WalkingToExit,
        Leaving
    }

    // Private Variables
    private State _currentState;
    private State _isWonderer;

    // Dictionaries
    private Dictionary<ProductEntity, int> ShoppingList;

    //========== Init ==========

    public void Init()
    {

        if (WorldService.I is null)
        {
            GD.PrintErr($"Customer {Name}: WorldService not found!");
            return;
        }

        GD.Print($"==== Customer {Name}: Starting init.");

        var productsInStore = WorldService.I.GetProducts();
        if (productsInStore is null || productsInStore.Count < 1)
        {
            GD.Print($"Customer {Name}: There are no products in your store!");
            return;
        }

        // populate the shopping list

        var targetProductsCount =  (int) GD.RandRange(0, productsInStore.Count);
        if (targetProductsCount == 0)
        {
            GD.Print($"Customer {Name}: Not buying anything, just looking around.");
            return;
        }

        for (int i = 1; i<=targetProductsCount; i+= 1) // repeat this targetProductsCount times
        {
            var index = (int) GD.RandRange(0, productsInStore.Count - 1);
            var quantity = (int) GD.RandRange(1, 5);
            var targetProduct = productsInStore[index];
            GD.Print($"Customer {Name}: I want to buy <{quantity}> of product <{targetProduct.DispName}>");
        }
        
    }

    // ========== State machine ========== (btw, why do they even call it that?)
    private void SwitchState(State newState)
    {
        _currentState = newState;
        GD.Print($"Customer {Name}: Switching to <{newState}> state.");
    }

    
    
}
