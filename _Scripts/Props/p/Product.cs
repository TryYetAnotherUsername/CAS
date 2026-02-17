using Godot;

public class Product
{
	public enum ProductCategoryEnum
	{
		Food,
		Drinks,
		Meat,
		Electronics
	}

    public string ProductCode;
    public string ProductName;
    public ProductCategoryEnum Category;
    public float Price;
    public PackedScene Scene;
}