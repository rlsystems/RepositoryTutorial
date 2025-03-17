using RepositoryTutorial.Common;

namespace RepositoryTutorial.Services.ProductService.Filters
{
    public class ProductTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
    }
}
