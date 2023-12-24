using MediatR;
using Shopzilla.Entities;

namespace Shopzilla.Features.Products.GetAllProducts;

public sealed class Query : IRequest<List<Product>> { }