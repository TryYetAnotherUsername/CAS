using Godot;
using System;

public partial class CatalogAuto : TabContainer
{
	[Export] private TabContainer _tabContainerRoot;
	[Export] private PackedScene _catContainer;
	[Export] private PackedScene _objectCard;

	public override void _Ready()
    {
        CatalogConfig.Init();
        GD.Print("::== CatalogueAuto: Starting generation...");
        GenAllCats();
        GD.Print("==>> CatalogueAuto: Generation done!");
    }

    private void GenAllCats()
    {
        foreach (CatalogEntity.EGroup cat in Enum.GetValues<CatalogEntity.EGroup>())
        {
            var catCont = _catContainer.Instantiate();
            catCont.Name = cat.ToString();
            _tabContainerRoot.AddChild(catCont);
			GenAllObjectsInCat(cat, catCont as ScrollContainer);
        }
    }

	private void GenAllObjectsInCat(CatalogEntity.EGroup targCatEnum, ScrollContainer catCont)
    {
        if (catCont is null)
        {
            GD.Print($"CatalogueAuto: GenAllObjectsInCat(): catCont (CatContainer UI) is null. The catogory is: {targCatEnum}. Returning.");
            return;
        }

        // loop through all objects on the dict
		foreach (CatalogEntity catalogEntity in CatalogConfig.Catalog)
        {
            CatalogEntity.EGroup foundCategory = catalogEntity.CatalogGroup;
            
			if (foundCategory == targCatEnum)
            {
                // Make and setup a new object card
				var objCard = _objectCard.Instantiate();
				var cardScript = objCard as CatalogueCard;
				cardScript.Init(catalogEntity);

                // Now find the HBox where this card will go into. (See CatalogueCatContainer scene)
				var vbox = catCont.FindChild("VBoxContainer");

                // Add the card to scene and print sucess message!
                vbox.AddChild(objCard);
                GD.Print($"New object card created- {catalogEntity.DispName} - UID <{catalogEntity.UID}>");
            }
        }
    }
}
