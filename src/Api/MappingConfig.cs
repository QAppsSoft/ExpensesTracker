using Api.Endpoints.CategoryEndpoint.DTO;
using Api.Models;
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