using MediatR;
using Shopzilla.Entities;
using Shopzilla.Features.Products.Data;

namespace Shopzilla.Features.Products.GetProductById;

public sealed class GetProductByIdHandler : IRequestHandler<Query, Product?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<Product?> Handle(Query request, CancellationToken cancellationToken)
    {
        try
        {
            return await _productRepository.GetProductByIdAsync(request.ProductId);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
