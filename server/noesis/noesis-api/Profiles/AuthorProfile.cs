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
            CreateMap<BookAuthor, AuthorListDTO>();

            CreateMap<AuthorListDTO, Author>();
        }
    }
}
