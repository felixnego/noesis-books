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
                .ForMember(dest => dest.Name,
                            opt => opt.MapFrom(src => src.Author.Name));

            CreateMap<AuthorListDTO, Author>();
        }
    }
}
