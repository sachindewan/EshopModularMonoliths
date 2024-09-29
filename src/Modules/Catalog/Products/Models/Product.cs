using Catalog.Products.Events;

namespace Catalog.Products.Models
{
    public class Product : Aggregate<Guid>
    {
        public string Name { get; private set; } = default!;
        public List<string> Category { get; private set; } = new();
        public string Description { get; private set; } = default!;
        public string ImageFile { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;
        public static Product Create(Guid id, string name, List<string> category, string description, string imagefile, decimal price)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));
            Product product = new()
            {
                Id = id,
                Name = name,
                Category = category,
                Description = description,
                ImageFile = imagefile,
                Price = price
            };
            product.AddDomainEvents(new ProductCreatedEvent(product));
            return product;

        }
        public void Update(string name, List<string> category, string description, string imagefile, decimal price)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));
            Name = name;
            Category = category;
            Description = description;
            ImageFile = imagefile;
            Price = price;
            // if product price changed then raise ProductPriceChanged a domain events
            if (Price != price)
            {
                Price = price;
                AddDomainEvents(new ProductPriceChangedEvent(this));
            }
        }
    }
}
