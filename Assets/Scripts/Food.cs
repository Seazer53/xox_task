
/// <summary>
/// Food product class which contains properties of a product.
/// </summary>
public class Food : Product
{
    public Food(int price, string name, ProductType type) : base(price, name, type)
    {
        Price = price;
        Name = name;
        Type = type;
    }
}
