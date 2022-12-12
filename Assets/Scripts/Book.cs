
/// <summary>
/// Book product class which contains properties of a product.
/// </summary>
public class Book : Product
{
    public Book(int price, string name, ProductType type) : base(price, name, type)
    {
        Price = price;
        Name = name;
        Type = type;
    }
}
