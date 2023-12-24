using MediatR;
using Shopzilla.Entities;

namespace Shopzilla.Features.Products.GetProductById;

public sealed class Query : IRequest<Product?>
{
    public Guid ProductId { get; set; }
}
