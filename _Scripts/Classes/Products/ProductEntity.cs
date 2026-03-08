using Godot;

/// <summary>
/// Class for products, used everywhere in shelf logic/ checkout animations rendering
/// </summary>
public class ProductEntity
{
	public enum EProductCategory
	{
		General,
		Food,
	}

	public string UID {get; set;}
    public string DispName {get; set;}
    public EProductCategory Category {get; set;} = EProductCategory.General;

    public float PriceImport{get; set;}
	public float PriceSell{get; set;}
	public float Popularity {get; set;} = 1;
	public bool AgeRestricted {get; set;}= false;
	public bool LocalImport {get; set;} = false;
	public string Supplier {get; set;} = "-";
}