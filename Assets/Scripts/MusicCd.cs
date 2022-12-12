
/// <summary>
/// MusicCd product class which contains properties of a product.
/// </summary>
public class MusicCd : Product
{
    public MusicCd(int price, string name, ProductType type) : base(price, name, type)
    {
        Price = price;
        Name = name;
        Type = type;
    }
}
