
/// <summary>
/// Magazine product class which contains properties of a product.
/// </summary>
public class Magazine : Product
{
    public Magazine(int price, string name, ProductType type) : base(price, name, type)
    {
        Price = price;
        Name = name;
        Type = type;
    }
}
