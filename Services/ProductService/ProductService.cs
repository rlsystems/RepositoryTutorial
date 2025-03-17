using AutoMapper;
using RepositoryTutorial.Application.Services.ProductService.Specifications;
using RepositoryTutorial.Common;
using RepositoryTutorial.Domain;
using RepositoryTutorial.Infrastructure;
using RepositoryTutorial.Services.ProductService.DTOs;
using RepositoryTutorial.Services.ProductService.Filters;
using RepositoryTutorial.Services.ProductService.Specifications;

namespace RepositoryTutorial.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public ProductService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<ProductDTO>>> GetProductsAsync(string keyword = "")
        {
            ProductSearchList specification = new(keyword); // ardalis specification
            IEnumerable<ProductDTO> list = await _repository.GetListAsync<Product, ProductDTO, Guid>(specification); // full list, entity mapped to dto
            return Response<IEnumerable<ProductDTO>>.Success(list);
        }


        // get single Product by Id 
        public async Task<Response<ProductDTO>> GetProductAsync(Guid id)
        {
            try
            {
                ProductDTO dto = await _repository.GetByIdAsync<Product, ProductDTO, Guid>(id);
                return Response<ProductDTO>.Success(dto);
            }
            catch (Exception ex)
            {
                return Response<ProductDTO>.Fail(ex.Message);
            }
        }

        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<ProductDTO>> GetProductsPaginatedAsync(ProductTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword)) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            ProductSearchList specification = new(filter?.Keyword); // ardalis specification
            PaginatedResponse<ProductDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Product, ProductDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;

        }

        // create new Product
        public async Task<Response<Guid>> CreateProductAsync(CreateProductRequest request)
        {
            ProductMatchName specification = new(request.Name); // ardalis specification 
            bool ProductExists = await _repository.ExistsAsync<Product, Guid>(specification);
            if (ProductExists)
            {
                return Response<Guid>.Fail("Product already exists");
            }

            Product newProduct = _mapper.Map(request, new Product()); // map dto to domain entity

            try
            {
                Product response = await _repository.CreateAsync<Product, Guid>(newProduct); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return Response<Guid>.Success(response.Id); // return id
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }

        // update Product
        public async Task<Response<Guid>> UpdateProductAsync(UpdateProductRequest request, Guid id)
        {
            Product ProductInDb = await _repository.GetByIdAsync<Product, Guid>(id); // get existing entity
            if (ProductInDb == null)
            {
                return Response<Guid>.Fail("Not Found");
            }

            Product updatedProduct = _mapper.Map(request, ProductInDb); // map dto to domain entity

            try
            {
                Product response = await _repository.UpdateAsync<Product, Guid>(updatedProduct);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return Response<Guid>.Success(response.Id); // return id
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }

        // delete Product
        public async Task<Response<Guid>> DeleteProductAsync(Guid id)
        {
            try
            {
                Product? Product = await _repository.RemoveByIdAsync<Product, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return Response<Guid>.Success(Product.Id);
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }
    }
}

