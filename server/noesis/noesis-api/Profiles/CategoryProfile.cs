using System;
using AutoMapper;
using noesis_api.Models;
using noesis_api.DTOs;

namespace noesis_api.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<BookCategory, CategoryDTO>()
                .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Category.Id))

                .ForMember(dest => dest.CategoryDescription,
                opt => opt.MapFrom(src => src.Category.CategoryDescription));

            CreateMap<Category, CategoryDTO>();
        }
    }
}
