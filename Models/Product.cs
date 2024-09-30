public class Product
{
    public int ProductId { get; set; }
    public string ProductTitle { get; set; }
    public string? ProductDescription { get; set; }
    public double ProductPrice { get; set; }
    public string? ProductThumbnail { get; set; }
    public bool? ProductIsFeatured { get; set; }

    public Product() {}

    public Product(string productTitle)
    {
        this.ProductTitle = productTitle;
    }
}