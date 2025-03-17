using RepositoryTutorial.Common;
using RepositoryTutorial.Services.ProductService.DTOs;


namespace RepositoryTutorial.Services.ProductService
{
    public interface IProductService 
    {
        Task<Response<IEnumerable<ProductDTO>>> GetProductsAsync(string keyword = "");
        Task<Response<ProductDTO>> GetProductAsync(Guid id);
        Task<Response<Guid>> CreateProductAsync(CreateProductRequest request);
        Task<Response<Guid>> UpdateProductAsync(UpdateProductRequest request, Guid id);
        Task<Response<Guid>> DeleteProductAsync(Guid id);

    }
}
