using System;
using AutoMapper;
using noesis_api.Models;
using noesis_api.DTOs;

namespace noesis_api.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<UserComment, CommentDTO>()
                .ForMember(dest => dest.Username,
                    opt => opt.MapFrom(src => src.User.Username))

                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Comment.Id))

                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.Comment.Text))

                .ForMember(dest => dest.AddedOn,
                    opt => opt.MapFrom(src => src.Comment.AddedOn));
        }
    }
}
