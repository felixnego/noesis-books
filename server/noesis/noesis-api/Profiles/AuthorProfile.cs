using System;
using AutoMapper;
using noesis_api.Models;
using noesis_api.DTOs;

namespace noesis_api.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<BookAuthor, AuthorListDTO>()
                .ForMember(dest => dest.Id,
                            opt => opt.MapFrom(src => src.Author.Id))
                .ForMember(dest => dest.Name,
                            opt => opt.MapFrom(src => src.Author.Name));

            CreateMap<AuthorListDTO, Author>();

            CreateMap<Author, AuthorListDTO>();
        }
    }
}
