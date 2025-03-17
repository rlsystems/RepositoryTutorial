using Ardalis.Specification;
using RepositoryTutorial.Domain;


namespace RepositoryTutorial.Application.Services.ProductService.Specifications
{
    public class ProductSearchList : Specification<Product>
    {
        public ProductSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Name.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.Priority); // default sort order

        }
    }
}
