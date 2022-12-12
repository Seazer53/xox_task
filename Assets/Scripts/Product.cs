

/// <summary>
/// Product class that contains product's properties such as price, name and type.
/// </summary>
public abstract class Product
{
    private int _price;
    private string _name;
    private ProductType _type;

    public enum ProductType 
    {
        Magazine,
        MusicCd,
        Food,
        Book
    }

    public int Price
    {
        get => _price; 
        set => _price = value;
    }

    public string Name
    {
        get => _name; 
        set => _name = value;
    }

    public ProductType Type
    {
        get => _type; 
        set => _type = value;
    }

    /// <summary>
    /// Constructor for product class which sets the values of instantiated object with given parameters.
    /// </summary>
    /// <param name="price">Integer price of the product</param>
    /// <param name="name">Name of the product</param>
    /// <param name="type">Product type</param>
    protected Product(int price, string name, ProductType type)
    {
        _price = price;
        _name = name;
        _type = type;
    }
    
}



