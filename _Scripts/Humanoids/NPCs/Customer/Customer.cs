// NOTE TO SELF
// The wandering state is not implemented yet, it just despawns.

using Godot;
using System.Collections.Generic;

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
    private State _currentState;
    [Export] private bool _isWander;
    [Export] private Label _nameTag;
    [Export] private AnimationTree _aniTree;


    private int _currentShoppingListIndex;
    private ShoppingListService.ShoppingItem _currentWantItem;
    private Checkout _currentCheckout;
    private Shelf _currentShelf;

    private Timer _timer;

    // Shopping list
    private List<ShoppingListService.ShoppingItem> _shoppingList = new ();
    private List< ShoppingListService.ShoppingItem> _broughtitems = new ();

    private static readonly List<string> _names =
    [
        // "pilotimothy",
        // "Rowey",
        // "Hendo🏀",
        // "Haaris",
        // "Massive Missile Mustafa",
        // "Will",
        // "Bails",
        // "Ben",
        "Customer"
    ];

    // ========== Godot native ==========
    public override void _ExitTree()
    {
        NpcSpawnerService.I.SubtractCount();
        NpcSpawnerService.OnClearAll -= QueueFree;
    }

    // ========== Init ==========

    public void Init()
    {
        NpcSpawnerService.I.AddCount();


        GD.Print($"\n::== Customer {Name}: Starting init.");
        
        _timer = new Timer();
        AddChild(_timer);

        // sub to events
        NpcSpawnerService.OnClearAll += QueueFree;

        _shoppingList = ShoppingListService.I.GetNew();

        // set name
        _nameTag.Text = _names[(int) GD.RandRange(0, _names.Count - 1)];

        GD.Print($"==>> Customer {Name}: Init done.\n");
        SwitchState(State.CheckingShoppingList);

        if (_shoppingList is null)
        {
            GD.PrintErr("My shopping list is null.");
            QueueFree();
        }

        GlobalPosition = WorldService.I.GetEntrance().GlobalPosition;
        GD.Print(_navigationAgent);
    }

    // ========== State machine ========== (btw, why do they even call it that?)
    private void SwitchState(State newState)
    {
        int aniBlendVal = 0;
        switch (newState)
        {
            case State.WalkingToShelf:
                // Set target to shelf
                aniBlendVal = 0;
                _currentShelf = WorldService.I.GetShelf(_currentWantItem.Product);
                Vector3 targetPos = _currentShelf.NavTarget.GlobalPosition;
                SetMovementTarget(targetPos);
                GD.Print($"🟩 Target set to shelf to buy the product <{_currentWantItem.Product.DispName}>");
                break;

            case State.UsingShelf:
                GD.Print("🟩 Using shelf...");
                aniBlendVal = 1;
                _currentShelf.TryTakeProduct(_currentWantItem.Product, _currentWantItem.Quantity, (resultQuantity) =>
                {
                    var result = resultQuantity;

                    if (result == -1)
                    {
                        Print("I couldn't find any " + _currentWantItem.Product.DispName + " in stock!");
                        EconomyService.I.ModAttractiveness(-2);
                        SwitchState(State.CheckingShoppingList);
                        return;
                    }

                    Print("I found " + _currentWantItem.Product.DispName + " !");
                    EconomyService.I.ModAttractiveness(2);
                    _broughtitems.Add(new ShoppingListService.ShoppingItem{Product = _currentWantItem.Product, Quantity = result});
                    GD.Print($"Customer {Name}: Took {result} of {_currentWantItem.Product.DispName} from shelf.");
                    SwitchState(State.CheckingShoppingList);
                });

                break;
                
            case State.WalkingToQueue:
                aniBlendVal = 0;
                Checkout checkoutQueue = WorldService.I.GetCheckoutQueue();
                if (WorldService.I.GetCheckoutQueue()is null)
                {
                    QueueFree();
                    return; // the logic here is: if the state gets here, retuns, it never changed the state yet, so it loops back here.
                }
                SetMovementTarget(checkoutQueue.GlobalPosition);
                break;

            case State.Queueing:
                aniBlendVal = 1;
                _timer.WaitTime = 10;
                _timer.Start();
                _timer.Timeout += () => LeaveAndComplain("I've been waiting here for too long! 🤬");
                break;

            case State.WalkingToCheckout:
                aniBlendVal = 0;
                SetMovementTarget(_currentCheckout.NavTarg.GlobalPosition);
                break;
 
            case State.UsingCheckout:
                aniBlendVal = 1;
                if (_currentCheckout == null) // guard condition
                {
                    QueueFree();
                    return;
                }
                var tween2 = CreateTween();
                tween2.TweenProperty(_aniTree, "parameters/blend_2/blend_amount", 1, 0.5f);
                _currentCheckout.IsFree = false;
                _currentCheckout.IsFinishedPaying = false;
                _currentCheckout.UseCheckout(_broughtitems);
                break;

            case State.WalkingToExit:
                aniBlendVal = 0;
                GD.Print("🟩 Walking to exit...");
                SetMovementTarget(WorldService.I.GetEntrance().GlobalPosition);
                break;

        }

        var tween1 = CreateTween();
        tween1.TweenProperty(_aniTree, "parameters/blend_2/blend_amount", aniBlendVal, 0.5f);

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
        //GD.Print("🟩 Using shelf...");
        //SwitchState(State.CheckingShoppingList);
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

        _timer.Stop();
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
        if (_navigationAgent.IsNavigationFinished())
        {
            GD.Print("Finished paying.");
            SwitchState(State.Despawn);
        }
    }

    private void Despawn()
    {
        GD.Print("🟩 Despawning...");
        QueueFree();
    }
    
    private void LeaveAndComplain(string message)
    {
        NotificationService.I.Print("One of your customers just left without paying. Prehaps you should treat them better?\nTry building more checkouts.");
        SwitchState(State.WalkingToExit);
    }
}
