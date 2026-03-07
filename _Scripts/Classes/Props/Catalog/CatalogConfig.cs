using Godot;
using System.Collections.Generic;
using System.Linq;
using static CatalogEntity;

public static class CatalogConfig
{
    public static readonly List<CatalogEntity> Catalog =
    [
        // ========== FLOORS ==========
        new CatalogEntity { DispName = "Wooden floor",      UID = "uid://y7yesnlndrfg",  CatalogGroup = EGroup.Floors , Cost = 10, Discription = "A nice floor your customers may walk on."},
        new CatalogEntity { DispName = "Ceiling", UID = "uid://8bnwbc5t1l0m", CatalogGroup = EGroup.Floors, Cost = 5, Discription = "A nice ceiling your customer's won't walk on."},
        // ========== WALLS ==========
        new CatalogEntity { DispName = "Wall",              UID = "uid://dtmwx7ig68eac", CatalogGroup = EGroup.Walls, Cost = 10, Discription = "Probably the most boring object in this game?"},
        new CatalogEntity { DispName = "Wall (Double)",     UID = "uid://bkwafr84yuc0q", CatalogGroup = EGroup.Walls, Cost = 20, Discription = "A bland, boring wall. A tad bit less boring than the other one."},
        new CatalogEntity { DispName = "Wall (Windowed)",   UID = "uid://b37vt42e0fhud", CatalogGroup = EGroup.Walls, Cost = 25, Discription = "A wall with a big window cut through it. Zombies hate these."},
        new CatalogEntity { DispName = "Entrance",          UID = "uid://cooe23t4c7kcc", CatalogGroup = EGroup.Walls, Cost = 100, Discription = "Customers enter your store from the entrance.\nAt least 1 of this item is required in the store."},
        // ========== SHELVES ==========
        new CatalogEntity { DispName = "Basic shelf",       UID = "uid://c3rwyrjlanyer", CatalogGroup = EGroup.Shelves, Cost = 500, Discription = "After a shelf is placed, you can try [Alt + LMB] to open a menu to stock it!\n...now the customers have something to buy!"},
        new CatalogEntity { DispName = "Oversized vending machine",UID = "uid://ym7aa55dc4pg", CatalogGroup = EGroup.Shelves, Cost = 4000, Discription = "You can fit lots of products on this shelf!"},
        // ========== CHECKOUTS ==========
        new CatalogEntity { DispName = "Self checkout",     UID = "uid://kmvwjfwijr4y", CatalogGroup = EGroup.Misc, Cost = 1000, Discription = "Customers can take a while to use it, so place a few of these checkouts if your store gets crowded!\nAt least 1 of this item is required in the store."},
        new CatalogEntity { DispName = "Checkout queue point",  UID = "uid://btfdwcjiul0pn", CatalogGroup = EGroup.Misc, Cost = 100, Discription = "Customers can queue up here. place a few of these near the checkouts.\nAt least 1 of this item is required in the store."},
        // ========== DECOR ==========
        new CatalogEntity { DispName = "Ceiling light",     UID = "uid://wbgymum0154n", CatalogGroup = EGroup.Decorations, Cost = 1000, Discription = "Put these on the ceiling to light up your store."},
    ];

    public static CatalogEntity FindByUID(string uid)
    {
        return Catalog.FirstOrDefault(e => e.UID == uid);
    }

    public static void Init()
    {
        GD.Print("::== CatalogConfig: Starting init...");

        int errors = 0;
        foreach (var entity in Catalog)
        {
            long id = ResourceUid.TextToId(entity.UID);
            if (!ResourceUid.HasId(id))
            {
                GD.PrintErr($"[FAIL] CatalogConfig: UID <{entity.UID}> for '{entity.DispName}' not found in project.");
                errors++;
            }
            else
            {
                GD.Print($"[OK] CatalogConfig: '{entity.DispName}'");
            }
        }

        int total = Catalog.Count;
        if (errors == 0)
            GD.Print($"CatalogConfig: All ({total}) objects validated with no errors.");
        else
            GD.PrintErr($"CatalogConfig: {errors}/{total} objects failed validation.");

        GD.Print("==>> CatalogConfig: Init done!");
    }
}