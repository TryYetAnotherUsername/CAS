using Godot;
using System;
using System.ComponentModel;

public partial class CatalogueAuto : Control
{
	[Export] private TabContainer _tabContainerRoot;
	[Export] private PackedScene _catContainer;
	[Export] private PackedScene _objectCard;

	public override void _Ready()
    {
        CatalogConfig.Init();
        GD.Print("::== CatalogueAuto: Starting generation...");
        GenAllCats();
        GD.Print("==>> CatalogueAuto: Generation done!}");
    }

    private void GenAllCats()
    {
        foreach (CatalogEntity.ECat cat in Enum.GetValues<CatalogEntity.ECat>())
        {
            var catCont = _catContainer.Instantiate();
            catCont.Name = cat.ToString();
            _tabContainerRoot.AddChild(catCont);
			GenAllObjectsInCat(cat, catCont as ScrollContainer);
        }
    }

	private void GenAllObjectsInCat(CatalogEntity.ECat targCatEnum, ScrollContainer catCont)
    {
        if (catCont is null)
        {
            GD.Print($"CatalogueAuto: GenAllObjectsInCat(): catCont (CatContainer UI) is null. The catogory is: {targCatEnum}. Returning.");
            return;
        }

        // loop through all objects on the dict
		foreach (var kvp in CatalogConfig.BuildModeItems)
        {
            CatalogEntity.ECat foundCat = kvp.Value.Cat;
            
			if (foundCat == targCatEnum)
            {
                // Make and setup a new object card
				var objCard = _objectCard.Instantiate();
				var cardScript = objCard as CatalogueObjectCard;
				cardScript.Item = kvp.Value;

                // Now find the HBox where this card will go into. (See CatalogueCatContainer scene)
				var scrollCont = catCont.FindChild("CatCardCont");

                // Add the card to scene and print sucess message!
                scrollCont.AddChild(objCard);
                GD.Print($"New object card created- {kvp.Key}");
            }
        }
    }
}
