using MediatR;
using Shopzilla.Entities;
using Shopzilla.Features.Products.Data;

namespace Shopzilla.Features.Products.GetAllProducts;

public sealed class GetAllProductsHandler : IRequestHandler<Query, List<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<List<Product>> Handle(Query request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetAllProductsAsync();
    }
}
