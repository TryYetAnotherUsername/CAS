using Godot;
using System.Collections.Generic;
using System.Linq;
using static CatalogEntity;

public static class CatalogConfig
{
    public static readonly List<CatalogEntity> Catalog =
    [
        // ========== FLOORS ==========
        new CatalogEntity { DispName = "Wooden floor",      UID = "uid://y7yesnlndrfg",  CatalogGroup = EGroup.Floors },
        // ========== WALLS ==========
        new CatalogEntity { DispName = "Wall",              UID = "uid://dtmwx7ig68eac", CatalogGroup = EGroup.Walls },
        new CatalogEntity { DispName = "Wall (Double)",     UID = "uid://bkwafr84yuc0q", CatalogGroup = EGroup.Walls },
        new CatalogEntity { DispName = "Wall (Windowed)",   UID = "uid://b37vt42e0fhud", CatalogGroup = EGroup.Walls },
        // ========== SHELVES ==========
        new CatalogEntity { DispName = "Basic shelf",       UID = "uid://c3rwyrjlanyer", CatalogGroup = EGroup.Shelves },
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