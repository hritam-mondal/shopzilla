using Shopzilla.Entities;

namespace Shopzilla.Features.Products.Data;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid productId);
}
