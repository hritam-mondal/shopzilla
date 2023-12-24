namespace Shopzilla.Entities;

public class Product
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = String.Empty;
    public string ProductImage { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public decimal DiscountPercentage { get; set; }
    public int StockQuantity { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public string? CategoryName { get; set; }
    public string? BrandName { get; set; }
    public string Gender { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}