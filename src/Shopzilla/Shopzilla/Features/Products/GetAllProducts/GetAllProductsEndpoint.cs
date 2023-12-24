using Carter;
using MediatR;

namespace Shopzilla.Features.Products.GetAllProducts;

public class GetAllProductsEndpoint : CarterModule
{
    public GetAllProductsEndpoint() : base("api/v1/products") { }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ISender sender) =>
        {
            var products = await sender.Send(new GetAllProducts.Query());
            return Results.Ok(products);
        });
    }
}