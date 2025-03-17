using RepositoryTutorial.Common;

namespace RepositoryTutorial.Services.ProductService.DTOs
{
    public class ProductDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Priority { get; set; }
    }
}

