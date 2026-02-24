using Godot;

public class ProductEntity
{
	public enum ProductCategoryEnum
	{
		Food,
		Drinks,
		Meat,
		Electronics
	}

	public string UID;
    public string DispName;
    public ProductCategoryEnum Category;
    public float UnitPrice;
}