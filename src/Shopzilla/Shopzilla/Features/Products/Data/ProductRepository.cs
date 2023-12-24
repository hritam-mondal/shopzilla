using Npgsql;
using Shopzilla.Entities;

namespace Shopzilla.Features.Products.Data;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;
    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Database")!;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        try
        {
            var products = new List<Product>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM Products";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var product = await MapProductFromReader(reader);
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        try
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM products WHERE product_id = @productId", connection))
                {
                    command.Parameters.AddWithValue("productId", productId);

                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return await MapProductFromReader(reader);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine("Exception while getting product details: " + ex.Message);
            throw;
        }
    }

    private async Task<Product> MapProductFromReader(NpgsqlDataReader reader)
    {
        var product = new Product
        {
            ProductId = reader.GetGuid(reader.GetOrdinal("product_id")),
            ProductName = reader.GetString(reader.GetOrdinal("product_name")),
            ProductImage = reader.GetString(reader.GetOrdinal("product_image")),
            Description = reader.GetString(reader.GetOrdinal("description")),
            Price = reader.GetDecimal(reader.GetOrdinal("price")),
            DiscountPercentage = reader.GetDecimal(reader.GetOrdinal("discount_percentage")),
            StockQuantity = reader.GetInt32(reader.GetOrdinal("stock_quantity")),
            CategoryId = reader.IsDBNull(reader.GetOrdinal("category_id")) ? null : reader.GetGuid(reader.GetOrdinal("category_id")),
            BrandId = reader.IsDBNull(reader.GetOrdinal("brand_id")) ? null : reader.GetGuid(reader.GetOrdinal("brand_id")),
            Gender = reader.GetString(reader.GetOrdinal("gender")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? null : reader.GetDateTime(reader.GetOrdinal("updated_at"))
        };

        // Retrieve category and brand names using their IDs
        product.CategoryName = await GetCategoryNameAsync(product.CategoryId);
        product.BrandName = await GetBrandNameAsync(product.BrandId);

        return product;
    }

    private async Task<string?> GetCategoryNameAsync(Guid? categoryId)
    {
        if (categoryId == null)
            return null;

        using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT name FROM categories WHERE category_id = @CategoryId", connection))
            {
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                var categoryName = await command.ExecuteScalarAsync();
                return categoryName?.ToString();
            }
        }
    }

    private async Task<string?> GetBrandNameAsync(Guid? brandId)
    {
        if (brandId == null)
            return null;

        using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT name FROM brands WHERE brand_id = @BrandId", connection))
            {
                command.Parameters.AddWithValue("@BrandId", brandId);
                var brandName = await command.ExecuteScalarAsync();
                return brandName?.ToString();
            }
        }
    }
}
