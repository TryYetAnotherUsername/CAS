using Godot;

public class ProductEntity
{
	public enum EProductCategory
	{
		General,
		Food,
	}

	public string UID;
    public string DispName;
    public EProductCategory Category = EProductCategory.General;

    public float PriceImport;
	public float PriceSell;
	public float Popularity = 1;
	public bool AgeRestricted = false;
	public bool LocalImport = false;
	public string Supplier = "-";
}