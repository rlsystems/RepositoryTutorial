using RepositoryTutorial.Common;


namespace RepositoryTutorial.Services.ProductService.DTOs
{
    public class UpdateProductRequest : IDto
    {
        public string Name { get; set; }
        public int Priority { get; set; }

    }

}

