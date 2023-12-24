using Carter;
using MediatR;

namespace Shopzilla.Features.Products.GetProductById;

public class GetProductByIdEndpoint : CarterModule
{
    public GetProductByIdEndpoint() : base("api/v1/product") { }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/{productId}", async (Guid productId, ISender sender) =>
        {
            var product = await sender.Send(new Query { ProductId = productId });
            if (product == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(product);
        });
    }
}
