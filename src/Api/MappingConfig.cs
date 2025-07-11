using Api.Data;
using Api.Endpoints.CategoryEndpoint.DTO;
using AutoMapper;

namespace Api;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Category, CategoryCreateDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}