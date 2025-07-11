using Api.Data;
using Api.Models.Dto.Categories;
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