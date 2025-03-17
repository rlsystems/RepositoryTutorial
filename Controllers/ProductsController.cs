using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryTutorial.Common;
using RepositoryTutorial.Services.ProductService;
using RepositoryTutorial.Services.ProductService.DTOs;
using RepositoryTutorial.Services.ProductService.Filters;


namespace RepositoryTutorial.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _ProductService;

        public ProductsController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }

        // full list
        [HttpGet]
        public async Task<IActionResult> GetProductsAsync(string keyword = "")
        {
            Response<IEnumerable<ProductDTO>> result = await _ProductService.GetProductsAsync(keyword);
            return Ok(result);
        }


        // single by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(Guid id)
        {
            Response<ProductDTO> result = await _ProductService.GetProductAsync(id);
            return Ok(result);
        }

        // paginated & filtered list (Tanstack Table v8)
        [HttpPost("products-paginated")]
        public async Task<IActionResult> GetProductsPaginatedAsync(ProductTableFilter filter)
        {
            PaginatedResponse<ProductDTO> products = await _ProductService.GetProductsPaginatedAsync(filter);
            return Ok(products);
        }

        // create
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(CreateProductRequest request)
        {
            try
            {
                Response<Guid> result = await _ProductService.CreateProductAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(UpdateProductRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _ProductService.UpdateProductAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _ProductService.DeleteProductAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
