using System;
using AutoMapper;
using noesis_api.Models;
using noesis_api.DTOs;


namespace noesis_api.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<UserNote, NoteDTO>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Note.Id))

                .ForMember(dest => dest.Text,
                    opt => opt.MapFrom(src => src.Note.Text))

                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(src => src.User.Id));
        }
    }
}
