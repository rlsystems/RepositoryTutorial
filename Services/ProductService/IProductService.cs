using RepositoryTutorial.Common;
using RepositoryTutorial.Services.ProductService.DTOs;
using RepositoryTutorial.Services.ProductService.Filters;


namespace RepositoryTutorial.Services.ProductService
{
    public interface IProductService 
    {
        Task<Response<IEnumerable<ProductDTO>>> GetProductsAsync(string keyword = "");
        Task<Response<ProductDTO>> GetProductAsync(Guid id);
        Task<PaginatedResponse<ProductDTO>> GetProductsPaginatedAsync(ProductTableFilter filter); // used by Tanstack Table v8 (React, Vue)

        Task<Response<Guid>> CreateProductAsync(CreateProductRequest request);
        Task<Response<Guid>> UpdateProductAsync(UpdateProductRequest request, Guid id);
        Task<Response<Guid>> DeleteProductAsync(Guid id);

    }
}
