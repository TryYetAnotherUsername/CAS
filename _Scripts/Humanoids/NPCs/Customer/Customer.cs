// NOTE TO SELF
// The wandering state is not implemented yet, it just despawns.


using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

public partial class Customer : NPC
{
    // Enums
    public enum State 
    {
        WalkingToEntrance,
        CheckingShoppingList,
        WalkingToShelf,
        UsingShelf,
        WalkingToQueue,
        Queueing,
        WalkingToCheckout,
        UsingCheckout,
        WalkingToExit,
        Despawn
    }

    // Private Variables
    [Export] private State _currentState;
    [Export] private bool _isWander;
    [Export] private Label _nameTag;

    private int _currentShoppingListIndex;
    private ShoppingItem _currentWantItem;
    private Checkout _currentCheckout;
    private Shelf _currentShelf;

    // shopping item
    public class ShoppingItem
    {
        public ProductEntity Product;
        public int Quantity;
    }

    // Shopping list
    private List<ShoppingItem> _shoppingList = new List<ShoppingItem>();
    private List<ShoppingItem> _broughtitems = new List<ShoppingItem>();

    private static readonly List<string> _names =
    [
        "pilotimothy",
        "Rowey",
        "Hendo🏀",
        "Haagrid",
        "MassivMisileMustafa",
        "William",
        "Bails",
        "Ben",
        "Cosmc",
    ];

    // ========== Godot native ==========
    public override void _ExitTree()
    {
        NpcSpawnerService.OnClearAll -= QueueFree;
    }

    // ========== Init ==========

    public void Init()
    {
        GD.Randomize();
        
        // null check
        if (WorldService.I is null || NpcSpawnerService.I is null)
        {
            GD.PrintErr($"Customer {Name}: WorldService or NpcSpawnerService not found!");
            return;
        }

        NpcSpawnerService.OnClearAll += QueueFree;

        GD.Print($"\n::== Customer {Name}: Starting init.");

        // set name
        _nameTag.Text = _names[(int) GD.RandRange(0, _names.Count - 1)];

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
                _currentShelf = WorldService.I.GetShelf(_currentWantItem.Product);
                Vector3 targetPos = _currentShelf.NavTarget.GlobalPosition;
                SetMovementTarget(targetPos);
                GD.Print($"🟩 Target set to shelf to buy the product <{_currentWantItem.Product.DispName}>");
                break;

            case State.UsingShelf:
                var result = _currentShelf.TakeProduct(_currentWantItem.Product, _currentWantItem.Quantity);
                _broughtitems.Add(new ShoppingItem{Product = _currentWantItem.Product, Quantity = result});
                GD.Print($"Customer {Name}: Took {result} of {_currentWantItem.Product.DispName} from shelf.");
                break;
                
            case State.WalkingToQueue:
                Checkout checkoutQueue = WorldService.I.GetCheckoutQueue();
                if (WorldService.I.GetCheckoutQueue()is null)
                {
                    QueueFree();
                    return; // the logic here is: if the state gets here, retuns, it never changed the state yet, so it loops back here without stack overflow.
                }
                SetMovementTarget(checkoutQueue.GlobalPosition);
                break;

            case State.Queueing:
                break;

            case State.WalkingToCheckout:
                SetMovementTarget(_currentCheckout.NavTarg.GlobalPosition);
                break;
 
            case State.UsingCheckout:
                if (_currentCheckout == null) // guard condition
                {
                    QueueFree();
                    return;
                }
                _currentCheckout.IsFree = false;
                _currentCheckout.IsFinishedPaying = false;
                _currentCheckout.UseCheckout(_broughtitems);
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

            case State.WalkingToQueue:
                WalkingToQueue();
                break;

            case State.Queueing:
                Queueing();
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
            SwitchState(State.WalkingToQueue);
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

    private void WalkingToQueue()
    {
        if (_navigationAgent.IsNavigationFinished())
        {
            GD.Print("Arrived at queue!");
            SwitchState(State.Queueing);
        }
    }

    private void Queueing()
    {
        _currentCheckout = WorldService.I.GetFreeCheckout();
        if (_currentCheckout is null)
        {
            _currentState = State.Queueing;
            return;
        }
        _currentCheckout.IsFree = false; // reserve a checkout so no one walks to it while i'm walking to it.
        SwitchState(State.WalkingToCheckout);
    }

    private void WalkingToCheckout()
    {
        if (_navigationAgent.IsNavigationFinished())
        {
            GD.Print("Arrived at checkout!");
            SwitchState(State.UsingCheckout);
        }
    }

    private void UsingCheckout()
    {
        if (_currentCheckout.IsFinishedPaying)
        {
            GD.Print("Finished paying.");
            SwitchState(State.WalkingToExit);
        }
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
