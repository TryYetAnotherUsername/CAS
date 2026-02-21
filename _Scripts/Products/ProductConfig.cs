using Godot;
using System.Collections.Generic;
using System.Linq;


public static class ProductConfig
{
    //some code copied from catalog config

    public static readonly List<ProductEntity> Catalog =
    [
        // ========== WALLS ==========
        new ProductEntity { DispName = "TestFoodProduct",           UID = "uid://dutcuyuk8d0b6",    Category = ProductEntity.ProductCategoryEnum.Food }, // dummy uid
        new ProductEntity { DispName = "TestElectronicProduct",     UID = "testUid2",               Category = ProductEntity.ProductCategoryEnum.Electronics },
        new ProductEntity { DispName = "TestMeatProduct",           UID = "testUid3",               Category = ProductEntity.ProductCategoryEnum.Meat },
        new ProductEntity { DispName = "TestDrinksProduct",         UID = "testUid3",               Category = ProductEntity.ProductCategoryEnum.Drinks },
    ];

    public static ProductEntity FindByUID(string uid)
    {
        return Catalog.FirstOrDefault(e => e.UID == uid);
    }

    public static void Init()
    {
        GD.Print("::== ProductConfig: Starting init...");

        int errors = 0;
        foreach (var entity in Catalog)
        {
            long id = ResourceUid.TextToId(entity.UID);
            if (!ResourceUid.HasId(id))
            {
                GD.PrintErr($"[FAIL] ProductConfig: UID <{entity.UID}> for '{entity.DispName}' not found in project.");
                errors++;
            }
            else
            {
                GD.Print($"[OK] ProductConfig: '{entity.DispName}'");
            }
        }

        int total = Catalog.Count;
        if (errors == 0)
            GD.Print($"ProductConfig: All ({total}) objects validated with no errors.");
        else
            GD.PrintErr($"ProductConfig: {errors}/{total} objects failed validation.");

        GD.Print("==>> ProductConfig: Init done!");
    }
}