using FluentValidation;

namespace Catalog.Products.Feature.CreateProduct
{
    public record CreateProductCommand(ProductDto Product)
     : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Product.Catagory).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    public class CreateProductHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly CatalogDbContext dbContext;

        public CreateProductHandler(CatalogDbContext context)
        {
            dbContext = context;
        }
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //create Product entity from command object
            //save to database
            //return result

            var product = CreateNewProduct(command.Product);

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }
        private Product CreateNewProduct(ProductDto productDto)
        {
            var product = Product.Create(
                Guid.NewGuid(),
                productDto.Name,
                productDto.Catagory,
                productDto.Description,
                productDto.ImageFile,
                productDto.Price);

            return product;
        }
    }
}
