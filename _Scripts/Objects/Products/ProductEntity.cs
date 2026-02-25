using Godot;

public class ProductEntity
{
	public enum EProductCategory
	{
		General,
		Food,
	}

	public enum EPopularity
	{
		Popular,
		Normal,
		Niche
	}

	public string UID;
    public string DispName;
    public EProductCategory Category = EProductCategory.General;

    public float PriceImport;
	public float PriceSell;
	public EPopularity Popularity = EPopularity.Normal;
	public bool AgeRestricted = false;
	public bool LocalImport = false;
	public string Supplier = "-";
}