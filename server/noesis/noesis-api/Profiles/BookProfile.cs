using System;
using AutoMapper;
using noesis_api.Models;
using noesis_api.DTOs;
using System.Linq;

namespace noesis_api.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookListDTO>()
                .ForMember(
                    dest => dest.AverageRating,
                    opt => opt.MapFrom(src => src.UserRatings.Count == 0 ? 0 : Math.Round(src.UserRatings.Average(userRating => userRating.RatingValue), 1)));

            CreateMap<BookListDTO, Book>();
        }
    }
}
