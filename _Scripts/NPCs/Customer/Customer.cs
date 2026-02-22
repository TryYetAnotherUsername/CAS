// NOTE TO SELF
// The wandering state is not implemented yet, it just despawns.


using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Customer : NPC
{
    // Enums
    public enum State 
    {
        WalkingToEntrance,
        CheckingShoppingList,
        WalkingToShelf,
        UsingShelf,
        WalkingToCheckout,
        UsingCheckout,
        WalkingToExit,
        Despawn
    }

    // Private Variables
    [Export] private State _currentState;
    [Export] private bool _isWander;
    private int _currentShoppingListIndex;
    private ShoppingItem _currentWantItem;

    // shopping item
    public class ShoppingItem
    {
        public ProductEntity Product;
        public int Quantity;
    }

    // Shopping list
    private List<ShoppingItem> _shoppingList = new List<ShoppingItem>();

    //========== Init ==========

    public void Init()
    {
        // null check
        if (WorldService.I is null)
        {
            GD.PrintErr($"Customer {Name}: WorldService not found!");
            return;
        }

        GD.Print($"\n::== Customer {Name}: Starting init.");

        // check if there are any products in this store
        var productsInStore = WorldService.I.GetProducts();
        if (productsInStore is null || productsInStore.Count < 1)
        {
            GD.Print($"Customer {Name}: There are no products in your store!");
            return;
        }

        // Decide how many products to get overall
        var targetProductsCount =  (int) GD.RandRange(0, productsInStore.Count);
       
       // If I decide I don't want any, just wander around.
        if (targetProductsCount == 0)
        {
            _isWander = true;
            GD.Print($"Customer {Name}: Not buying anything, just looking around.");
            SwitchState(State.Despawn);
            return;
        }

        // Do this for the amount of items i want to get
        for (int i = 0; i < targetProductsCount; i++) 
        {
            var index = (int)GD.RandRange(0, productsInStore.Count - 1);
            var quantity = (int)GD.RandRange(1, 5);
            
            // Create the item and add it to the list
            var newItem = new ShoppingItem{Product = productsInStore[index], Quantity = quantity};
            GD.Print($"Customer {Name}: I wanna buy <{newItem.Quantity}> of <{newItem.Product.DispName}>");
            _shoppingList.Add(newItem);
        }

        GD.Print($"==>> Customer {Name}: Init done.\n");
        SwitchState(State.CheckingShoppingList);
    }

    // ========== State machine ========== (btw, why do they even call it that?)
    private void SwitchState(State newState)
    {
        switch (newState)
        {
            case State.WalkingToShelf:
                // Set target to shelf
                Vector3 targetPos = WorldService.I.GetShelf(_currentWantItem.Product).GlobalPosition;
                SetMovementTarget(targetPos);
                GD.Print($"🟩 Target set to shelf to buy the product <{_currentWantItem.Product.DispName}>");
                break;
                
            case State.WalkingToCheckout:
                // Set target for checkout here...
                break;
        }

        _currentState = newState;
        GD.Print($"Customer {Name}: Switching to <{newState}> state.");
    }

    // ========== Process ==========
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        switch (_currentState)
        {
            case State.CheckingShoppingList:
                CheckingShoppingList();
                break;

            case State.WalkingToShelf:
                WalkingToShelf();
                break;

            case State.UsingShelf:
                UsingShelf();
                break;

            case State.WalkingToCheckout:
                WalkingToCheckout();
                break;

            case State.UsingCheckout:
                UsingCheckout();
                break;
            
            case State.WalkingToExit:
                WalkingToExit();
                break;

            case State.Despawn:
                Despawn();
                break;
        }
    }

    // ========== state processes ==========
    private void CheckingShoppingList()
    {
        GD.Print("🟩 Checking my shopping list...");

        if (_currentShoppingListIndex > (_shoppingList.Count - 1)) // if the next item would be out of range
        {
            GD.Print($"Finished my shopping list!");
            SwitchState(State.WalkingToCheckout);
            return;
        }

        GD.Print($"I'm on index {_currentShoppingListIndex} of my shopping list...");

        // Get the target item on list
        _currentWantItem = _shoppingList[_currentShoppingListIndex];
        GD.Print($"I'm going to buy {_currentWantItem.Quantity} of {_currentWantItem.Product.DispName}");

        _currentShoppingListIndex += 1; // no one should use this index after this point

        SwitchState(State.WalkingToShelf);
    }

    private void WalkingToShelf()
    {
        if (_navigationAgent.IsNavigationFinished())
        {
            GD.Print("Arrived at shelf!");
            SwitchState(State.UsingShelf);
        }
    }

    private void UsingShelf()
    {
        GD.Print("🟩 Using shelf...");
        SwitchState(State.CheckingShoppingList);
    }

    private void WalkingToCheckout()
    {
        GD.Print("🟩 Walking to checkout...");
        SwitchState(State.UsingCheckout);
    }

    private void UsingCheckout()
    {
        GD.Print("🟩 Using the checkout...");
        SwitchState(State.WalkingToExit);
    }

    private void WalkingToExit()
    {
        GD.Print("🟩 Walking to exit...");
        SwitchState(State.Despawn);
    }

    private void Despawn()
    {
        GD.Print("🟩 Despawning...");
        QueueFree();
    }
    
}
