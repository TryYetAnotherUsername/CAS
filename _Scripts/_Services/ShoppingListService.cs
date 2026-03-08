using Godot;
using System.Collections.Generic;

/// <summary>
/// Generates shopping lists with popularity weighting
/// </summary>
public partial class ShoppingListService : Node
{
    public class ShoppingItem
    {
        public ProductEntity Product;
        public int Quantity;
    }

    public static ShoppingListService I;

    public override void _Ready()
    {
        I = this;
    }

    public  List<ShoppingItem> GetNew()
    {
        List<ShoppingItem> shoppingList = new();
        List<ProductEntity> wantToBuy = new();
        List<ProductEntity> pool = new();
        var productsStocked = WorldService.I.GetProducts();

        if (productsStocked is null) return null;

        foreach (ProductEntity product in productsStocked) // populate pool
        {
            for (int i = 0; i < product.Popularity * 10; i ++) // add to pool n times
            {
                pool.Add(product);
            }
            pool.Add(product);
        }

        while (wantToBuy.Count < GD.RandRange(1,5)) // see what to buy (havn't decided quantity yet)
        {
            var selectedProduct = pool[GD.RandRange(0, pool.Count - 1)];
            if (!wantToBuy.Contains(selectedProduct))
            {
                wantToBuy.Add(selectedProduct);
            }
        }

        foreach (ProductEntity product in wantToBuy)
        {
            int quant = 1;
            if (product.Popularity >= 1)
            {
                quant = GD.RandRange(1,5);
            }
            shoppingList.Add(new ShoppingItem
            {
                Product = product,
                Quantity = quant
            });
        }
        
        #region <old>
        // // check if there are any products in this store
        // var productsInStore = WorldService.I.GetProducts();
        // if (productsInStore is null || productsInStore.Count < 1)
        // {
        //     GD.Print($"Customer {Name}: There are no products in your store!");
        //     return null;
        // }

        //     // Decide how many products to get overall
        //     var targetProductsCount =  (int) GD.RandRange(0, productsInStore.Count);
        
        // // If I decide I don't want any, just wander around.
        // if (targetProductsCount == 0)
        // {
        //     _isWander = true;
        //     GD.Print($"Customer {Name}: Not buying anything, just looking around.");
        //     SwitchState(State.Despawn);
        //     return null;
        // }

        // // Do this for the amount of items i want to get
        // for (int i = 0; i < targetProductsCount; i++) 
        // {
        //     var index = (int)GD.RandRange(0, productsInStore.Count - 1);
        //     var quantity = (int)GD.RandRange(1, 5);
                
        //     // Create the item and add it to the list
        //     var newItem = new ShoppingItem{Product = productsInStore[index], Quantity = quantity};
        //     GD.Print($"Customer {Name}: I wanna buy <{newItem.Quantity}> of <{newItem.Product.DispName}>");
        //     _shoppingList.Add(newItem);
        // }
        #endregion <old>

        return shoppingList;
    }
}