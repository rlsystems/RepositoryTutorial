using AutoMapper;
using RepositoryTutorial.Domain;
using RepositoryTutorial.Services.ProductService.DTOs;

namespace RepositoryTutorial.Infrastructure
{
    public class MappingProfiles : Profile // automapper mapping configurations
    {
        public MappingProfiles()
        {

            // product mappings
            _ = CreateMap<Product, ProductDTO>();
            _ = CreateMap<CreateProductRequest, Product>();
            _ = CreateMap<UpdateProductRequest, Product>();

            // add new entity mappings here...
        }
    }
}
