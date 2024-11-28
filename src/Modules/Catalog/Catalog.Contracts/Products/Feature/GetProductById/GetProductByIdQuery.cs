using Catalog.Products.Dtos;
using Shared.Contracts.CQRS;

namespace Catalog.Contracts.Products.Feature.GetProductById
{
    public record GetProductByIdQuery(Guid Id)
       : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(ProductDto Product);
}
