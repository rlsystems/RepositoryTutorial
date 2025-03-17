using RepositoryTutorial.Common;

namespace RepositoryTutorial.Services.ProductService.DTOs
{
    public class CreateProductRequest : IDto
    {
        public string Name { get; set; }
        public int Priority { get; set; }

    }

}
