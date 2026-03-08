using Godot;
using System.Collections.Generic;

/// <summary>
/// This class binds godot UIDs to their ProductEntity on startup.
/// </summary>
public static class ProductConfig
{
    public static readonly List<ProductEntity> Catalog =
    [
        // ========== WALLS ==========
        new ProductEntity { DispName = "Bread loaf", UID = "uid://cxcijsn8eyi38", Category = ProductEntity.EProductCategory.Food, PriceImport = 1f, PriceSell = 1.2f, Popularity = 2, LocalImport = true, Supplier = "Grainy Bakery Ltd."},
        new ProductEntity { DispName = "Carrot Pi 5", UID = "uid://37w2t4frtph5", Category = ProductEntity.EProductCategory.Food, PriceImport = 45f, PriceSell = 85.99f, Popularity = 0.05f, Supplier = "John Darwin computer distributers"},
        new ProductEntity { DispName = "Cheese & honey sandwich", UID = "uid://db8sywujetaqj", Category = ProductEntity.EProductCategory.Food, PriceImport = 3.50f, PriceSell = 4.10f, Popularity = 2, Supplier = "BLT Wholesale"},
        new ProductEntity { DispName = "Generic Crisps", UID = "uid://ssssxvop6cd4", Category = ProductEntity.EProductCategory.Food, PriceImport = 0.60f, PriceSell = 2, Popularity = 1, Supplier = "International snacks suppliers"},
        new ProductEntity { DispName = "Carton of milk", UID = "uid://k6yq3isnp6aa", Category = ProductEntity.EProductCategory.Food, PriceImport = 1.3f, PriceSell = 1.5f, Popularity = 1, LocalImport = true, Supplier = "Goldsmith & Sons. family dairy"},
        new ProductEntity { DispName = "School food", UID = "uid://c76oxjy730xxn", Category = ProductEntity.EProductCategory.Food, PriceImport = 2, PriceSell = 5, Popularity = 0.1f, AgeRestricted = true, Supplier = "Totally ethical catering services"},
        new ProductEntity { DispName = "Chemical soup", UID = "uid://dnp4qom8tlh68", Category = ProductEntity.EProductCategory.Food, PriceImport = 1.20f, PriceSell = 2.35f, Popularity = 1, Supplier = "Toxic chemical research Corp."}
    ];


    /// <summary>
    /// Init checks that provided UIDs are valid. 
    /// </summary>
    public static void Init()
    {
        // start
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

        // print error count
        if (errors == 0)
            GD.Print($"ProductConfig: All ({total}) objects validated with no errors.");
        else
            GD.PrintErr($"ProductConfig: {errors}/{total} objects failed validation.");

        GD.Print("==>> ProductConfig: Init done!");
    }
}